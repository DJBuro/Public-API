using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AndroCloudDataAccess;
using AndroCloudDataAccessEntityFramework;

namespace AndroCloudMVCServices
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class Global : System.Web.HttpApplication
    {
        internal static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // Inject the data access factory
            DataAccessHelper.DataAccessFactory = new EntityFrameworkDataAccessFactory();
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "sitedetails", // Route name
                "weborderapi/sitedetails/{siteId}", // URL with parameters
                new { controller = "weborderapi", action = "sitedetails", siteId = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "hosts", // Route name
                "weborderapi/host", // URL with parameters
                new { controller = "weborderapi", action = "host" } // Parameter defaults
            );

            routes.MapRoute(
                "site", // Route name
                "weborderapi/site", // URL with parameters
                new { controller = "weborderapi", action = "site" } // Parameter defaults
            );

            routes.MapRoute(
                "menu", // Route name
                "weborderapi/menu/{siteid}", // URL with parameters
                new { controller = "weborderapi", action = "menu" } // Parameter defaults
            );

            

            //[WebInvoke(Method = "PUT", UriTemplate = "order/{siteId}/{orderId}?partnerId={partnerId}&applicationId={applicationId}")]
            routes.MapRoute(
                "order", // Route name
                "weborderapi/order/{siteId}/{orderId}", // URL with parameters
                new { controller = "weborderapi", action = "order" } // Parameter defaults
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

            // Use LocalDB for Entity Framework by default
            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}