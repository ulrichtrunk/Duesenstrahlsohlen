namespace Batchworker.Test.Depots
{
    using Data.Depots;
    using Shared.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using NPoco;

    class TestCalculationViewDepot : CalculationViewDepot
    {
        public TestCalculationViewDepot(IDatabase database) : base(database)
        {
        }
    }
}