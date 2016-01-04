using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Loyalty
{
    public class LoyaltyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Loyalty";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Loyalty_default",
                "Loyalty/Chain/{chainId}/{externalSiteId}/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
