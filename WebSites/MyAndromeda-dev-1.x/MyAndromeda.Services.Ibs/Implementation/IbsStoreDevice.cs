using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Services.Ibs;
using MyAndromeda.Services.Ibs.Models;
using MyAndromeda.Services.StoreDevices;

namespace MyAndromeda.Services.Ibs.Implementation
{
    public class IbsStoreDevice : IIbsStoreDevice
    {
        private readonly AndroAdminDbContext androAdminDbContext;
        private readonly IStoreDeviceSettings storeDeviceSettings;

        public IbsStoreDevice(AndroAdminDbContext androAdminDbContext, IStoreDeviceSettings storeDeviceSettings)
        {
            this.storeDeviceSettings = storeDeviceSettings;
            this.androAdminDbContext = androAdminDbContext;
        }

        public async Task<IbsStoreSettings> GetIbsStoreDeviceAsync(int andromedaSiteId) 
        {
            var storeDevice = await androAdminDbContext
                                                       .StoreDevices
                                                       .Include(e => e.Device)
                                                       .Include(e => e.Device.ExternalApi)
                                                       .Where(e => e.Device.Name == "IBS")
                                                       .Where(e => e.Store.AndromedaSiteId == andromedaSiteId)
                                                       .FirstOrDefaultAsync();

            if (storeDevice == null)
            {
                throw new ArgumentNullException("StoreDevice (IBS) is null for andromedaSiteId:" + andromedaSiteId);
            }

            var settings = this.storeDeviceSettings.GetObject<IbsStoreSettings>(storeDevice.Device.ExternalApi.Parameters, storeDevice.Parameters);

            return settings;
        }
 
        public async Task<bool> IsIbsSetup(int andromedaSiteId)
        {
            var anyIbsDevices = await androAdminDbContext
                                                         .StoreDevices
                                                         .AsNoTracking()
                                                         .Where(e => e.Device.Name == "IBS")
                                                         .Where(e => e.Store.AndromedaSiteId == andromedaSiteId)
                                                         .AnyAsync();

            return anyIbsDevices;
        }
    }
}