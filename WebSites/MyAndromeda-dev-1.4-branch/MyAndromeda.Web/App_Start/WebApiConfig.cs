using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MyAndromeda.Web.AppStart
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.MessageHandlers.Add(new Handlers.LogHandler());
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "StoreApi",
                routeTemplate: "api/{AndromedaSiteId}/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "GprsCallBackApi",
                routeTemplate: "api/site/{AndromedaSiteId}/gprs-{controller}/order/{OrderId}",
                defaults: new { });
        }
    }
}
