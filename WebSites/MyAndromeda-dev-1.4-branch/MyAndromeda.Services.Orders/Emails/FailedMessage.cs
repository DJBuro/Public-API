using System;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using AndroAdminDataAccess.Domain.WebOrderingSetup;

namespace MyAndromeda.Services.Orders.Emails
{
    public class FailedMessage : Postal.Email, ICommonEmailSetings
    {
        public OrderHeader Order { get; set; }

        public Customer Customer { get; set; }

        public Contact Contact { get; set; }

        public string Message { get; set; }

        public MyAndromedaDataAccess.Domain.Site Site { get; set; }

        public Store Store { get; set; }

        public AndroWebOrderingWebsite WebsiteStuff
        {
            get;
            set;
        }

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