namespace Batchworker.Test.Depots
{
    using Data.Depots;
    using Shared.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using NPoco;

    class TestCalculationDepot : CalculationDepot
    {
        List<Calculation> calculations = new List<Calculation>();

        private TestStateDepot testStateDepot;

        public TestCalculationDepot(IDatabase database) : base(database)
        {
            testStateDepot = new TestStateDepot(database);
        }

        public override void Save(Calculation entity)
        {
            if (!calculations.Contains(entity))
            {
                calculations.Add(entity);
            }
        }

        public override Calculation GetFirstByStateId(int stateId)
        {
            return calculations.FirstOrDefault(c => c.StateId == stateId);
        }

        public override bool HasState(int calculationId, int stateId)
        {
            return calculations.Any(c => c.Id == calculationId && c.StateId == stateId);
        }

        public override bool HasState(int calculationId, string stateName)
        {
            var state = testStateDepot.GetByName(stateName);

            return HasState(calculationId, state.Id);
        }
    }
}