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

}
