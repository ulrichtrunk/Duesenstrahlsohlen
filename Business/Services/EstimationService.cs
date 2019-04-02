using Data;
using Data.Depots;
using Shared;
using Shared.Entities;
using System;

namespace Business.Services
{
    public class EstimationService
    {
        private EstimationDepot estimationDepot;

        public EstimationService()
        {
            var databaseFactory = new NPocoDataBaseFactory();
            this.estimationDepot = new EstimationDepot(databaseFactory.GetDatabase());
        }

        public EstimationService(EstimationDepot estimationDepot)
        {
            this.estimationDepot = estimationDepot;
        }


        /// <summary>
        /// Saves the estimation.
        /// </summary>
        /// <param name="cpuFactor">The cpu factor.</param>
        /// <param name="dbInsertFactor">The database insert factor.</param>
        public virtual void Save(double cpuFactor, double dbInsertFactor)
        {
            // Only save if both values have been provided.
            if(cpuFactor <= 0 ||  double.IsInfinity(cpuFactor) || dbInsertFactor <= 0 || double.IsInfinity(dbInsertFactor))
            {
                return;
            }
            
            var estimation = estimationDepot.GetFirstOrDefault();

            if(estimation == null)
            {
                estimation = new Estimation();
            }

            estimation.CPUFactor = cpuFactor;
            estimation.DBInsertFactor = dbInsertFactor;

            estimationDepot.Save(estimation);
        }


        /// <summary>
        /// Calculates the estimated calculation time in seconds.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="sealingSlabs">The sealing slabs.</param>
        /// <param name="cpuFactor">The cpu factor.</param>
        /// <returns></returns>
        public static double CalculateEstimatedCalculationTimeInSeconds(double radius, int sealingSlabs, double cpuFactor)
        {
            return radius * radius * Math.PI * sealingSlabs * cpuFactor;
        }


        /// <summary>
        /// Calculates the estimated database insert time in seconds.
        /// </summary>
        /// <param name="sealingSlabs">The sealing slabs.</param>
        /// <param name="dbInsertFactor">The database insert factor.</param>
        /// <param name="circleChunkSize">Size of the circle chunk.</param>
        /// <returns></returns>
        public static double CalculateEstimatedDBInsertTimeInSeconds(int sealingSlabs, double dbInsertFactor, int circleChunkSize)
        {
            return (double)sealingSlabs / circleChunkSize * dbInsertFactor;
        }


        /// <summary>
        /// Calculates the cpu factor.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="sealingSlabs">The sealing slabs.</param>
        /// <param name="measuredSeconds">The measured seconds.</param>
        /// <returns></returns>
        public static double CalculateCpuFactor(double radius, int sealingSlabs, double measuredSeconds)
        {
            return measuredSeconds / CalculateEstimatedCalculationTimeInSeconds(radius, sealingSlabs, 1);
        }


        /// <summary>
        /// Calculates the database insert factor.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="sealingSlabs">The sealing slabs.</param>
        /// <param name="measuredSeconds">The measured seconds.</param>
        /// <param name="circleChunkSize">Size of the circle chunk.</param>
        /// <returns></returns>
        public static double CalculateDBInsertFactor(double radius, int sealingSlabs, double measuredSeconds, int circleChunkSize)
        {
            return measuredSeconds / CalculateEstimatedDBInsertTimeInSeconds(sealingSlabs, 1, circleChunkSize);
        }


        /// <summary>
        /// Calculates the estimated time in seconds.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="sealingSlabs">The sealing slabs.</param>
        /// <param name="saveSealingSlabCount">The save sealing slab count.</param>
        /// <param name="circleChunkSize">Size of the circle chunk.</param>
        /// <returns></returns>
        public double CalculateEstimatedTimeInSeconds(double radius, int sealingSlabs, int saveSealingSlabCount, int circleChunkSize)
        {
            // Use measured factors from DB
            var estimation = estimationDepot.GetFirstOrDefault();

            if (estimation == null)
            {
                return 0;
            }

            double secondsCalculation = CalculateEstimatedCalculationTimeInSeconds(radius, sealingSlabs, estimation.CPUFactor);

            App.Logger.Info($"Estimated time for caluclation {secondsCalculation}.");

            double secondsDBInsert = CalculateEstimatedDBInsertTimeInSeconds(saveSealingSlabCount, estimation.DBInsertFactor, circleChunkSize);

            App.Logger.Info($"Estimated time for saving {secondsDBInsert}.");

            return secondsCalculation + secondsDBInsert;
        }
    }
}