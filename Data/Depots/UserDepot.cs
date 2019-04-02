using Shared.Entities;
using NPoco;
using Shared;
using System;
using System.Linq;

namespace Data.Depots
{
    public class UserDepot : EntityDepot<User>
    {
        public UserDepot(IDatabase database)
            : base(database)
        {

        }

        public UserDepot(NPocoDataBaseFactory databaseFactory)
            : this(databaseFactory.GetDatabase())
        {

        }


        /// <summary>
        /// Gets the password hash by user identifier. 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetPasswordHashById(int id)
        {
            return Database.Fetch<User>().Where(x => x.Id == id).Select(x => x.Password).SingleOrDefault();
        }


        /// <summary>
        /// Gets the user by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public User GetByUserName(string userName)
        {
            return Database.Fetch<User>().Where(x => x.Name == userName).SingleOrDefault();
        }
    }
}