using Business.Drawers;
using Business.Grids;
using Business.Plotters;
using Data;
using Data.Depots;
using NPoco;
using Shared;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Services
{
    public class DispatchService
    {
        public class UnsetAreaResult
        {
            public List<SealingSlab>[] IterationSealingSlabs { get; private set; }
            public double[] FreeToTotalPixelRatios { get; private set; }
            public double FreeToTotalPixelRatio { get; private set; }

            public UnsetAreaResult(List<SealingSlab>[] iterationSealingSlabs, double[] freeToTotalPixelRatios)
            {
                IterationSealingSlabs = iterationSealingSlabs;
                FreeToTotalPixelRatios = freeToTotalPixelRatios;
                FreeToTotalPixelRatio = (freeToTotalPixelRatios.Sum() / iterationSealingSlabs.Length);
            }
        }

        private StateDepot stateDepot;
        private CalculationDepot calculationDepot;
        private CalculationDepot calculationDepotTimer;
        private SealingSlabDepot sealingSlabDepot;
        private CalculationService calculationService;
        private EstimationService estimationService;
        private IDatabase database;

        // Limit amount of deleting datasets to prevent timeout.
        private const int deleteMaxDataSets = 500000;
        // Timeout for delete operation in seconds.
        private const int deleteTimeout = 60 * 4;

        public DispatchService()
        {
            var databaseFactory = new NPocoDataBaseFactory();
            database = databaseFactory.GetDatabase();
            var databaseTimer = databaseFactory.GetDatabase();

            stateDepot = new StateDepot(database);
            calculationDepot = new CalculationDepot(database);
            calculationDepotTimer = new CalculationDepot(databaseTimer);
            sealingSlabDepot = new SealingSlabDepot(database);
            calculationService = new CalculationService(stateDepot, calculationDepot, sealingSlabDepot, new CalculationViewDepot(database), databaseFactory);
            estimationService = new EstimationService(new EstimationDepot(database));
        }

        public DispatchService(StateDepot stateDepot,
            CalculationDepot calculationDepot,
            CalculationDepot calculationDepotTimer,
            SealingSlabDepot sealingSlabDepot,
            CalculationService calculationService,
            EstimationService estimationService,
            IDatabase database)
        {
            this.stateDepot = stateDepot;
            this.calculationDepot = calculationDepot;
            this.calculationDepotTimer = calculationDepotTimer;

            this.sealingSlabDepot = sealingSlabDepot;
            this.calculationService = calculationService;
            this.estimationService = estimationService;

            this.database = database;
        }

        /// <summary>
        /// Restarts running but not completed calculations.
        /// </summary>
        public void ReEnqueueRunning()
        {
            App.Logger.Info("Reenqueue running calculations...");
            
            var stateRunning = stateDepot.GetByName(State.NameRunning);
            var stateEnqueued = stateDepot.GetByName(State.NameEnqueued);

            var calculations = calculationDepot.GetByStateId(stateRunning.Id);

            foreach (var calculation in calculations)
            {
                App.Logger.Info($"Set calculation {calculation.Id} from state {stateRunning.Name} to {stateEnqueued.Name}.");

                calculation.StateId = stateEnqueued.Id;
                calculation.StartDate = null;
                calculation.EstimatedEndDate = null;
                calculation.EndDate = null;
            }

            calculationDepot.Save(calculations);
        }


        /// <summary>
        /// Cancels the calculations with status cancelling.
        /// </summary>
        public void CancelCancelling()
        {
            var stateCancelling = stateDepot.GetByName(State.NameCancelling);
            var stateCancelled = stateDepot.GetByName(State.NameCancelled);
            
            var calculations = calculationDepot.GetByStateId(stateCancelling.Id);

            foreach (var calculation in calculations)
            {
                calculation.StateId = stateCancelled.Id;
            }

            if(calculations.Count > 0)
            {
                App.Logger.Info($"Cancel calculations...");
            }

            calculationDepot.Save(calculations);
        }

        /// <summary>
        /// Deletes the calculation by identifier.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        private void Delete(int calculationId)
        {
            try
            {
                using (var transaction = database.GetTransaction())
                {
                    int minId = sealingSlabDepot.GetMinId(calculationId) ?? 0;
                    int maxId = sealingSlabDepot.GetMaxId(calculationId) ?? 0;
                    int difference = maxId - minId;

                    // This operation can take a lot of time so we set the time out to 4 minutes.
                    database.OneTimeCommandTimeout = deleteTimeout;

                    if (difference > deleteMaxDataSets)
                    {
                        // Only delete the max defined amount of datasets.
                        // The remaining datasets will be deleted in the next task.
                        sealingSlabDepot.DeleteByCalculationId(calculationId, minId, minId + deleteMaxDataSets);
                    }
                    else
                    {
                        sealingSlabDepot.DeleteByCalculationId(calculationId);
                        calculationDepot.Delete(calculationId);
                    }

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                App.Logger.Error(ex);

                var stateError = stateDepot.GetByName(State.NameError);
                var calculation = calculationDepot.GetById(calculationId);
                calculation.StateId = stateError.Id;

                calculationDepot.Save(calculation);
            }
        }


        /// <summary>
        /// Deletes the calculations with status deleting.
        /// </summary>
        public void DeleteDeleting()
        {
            var stateDeleting = stateDepot.GetByName(State.NameDeleting);

            if(stateDeleting == null)
            {
                return;
            }

            var calculation = calculationDepot.GetFirstByStateId(stateDeleting.Id);

            if(calculation == null)
            {
                return;
            }

            App.Logger.Info($"Delete calculation {calculation.Id}...");

            Delete(calculation.Id);
        }


        /// <summary>
        /// Saves a calculation that has been dispatched by the batchworker.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <param name="sealingSlabs">The sealing slabs.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="dbInsertFactor">The database insert factor.</param>
        /// <exception cref="System.Exception">
        /// </exception>
        private void SaveDispatched(Calculation calculation, IEnumerable<SealingSlab> sealingSlabs, CancellationToken cancellationToken, out double dbInsertFactor) 
        {
            cancellationToken.ThrowIfCancellationRequested();

            dbInsertFactor = 0;
            
            var stateRunning = stateDepot.GetByName(State.NameRunning);

            if (calculation.StateId != stateRunning.Id)
            {
                throw new Exception($"Not in state {stateRunning.Name} for saving dispatched calulation {calculation.Id}.");
            }

            if (sealingSlabs == null)
            {
                throw new Exception($"No sealing slabs for calulation {calculation.Id}.");
            }

            foreach (var sealingSlab in sealingSlabs)
            {
                sealingSlab.CalculationId = calculation.Id;
            }

            DateTime timeStart = DateTime.Now;

            int maxIteration = sealingSlabDepot.GetMaxIteration(calculation.Id);

            // Continue only with sealing slabs that haven't been saved yet.
            // Only take the defined amount of iterations.
            var iterationSealingSlabs = sealingSlabs
                .Where(x => x.Iteration > maxIteration && x.Iteration <= App.Config.SaveIterationsCount)
                .GroupBy(x => x.Iteration);
           
            foreach(var sealingSlabGroup in iterationSealingSlabs)
            {
                // Use transaction to make sealing slabs consistent per iteration.
                using (var transaction = database.GetTransaction())
                {
                    sealingSlabDepot.InsertBulk(sealingSlabGroup, App.Config.SealingSlabChunkSize);

                    transaction.Complete();

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }

            var diff = (DateTime.Now - timeStart);
            double measuredSeconds = diff.TotalSeconds;
            int sealingSlabCount = iterationSealingSlabs.Sum(x => x.Count());

            dbInsertFactor = EstimationService.CalculateDBInsertFactor(calculation.SealingSlabRadiusPixels, sealingSlabCount, measuredSeconds, App.Config.SealingSlabChunkSize);

            App.Logger.Info($"Calculated DBInsertFactor: {dbInsertFactor}.");

            var stateDone = stateDepot.GetByName(State.NameDone);

            calculation.StateId = stateDone.Id;

            calculationDepot.Save(calculation); // database
        }


        /// <summary>
        /// Plots and draws the sealing slabs.
        /// </summary>
        /// <param name="plotter">The plotter.</param>
        /// <param name="drawer">The drawer.</param>
        /// <param name="calculation">The calculation.</param>
        /// <param name="iteration">The iteration.</param>
        /// <param name="freeToTotalPixelRatioPercent">The free to total pixel ratio percent.</param>
        /// <param name="sealingSlabs">The sealing slabs.</param>
        private void PlotAndDraw(IPlotter plotter, BitBresenhamDrawer drawer, Calculation calculation, int iteration, out double freeToTotalPixelRatioPercent, out List<SealingSlab> sealingSlabs)
        {
            // Instantiate for each loop instead of initially, otherwise the memory will be exhausted.
            var grid = new BitGrid(calculation.WidthPixels, calculation.HeightPixels);

            sealingSlabs = plotter.PlotSealingSlabs<SealingSlab>().ToList();

            // Only save sealing slabs that were drawn.
            sealingSlabs = calculationService.DrawSealingSlabs(drawer, grid, sealingSlabs);

            foreach (var sealingSlab in sealingSlabs)
            {
                sealingSlab.CalculationId = calculation.Id;
                sealingSlab.Iteration = iteration + 1;
            }

            // Save in percent.
            freeToTotalPixelRatioPercent = grid.FreeToTotalPixelRatioPercent;
        }


        /// <summary>
        /// Calculates the unset area.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <param name="plotter">The plotter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private UnsetAreaResult CalculateUnsetArea(Calculation calculation, IPlotter plotter, CancellationToken cancellationToken)
        {
            var drawer = new BitBresenhamDrawer();
            double[] freeToTotalPixelRatios = new double[calculation.Iterations];
            var iterationSealingSlabs = new List<SealingSlab>[calculation.Iterations];

            Parallel.For(0, calculation.Iterations, (i) =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                double freeToTotalPixelRatioPercent;
                List<SealingSlab> sealingSlabs;

                PlotAndDraw(plotter, drawer, calculation, i, out freeToTotalPixelRatioPercent, out sealingSlabs);

                freeToTotalPixelRatios[i] = freeToTotalPixelRatioPercent;
                iterationSealingSlabs[i] = sealingSlabs;
            });

            return new UnsetAreaResult(iterationSealingSlabs, freeToTotalPixelRatios);
        }


        /// <summary>
        /// Gets the estimated seconds.
        /// </summary>
        /// <param name="plotter">The plotter.</param>
        /// <param name="calculation">The calculation.</param>
        /// <param name="circleChunkSize">Size of the circle chunk.</param>
        /// <returns></returns>
        private double GetEstimatedSeconds(IPlotter plotter, Calculation calculation, int circleChunkSize)
        {
            // Plot one iteration to have an estimation of sealing slab count
            double freeToTotalPixelRatioPercent;
            List<SealingSlab> sealingSlabs;
            var drawer = new BitBresenhamDrawer();
            PlotAndDraw(plotter, drawer, calculation, 1, out freeToTotalPixelRatioPercent, out sealingSlabs);
            int sealingSlabCount = sealingSlabs.Count();

            int averageSealingSlabCount = sealingSlabCount * calculation.Iterations;

            // Not all iterations sealing slabs have to be saved and some may have already been saved.
            int maxIteration = sealingSlabDepot.GetMaxIteration(calculation.Id);
            int saveSealingSlabCount = sealingSlabCount * (App.Config.SaveIterationsCount - maxIteration);

            return estimationService.CalculateEstimatedTimeInSeconds(calculation.SealingSlabRadiusPixels, 
                averageSealingSlabCount,
                saveSealingSlabCount, 
                circleChunkSize);
        }


        /// <summary>
        /// Dispatches the calculation.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="circleChunkSize">Size of the circle chunk.</param>
        private void DispatchCalculation(Calculation calculation, CancellationToken cancellationToken, int circleChunkSize)
        {
            App.Logger.Info($"Dispatch calculation {calculation.Id}...");

            cancellationToken.ThrowIfCancellationRequested();
            
            var stateCancelling = stateDepot.GetByName(State.NameCancelling);

            var stateRunning = stateDepot.GetByName(State.NameRunning);
            var plotter = calculationService.GetSealingSlabPlotter(calculation);

            calculation.StateId = stateRunning.Id;
            calculation.StartDate = DateTime.Now;
   
            double estimatedSeconds = GetEstimatedSeconds(plotter, calculation, circleChunkSize);

            if (estimatedSeconds > 0)
            { 
                calculation.EstimatedEndDate = calculation.StartDate.Value.AddSeconds(estimatedSeconds);
            }

            calculation.EndDate = null;

            calculationDepot.Save(calculation);

            App.Logger.Info($"State of calculation {calculation.Id} set to {stateRunning.Name}.");

            var startTime = DateTime.Now;

            var unsetAreaResult = CalculateUnsetArea(calculation, plotter, cancellationToken);

            var allSealingSlabs = unsetAreaResult.IterationSealingSlabs.SelectMany(x => x).ToList();

            double measuredSeconds = (DateTime.Now - startTime).TotalSeconds;
            double cpuFactor = EstimationService.CalculateCpuFactor(calculation.SealingSlabRadiusPixels, allSealingSlabs.Count(), measuredSeconds);

            App.Logger.Info($"Calculation time: {measuredSeconds}.");
            App.Logger.Info($"Calculated EstimationCPUFactor: {cpuFactor}.");

            calculation.UnsetAreaResult = (decimal)unsetAreaResult.FreeToTotalPixelRatio;
            calculation.ResidualWaterResult = calculationService.CalculateResidualWater(calculation);

            // Result is the most important part and can already be saved here.
            calculationDepot.Save(calculation);

            SaveCSVExport(calculation, unsetAreaResult);

            double dbInsertFactor = 0;

            SaveDispatched(calculation, allSealingSlabs, cancellationToken, out dbInsertFactor);

            // Save the estimation factors for cpu and db-insert operations.
            estimationService.Save(cpuFactor, dbInsertFactor);

            App.Logger.Info($"State of calculation {calculation.Id} set to done.");
        }

        /// <summary>
        /// Dispatches a calculation and sets its state to running.
        /// </summary>
        /// <param name="circleChunkSize">Size of the circle chunk.</param>
        public void Dispatch(int circleChunkSize)
        {
            var calculation = calculationService.GetFirstByState(State.NameEnqueued);
            var stateCancelling = stateDepot.GetByName(State.NameCancelling);

            Timer cancellingTimer = null;
            CancellationTokenSource cts = new CancellationTokenSource();

            try
            {
                if (calculation == null)
                {
                    return;
                }

                var runningCalculation = calculationService.GetFirstByState(State.NameRunning);

                if (runningCalculation != null)
                {
                    App.Logger.Info($"Calculation with state {State.NameEnqueued} found but already a calculation in state {State.NameRunning}.");
                    return;
                }

                cancellingTimer = new Timer((t) =>
                {
                    if (calculationDepotTimer.HasState(calculation.Id, State.NameCancelling))
                    {
                        // Cancel Task and stop timer
                        cts.Cancel();
                        cancellingTimer.Dispose();
                    }
                }, null, App.Config.CancellationCheckInterval, App.Config.CancellationCheckInterval);

                DispatchCalculation(calculation, cts.Token, circleChunkSize);
            }
            catch(AggregateException ex)
            {
                // Parallel Loop throws Aggregate Exception
                if(ex.InnerExceptions.Any(x => x is OperationCanceledException))
                {
                    Cancel(calculation);
                }
            }
            catch(OperationCanceledException)
            {
                Cancel(calculation);
            }
            catch (Exception)
            {
                var stateError = stateDepot.GetByName(State.NameError);
                calculation.StateId = stateError.Id;

                throw;
            }
            finally
            {
                if (calculation != null)
                {
                    calculation.EndDate = DateTime.Now;
                    calculationDepot.Save(calculation);
                }

                if(cancellingTimer != null)
                {
                    cancellingTimer.Dispose();
                }
            }
        }

        /// <summary>
        /// Cancels the calculation.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        private void Cancel(Calculation calculation)
        {
            // Set State to cancelled if calculation has been cancelled during dispatch
            var stateCancelled = stateDepot.GetByName(State.NameCancelled);
            calculation.StateId = stateCancelled.Id;
        }


        /// <summary>
        /// Saves the CSV-Export to the hard drive. It can then be downloaded from the website.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <param name="unsetAreaResult">The unset area result.</param>
        public void SaveCSVExport(Calculation calculation, UnsetAreaResult unsetAreaResult)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Width;{calculation.Width}");
                stringBuilder.AppendLine($"Height;{calculation.Height}");
                stringBuilder.AppendLine($"Border X;{calculation.BorderX}");
                stringBuilder.AppendLine($"Border Y;{calculation.BorderY}");
                stringBuilder.AppendLine($"Drilling Point Distance X;{calculation.DrillingPointDistanceX}");
                stringBuilder.AppendLine($"Drilling Point Distance Y;{calculation.DrillingPointDistanceY}");
                stringBuilder.AppendLine($"Sealing Slab Diameter;{calculation.SealingSlabDiameter}");
                stringBuilder.AppendLine($"Depth;{calculation.Depth}");
                stringBuilder.AppendLine($"Standard Derivation Offset %;{calculation.StandardDerivationOffset}");
                stringBuilder.AppendLine($"Standard Derivation Radius;{calculation.StandardDerivationRadius}");
                stringBuilder.AppendLine($"Standard Derivation Drilling Point;{calculation.StandardDerivationDrillingPoint}");
                stringBuilder.AppendLine($"Iterations;{calculation.Iterations}");
                stringBuilder.AppendLine($"Water Level Difference;{calculation.WaterLevelDifference}");
                stringBuilder.AppendLine($"Sealing Slab Thickness;{calculation.SealingSlabThickness}");
                stringBuilder.AppendLine($"Permeability Of Sole Without Unset Area;{calculation.PermeabilityOfSoleWithoutUnsetArea}");
                stringBuilder.AppendLine($"Permeability Of Sole At Unset Area;{calculation.PermeabilityOfSoleAtUnsetArea}");
                stringBuilder.AppendLine($"Residual Water Result;{calculation.ResidualWaterResult}");
                stringBuilder.AppendLine($"Average Unset Area Result;{calculation.UnsetAreaResult.Value}");
                stringBuilder.AppendLine($"Pixel per meter;{calculation.PixelsPerMeter}");
                stringBuilder.AppendLine();

                stringBuilder.AppendLine($"Iteration Nr;Sealing Slab Nr;Unset Area;Area;Offset X;Offset Y;Offset Radius;Offset Drilling Point X;Offset Drilling Point Y;");
                for (int i = 0; i < unsetAreaResult.IterationSealingSlabs.Length; i++)
                {
                    for (int j = 0; j < unsetAreaResult.IterationSealingSlabs[i].Count; j++)
                    {
                        var sealingSlab = unsetAreaResult.IterationSealingSlabs[i][j];

                        var sealingSlabArea = ((double)sealingSlab.Radius / calculation.PixelsPerMeter) * ((double)sealingSlab.Radius / calculation.PixelsPerMeter) * Math.PI;
                        var sealingSlabOffsetX = (double)sealingSlab.OffsetX / calculation.PixelsPerMeter;
                        var sealingSlabOffsetY = (double)sealingSlab.OffsetY / calculation.PixelsPerMeter;
                        var sealingSlabOffsetRadius = ((double)sealingSlab.Radius - (calculation.SealingSlabDiameterPixels / 2)) / calculation.PixelsPerMeter;
                        var sealingSlabOffsetDrillingPointX = (double)sealingSlab.OffsetDrillingPointX / calculation.PixelsPerMeter;
                        var sealingSlabOffsetDrillingPointY = (double)sealingSlab.OffsetDrillingPointY / calculation.PixelsPerMeter;

                        stringBuilder.AppendLine($"{i + 1};{j + 1};{unsetAreaResult.FreeToTotalPixelRatios[i]};{sealingSlabArea:F3};{sealingSlabOffsetX:F3};{sealingSlabOffsetY:F3};{sealingSlabOffsetRadius:F3};{sealingSlabOffsetDrillingPointX:F3};{sealingSlabOffsetDrillingPointY:F3}");
                    }
                }

                File.WriteAllText(calculationService.GetCSVExportPath(calculation.Id), stringBuilder.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                App.Logger.Error(ex);
            }
        }
    }
}