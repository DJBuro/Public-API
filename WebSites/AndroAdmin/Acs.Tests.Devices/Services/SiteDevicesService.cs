using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acs.Tests.Devices.Model;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace Acs.Tests.Devices.Services
{
    interface ISiteDevicesService
    {
        bool ContainsAnyDevice(Guid siteId);
        bool ContainsDevice(Guid siteId, string deviceName);

        dynamic GetSettings(Guid siteId, string deviceName);
    }

    public class SiteDevicesService : ISiteDevicesService
    {
        private readonly Entities dbContext;

        public SiteDevicesService()
        {
            this.dbContext = new Model.Entities();
        }

        public bool ContainsAnyDevice(Guid siteId)
        {
            return dbContext.SiteDevices.Any(e => e.SiteId == siteId);
        }

        public bool ContainsDevice(Guid siteId, string deviceName)
        {
            return dbContext.SiteDevices.Any(e => e.SiteId == siteId && e.Device.Name == deviceName);
        }

        public dynamic GetSettings(Guid siteId, string deviceName)
        {
            var table = dbContext.SiteDevices
                .Include(e => e.Device)
                .Include(e => e.Device.ExternalApi);

            var storeDeviceResult = table.FirstOrDefault(e => e.SiteId == siteId);

            if (storeDeviceResult == null) { throw new ArgumentNullException("storeDeviceResult"); }

            if (storeDeviceResult.Device.ExternalApi == null)
            {
                Console.WriteLine("No settings");
                return new ExpandoObject();
            }
            //api parameters - may contain default parameters - credentials 
            dynamic apiParameters = JsonConvert.DeserializeObject(storeDeviceResult.Device.ExternalApi.Parameters);
            //store specific parameters - printer id, currency type etc 
            dynamic storeDeviceParameters = JsonConvert.DeserializeObject(storeDeviceResult.Parameters);

            dynamic all = new ExpandoObject();
            IDictionary<string, object> allDictionary = all;

            foreach (JProperty param in apiParameters)
            {
                allDictionary.Add(param.Name, param.Value);
            }

            if (storeDeviceParameters != null)
            {
                foreach (JProperty param in storeDeviceParameters)
                {
                    if (allDictionary.ContainsKey(param.Name))
                    {
                        allDictionary[param.Name] = param.Value;
                        continue;
                    }
                    allDictionary.Add(param.Name, param.Value);
                }
            }
            //JsonConvert.PopulateObject(storeDeviceResult.Device.ExternalApi.Parameters, all);
            //JsonConvert.PopulateObject(storeDeviceResult.Device.ExternalApi.Parameters, storeDeviceResult.Parameters);

            return all;
        }
    }
}
