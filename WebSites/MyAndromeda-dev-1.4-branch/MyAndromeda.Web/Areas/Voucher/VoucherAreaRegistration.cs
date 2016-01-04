using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Voucher
{
    public class VoucherAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Voucher";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Voucher_default",
                "Voucher/Chain/{chainId}/{externalSiteId}/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
