using System;
using System.Linq;
using MyAndromeda.Data.DataWarehouse.Models;
using AndroAdminDataAccess.Domain.WebOrderingSetup;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.SendGridService.Models;

namespace MyAndromeda.Services.Orders.Emails
{
    public class SuccessMessage : Postal.Email, ICommonEmailSetings
    {
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>The customer.</value>
        public Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>The contact.</value>
        public Contact Contact { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the delivery time.
        /// </summary>
        /// <value>The delivery time.</value>
        public string DeliveryTime { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        /// <value>The site.</value>
        public MyAndromeda.Data.Domain.SiteDomainModel Site { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public OrderHeader Order { get; set; }

        /// <summary>
        /// Gets or sets the store.
        /// </summary>
        /// <value>The store.</value>
        public Store Store { get; set; }

        /// <summary>
        /// Gets or sets the website stuff.
        /// </summary>
        /// <value>The website stuff.</value>
        public AndroWebOrderingWebsite WebsiteStuff { get; set; }

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
