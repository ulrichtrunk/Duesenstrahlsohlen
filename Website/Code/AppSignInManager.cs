using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace Website.Code
{
    public class AppSignInManager : SignInManager<AppUser, int>
    {
        public AppSignInManager(Microsoft.AspNet.Identity.UserManager<AppUser, int> userManager, IAuthenticationManager authenticationManager) 
            : base(userManager, authenticationManager)
        { }


        /// <summary>
        /// Called to generate the ClaimsIdentity for the user, override to add additional claims before SignIn
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override async Task<ClaimsIdentity> CreateUserIdentityAsync(AppUser user)
        {
            var identity = await base.CreateUserIdentityAsync(user);

            // Add additional info to User identity which we need in the application.
            identity.AddClaim(new Claim(IdentityExtensions.ClaimIsLocal, user.IsLocal.ToString()));

            return identity;
        }
    }
}