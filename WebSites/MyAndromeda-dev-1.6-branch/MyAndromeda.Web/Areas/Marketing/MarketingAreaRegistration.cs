using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Marketing
{
    public class MarketingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Marketing";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Marketing_default",
                "Marketing/Chain/{chainId}/{externalSiteId}/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
