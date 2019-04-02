namespace Batchworker.Test.Depots
{
    using Data.Depots;
    using Shared.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using NPoco;

    class TestSealingSlabDepot : SealingSlabDepot
    {
        private List<SealingSlab> sealingSlabs = new List<SealingSlab>();

        public TestSealingSlabDepot(IDatabase database) : base(database)
        {
        }

        public override int GetMaxIteration(int calculationId)
        {
            return sealingSlabs.Any(c => c.CalculationId == calculationId) ? sealingSlabs.Where(c => c.CalculationId == calculationId).Max(c => c.Iteration) : 0;
        }

        public override void InsertBulk<T>(IEnumerable<T> entities, int chunkSize)
        {
            sealingSlabs.AddRange(entities.Cast<SealingSlab>());
        }
    }
}