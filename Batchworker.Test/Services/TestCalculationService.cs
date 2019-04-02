namespace Batchworker.Test.Services
{
    using System.Linq;
    using Shared.Entities;
    using Business.Services;
    using Data;
    using Data.Depots;

    class TestCalculationService : CalculationService
    {
        public TestCalculationService(StateDepot stateDepot, 
            CalculationDepot calculationDepot, 
            SealingSlabDepot circleDepot, 
            CalculationViewDepot calculationViewDepot, 
            NPocoDataBaseFactory databaseFactory)
            : base(stateDepot, calculationDepot, circleDepot, calculationViewDepot, databaseFactory)
        {

        }

        public override Calculation GetFirstByState(string stateName)
        {
            return base.GetFirstByState(stateName);
        }

        public override string GetCSVExportPath(int calculationId)
        {
            return System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{calculationId}.csv");
        }
    }
}