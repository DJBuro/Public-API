using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.DeliveryZone
{
    public class DeliveryZoneAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DeliveryZone";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        { 
            //context.MapRoute(
            //    "DeliveryZone_default",
            //    "DeliveryZone/Chain/{chainId}/{externalSiteId}/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

            context.MapRoute(
               "DeliveryZone_default",
               "DeliveryZone/Chain/{chainId}/{externalSiteId}/{controller}/{action}",
               new { action = "DeliveryZonesByRadius"}
           );
        }
    }
}
