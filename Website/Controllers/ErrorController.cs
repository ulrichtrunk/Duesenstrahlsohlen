using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Indexes the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public ActionResult Index(Exception exception)
        {
            return View(exception);
        }
    }
}