using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Data.DataAccess.Devices
{
    public interface IDevicesDataService : IDataProvider<Device>, IDependency
    {
    }
}
