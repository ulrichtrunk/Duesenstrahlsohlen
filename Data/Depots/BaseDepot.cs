using Shared.Entities;
using NPoco;
using System.Collections.Generic;

namespace Data.Depots
{
    public abstract class BaseDepot
    {
        protected IDatabase Database { get; private set; }

        protected BaseDepot(IDatabase database)
        {
            Database = database;
        }
    }
}