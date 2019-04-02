using Data;
using Data.Depots;
using Shared;
using Shared.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class UserService
    {
        private UserDepot userDepot;

        public UserService()
        {
            var databaseFactory = new NPocoDataBaseFactory();
            userDepot = new UserDepot(databaseFactory);
        }

        /// <summary>
        /// Gets the password hash by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetPasswordHashById(int id)
        {
            return userDepot.GetPasswordHashById(id);
        }


        /// <summary>
        /// Gets the user by name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public User GetByUserName(string userName)
        {
            return userDepot.GetByUserName(userName);
        }


        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public User GetById(int id)
        {
            return userDepot.GetById(id);
        }

        /// <summary>
        /// Saves the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Save(User user)
        {
            userDepot.Save(user);
        }
    }
}