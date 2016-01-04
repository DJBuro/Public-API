using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Store
{
    public class StoreAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Store";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Store_default",
                "Chain/{chainId}/Store/{externalSiteId}/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, controller= "Store" }
            );
        }
    }
}
