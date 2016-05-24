using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Net;
using AndroCloudServices;
using System.ServiceModel.Activation;
using AndroCloudPrivateWCFServices.Services;
using System.Text;
using AndroCloudHelper;
using Newtonsoft.Json;

namespace AndroCloudPrivateWCFServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true)]
    public class RESTServices : IRESTServices
    {
        [WebInvoke(Method = "GET", UriTemplate = "host/{siteId}?licenseKey={licenseKey}&hardwareKey={hardwareKey}")]
        public Stream GetHosts(string siteId, string licenseKey, string hardwareKey)
        {
            string responseText = Hosts.GetHosts(Helper.GetDataTypes(), siteId, licenseKey, hardwareKey);

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        [WebInvoke(Method = "POST", UriTemplate = "menu/{siteId}?licenseKey={licenseKey}&hardwareKey={hardwareKey}&version={version}")]
        public async Task<Stream> PostMenu(Stream input, string siteId, string licenseKey, string hardwareKey, string version)
        {
            string responseText = Menus.PostMenu(Helper.GetDataTypes(), input, siteId, licenseKey, hardwareKey, version);

            if (string.IsNullOrWhiteSpace(responseText))
            {
                responseText = await MyAndromedaWebHooks.CallWebHooksForMenuUpdate(siteId, version);
            }
            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        [WebInvoke(Method = "PUT", UriTemplate = "order/{siteId}/{orderId}?licenseKey={licenseKey}&hardwareKey={hardwareKey}")]
        public async Task<Stream> PutOrder(Stream input, string siteId, string orderId, string licenseKey, string hardwareKey)
        {
            //AndroCloudServices.Domain.OrderStatusUpdate orderStatusUpdate = null;
            string responseText = await Orders.PutOrder(Helper.GetDataTypes(), input, siteId, orderId, licenseKey, hardwareKey);

            //it never did anything. 
            //if (orderStatusUpdate != null && string.IsNullOrWhiteSpace(responseText))
            //{
            //    responseText = await MyAndromedaWebHooks.CallWebHooksForOrderStatusChange(orderStatusUpdate, siteId, orderId);
            //}

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        [WebInvoke(Method = "POST", UriTemplate = "order/{siteId}/{orderId}?licenseKey={licenseKey}&hardwareKey={hardwareKey}")]
        public async Task<Stream> PostOrder(Stream input, string siteId, string orderId, string licenseKey, string hardwareKey)
        {
            AndroCloudServices.Domain.OrderStatusUpdate orderStatusUpdate = null;
            AndroCloudDataAccess.Domain.Site site;

            string responseText = Orders.PostOrder(Helper.GetDataTypes(), input, siteId, orderId, licenseKey, hardwareKey, out orderStatusUpdate, out site);

            //site id is the andromeda site id apparently. 
            if (orderStatusUpdate != null && string.IsNullOrWhiteSpace(responseText))
            {
                responseText = await MyAndromedaWebHooks.CallWebHooksForOrderStatusChange(
                    site,
                    orderStatusUpdate, siteId, orderId);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        [WebInvoke(Method = "POST", UriTemplate = "site/{siteId}?licenseKey={licenseKey}&hardwareKey={hardwareKey}")]
        public async Task<Stream> PostSite(Stream input, string siteId, string licenseKey, string hardwareKey)
        {
            AndroCloudServices.Domain.SiteUpdate siteUpdate = null;
            string responseText = Sites.PostSite(Helper.GetDataTypes(), input, siteId, licenseKey, hardwareKey, out siteUpdate);

            if (siteUpdate != null)
            {
                responseText = await MyAndromedaWebHooks.CallWebHooksForEtdChange(siteUpdate, siteId);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        [WebInvoke(Method = "GET", UriTemplate = "sync?key={key}")]
        public Stream GetDataVersion(string key)
        {
            string responseText = Sync.GetDataVersion(Helper.GetDataTypes(), key);

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        [WebInvoke(Method = "PUT", UriTemplate = "sync?key={key}")]
        public Stream PostSync(Stream input, string key)
        {
            string responseText = Sync.PutSync(Helper.GetDataTypes(), input, key);

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }
    }
}