using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.AndroWebOrdering
{
    public class AndroWebOrderingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AndroWebOrdering";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AndroWebOrdering_default",
                "AndroWebOrdering/Chain/{chainId}/{externalSiteId}/{controller}/{action}/{id}",
                new { controller="AndroWebOrdering", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}