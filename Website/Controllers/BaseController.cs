using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Shared;

namespace Website.Controllers
{
    public class BaseController : Controller
    {
        public const string DefaultTempMessageKey = "DefaultMessageKey";
        public static readonly string ErrorMessageKey = string.Empty;

        public int UserId
        {
            get
            {
                return Convert.ToInt32(User.Identity.GetUserId());
            }
        }


        /// <summary>
        /// Sets the success message.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void SetSuccessMessage(string message)
        {
            TempData[DefaultTempMessageKey] = message;
        }


        /// <summary>
        /// Executes the action with validation.
        /// </summary>
        /// <param name="actionToExecute">The action to execute.</param>
        /// <param name="addErrorsToModelState">if set to <c>true</c> [add errors to model state].</param>
        /// <returns></returns>
        public bool ExecuteActionWithValidation(Action actionToExecute, bool addErrorsToModelState = true)
        {
            try
            {
                actionToExecute?.Invoke();

                return true;
            }
            catch (Exception ex)
            {
                if (!addErrorsToModelState)
                {
                    return false;
                }

                this.ModelState.AddModelError(ErrorMessageKey, ex.Message);
            }

            return false;
        }


        /// <summary>
        /// Gets the validation message.
        /// </summary>
        /// <returns></returns>
        public string GetValidationMessage()
        {
            if (ModelState.IsValid)
            {
                return TempData[DefaultTempMessageKey].ToString();
            }

            return string.Join(Environment.NewLine, ModelState[ErrorMessageKey].Errors.Select(x => x.ErrorMessage));
        }
    }
}