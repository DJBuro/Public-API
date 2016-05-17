using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyAndromeda.Services.StoreDevices
{
    public class StoreDeviceSettings : IStoreDeviceSettings
    {
        public TModel GetObject<TModel>(string apiJson, string storeSpecificJson)
        {
            JObject o1 = JObject.Parse(string.IsNullOrWhiteSpace(apiJson) ? "{}" : apiJson);

            JObject o2 = JObject.Parse(string.IsNullOrWhiteSpace(storeSpecificJson) ? "{}" : storeSpecificJson);

            o1.Merge(o2, new JsonMergeSettings
            {
                // union array values together to avoid duplicates
                MergeArrayHandling = MergeArrayHandling.Union
            });

            return JsonConvert.DeserializeObject<TModel>(o1.ToString());
        }
    }
}