namespace Batchworker.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Batchworker.Test.Depots;
    using Business.Services;
    using Shared.Entities;

    [TestClass]
    public class DispatchServiceTest
    {
        [TestMethod]
        public void CalculationValueHaveToChangeTest()
        {
            var databaseFactory = new NPocoDataBaseFactory();
            var database = databaseFactory.GetDatabase();
            var databaseTimer = databaseFactory.GetDatabase();

            var stateDepot = new TestStateDepot(database);
            var calculationDepot = new TestCalculationDepot(database);
            var calculationDepotTimer = new TestCalculationDepot(databaseTimer);
            var circleDepot = new TestSealingSlabDepot(database);
            var estimationDepot = new TestEstimationDepot(database);
            var calculationViewDepot = new TestCalculationViewDepot(database);

            var stateEnqueued = stateDepot.GetByName("Enqueued");

            var calculation = new Calculation
            {
                Id = 1,
                StateId = stateEnqueued.Id,
                PixelsPerMeter = 1,
                Height = 500,
                Width = 500,
                BorderX = 20,
                BorderY = 20,
                Depth = 100,
                DrillingPointDistanceX = 50,
                DrillingPointDistanceY = 50,
                SealingSlabDiameter = 80,
                Iterations = 1,
                StandardDerivationOffset = 0.8m,
                StandardDerivationDrillingPoint = 0,
                StandardDerivationRadius = 0,
                SealingSlabThickness = 10,
                PermeabilityOfSoleAtUnsetArea = 15,
                PermeabilityOfSoleWithoutUnsetArea = 12,
                WaterLevelDifference = 11,
            };

            calculationDepot.Save(calculation);

            var calculationService = new Services.TestCalculationService(stateDepot, calculationDepot, circleDepot, calculationViewDepot, databaseFactory);
            var estimationService = new Services.TestEstimationService(estimationDepot);
            
            var dispatchService = new DispatchService(stateDepot, calculationDepot, calculationDepotTimer, circleDepot, calculationService, estimationService, database);

            dispatchService.Dispatch(400);

            Assert.IsNotNull(calculation.UnsetAreaResult, "UnsetAreaResult may not be null after dispatching");
            Assert.IsNotNull(calculation.ResidualWaterResult, "ResidualWaterResult may not be null after dispatching");

            var stateDone = stateDepot.GetByName("Done");
            Assert.AreEqual(stateDone.Id, calculation.StateId, "State has to be \"done\" after dispatching");
        }

        [TestMethod]
        public void CalculationValuesMayNotChangeTest()
        {
            var databaseFactory = new NPocoDataBaseFactory();
            var database = databaseFactory.GetDatabase();
            var databaseTimer = databaseFactory.GetDatabase();

            var stateDepot = new TestStateDepot(database);
            var calculationDepot = new TestCalculationDepot(database);
            var calculationDepotTimer = new TestCalculationDepot(databaseTimer);
            var circleDepot = new TestSealingSlabDepot(database);
            var estimationDepot = new TestEstimationDepot(database);
            var calculationViewDepot = new TestCalculationViewDepot(database);

            var calculation = new Calculation
            {
                Id = 1,
                StateId = 1,
                PixelsPerMeter = 1,
                Height = 500,
                Width = 500,
                BorderX = 20,
                BorderY = 20,
                Depth = 100,
                DrillingPointDistanceX = 50,
                DrillingPointDistanceY = 50,
                SealingSlabDiameter = 80,
                Iterations = 1,
                StandardDerivationOffset = 0.8m,
                StandardDerivationDrillingPoint = 0,
                StandardDerivationRadius = 0,
                SealingSlabThickness = 10,
                PermeabilityOfSoleAtUnsetArea = 15,
                PermeabilityOfSoleWithoutUnsetArea = 12,
                WaterLevelDifference = 11,
            };

            calculationDepot.Save(calculation);

            var calculationService = new Services.TestCalculationService(stateDepot, calculationDepot, circleDepot, calculationViewDepot, databaseFactory);
            var estimationService = new Services.TestEstimationService(estimationDepot);

            var dispatchService = new DispatchService(stateDepot, calculationDepot, calculationDepotTimer, circleDepot, calculationService, estimationService, database);

            dispatchService.Dispatch(400);

            Assert.AreEqual(1, calculation.Id);
            Assert.AreEqual(1, calculation.PixelsPerMeter);
            Assert.AreEqual(500, calculation.Height);
            Assert.AreEqual(500, calculation.Width);
            Assert.AreEqual(20, calculation.BorderX);
            Assert.AreEqual(20, calculation.BorderY);
            Assert.AreEqual(100, calculation.Depth);
            Assert.AreEqual(50, calculation.DrillingPointDistanceX);
            Assert.AreEqual(50, calculation.DrillingPointDistanceY);
            Assert.AreEqual(80, calculation.SealingSlabDiameter);
            Assert.AreEqual(1, calculation.Iterations);
            Assert.AreEqual(0.8m, calculation.StandardDerivationOffset);
            Assert.AreEqual(0, calculation.StandardDerivationDrillingPoint);
            Assert.AreEqual(0, calculation.StandardDerivationRadius);
            Assert.AreEqual(10, calculation.SealingSlabThickness);
            Assert.AreEqual(15, calculation.PermeabilityOfSoleAtUnsetArea);
            Assert.AreEqual(12, calculation.PermeabilityOfSoleWithoutUnsetArea);
            Assert.AreEqual(11, calculation.WaterLevelDifference);
        }

        [TestMethod]
        public void EstimationTest()
        {
            var databaseFactory = new NPocoDataBaseFactory();
            var database = databaseFactory.GetDatabase();
            var databaseTimer = databaseFactory.GetDatabase();

            var stateDepot = new TestStateDepot(database);
            var calculationDepot = new TestCalculationDepot(database);
            var calculationDepotTimer = new TestCalculationDepot(databaseTimer);
            var circleDepot = new TestSealingSlabDepot(database);
            var estimationDepot = new TestEstimationDepot(database);
            var calculationViewDepot = new TestCalculationViewDepot(database);

            var calculation = new Calculation
            {
                Id = 1,
                StateId = 1,
                PixelsPerMeter = 10,
                Height = 500,
                Width = 500,
                BorderX = 20,
                BorderY = 20,
                Depth = 100,
                DrillingPointDistanceX = 50,
                DrillingPointDistanceY = 50,
                SealingSlabDiameter = 80,
                Iterations = 1,
                StandardDerivationOffset = 0.8m,
                StandardDerivationDrillingPoint = 0,
                StandardDerivationRadius = 0,
                SealingSlabThickness = 10,
                PermeabilityOfSoleAtUnsetArea = 15,
                PermeabilityOfSoleWithoutUnsetArea = 12,
                WaterLevelDifference = 11,
            };

            calculationDepot.Save(calculation);

            var calculationService = new Services.TestCalculationService(stateDepot, calculationDepot, circleDepot, calculationViewDepot, databaseFactory);
            var estimationService = new Services.TestEstimationService(estimationDepot);

            var dispatchService = new DispatchService(stateDepot, calculationDepot, calculationDepotTimer, circleDepot, calculationService, estimationService, database);

            dispatchService.Dispatch(400);


            var calculation2 = new Calculation
            {
                Id = 2,
                StateId = 1,
                PixelsPerMeter = 10,
                Height = 500,
                Width = 500,
                BorderX = 20,
                BorderY = 20,
                Depth = 100,
                DrillingPointDistanceX = 50,
                DrillingPointDistanceY = 50,
                SealingSlabDiameter = 80,
                Iterations = 1,
                StandardDerivationOffset = 0.8m,
                StandardDerivationDrillingPoint = 0,
                StandardDerivationRadius = 0,
                SealingSlabThickness = 10,
                PermeabilityOfSoleAtUnsetArea = 15,
                PermeabilityOfSoleWithoutUnsetArea = 12,
                WaterLevelDifference = 11,
            };

            calculationDepot.Save(calculation2);

            var stopWatch = System.Diagnostics.Stopwatch.StartNew();

            dispatchService.Dispatch(400);

            stopWatch.Stop();

            var measuredTimeSpan = stopWatch.Elapsed;

            var estimatedTimeSpan = calculation2.EstimatedEndDate - calculation2.StartDate;
        }
    }
}