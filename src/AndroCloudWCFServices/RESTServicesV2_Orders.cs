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
    partial class RestServicesV2 : IRESTServicesV2
    {
        /// <summary>
        /// Sends an order to a store
        /// </summary>
        /// <param name="input"></param>
        /// <param name="siteId"></param>
        /// <param name="orderId"></param>
        /// <param name="applicationId"></param>
        /// <returns>A response indicating whether or not the order was successfully received by the store</returns>
        [WebInvoke(Method = "PUT", UriTemplate = "sites/{siteId}/orders/{orderId}?applicationId={applicationId}")]
        public async Task<Stream> PutOrder(Stream input, string siteId, string orderId, string applicationId)
        {
            try
            {
                // Pass through to v1
                string responseText = await AndroCloudWCFServices.Services.Order.PutOrder(Helper.GetDataTypes(), input, siteId, orderId, null, applicationId);

                // Convert the response text to a binary stream
                return Helper.StringToStream(responseText);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                return Helper.StringToStream(Helper.ProcessCatastrophicException(exception));
            }
        }

        /// <summary>
        /// Gets an existing order
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="orderId"></param>
        /// <param name="applicationId"></param>
        /// <returns>An order</returns>
        [WebInvoke(Method = "GET", UriTemplate = "sites/{siteId}/order/{orderId}?applicationId={applicationId}")]
        public Stream GetOrder(string siteId, string orderId, string applicationId)
        {
            try
            {
                // Pass through to v1
                RESTServices restServices = new RESTServices();
                return restServices.GetOrder(siteId, orderId, null, applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                return Helper.StringToStream(Helper.ProcessCatastrophicException(exception));
            }
        }

        /// <summary>
        /// Checks the voucher codes for an order
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="orderId"></param>
        /// <param name="applicationId"></param>
        /// <returns>An order</returns>
        [WebInvoke(Method = "PUT", UriTemplate = "sites/{siteId}/ordervouchers?applicationId={applicationId}")]
        public Stream CheckOrderVouchers(Stream input, string siteId, string applicationId)
        {
            try
            {
                string responseText = AndroCloudWCFServices.Services.Order.CheckOrderVouchers(Helper.GetDataTypes(), input, siteId, null, applicationId);

                // Convert the response text to a binary stream
                return Helper.StringToStream(responseText);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                return Helper.StringToStream(Helper.ProcessCatastrophicException(exception));
            }
        }
    }
}