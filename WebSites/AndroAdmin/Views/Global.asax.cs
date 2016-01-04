using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AndroAdmin.Controllers;
using AndroAdmin.Helpers;
using AndroAdmin.DataAccess;

namespace AndroAdmin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    "DeleteAMSServerFTPServerPair", // Route name
            //    "Store/DeleteAMSServerFTPServerPair/{subId}/{id}", // URL with parameters
            //    new { controller = "Store", action = "DeleteAMSServerFTPServerPair", subId = UrlParameter.Optional, id = UrlParameter.Optional } // Parameter defaults
            //);

            routes.MapRoute(
                "Default2", // Route name
                "{controller}/{action}/{id}/{subId}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional, subId = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            // We will use our own controller factory so we can inject data access objects into the controller
            ControllerBuilder.Current.SetControllerFactory(new AndroAdminControllerFactory());
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();

            ErrorHelper.LogError("Global.asx Application_Error", exception);

            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Index";
            routeData.Values["exception"] = exception;
            Response.StatusCode = 500;
            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 403:
                        routeData.Values["action"] = "Http403";
                        break;
                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                }
            }

            IController errorsController = new ErrorController();
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            errorsController.Execute(rc);
        }
    }
}