using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataAccess.Devices;
using MyAndromeda.Data.Model.AndroAdmin;

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

        public bool IsStoreGprsDevicebyAndromedaSiteId(int andromedaSiteId)
        {
            var devices = this.deviceDataService
                              .List()
                              .Where(e=> !e.Removed)
                              .Where(e => e.StoreDevices.Any(storeDevice => storeDevice.Store.AndromedaSiteId == andromedaSiteId && !storeDevice.Removed))
                              .ToArray();

            return Evaluation(devices);
        }

        public async Task<bool> IsStoreGprsDevicebyAndromedaSiteIdAsync(int andromedaSiteId)
        {
            var devices = await this.deviceDataService
                              .List()
                              .Where(e => !e.Removed)
                              .Where(e => e.StoreDevices.Any(storeDevice => storeDevice.Store.AndromedaSiteId == andromedaSiteId && !storeDevice.Removed))
                              .ToArrayAsync();

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