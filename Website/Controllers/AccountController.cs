using Website.Code;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Website.Models;
using Microsoft.AspNet.Identity;
using Business.Services;
using Shared.Entities;

namespace Website.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
        {
           
        }
        /// <summary>
        /// Is called when an unauthorized request is made by an user.
        /// When provided the user will be authorized by header information from switch
        /// or otherwise the login form will be returned.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> Login(string returnUrl)
        {
            var headers = HttpContext.Request.Headers;

            string sessionId = headers["shib-session-id"];
            string mail = headers["mail"];

            // Login by Switch
            if (!string.IsNullOrEmpty(sessionId) && !string.IsNullOrEmpty(mail))
            {
                string password = mail;

                var userService = new UserService();

                var user = new UserService().GetByUserName(mail);
                
                if (user == null)
                {
                    user = new User();
                    user.Name = mail;
                    user.Password = new Argon2PasswordHasher().Hash(password); 

                    userService.Save(user);
                }

                var loginViewModel = new LoginViewModel();
                loginViewModel.Email = user.Name;
                loginViewModel.Password = password;

                return await Login(loginViewModel, returnUrl);
            }

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }


        /// <summary>
        /// Authorizes a user by the provided login information and redirects to the
        /// url from the original request.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var signInManager = HttpContext.GetOwinContext().Get<AppSignInManager>();
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


        /// <summary>
        /// Signs out an authorized user. This is only used by local users not by switch users.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction(nameof(Login));
        }


        /// <summary>
        /// If provided redirects to the return url or otherwise to the default page.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(CalculationController.Index), "Calculation");
        }
    }
}