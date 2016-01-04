using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Services.WebHooks.Models;

namespace MyAndromeda.Services.Bringg.Outgoing
{
    public class BringOutgoingWebHook : IHook
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string UserId { get; set; }
        
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the Andromeda site id.
        /// </summary>
        /// <value>The andromeda site id.</value>
        public int AndromedaSiteId { get;set; }

        /// <summary>
        /// Gets or sets the external site id.
        /// </summary>
        /// <value>The external site id.</value>
        public string ExternalSiteId { get;set; }

        /// <summary>
        /// Gets or sets the source of the update.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the andromeda order id.
        /// </summary>
        /// <value>The andromeda order id.</value>
        public string AndromedaOrderId { get; set; }

        /// <summary>
        /// Gets or sets the external order id.
        /// </summary>
        /// <value>The external order id.</value>
        public string ExternalId { get; set; }

        public int AndromedaOrderStatusId { get; set; }
    }
}
