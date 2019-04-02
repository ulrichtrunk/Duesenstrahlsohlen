namespace Batchworker.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Batchworker.Test.Depots;

    [TestClass]
    public class CalculationServiceTest
    {
        [TestMethod]
        public void GetCsvExportPathTest()
        {
            var databaseFactory = new NPocoDataBaseFactory();
            var database = databaseFactory.GetDatabase();

            var stateDepot = new TestStateDepot(database);
            var calculationDepot = new TestCalculationDepot(database);
            var circleDepot = new TestSealingSlabDepot(database);
            var calculationViewDepot = new TestCalculationViewDepot(database);

            var calculationService = new Services.TestCalculationService(stateDepot, calculationDepot, circleDepot, calculationViewDepot, databaseFactory);

            int calculationId = 10;

            var csvExportPathExpected = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{calculationId}.csv");

            var csvExportPathActual = calculationService.GetCSVExportPath(calculationId);

            Assert.AreEqual(csvExportPathExpected, csvExportPathActual);
        }
    }
}