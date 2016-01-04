using System.Collections.Generic;

namespace MyAndromeda.Services.WebHooks.Models.Settings
{
    public class WebhookSettings 
    {
        /// <summary>
        /// Gets or sets the store online status.
        /// </summary>
        /// <value>The store online status.</value>
        public List<WebHookEnrolement> StoreOnlineStatus { get; set; }

        /// <summary>
        /// Gets or sets the edt.
        /// </summary>
        /// <value>The edt.</value>
        public List<WebHookEnrolement> Edt { get; set; }

        /// <summary>
        /// Gets or sets the menu version.
        /// </summary>
        /// <value>The menu version.</value>
        public List<WebHookEnrolement> MenuVersion { get; set; }

        public List<WebHookEnrolement> MenuItems { get; set; }

        /// <summary>
        /// Gets or sets the order status.
        /// </summary>
        /// <value>The order status.</value>
        public List<WebHookEnrolement> OrderStatus { get; set; }

        /// <summary>
        /// Gets or sets the bring updates.
        /// </summary>
        /// <value>The bring updates.</value>
        public List<WebHookEnrolement> BringUpdates { get; set; }

        /// <summary>
        /// Gets or sets the bring eta updates.
        /// </summary>
        /// <value>The bring eta updates.</value>
        public List<WebHookEnrolement> BringEtaUpdates { get; set; }
    }
}