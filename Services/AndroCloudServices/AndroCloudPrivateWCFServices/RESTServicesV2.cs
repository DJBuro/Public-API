using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.ServiceModel.Activation;
using AndroCloudPrivateWCFServices.Services;
using AndroCloudHelper;
using System.Threading.Tasks;

namespace AndroCloudPrivateWCFServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true)]
    public class RESTServicesV2 : IRESTServicesV2
    {
        [WebInvoke(Method = "GET", UriTemplate = "host/{siteId}?licenseKey={licenseKey}&hardwareKey={hardwareKey}")]
        public Stream GetHosts(string siteId, string licenseKey, string hardwareKey)
        {
            string responseText = Hosts.GetHostsV2(Helper.GetDataTypes(), siteId, licenseKey, hardwareKey);

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        [WebInvoke(Method = "POST", UriTemplate = "menu/{siteId}?licenseKey={licenseKey}&hardwareKey={hardwareKey}&version={version}")]
        public async Task<Stream> PostMenu(Stream input, string siteId, string licenseKey, string hardwareKey, string version)
        {
            // Pass through to v1
            RESTServices restServices = new RESTServices();
            var result = await restServices.PostMenu(input, siteId, licenseKey, hardwareKey, version);

            return result;
        }

        [WebInvoke(Method = "PUT", UriTemplate = "order/{siteId}/{orderId}?licenseKey={licenseKey}&hardwareKey={hardwareKey}")]
        public async Task<Stream> PutOrder(Stream input, string siteId, string orderId, string licenseKey, string hardwareKey)
        {
            // Pass through to v1
            RESTServices restServices = new RESTServices();
            var result = await restServices.PutOrder(input, siteId, orderId, licenseKey, hardwareKey);

            return result;
        }

        [WebInvoke(Method = "POST", UriTemplate = "order/{siteId}/{orderId}?licenseKey={licenseKey}&hardwareKey={hardwareKey}")]
        public async Task<Stream> PostOrder(Stream input, string siteId, string orderId, string licenseKey, string hardwareKey)
        {
            // Pass through to v1
            RESTServices restServices = new RESTServices();
            var result = await restServices.PostOrder(input, siteId, orderId, licenseKey, hardwareKey);

            return result;
        }

        [WebInvoke(Method = "POST", UriTemplate = "site/{siteId}?licenseKey={licenseKey}&hardwareKey={hardwareKey}")]
        public async Task<Stream> PostSite(Stream input, string siteId, string licenseKey, string hardwareKey)
        {
            // Pass through to v1
            RESTServices restServices = new RESTServices();
            var result = await restServices.PostSite(input, siteId, licenseKey, hardwareKey);

            return result;
        }

        [WebInvoke(Method = "GET", UriTemplate = "sync?key={key}")]
        public Stream GetDataVersion(string key)
        {
            // Pass through to v1
            RESTServices restServices = new RESTServices();
            return restServices.GetDataVersion(key);
        }

        [WebInvoke(Method = "PUT", UriTemplate = "sync?key={key}")]
        public Stream PostSync(Stream input, string key)
        {
            // Pass through to v1
            RESTServices restServices = new RESTServices();
            return restServices.PostSync(input, key);
        }
    }
}