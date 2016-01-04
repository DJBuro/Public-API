using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MyAndromeda.Framework.Logging;
using MyAndromeda.WebApiServices;

namespace MyAndromeda.WebApiServices
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //log4net
            log4net.Config.XmlConfigurator.Configure();

            var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();

            logger.Debug("App Start - test ibacks request");

            GlobalConfiguration.Configuration.MessageHandlers.Add(new Handlers.LogHandler());
        }

        protected void Application_Error()
        {
            var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
            Exception exception = Server.GetLastError();

            logger.Error("Picked up an error at application level:");
            logger.Error(exception);
        }
    }
}