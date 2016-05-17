using System;
using System.Linq;
using AndroAdminDataAccess.Domain.WebOrderingSetup;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.SendGridService.Models
{
    public class MarketingEmailMessage : Postal.Email, ICommonEmailSetings 
    {
        public Store Store
        {
            get;
            set;
        }

        public AndroWebOrderingWebsite WebsiteStuff
        {
            get;
            set;
        }

        private WebSiteConfigurations previewConfig;
        /// <summary>
        /// Gets the preview website configuration.
        /// </summary>
        /// <value>The preview website configuration.</value>
        public WebSiteConfigurations PreviewWebsiteConfiguration
        {
            get
            {
                return previewConfig ?? (previewConfig = WebSiteConfigurations.DeserializeJson(this.WebsiteStuff.PreviewSettings));
            }
        }
    }
}