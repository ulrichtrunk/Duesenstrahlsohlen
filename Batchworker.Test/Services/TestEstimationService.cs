namespace Batchworker.Test.Services
{
    using Business.Services;
    using System.Linq;

    class TestEstimationService : EstimationService
    {
        Data.Depots.EstimationDepot estimationDepot;

        public TestEstimationService(Data.Depots.EstimationDepot estimationDepot)
            : base(estimationDepot)
        {
            this.estimationDepot = estimationDepot;
        }

        public override void Save(double cpuFactor, double dbInsertFactor)
        {
            this.estimationDepot.Save(new Shared.Entities.Estimation { Id = 1, CPUFactor = cpuFactor, DBInsertFactor = dbInsertFactor });
        }
    }
}