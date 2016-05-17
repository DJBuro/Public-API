using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.OrderManagement
{
    public class OrderManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OrderManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OrderManagement_default",
                "OrderManagement/Chain/{chainId}/{externalSiteId}/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}