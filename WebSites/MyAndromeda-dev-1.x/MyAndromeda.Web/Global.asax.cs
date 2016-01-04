using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MyAndromeda.Web.AppStart;
using MyAndromeda.Logging;

namespace MyAndromeda.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            ViewEngines.Engines.Clear();

            IViewEngine razorEngine = new RazorViewEngine()
            {
                FileExtensions = new string[] { "cshtml" }
            };
            ViewEngines.Engines.Add(razorEngine);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //log4net
            log4net.Config.XmlConfigurator.Configure();

            var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
            logger.Debug("App Start");


            GlobalFilters.Filters.Add(new MyAndromeda.Web.AppErrors.WatchHandleErrorAttribute());
            AppStart.WebBackgrounderSetup.Start();
        }

        protected void Application_BeginRequest() 
        {
            //var settings = DependencyResolver.Current.GetService<IApplicationSettings>();

            //if (settings.ProfileApplication) 
            //{ 
            //    MiniProfiler.Start();
            //}
        }

        protected void Application_EndRequest()
        {
            //var settings = DependencyResolver.Current.GetService<IApplicationSettings>();

            //if (settings.ProfileApplication)
            //{
            //    MiniProfiler.Stop();
            //}
        }

        protected void Application_Error() 
        {
            var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
            Exception exception = Server.GetLastError();

            logger.Error("Picked up an error at application level:");
            logger.Error(exception);

            this.RedirectToError();
        }

        public void RedirectToError()
        {
            //var exception = Server.GetLastError();
            //var httpException = exception as HttpException;
            //Response.Clear();
            //Server.ClearError();
            //var routeData = new RouteData();
            //routeData.Values["controller"] = "Error";
            //routeData.Values["action"] = "Index";
            //routeData.Values["area"] = string.Empty;
            //routeData.Values["exception"] = exception;
            //Response.StatusCode = 500;
            
            //IController errorsController = DependencyResolver.Current.GetService<MyAndromeda.Web.Controllers.ErrorController>(); // new ErrorsController();
            //var requestContext = new RequestContext(new HttpContextWrapper(Context), routeData);

            //INotifier notifier = DependencyResolver.Current.GetService<INotifier>();
            //notifier.Exception(httpException);

            //errorsController.Execute(requestContext);
        }
    }
}