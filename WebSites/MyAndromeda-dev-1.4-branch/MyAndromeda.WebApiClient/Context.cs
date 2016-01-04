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
    }
}
