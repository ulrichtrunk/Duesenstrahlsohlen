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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Business.Services
{
    public class CalculationService
    {
        private StateDepot stateDepot;
        private CalculationDepot calculationDepot;
        private SealingSlabDepot sealingSlabDepot;
        private NPocoDataBaseFactory databaseFactory;
        private CalculationViewDepot calculationViewDepot;
        private IDatabase database;

        public CalculationService()
        {
            this.databaseFactory = new NPocoDataBaseFactory();
            this.database = databaseFactory.GetDatabase();
            this.stateDepot = new StateDepot(database);
            this.calculationDepot = new CalculationDepot(database);
            this.sealingSlabDepot = new SealingSlabDepot(database);
            this.calculationViewDepot = new CalculationViewDepot(database);
        }

        public CalculationService(StateDepot stateDepot, 
            CalculationDepot calculationDepot, 
            SealingSlabDepot sealingSlabDepot,
            CalculationViewDepot calculationViewDepot,
            NPocoDataBaseFactory databaseFactory)
        {
            this.stateDepot = stateDepot;
            this.calculationDepot = calculationDepot;
            this.sealingSlabDepot = sealingSlabDepot;
            this.calculationViewDepot = calculationViewDepot;
            this.databaseFactory = databaseFactory;
        }

        /// <summary>
        /// Gets the calculation by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Calculation GetById(int id)
        {
            return calculationDepot.GetById(id);
        }


        /// <summary>
        /// Gets the paged list with calculation views.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public PagedList<CalculationView> GetForGrid(SortAndPagingParameters parameters)
        {
            return calculationViewDepot.GetForGrid(parameters);
        }


        /// <summary>
        /// Gets the view by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public CalculationView GetViewById(int id)
        {
            return calculationViewDepot.GetById(id);
        }


        /// <summary>
        /// Checks the dimension limit.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <param name="expression">The expression.</param>
        private void CheckDimensionLimit(Calculation calculation, Expression<Func<Calculation, int>> expression)
        {
            var property = expression.GetPropertyFromExpression();
            int value = (int)property.GetValue(calculation);

            CheckDimensionLimit(calculation, property.Name, (decimal)value);
        }


        /// <summary>
        /// Checks the dimension limit.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <param name="expression">The expression.</param>
        private void CheckDimensionLimit(Calculation calculation, Expression<Func<Calculation, decimal>> expression)
        {
            var property = expression.GetPropertyFromExpression();
            decimal value = (decimal)property.GetValue(calculation);

            CheckDimensionLimit(calculation, property.Name, (decimal)value);
        }


        /// <summary>
        /// Checks the dimension limit.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.Exception"></exception>
        private void CheckDimensionLimit(Calculation calculation, string propertyName, decimal value)
        {
            if (value > App.Config.MaxPixelDimension)
            {
                throw new Exception($"The dimension of {propertyName} is {value} and exceeds the limit of {App.Config.MaxPixelDimension}.");
            }
        }


        /// <summary>
        /// Validates the max value of properties to prevent exhaustion.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        private void CheckDimensionLimits(Calculation calculation)
        {
            CheckDimensionLimit(calculation, (c) => c.WidthPixels);
            CheckDimensionLimit(calculation, (c) => c.HeightPixels);
            CheckDimensionLimit(calculation, (c) => c.BorderXPixels);
            CheckDimensionLimit(calculation, (c) => c.BorderYPixels);
            CheckDimensionLimit(calculation, (c) => c.DrillingPointDistanceXPixels);
            CheckDimensionLimit(calculation, (c) => c.DrillingPointDistanceYPixels);
            CheckDimensionLimit(calculation, (c) => c.DepthPixels);
            CheckDimensionLimit(calculation, (c) => c.SealingSlabDiameterPixels);
        }


        /// <summary>
        /// Validates a calculation and sets its state to running. It will then be fetched by the batchworker.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <exception cref="System.Exception"></exception>
        public void Execute(Calculation calculation)
        {
            CheckDimensionLimits(calculation);
            
            if(!calculation.IsTransient)
            {
                // Rebind all values from the existing calculation
                calculation = calculationDepot.GetById(calculation.Id);
                calculation.StartDate = null;
                calculation.EndDate = null;
                calculation.EstimatedEndDate = null;

                var state = stateDepot.GetById(calculation.StateId);

                if (!State.ValidExecuteStates.Contains(state.Name))
                {
                    throw new Exception($"Invalid state {state.Name} for executing calculation.");
                }
            }

            var stateEnqueued = stateDepot.GetByName(State.NameEnqueued);

            calculation.StateId = stateEnqueued.Id;

            calculationDepot.Save(calculation);
        }


        /// <summary>
        /// Returns an image of the sealing slabs from one iteration.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <param name="sealingSlabs">The sealing slabs.</param>
        /// <returns></returns>
        public Image GetIterationImage(Calculation calculation, IEnumerable<SealingSlab> sealingSlabs)
        {
            var drawer = new ByteBresenhamDrawer();
            var grid = new ByteGrid(calculation.WidthPixels, calculation.HeightPixels);

            DrawSealingSlabs(drawer, grid, sealingSlabs);

            return grid.GetBitmap();
        }


        /// <summary>
        /// Returns an optimal preview image of a calculation.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <returns></returns>
        public Image GetBasePreviewImage(Calculation calculation)
        {
            CheckDimensionLimits(calculation);

            var sealingSlabPlotter = new Plotter(calculation.WidthPixels,
                calculation.HeightPixels,
                calculation.DrillingPointDistanceXPixels,
                calculation.DrillingPointDistanceYPixels,
                calculation.SealingSlabRadiusPixels,
                calculation.BorderXPixels,
                calculation.BorderYPixels);

            // Plot optimal sealing slabs
            var sealingSlabs = sealingSlabPlotter.PlotSealingSlabs<SealingSlab>();

            return GetIterationImage(calculation, sealingSlabs);
        }


        /// <summary>
        /// Returns a preview image of a calculation with derivations.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <returns></returns>
        public Image GetDerivationPreviewImage(Calculation calculation)
        {
            CheckDimensionLimits(calculation);

            var plotter = GetSealingSlabPlotter(calculation);

            // Plot optimal sealing slabs
            var sealingSlabs = plotter.PlotSealingSlabs<SealingSlab>();

            return GetIterationImage(calculation, sealingSlabs);
        }


        /// <summary>
        /// Returns an image of an existing calculations iteration.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <param name="iteration">The iteration.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public Image GetIterationImage(Calculation calculation, int iteration)
        {
            var sealingSlabs = sealingSlabDepot.GetByCalculationIdAndIteration(calculation.Id, iteration);

            if(sealingSlabs.Count == 0)
            {
                throw new Exception($"Iteration {iteration} not found.");
            }

            return GetIterationImage(calculation, sealingSlabs);
        }


        /// <summary>
        /// Gets the iteration image.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <param name="iteration">The iteration.</param>
        /// <returns></returns>
        public Image GetIterationImage(int calculationId, int iteration)
        {
            var calculation = calculationDepot.GetById(calculationId);

            return GetIterationImage(calculation, iteration);
        }


        /// <summary>
        /// Draws the soles into a grid. It also calculates the unset area.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="drawer">The drawer.</param>
        /// <param name="grid">The grid.</param>
        /// <param name="sealingSlabs">The sealing slabs.</param>
        /// <returns></returns>
        public List<SealingSlab> DrawSealingSlabs<T>(IDrawer<T> drawer, IGrid<T> grid, IEnumerable<SealingSlab> sealingSlabs) where T : struct
        {
            List<SealingSlab> intersectingSealingSlabs = new List<SealingSlab>();

            sealingSlabs.ToList().ForEach(sealingSlab =>
            {
                if (!grid.Intersects(sealingSlab.X, sealingSlab.Y))
                {
                    drawer.DrawSealingSlab(grid, sealingSlab.X, sealingSlab.Y, sealingSlab.Radius);
                }
                else
                {
                    intersectingSealingSlabs.Add(sealingSlab);
                }
            });

            // Only return the non-intersecting sealing slabs
            return sealingSlabs.Except(intersectingSealingSlabs).ToList();
        }


        /// <summary>
        /// Returns the first calculation which has a certain state.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <returns></returns>
        public virtual Calculation GetFirstByState(string stateName)
        {
            var state = stateDepot.GetByName(stateName);

            var calculation = calculationDepot.GetFirstByStateId(state.Id);

            return calculation;
        }


        /// <summary>
        /// Gets the CSV export path.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <returns></returns>
        public virtual string GetCSVExportPath(int calculationId)
        {
            return Path.Combine(App.Config.CsvExportDirectory, $"{calculationId}.csv");
        }


        /// <summary>
        /// Returns the data of a CSV-Export from a calculation.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <returns></returns>
        public byte[] GetCSVExport(int calculationId)
        {
            try
            {
                string contents = File.ReadAllText(GetCSVExportPath(calculationId));

                return Encoding.UTF8.GetBytes(contents);
            }
            catch (Exception ex)
            {
                App.Logger.Error(ex);
            }

            return null;
        }


        /// <summary>
        /// Gets the sealing slab plotter.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <returns></returns>
        public Plotter GetSealingSlabPlotter(Calculation calculation)
        {
            return new NormalDistributionPlotter(
                calculation.WidthPixels,
                calculation.HeightPixels,
                calculation.DrillingPointDistanceXPixels,
                calculation.DrillingPointDistanceYPixels,
                calculation.SealingSlabRadiusPixels,
                calculation.BorderXPixels,
                calculation.BorderYPixels,
                calculation.DepthPixels,
                (double)calculation.StandardDerivationOffset,
                (double)calculation.StandardDerivationRadius,
                (double)calculation.StandardDerivationDrillingPoint);
        }


        /// <summary>
        /// Clones the calculation identifier.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <returns></returns>
        public Calculation Clone(int calculationId)
        {
            var calculation = GetById(calculationId);
            calculation.Id = 0;

            return calculation;
        }


        /// <summary>
        /// Deletes the calculation identifier.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <exception cref="System.Exception"></exception>
        public void Delete(int calculationId)
        {
            var calculation = calculationDepot.GetById(calculationId);
            var state = stateDepot.GetById(calculation.StateId);

            if (!State.ValidDeleteStates.Contains(state.Name))
            {
                throw new Exception($"Calculation with state {state.Name} cannot be deleted.");
            }

            var stateDeleting = stateDepot.GetByName(State.NameDeleting);
            calculation.StateId = stateDeleting.Id;

            calculationDepot.Save(calculation);
        }


        /// <summary>
        /// Cancels the calculation identifier.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <exception cref="System.Exception"></exception>
        public void Cancel(int calculationId)
        {
            var calculation = calculationDepot.GetById(calculationId);
            var state = stateDepot.GetById(calculation.StateId);

            if (!State.ValidCancelStates.Contains(state.Name))
            {
                throw new Exception($"Calculation with state {state.Name} cannot be cancelled.");
            }

            var stateCancelling = stateDepot.GetByName(State.NameCancelling);

            calculation.StateId = stateCancelling.Id;

            calculationDepot.Save(calculation);
        }


        /// <summary>
        /// Determines whether the specified calculation is cancelling.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <returns>
        ///   <c>true</c> if the specified calculation identifier is cancelling; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCancelling(int calculationId)
        {
            return calculationDepot.HasState(calculationId, State.NameCancelling);
        }


        /// <summary>
        /// Calculates the residual water.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <returns></returns>
        public decimal CalculateResidualWater(Calculation calculation)
        {
            decimal aS = calculation.Width * calculation.Height;
            decimal aF = calculation.UnsetAreaResult.Value / 100 * aS;
            decimal h = calculation.WaterLevelDifference;
            decimal dS = calculation.SealingSlabThickness;
            decimal kS = calculation.PermeabilityOfSoleWithoutUnsetArea;
            decimal kF = calculation.PermeabilityOfSoleAtUnsetArea;
            const int decimalPlaces = 2;

            if(dS == decimal.Zero)
            {
                return decimal.Zero;
            }

            return Math.Round((h * (1 / dS)) * (aF * kF + (aS - aF) * kS), decimalPlaces);
        }
    }
}