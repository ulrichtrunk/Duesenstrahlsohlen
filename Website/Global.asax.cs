using Business.Services;
using Website.Code;
using Website.Controllers;
using Shared;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity.Owin;
using Shared.Entities;

namespace Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            new BootstrapService(new DummyPasswordHasher()).Run(HttpRuntime.AppDomainAppPath.Replace("\\Website", ""));
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();

            if (ex != null)
                App.Logger.Error(ex);

            HttpContext httpContext = HttpContext.Current;

            if (httpContext != null)
            {
                RequestContext requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;

                /* when the request is ajax the system can automatically handle a mistake with a JSON response. then overwrites the default response */
                if (requestContext.HttpContext.Request.IsAjaxRequest())
                {
                    httpContext.Response.Clear();
                    string controllerName = requestContext.RouteData.GetRequiredString("controller");
                    IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
                    IController controller = factory.CreateController(requestContext, controllerName);
                    ControllerContext controllerContext = new ControllerContext(requestContext, (ControllerBase)controller);

                    JsonResult jsonResult = new JsonResult();
                    jsonResult.Data = new { success = false, serverError = "500" };
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    jsonResult.ExecuteResult(controllerContext);
                    httpContext.Response.End();
                }
                else
                {
                    Exception exception = Server.GetLastError();

                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;

                    // RouteValues erstellen um danach auszufuehren
                    RouteData routeData = new RouteData();
                    routeData.Values.Add("controller", "Error");
                    routeData.Values.Add("action", "Index");
                    routeData.Values.Add("exception", exception);

                    // Error-Seite aufrufen
                    IController errorController = new ErrorController();
                    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }
        }
    }
}
