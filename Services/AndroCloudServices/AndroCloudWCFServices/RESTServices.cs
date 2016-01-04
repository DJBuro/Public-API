using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Net;
using AndroCloudServices;
using System.ServiceModel.Activation;
using AndroCloudWCFServices.Services;
using System.Text;
using System.Configuration;
using AndroCloudHelper;
using System.Threading.Tasks;

namespace AndroCloudWCFServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class RESTServices : IRESTServices
    {
        [WebGet(UriTemplate = "ping")]
        public Stream Ping()
        {
            byte[] returnBytes = Encoding.UTF8.GetBytes("PING!!!");
            return new MemoryStream(returnBytes);
        }

        /// <summary>
        /// Hosts GET
        /// </summary>
        /// <param name="partnerId">For backwards compatibility only</param>
        /// <param name="applicationId"></param>
        /// <returns>A list of hosts</returns>
        [WebGet(UriTemplate = "host?partnerId={partnerId}&applicationId={applicationId}")]
        public Stream GetHost(string partnerId, string applicationId)
        {
            string responseText = Host.GetHosts(Helper.GetDataTypes(), partnerId, applicationId);

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        /// <summary>
        /// Menu GET
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="partnerId">For backwards compatibility only</param>
        /// <param name="applicationId"></param>
        /// <returns>A menu (XML or JSON)</returns>
        [WebGet(UriTemplate = "menu/{siteId}?partnerId={partnerId}&applicationId={applicationId}")]
        public Stream GetMenu(string siteId, string partnerId, string applicationId)
        {
            string responseText = Menu.GetMenu(Helper.GetDataTypes(), siteId, partnerId, applicationId);

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        /// <summary>
        /// Site GET
        /// </summary>
        /// <param name="partnerId">For backwards compatibility only</param>
        /// <param name="groupIdFilter"></param>
        /// <param name="maxDistanceFilter"></param>
        /// <param name="longitudeFilter"></param>
        /// <param name="latitudeFilter"></param>
        /// <param name="applicationId"></param>
        /// <returns>A list of sites</returns>
        [WebGet(UriTemplate = "site?partnerId={partnerId}&groupId={groupIdFilter}&maxDistance={maxDistanceFilter}&longitude={longitudeFilter}&latitude={latitudeFilter}&applicationId={applicationId}")]
        public Stream GetSite(string partnerId, string groupIdFilter, string maxDistanceFilter, string longitudeFilter, string latitudeFilter, string applicationId)
        {
            string responseText = AndroCloudWCFServices.Services.Site.GetSites(Helper.GetDataTypes(), partnerId, maxDistanceFilter, longitudeFilter, latitudeFilter, null, applicationId);

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        /// <summary>
        /// SiteDetails GET
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="partnerId">For backwards compatibility only</param>
        /// <param name="applicationId"></param>
        /// <returns>The site details</returns>
        [WebGet(UriTemplate = "sitedetails/{siteId}?partnerId={partnerId}&applicationId={applicationId}")]
        public Stream GetSiteDetails(string siteId, string partnerId, string applicationId)
        {
            string responseText = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(Helper.GetDataTypes(), partnerId, siteId, applicationId);

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        /// <summary>
        /// Order PUT
        /// </summary>
        /// <param name="input">The order XML/JSON</param>
        /// <param name="siteId"></param>
        /// <param name="orderId"></param>
        /// <param name="partnerId">For backwards compatibility only</param>
        /// <param name="applicationId"></param>
        /// <returns>A response indicating whether or not the order was successfully received by the store</returns>
        [WebInvoke(Method = "PUT", UriTemplate = "order/{siteId}/{orderId}?partnerId={partnerId}&applicationId={applicationId}")]
        public async Task<Stream> PutOrder(Stream input, string siteId, string orderId, string partnerId, string applicationId)
        {
            string responseText = await AndroCloudWCFServices.Services.Order.PutOrder(Helper.GetDataTypes(), input, siteId, orderId, partnerId, applicationId);

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        /// <summary>
        /// Order GET
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="orderId"></param>
        /// <param name="partnerId">For backwards compatibility only</param>
        /// <param name="applicationId"></param>
        /// <returns>The order details (only status at this time)</returns>
        [WebGet(UriTemplate = "order/{siteId}/{orderId}?partnerId={partnerId}&applicationId={applicationId}")]
        public Stream GetOrder(string siteId, string orderId, string partnerId, string applicationId)
        {
            string responseText = AndroCloudWCFServices.Services.Order.GetOrder(Helper.GetDataTypes(), siteId, orderId, partnerId, applicationId);

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }
    }
}