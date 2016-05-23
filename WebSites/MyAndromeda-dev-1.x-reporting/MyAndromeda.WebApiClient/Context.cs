using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core;

namespace MyAndromeda.WebApiClient
{
    public interface IWebApiClientContext : ISingletonDependency
    {
        /// <summary>
        /// Gets or sets the base address.
        /// </summary>
        /// <value>The base address.</value>
        string BaseAddress { get; set; }

        /// <summary>
        /// Gets or sets the syncing endpoint.
        /// </summary>
        /// <value>The syncing endpoint.</value>
        string SyncingMenuEndpoint { get; set; }

        ///// <summary>
        ///// Gets the bringg webhook endpoint.
        ///// </summary>
        ///// <value>The bringg webhook endpoint.</value>
        //string WebhookBringgEndpoint { get; }

        ///// <summary>
        ///// Gets the order status change endpoint.
        ///// </summary>
        ///// <value>The order status change endpoint.</value>
        //string WebhookOrderStatusChangeEndpoint { get; }

        ///// <summary>
        ///// Gets the webhook menu changed endpoint.
        ///// </summary>
        ///// <value>The webhook menu changed endpoint.</value>
        //string WebhookMenuChangedEndpoint { get; }

        /// <summary>
        /// Gets the webhook menu items changed endpoint.
        /// </summary>
        /// <value>The webhook menu items changed endpoint.</value>
        //string WebhookMenuItemsChangedEndpoint { get; }
    }
}
