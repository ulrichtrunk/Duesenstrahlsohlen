using Shared.Entities;
using NPoco;

namespace Data.Depots
{
    public class StateDepot : EntityDepot<State>
    {
        public StateDepot(IDatabase database)
            : base(database)
        {

        }


        /// <summary>
        /// Gets the state by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual State GetByName(string name)
        {
            return Database.Query<State>().Where(x => x.Name == name).Single();
        }
    }
}
