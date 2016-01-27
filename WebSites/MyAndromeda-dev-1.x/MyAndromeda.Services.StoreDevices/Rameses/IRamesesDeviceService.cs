using MyAndromeda.Core;
using MyAndromeda.Data.Model.AndroAdmin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Logging;

namespace MyAndromeda.Services.StoreDevices.Rameses
{
    public interface IRamesesDeviceService : IDependency
    {
        /// <summary>
        /// Gets the rameses device async.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <returns></returns>
        Task<RamesesDeviceSettings> GetRamesesDeviceAsync(int andromedaSiteId);

        /// <summary>
        /// Determines whether [is rameses setup] [the specified andromeda site id].
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <returns></returns>
        Task<bool> IsRamesesSetup(int andromedaSiteId);

    }

    public class RamesesDeviceService : IRamesesDeviceService
    {
        private readonly AndroAdminDbContext androAdminDbContext;
        private readonly IStoreDeviceSettings storeDeviceSettings;
        private readonly IMyAndromedaLogger logger; 
        
        public RamesesDeviceService(IStoreDeviceSettings storeDeviceSettings, AndroAdminDbContext androAdminDbContext)
        {
            this.androAdminDbContext = androAdminDbContext;
            this.storeDeviceSettings = storeDeviceSettings;
        }

        public async Task<RamesesDeviceSettings> GetRamesesDeviceAsync(int andromedaSiteId) 
        {
            var storeDevice = await androAdminDbContext
                                                       .StoreDevices
                                                       .Include(e => e.Device)
                                                       .Include(e => e.Device.ExternalApi)
                                                       .Where(e => e.Device.Name == "Rameses")
                                                       .Where(e => e.Store.AndromedaSiteId == andromedaSiteId)
                                                       .Where(e=> !e.Removed)
                                                       .FirstOrDefaultAsync();

            if (storeDevice == null)
            {
                throw new ArgumentNullException("Ramese - StoreDevice is null for andromedaSiteId:" + andromedaSiteId);
            }

            if (storeDevice.Device.ExternalApi == null) 
            {
                throw new ArgumentNullException("Ramese - storeDevice.Device.ExternalApi is null for andromedaSiteId:" + andromedaSiteId);
            }

            string externalApiParameters = string.IsNullOrWhiteSpace(storeDevice.Device.ExternalApi.Parameters)
                ? "{}"
                : storeDevice.Device.ExternalApi.Parameters;
            string storeDeviceParameters = string.IsNullOrWhiteSpace(storeDevice.Parameters)
                ? "{}"
                : storeDevice.Parameters;

            var settings = this.storeDeviceSettings.GetObject<RamesesDeviceSettings>(externalApiParameters, storeDeviceParameters);
            
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

    public class RamesesDeviceSettings 
    {
        public bool? AllowEmails { get; set; }
    }
}
