using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyAndromeda.Web.AppStart
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "Error",
                url: "Error", 
                defaults: new { controller = "Error", action = "Index", Area ="" } 
            );

            //routes.MapOwinRoute("

            routes.MapRoute(
                name: "Site", // Route name
                url: "Chain/{chainId}/Site/{externalSiteId}", // URL with parameters
                defaults: new { controller = "Site", action = "Index", externalSiteId = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                name: "ChangeAddress", // Route name
                url: "Chain/{chainId}/Site/{externalSiteId}/ChangeAddress", // URL with parameters
                defaults: new { controller = "Site", action = "ChangeAddress", externalSiteId = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                name: "ChangeOpeningTimes", // Route name
                url: "Chain/{chainId}/Site/{externalSiteId}/ChangeOpeningTimes", // URL with parameters
                defaults: new { controller = "Site", action = "ChangeOpeningTimes", externalSiteId = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                name: "Sites",
                url: "Chain/{chainId}/Sites/",
                defaults: new { controller = "Sites", action = "Index", Area = "" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/",
                defaults: new { controller = "Home", action = "Index", Area ="" }
            );

            routes.IgnoreRoute("favicon.ico");
        }
    }
}