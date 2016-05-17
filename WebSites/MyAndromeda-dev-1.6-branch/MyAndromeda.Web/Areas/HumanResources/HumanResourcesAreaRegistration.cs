using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.HumanResources
{
    public class HumanResourcesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HumanResources";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HumanResources_default_store",
                "hr/{chainId}/store/{externalSiteId}/{action}",
                new { controller="strore", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}