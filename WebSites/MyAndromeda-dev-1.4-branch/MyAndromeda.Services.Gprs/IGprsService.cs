using MyAndromeda.Core;
using System;
using System.Linq;

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
        /// Determines whether [is store GPRS device by andromeda site id] [the specified andromeda site id].
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <returns></returns>
        bool IsStoreGprsDevicebyAndromedaSiteId(int andromedaSiteId);


    }
}
