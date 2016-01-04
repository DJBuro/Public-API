using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Menu
{
    public class MenuAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Menu";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Menu_default",
                "Menus/Chain/{chainId}/{externalSiteId}/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                name: "Menu_Chain",
                url: "Menus/Chain/{chainId}/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
