using Shared.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Website.Code
{
    public class AppUserManager : UserManager<AppUser, int>
    {
        public AppUserManager(IUserStore<AppUser, int> store) : base(store)
        {
            this.PasswordHasher = new DummyPasswordHasher();
        }
    }
}