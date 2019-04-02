using Shared.Entities;
using NPoco;
using System.Collections.Generic;

namespace Data.Depots
{
    public class EstimationDepot : EntityDepot<Estimation>
    {
        public EstimationDepot(IDatabase database)
            : base(database)
        {

        }

        /// <summary>
        /// Gets the first or default.
        /// </summary>
        /// <returns></returns>
        public virtual Estimation GetFirstOrDefault()
        {
            return Database.Query<Estimation>().FirstOrDefault();
        }
    }
}