using System;
using System.Linq;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using AndroAdminDataAccess.Domain.WebOrderingSetup;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;

namespace MyAndromeda.Services.Orders.Emails
{
    public class SuccessMessage : Postal.Email, ICommonEmailSetings
    {
        public Customer Customer { get; set; }
        public Contact Contact { get; set; }

        public string Message { get; set; }
        public string DeliveryTime { get; set; }

        public MyAndromedaDataAccess.Domain.Site Site { get; set; }

        public OrderHeader Order { get; set; }
        public Store Store { get; set; }

        public AndroWebOrderingWebsite WebsiteStuff { get; set; }

        private WebSiteConfigurations previewConfig;
        public WebSiteConfigurations PreviewWebsiteConfiguration
        {
            get
            {
                return previewConfig ?? (previewConfig = WebSiteConfigurations.DeserializeJson(this.WebsiteStuff.PreviewSettings));
            }
        }
    }
}
