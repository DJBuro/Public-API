using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.ChainReporting
{
    public class ChainReportingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ChainReporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ChainReporting_default",
                "ChainReporting/{chainId}/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
