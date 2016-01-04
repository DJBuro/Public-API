using System.Web.Mvc;

namespace AndroAdmin.Areas.ThreatDashboard
{
    public class ThreatDashboardAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ThreatDashboard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ThreatDashboard_default",
                "ThreatDashboard/{controller}/{action}/{id}",
                new { action = "Index", controler = "ThreatDashboardHome", id = UrlParameter.Optional }
            );
        }
    }
}
