using MyAndromeda.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Gprs
{
    public interface IGprsService : IDependency
    {


        /// <summary>
        /// Determines whether the store has a gprs device
        /// </summary>
        /// <param name="siteId">The site id.</param>
        /// <returns></returns>
        bool IsStoreGprsDeviceBySiteId(int siteId);

        /// <summary>
        /// Determines whether the site has a GRPS device
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <returns></returns>
        bool IsStoreGprsDevicebyAndromedaSiteId(int andromedaSiteId);

        Task<bool> IsStoreGprsDevicebyAndromedaSiteIdAsync(int andromedaSiteId);
    }
}
