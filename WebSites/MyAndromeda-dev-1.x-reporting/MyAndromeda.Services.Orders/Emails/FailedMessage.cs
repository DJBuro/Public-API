using System;
using MyAndromeda.Data.DataWarehouse.Models;
using AndroAdminDataAccess.Domain.WebOrderingSetup;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.SendGridService.Models;

namespace MyAndromeda.Services.Orders.Emails
{
    public class FailedMessage : Postal.Email, ICommonEmailSetings
    {
        public OrderHeader Order { get; set; }

        public Customer Customer { get; set; }

        public Contact Contact { get; set; }

        public string Message { get; set; }

        public MyAndromeda.Data.Domain.SiteDomainModel Site { get; set; }

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