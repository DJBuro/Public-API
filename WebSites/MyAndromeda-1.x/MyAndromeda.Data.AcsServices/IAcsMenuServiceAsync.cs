using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using System.Threading.Tasks;
using MyAndromeda.Data.AcsServices.Models;

namespace MyAndromeda.Data.AcsServices
{
    public interface IAcsMenuServiceAsync : IDependency
    {
        /// <summary>
        /// Gets the menu data from endpoints async.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="endpoints">The endpoints.</param>
        /// <returns></returns>
        Task<MyAndromedaMenu> GetMenuDataFromEndpointsAsync(int andromedaSiteId, string externalSiteId, IEnumerable<string> endpoints);

        /// <summary>
        /// Gets the raw menu data from endpoints async.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="endpoints">The endpoints.</param>
        /// <returns></returns>
        Task<dynamic> GetRawMenuDataFromEndpointsAsync(int andromedaSiteId, IEnumerable<string> endpoints);
    }
}
