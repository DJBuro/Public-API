using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Data.AcsServices.Context
{
    public interface IActiveMenuContext : IDependency
    {
        /// <summary>
        /// Gets or sets the media server.
        /// </summary>
        /// <value>The media server.</value>
        SiteMenuMediaServer MediaServer { get; set; }

        /// <summary>
        /// Gets or sets the menu.
        /// </summary>
        /// <value>The menu.</value>
        SiteMenu Menu { get; set; }

        /// <summary>
        /// Gets the content path.
        /// </summary>
        /// <value>The content path.</value>
        string ContentPath { get; }

        /// <summary>
        /// Setups the specified andromeda site id.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="externalSiteId">The external site id.</param>
        void Setup(int andromedaSiteId, string externalSiteId);
    }
}
