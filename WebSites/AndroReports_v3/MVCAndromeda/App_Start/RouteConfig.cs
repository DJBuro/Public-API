using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCAndromeda
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var today = DateTime.Today;
            routes.MapRoute(name: "dateroute", url: "{year}/{month}/{day}/{controller}/{action}/{id}", defaults: new
            {
                id = UrlParameter.Optional,
                year = today.Year,
                month = today.Month,
                day = today.Day
            });
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}