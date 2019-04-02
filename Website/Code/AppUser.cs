using Shared.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Code
{
    public class AppUser : IUser<int>
    {
        private int id;

        public AppUser(User user)
        {
            this.id = user.Id;
            this.UserName = user.Name;
            this.IsLocal = user.IsLocal;
        }

        public int Id { get { return id; } }
        public string UserName { get; set; }
        public bool IsLocal { get; private set; }
    }
}