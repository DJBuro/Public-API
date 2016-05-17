using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Acs
{
    public class AcsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Acs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Acs_WebHooks_default",
                "Acs/WebHooks/{action}/{AcsApplicationId}",
                new { controller = "WebHooks", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Acs_default",
                "Acs/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            
        }
    }
}