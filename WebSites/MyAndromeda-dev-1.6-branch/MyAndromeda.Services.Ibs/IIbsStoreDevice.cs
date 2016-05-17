using System.Threading.Tasks;
using MyAndromeda.Core;
using System.Linq;
using System;
using MyAndromeda.Services.Ibs.Models;

namespace MyAndromeda.Services.Ibs
{
    public interface IIbsStoreDevice : IDependency
    {
        Task<IbsStoreSettings> GetIbsStoreDeviceAsync(int andromedaSiteId);
        Task<bool> IsIbsSetup(int andromedaSiteId);
    }
}