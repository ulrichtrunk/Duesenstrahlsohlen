using Website.Code;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

[assembly: OwinStartup(typeof(Website.App_Start.Startup))]

namespace Website.App_Start
{
    public class Startup
    {
        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }


        /// <summary>
        /// Configures the authentication.
        /// </summary>
        /// <param name="app">The application.</param>
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext<AppUserManager>(() => new AppUserManager(new AppUserStore()));
            app.CreatePerOwinContext<AppSignInManager>((options, context) => new AppSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication));

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromHours(24),
                SlidingExpiration = true
            });
        }
    }
}