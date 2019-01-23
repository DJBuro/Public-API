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
    partial class RESTServicesV2_Host : IRESTServicesV2
    {
        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="username"></param>
        /// <param name="applicationId"></param>
        /// <returns>A customer</returns>
        [WebInvoke(Method = "GET", UriTemplate = "customers/{username}?applicationId={applicationId}&siteId={externalSiteId}")]
        public Stream GetCustomer(string username, string applicationId, string externalSiteId)
        {
            string responseText = "";

            try
            {
                string password = Helper.GetPassword();
                responseText = AndroCloudWCFServices.Services.Customer.Get(Helper.GetDataTypes(), username, password, externalSiteId, applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        /// <summary>
        /// Gets a customers orders (summary only)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="applicationId"></param>
        /// <returns>A customers orders</returns>
        [WebInvoke(Method = "GET", UriTemplate = "customers/{username}/orders?applicationId={applicationId}")]
        public Stream GetCustomerOrders(string username, string applicationId)
        {
            string responseText = "";

            try
            {
                string password = Helper.GetPassword();
                responseText = AndroCloudWCFServices.Services.Order.GetCustomerOrders(Helper.GetDataTypes(), username, password, applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        /// <summary>
        /// Gets a customers order (detailed)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="applicationId"></param>
        /// <param name="orderid"></param>
        /// <returns>An order</returns>
        [WebInvoke(Method = "GET", UriTemplate = "customers/{username}/orders/{orderid}?applicationId={applicationId}")]
        public Stream GetCustomerOrder(string username, string orderid, string applicationId)
        {
            string responseText = "";

            try
            {
                string password = Helper.GetPassword();
                responseText = AndroCloudWCFServices.Services.Order.GetCustomerOrderDetails(Helper.GetDataTypes(), orderid, username, password, applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        /// <summary>
        /// Adds a new customer
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="applicationId"></param>
        /// <returns>A response indicating whether or not the customer was successfully added</returns>
        [WebInvoke(Method = "PUT", UriTemplate = "customers/{username}?applicationId={applicationId}&siteId={externalSiteId}")]
        public Stream PutCustomer(Stream input, string username, string applicationId, string externalSiteId)
        {
            string responseText = "";

            try
            {
                string password = Helper.GetPassword();
                if (password == null)
                {
                    responseText = Helper.ProcessError(Errors.MissingPassword);
                }
                else
                {
                    responseText = AndroCloudWCFServices.Services.Customer.Put(Helper.GetDataTypes(), input, username, password, externalSiteId, applicationId);
                }
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="applicationId"></param>
        /// <returns>A response indicating whether or not the customer was successfully added</returns>
        [WebInvoke(Method = "POST", UriTemplate = "customers/{username}?applicationId={applicationId}&newPassword={newPassword}")]
        public Stream PostCustomer(Stream input, string username, string applicationId, string newPassword)
        {
            string responseText = "";

            try
            {
                string password = Helper.GetPassword();
                if (password == null)
                {
                    responseText = Helper.ProcessError(Errors.MissingPassword);
                }
                else
                {
                    responseText = AndroCloudWCFServices.Services.Customer.Post(Helper.GetDataTypes(), input, username, password, applicationId, newPassword);
                }
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        /// <summary>
        /// Adds a new password reset request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="applicationId"></param>
        /// <returns>A response indicating whether or not the password reset request was successfully added</returns>
        [WebInvoke(Method = "PUT", UriTemplate = "customers/{username}/passwordresetrequest?applicationId={applicationId}")]
        public Stream PutPasswordResetRequest(string username, string applicationId)
        {
            string responseText = "";

            try
            {
                responseText = AndroCloudWCFServices.Services.PasswordResetRequest.Put(Helper.GetDataTypes(), username, applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        /// <summary>
        /// Updates an existing password reset request
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="token"></param>
        /// <returns>A response indicating whether or not the password reset request was successfully added</returns>
        [WebInvoke(Method = "POST", UriTemplate = "customers/{username}/passwordresetrequest?applicationId={applicationId}")]
        public Stream PostPasswordResetRequest(Stream input, string username, string applicationId)
        {
            string responseText = "";

            try
            {
                string newPassword = Helper.GetPassword();
                if (newPassword == null)
                {
                    responseText = Helper.ProcessError(Errors.MissingPassword);
                }
                else
                {
                    responseText = AndroCloudWCFServices.Services.PasswordResetRequest.Post(Helper.GetDataTypes(), input, username, newPassword, applicationId);
                }
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        [WebInvoke(Method = "POST", UriTemplate = "customers/{username}/customerloyalty?applicationId={applicationId}")]
        public Stream PostCustomerLoyalty(Stream input, string username, string applicationId)
        {
            string responseText = "";

            try
            {
                responseText = AndroCloudWCFServices.Services.Customer.PostCustomerLoyalty(Helper.GetDataTypes(), input, username, applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        ///http://localhost:49329/weborderapiv2/  customers/matthew.green@androtech.com/
        ///                                                            updateloyalty/Commit/
        ///                                                                                   17a5ecab7f5a4569b32ad95a73f576f0?
        ///                                                                                                      applicationId=47a40765-1e3f-42de-9ed1-b06eec55f4ee
        [WebInvoke(Method = "POST", UriTemplate = "customers/{username}/updateloyalty/{action}/{externalOrderRef}?applicationId={applicationId}")]
        public Stream PostApplyCustomerLoyalty(Stream input, string username, string action, string externalOrderRef, string applicationId)
        {
            string responseText = "";

            try
            {
                responseText = AndroCloudWCFServices.Services.Customer.PostApplyCustomerLoyalty(Helper.GetDataTypes(), input, username, externalOrderRef, action, applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        [WebGet(UriTemplate = "customers/ping")]
        public Stream Ping()
        {
            byte[] returnBytes = Encoding.UTF8.GetBytes("PING!!!");
            return new MemoryStream(returnBytes);
        }
    }
}