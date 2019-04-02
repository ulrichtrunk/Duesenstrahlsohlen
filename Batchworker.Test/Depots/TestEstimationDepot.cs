namespace Batchworker.Test.Depots
{
    using Data.Depots;
    using Shared.Entities;
    using System.Linq;
    using NPoco;
    using System.Collections.Generic;

    class TestEstimationDepot : EstimationDepot
    {
        List<Estimation> estimations = new List<Estimation>();

        public TestEstimationDepot(IDatabase database) : base(database)
        {
        }

        public override Estimation GetFirstOrDefault()
        {
            return estimations.FirstOrDefault();
            //return new Estimation
            //{
            //    Id = 1,
            //    CPUFactor = 5.3754383085608785E-08,
            //    DBInsertFactor = 0.0360256
            //};
        }

        public override void Save(Estimation entity)
        {
            estimations.Add(entity);
        }
    }
}