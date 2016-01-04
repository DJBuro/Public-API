using System.Web.Mvc;
using System.Web.Routing;
using OrderTrackingAdmin.Mvc.Spring;

namespace OrderTrackingAdmin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {


        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // /fr/Product/Category/All
            routes.MapRoute(
                "DefaultWithLanguage",
                "{languageCode}/{controller}/{action}/{id}",
                new { controller = "Store", action = "Index", id = ""},
                new { languageCode = "^[a-zA-Z]{2,2}$" } // accept only languageCode part, which is 2 chars in length
            );
            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Store", action = "Index", id = "" }  // Parameter defaults
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