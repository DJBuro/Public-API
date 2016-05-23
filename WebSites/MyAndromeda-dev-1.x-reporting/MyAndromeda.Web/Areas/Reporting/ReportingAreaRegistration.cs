using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Reporting
{
    public class ReportingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Reporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Reporting_Order_List",
                "Reporting/Chain/{chainId}/{externalSiteId}/Orders/{action}/from-{fromYear}-{fromMonth}-{fromDay}/to-{toYear}-{toMonth}-{toDay}",
                new { action = "List", controller = "Orders", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Reporting_default",
                "Reporting/Chain/{chainId}/{externalSiteId}/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Chain_Reports",
                "Reporting/Chain/{chainId}/{Controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
