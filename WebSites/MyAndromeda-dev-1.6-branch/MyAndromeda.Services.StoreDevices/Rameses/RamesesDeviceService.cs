using System;
using System.Linq;
using MyAndromeda.Data.Model.AndroAdmin;
using System.Data.Entity;
using System.Threading.Tasks;
using MyAndromeda.Logging;

namespace MyAndromeda.Services.StoreDevices.Rameses
{

    public class RamesesDeviceService : IRamesesDeviceService
    {
        private readonly AndroAdminDbContext androAdminDbContext;
        private readonly IStoreDeviceSettings storeDeviceSettings;
        private readonly IMyAndromedaLogger logger; 
        
        public RamesesDeviceService(IMyAndromedaLogger logger, IStoreDeviceSettings storeDeviceSettings, AndroAdminDbContext androAdminDbContext)
        {
            this.logger = logger;
            this.androAdminDbContext = androAdminDbContext;
            this.storeDeviceSettings = storeDeviceSettings;
        }

        public async Task<RamesesDeviceSettings> GetRamesesDeviceAsync(int andromedaSiteId) 
        {
            StoreDevice storeDevice = await androAdminDbContext
                                                       .StoreDevices
                                                       .Include(e => e.Device)
                                                       .Include(e => e.Device.ExternalApi)
                                                       .Where(e => e.Device.Name == "Rameses")
                                                       .Where(e => e.Store.AndromedaSiteId == andromedaSiteId)
                                                       .Where(e=> !e.Removed)
                                                       .FirstOrDefaultAsync();

            if (storeDevice == null)
            {
                throw new ArgumentNullException("Rameses - StoreDevice is null for andromedaSiteId:" + andromedaSiteId);
            }

            if (storeDevice.Device.ExternalApi == null) 
            {
                throw new ArgumentNullException("Rameses - storeDevice.Device.ExternalApi is null for andromedaSiteId:" + andromedaSiteId);
            }

            string externalApiParameters = string.IsNullOrWhiteSpace(storeDevice.Device.ExternalApi.Parameters)
                ? "{}"
                : storeDevice.Device.ExternalApi.Parameters;
            string storeDeviceParameters = string.IsNullOrWhiteSpace(storeDevice.Parameters)
                ? "{}"
                : storeDevice.Parameters;

            RamesesDeviceSettings settings = this.storeDeviceSettings.GetObject<RamesesDeviceSettings>(externalApiParameters, storeDeviceParameters);
            
            return settings;
        }

        public async Task<bool> IsRamesesSetup(int andromedaSiteId)
        {
            bool anyRamesesDevices = await androAdminDbContext
                                                         .StoreDevices
                                                         .AsNoTracking()
                                                         .Where(e => e.Device.Name == "Rameses")
                                                         .Where(e => e.Store.AndromedaSiteId == andromedaSiteId)
                                                         .Where(e => !e.Removed)
                                                         .AnyAsync();

            return anyRamesesDevices;
        }
    }

}