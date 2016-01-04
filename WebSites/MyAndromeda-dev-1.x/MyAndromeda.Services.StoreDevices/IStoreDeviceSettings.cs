using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core;

namespace MyAndromeda.Services.StoreDevices
{
    public interface IStoreDeviceSettings : ITransientDependency
    {
        TModel GetObject<TModel>(string apiJson, string storeSpecificJson);
    }
}
