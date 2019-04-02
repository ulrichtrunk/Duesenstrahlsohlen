using Shared.Entities;
using NPoco;
using System.Collections.Generic;
using System.Linq;

namespace Data.Depots
{
    public class CalculationDepot : EntityDepot<Calculation>
    {
        public CalculationDepot(IDatabase database)
            : base(database)
        {

        }


        /// <summary>
        /// Gets the first by state identifier.
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <returns></returns>
        public virtual Calculation GetFirstByStateId(int stateId)
        {
            return Database.Query<Calculation>().Where(x => x.StateId == stateId).FirstOrDefault();
        }


        /// <summary>
        /// Gets the by state identifier.
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <returns></returns>
        public List<Calculation> GetByStateId(int stateId)
        {
            return Database.Query<Calculation>().Where(x => x.StateId == stateId).ToList();
        }


        /// <summary>
        /// Determines whether the specified calculation identifier has state.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <param name="stateId">The state identifier.</param>
        /// <returns>
        ///   <c>true</c> if the specified calculation identifier has state; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool HasState(int calculationId, int stateId)
        {
            return Database.Query<Calculation>().Where(x => x.Id == calculationId && x.StateId == stateId).Count() > 0;
        }

        /// <summary>
        /// Determines whether the specified calculation identifier has state.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <param name="stateName">Name of the state.</param>
        /// <returns>
        ///   <c>true</c> if the specified calculation identifier has state; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool HasState(int calculationId, string stateName)
        {
            var state = Database.Query<State>().Where(x => x.Name == stateName).SingleOrDefault();

            return HasState(calculationId, state.Id);
        }
    }
}