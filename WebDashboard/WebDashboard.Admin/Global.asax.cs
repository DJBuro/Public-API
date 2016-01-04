using System.Web.Mvc;
using System.Web.Routing;
using WebDashboard.Mvc.Spring;

namespace WebDashboard.Admin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "HeadOffice", action = "Index", id = "" }  // Parameter defaults
            );

            routes.MapRoute(
            "Updates",                                              // Route name
            "{controller}/{action}/{id}/{trans}",                           // URL with parameters
            new { controller = "HeadOffice", action = "Index", id = "", trans = "" }  // Parameter defaults
        );

            routes.MapRoute(
                "Dashboard",                                              // Route name
                "{controller}/Dashboard/{action}/{id}/{trans}",                           // URL with parameters
                new { controller = "HeadOffice", action = "Index", id = "", trans = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();//must have
            ViewEngines.Engines.Add(new Mvc.SiteWebFormViewEngine());//register localization engine

            ControllerBuilder.Current.SetControllerFactory(typeof(SpringControllerFactory));
            RegisterRoutes(RouteTable.Routes);
        }
    }
}