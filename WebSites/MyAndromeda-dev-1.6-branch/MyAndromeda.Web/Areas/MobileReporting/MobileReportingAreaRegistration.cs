using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.MobileReporting
{
    public class MobileReportingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MobileReporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MobileReporting_default",
                "MobileReporting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
