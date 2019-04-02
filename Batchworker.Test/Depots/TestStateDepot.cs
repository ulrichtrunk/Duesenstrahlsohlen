namespace Batchworker.Test.Depots
{
    using Data.Depots;
    using Shared.Entities;
    using System.Linq;
    using NPoco;

    class TestStateDepot : StateDepot
    {
        public TestStateDepot(IDatabase database) : base(database)
        {
        }

        public override State GetByName(string name)
        {
            switch (name)
            {
                case "Enqueued":
                    return new State { Id = 1, Name = "Enqueued" };
                case "Running":
                    return new State { Id = 2, Name = "Running" };
                case "Cancelled":
                    return new State { Id = 3, Name = "Cancelled" };
                case "Error":
                    return new State { Id = 4, Name = "Error" };
                case "Done":
                    return new State { Id = 5, Name = "Done" };
                case "Cancelling":
                    return new State { Id = 6, Name = "Cancelling" };
                default:
                    throw new System.NotImplementedException($"State {name} not implemented");
            }
        }
    }
}