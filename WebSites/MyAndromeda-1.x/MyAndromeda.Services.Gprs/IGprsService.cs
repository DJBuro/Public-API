
using MyAndromeda.Core;
using System;
using System.Linq;
using MyAndromedaDataAccessEntityFramework.DataAccess.Devices;

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
        /// Determines whether [is store GPRS deviceby andromeda site id] [the specified andromeda site id].
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <returns></returns>
        bool IsStoreGprsDevicebyAndromedaSiteId(int andromedaSiteId);
    }

    public class GprsService : IGprsService
    {
        private readonly IDevicesDataService deviceDataService; 

        public GprsService(IDevicesDataService deviceDataService) 
        {
            this.deviceDataService = deviceDataService;

        }

        public bool IsStoreGprsDeviceBySiteId(int siteId)
        {
            var devices = this.deviceDataService.List(e => e.StoreDevices.Any(storeDevice => storeDevice.StoreId == siteId));

            return devices.Any(e => e.Name.IndexOf("GPRS", StringComparison.InvariantCultureIgnoreCase) >= 0);
        }

        public bool IsStoreGprsDevicebyAndromedaSiteId(int siteId)
        {
            var devices = this.deviceDataService
                .List().Where(e=> e.StoreDevices.Any(storeDevice => storeDevice.Store.AndromedaSiteId == siteId))
                .ToArray();

            return
                devices.Any(e => e.Name.IndexOf("iBT8000", StringComparison.InvariantCultureIgnoreCase) >= 0)
                ||
                devices.Any(e => e.Name.IndexOf("GPRS", StringComparison.InvariantCultureIgnoreCase) >= 0);
        }
    }
}
