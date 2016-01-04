using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromedaDataAccessEntityFramework.DataAccess.Devices;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;

namespace MyAndromeda.Services.Gprs
{
    public class GprsService : IGprsService
    {
        private readonly IDevicesDataService deviceDataService; 

        public GprsService(IDevicesDataService deviceDataService) 
        {
            this.deviceDataService = deviceDataService;
        }

        public bool IsStoreGprsDeviceBySiteId(int siteId)
        {
            var devices = this.deviceDataService
                              .List()
                              .Where(e=> !e.Removed)
                              .Where(e => e.StoreDevices.Any(storeDevice => storeDevice.StoreId == siteId && !storeDevice.Removed))
                              .ToArray();

            return Evaluation(devices);
        }

        public bool IsStoreGprsDevicebyAndromedaSiteId(int siteId)
        {
            var devices = this.deviceDataService
                              .List()
                              .Where(e=> !e.Removed)
                              .Where(e => e.StoreDevices.Any(storeDevice => storeDevice.Store.AndromedaSiteId == siteId && !storeDevice.Removed))
                              .ToArray();

            return Evaluation(devices);
        }

        private bool Evaluation(IEnumerable<Device> devices) 
        {
            var k =
                   devices.Any(e => e.Name.IndexOf("iBT8000", StringComparison.InvariantCultureIgnoreCase) >= 0) ||
                   devices.Any(e => e.Name.IndexOf("GPRS", StringComparison.InvariantCultureIgnoreCase) >= 0);

            return k;
        }
    }
}