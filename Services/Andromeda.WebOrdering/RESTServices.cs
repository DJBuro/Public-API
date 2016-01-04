using System.ServiceModel.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using Andromeda.WebOrdering.Services;
using System.Net;
using System.Configuration;
using Andromeda.WebOrdering.Model;

namespace Andromeda.WebOrdering
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class RESTServices : IRESTServices
    {
        [WebInvoke(Method = "GET", UriTemplate = "TEST")]
        public void TEST()
        {
            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                }
                else
                {
                    Andromeda.WebOrdering.PaymentProviders.MercanetPaymentProvider.SendAlertEmail
                    (
                        "MercanetPaymentAlertEmail",
                        domainConfiguration,
                        new MercanetResponse()
                        {
                            Amount = "99,99",
                            OrderId = "999",
                            PaymentDate = "2013/01/01",
                            PaymentTime = "11:00"
                        },
                        "{ order json }",
                        ""
                    );

                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("GET TEST", exception);

                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
        }

        [WebInvoke(Method = "GET", UriTemplate = "SiteList?deliveryZone={deliveryZoneFilter}&key={key}")]
        public Stream SiteList(string deliveryZoneFilter, string key)
        {
            Stream response = null;
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the site list from ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.SiteListServices.GetSiteList(deliveryZoneFilter, key, domainConfiguration, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "GET SiteList?deliveryZone=" + deliveryZoneFilter +
                    " SourceIP:" + sourceIPAddress + 
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() + 
                    " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "GET", UriTemplate = "SiteDetails/{siteId}?key={key}")]
        public Stream SiteDetails(string siteId, string key)
        {
            Stream response = null;
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the site details from ACS
                    string filteredJson = "";
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.SiteDetailServices.GetSiteDetails(key, domainConfiguration, siteId, out httpStatus, out responseJson, out filteredJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(filteredJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "GET SiteDetails/" + siteId +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "GET", UriTemplate = "Menu/{siteId}?key={key}")]
        public Stream Menu(string siteId, string key)
        {
            Stream response = null;
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the menu from ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.MenuServices.GetSiteMenu(key, domainConfiguration, siteId, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                // Do we need to log the call data?
                bool log = true;
                if (!bool.TryParse(ConfigurationManager.AppSettings["LogSiteCalls"], out log))
                {
                    Logger.Log.Error("LogMenuCalls setting missing or invalid");
                    log = true;
                }

                Logger.Log.Debug(
                    "GET Menu/" + siteId +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Response:" + (log ? responseJson : "Logging disabled"));
            }

            return response;
        }

        /// <summary>
        /// Initialises a Mercury payment.  This must be done before displaying the Mercury payment web page
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="siteId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(Method = "PUT", UriTemplate = "MercuryPayment/{siteId}?key={key}")]
        public Stream MercuryPayment(Stream stream, string siteId, string key)
        {
            Stream response = null;
            string paymentJson = "";
            string json = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the payment from the caller
                    paymentJson = Helper.StreamToString(stream);

                    // Setup the payment with mercury
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.PaymentProviders.MercuryPaymentProvider.PutMercuryPayment(key, domainConfiguration, siteId, paymentJson, out httpStatus, out json);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(json);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                json = "Exception";
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "PUT MercuryPayment/" + siteId +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Request:" + paymentJson +
                    " Response:" + json);
            }

            return response;
        }

        /// <summary>
        /// Initialises a Datacash payment.  This must be done before displaying the Datacash payment web page
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="siteId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(Method = "PUT", UriTemplate = "DataCashPayment/{siteId}?key={key}")]
        public Stream DataCashPayment(Stream stream, string siteId, string key)
        {
            Stream response = null;
            string orderJson = "";
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the payment from the caller
                    orderJson = Helper.StreamToString(stream);

                    // Setup the payment with mercury
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.PaymentProviders.DataCashPaymentProvider.PutDataCashPayment
                    (
                        key, 
                        domainConfiguration, 
                        siteId, 
                        orderJson, 
                        out httpStatus, 
                        out responseJson
                    );
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "PUT DataCashPayment/" + siteId +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Request:" + orderJson +
                    " Response:" + responseJson);
            }

            return response;
        }

        /// <summary>
        /// Initialises a Mercanet payment.  This must be done before displaying the Mercanet payment web page
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="siteId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(Method = "PUT", UriTemplate = "MercanetPayment/{siteId}?key={key}")]
        public Stream MercanetPayment(Stream stream, string siteId, string key)
        {
            Stream response = null;
            string paymentJson = "";
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the payment from the caller
                    paymentJson = Helper.StreamToString(stream);

                    // Setup the payment with Mercanet
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.PaymentProviders.MercanetPaymentProvider.PutMercanetPayment(key, domainConfiguration, siteId, paymentJson, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning HTML
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "PUT MercanetPayment/" + siteId +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Request:" + paymentJson +
                    " Response:" + responseJson);
            }

            return response;
        }

        /// <summary>
        /// Sends an order to a store.  If there is a pending payment then it is checked and completed and the payment details injected into the order
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="siteId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(Method = "PUT", UriTemplate = "Order/{siteId}?key={key}")]
        public Stream Order(Stream stream, string siteId, string key)
        {
            Stream response = null;
            string responseJson = "";
            string orderJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the order from the caller
                    orderJson = Helper.StreamToString(stream);

                    // Send the order to ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.OrderServices.PutOrder(key, domainConfiguration, siteId, orderJson, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "PUT Order/" + siteId +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Request:" + orderJson +
                    " Response:" + responseJson);
            }

            return response;
        }

        /// <summary>
        /// Checks the vouchers in an order.  Does NOT send the order to a store
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="siteId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebInvoke(Method = "PUT", UriTemplate = "OrderVouchers/{siteId}?key={key}")]
        public Stream CheckOrderVouchers(Stream stream, string siteId, string key)
        {
            Stream response = null;
            string responseJson = "";
            string orderJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the order from the caller
                    orderJson = Helper.StreamToString(stream);

                    // Send the order to ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.OrderServices.CheckOrderVouchers(key, domainConfiguration, siteId, orderJson, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "PUT OrderVouchers/" + siteId + "/CheckVouchers" +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Request:" + orderJson +
                    " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "GET", UriTemplate = "DeliveryZones/{siteId}?key={key}")]
        public Stream DeliveryZones(string siteId, string key)
        {
            Stream response = null;
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the site delivery zones from ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    Andromeda.WebOrdering.Services.DeliveryZonesServices.GetDeliveryZones(key, domainConfiguration, siteId, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "GET DeliveryZones/" + siteId +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "GET", UriTemplate = "Customers/{username}?key={key}&siteId={siteId}")]
        public Stream GetCustomers(string username, string siteId, string key)
        {
            Stream response = null;
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the password from the HTTP headers
                    string passwordHeader = (string)WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

                    // Get the customer from ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.CustomerServices.Get
                    (
                        key,
                        domainConfiguration,
                        username,
                        siteId,
                        passwordHeader,
                        out httpStatus,
                        out responseJson
                    );
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "GET Customers/" + username +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "PUT", UriTemplate = "Customers/{username}?key={key}&siteId={siteId}")]
        public Stream PutCustomers(Stream stream, string username, string siteId, string key)
        {
            Stream response = null;
            string responseJson = "";
            string customerJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the password from the HTTP headers
                    string passwordHeader = (string)WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

                    // Get the customer from the caller
                    customerJson = Helper.StreamToString(stream);

                    // Send the customer to ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.CustomerServices.Put
                    (
                        key,
                        domainConfiguration,
                        username,
                        siteId,
                        passwordHeader,
                        customerJson,
                        out httpStatus,
                        out responseJson
                    );
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "PUT Customers/" + username +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "POST", UriTemplate = "Customers/{username}?key={key}&newPassword={newPassword}")]
        public Stream PostCustomers(Stream stream, string username, string key, string newPassword)
        {
            Stream response = null;
            string responseJson = "";
            string customerJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the password from the HTTP headers
                    string passwordHeader = (string)WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

                    // Get the customer from the caller
                    customerJson = Helper.StreamToString(stream);

                    // Send the customer to ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.CustomerServices.Post
                    (
                        key,
                        domainConfiguration,
                        username,
                        false,
                        null,
                        passwordHeader,
                        newPassword,
                        customerJson,
                        out httpStatus,
                        out responseJson
                    );
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "POST Customers/" + username +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Request:" + customerJson +
                    " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "PUT", UriTemplate = "Customers/{username}/passwordresetrequest?key={key}")]
        public Stream PutPasswordResetRequest(Stream stream, string username, string key)
        {
            Stream response = null;
            string responseJson = "";
            string customerJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the password from the HTTP headers
                    string passwordHeader = (string)WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

                    // Get the customer from the caller
                    customerJson = Helper.StreamToString(stream);

                    // Send the customer to ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.CustomerServices.Post
                    (
                        key,
                        domainConfiguration,
                        username,
                        true,
                        null,
                        passwordHeader,
                        null,
                        customerJson,
                        out httpStatus,
                        out responseJson
                    );
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "PUT Customers/" + username + "/passwordresetrequest" +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Request:" + customerJson +
                    " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "POST", UriTemplate = "Customers/{username}/passwordresetrequest?key={key}&passwordResetToken={passwordResetToken}")]
        public Stream PostPasswordResetRequest(Stream stream, string username, string key, string passwordResetToken)
        {
            Stream response = null;
            string responseJson = "";
            string customerJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the password from the HTTP headers
                    string passwordHeader = (string)WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

                    // Get the customer from the caller
                    customerJson = Helper.StreamToString(stream);

                    // Send the customer to ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.CustomerServices.Post
                    (
                        key,
                        domainConfiguration,
                        username,
                        true,
                        passwordResetToken,
                        null,
                        passwordHeader,
                        customerJson,
                        out httpStatus,
                        out responseJson
                    );
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "POST Customers/" + username + "/passwordresetrequest&passwordResetToken=" + passwordResetToken +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Request:" + customerJson +
                    " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "POST", UriTemplate = "MercanetCallback")]
        public Stream MercanetCallback(Stream stream)
        {
            Stream response = null;
            string merchanetResponse = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the response from the caller
                merchanetResponse = Helper.StreamToString(stream);

                // Send the response to ACS
                bool success = Andromeda.WebOrdering.PaymentProviders.MercanetPaymentProvider.MerchanetCallback(merchanetResponse);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                merchanetResponse = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                   "POST MerchanetCallback" +
                   " SourceIP:" + sourceIPAddress +
                   " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                   " Response:" + merchanetResponse);
            }

            return response;
        }

        [WebInvoke(Method = "GET", UriTemplate = "Sites/{siteId}?key={key}&gotMenuVersion={gotMenuVersion}&statusCheck={statusCheck}")]
        public Stream GetSite(string siteId, string key, int gotMenuVersion, string statusCheck)
        {
            Stream responseJson = null;
            string originalResponseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    responseJson = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the site details from ACS
                    bool? statusCheckBool = statusCheck == null ? (bool?)null : statusCheck.ToUpper() == "TRUE" ? true : false;
                    string filteredResponseJson = "";
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.SiteServices.GetSite(key, domainConfiguration, siteId, gotMenuVersion, statusCheckBool, out httpStatus, out originalResponseJson, out filteredResponseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    responseJson = Helper.StringToStream(filteredResponseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                originalResponseJson = "Exception";
                responseJson = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                // Do we need to log the call data?
                bool log = true;
                if (!bool.TryParse(ConfigurationManager.AppSettings["LogSiteCalls"], out log))
                {
                    Logger.Log.Error("LogMenuCalls setting missing or invalid");
                    log = true;
                }

                Logger.Log.Debug(
                    "GET Sites/" + siteId + "&gotMenuVersion=" + gotMenuVersion.ToString() + "&statusCheck=" + statusCheck +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Response:" + (log ? originalResponseJson : "Logging disabled"));
            }

            return responseJson;
        }

        [WebInvoke(Method = "GET", UriTemplate = "DeliveryTowns?key={key}")]
        public Stream GetDeliveryTowns(string key)
        {
            Stream response = null;
            string sourceIPAddress = Helper.GetClientIPAddressPortString();
            string responseJson = "";

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the site details from ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.DeliveryTownsServices.GetDeliveryTowns(key, domainConfiguration, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "GET DeliveryTowns" +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "GET", UriTemplate = "DeliveryRoads/{postcode}?key={key}")]
        public Stream GetDeliveryRoads(string postcode, string key)
        {
            Stream response = null;
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the site details from ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.DeliveryRoadsServices.GetDeliveryRoads(key, domainConfiguration, postcode, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                    "GET DeliveryRoads/" + postcode +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "GET", UriTemplate = "customers/{username}/orders?key={key}")]
        public Stream GetCustomerOrders(string username, string key)
        {
            Stream response = null;
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the password from the HTTP headers
                    string passwordHeader = (string)WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

                    // Get the site details from ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.CustomerOrders.GetAll(key, domainConfiguration, username, passwordHeader, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                // Do we need to log the call data?
                bool log = true;
                if (!bool.TryParse(ConfigurationManager.AppSettings["LogSiteCalls"], out log))
                {
                    Logger.Log.Error("LogMenuCalls setting missing or invalid");
                    log = true;
                }

                Logger.Log.Debug(
                    "GET customers/" + username + "/orders" +
                    " SourceIP:" + sourceIPAddress +
                    " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                    " Response:" + (log ? responseJson : "Logging disabled"));
            }

            return response;
        }

        [WebInvoke(Method = "GET", UriTemplate = "customers/{username}/orders/{orderId}?key={key}")]
        public Stream GetCustomerOrder(string username, string orderId, string key)
        {
            Stream response = null;
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the password from the HTTP headers
                    string passwordHeader = (string)WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

                    // Get the site details from ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.CustomerOrders.GetByOrderId(key, domainConfiguration, orderId, username, passwordHeader, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                // Do we need to log the call data?
                bool log = true;
                if (!bool.TryParse(ConfigurationManager.AppSettings["LogSiteCalls"], out log))
                {
                    Logger.Log.Error("LogMenuCalls setting missing or invalid");
                    log = true;
                }
                
                Logger.Log.Debug(
                        "GET customers/" + username + "/orders/" + orderId +
                        " SourceIP:" + sourceIPAddress +
                        " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                        " Response:" + (log ? responseJson : "Logging disabled"));
            }

            return response;
        }

        [WebInvoke(Method = "PUT", UriTemplate = "PayPalPayment/{siteId}?key={key}")]
        public Stream PayPalPayment(Stream stream, string siteId, string key)
        {
            Stream response = null;
            string paymentJson = "";
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the payment from the caller
                    paymentJson = Helper.StreamToString(stream);

                    // Setup the payment with PayPal
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.PaymentProviders.PayPalPaymentProvider.PutPayPalPayment(key, domainConfiguration, siteId, paymentJson, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning HTML
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                        "PUT PayPalPayment/" + siteId +
                        " SourceIP:" + sourceIPAddress +
                        " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                        " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "POST", UriTemplate = "PayPalCallback/{orderId}?key={key}")]
        public Stream PayPalCallback(Stream stream, string orderId, string key)
        {
            Stream response = null;
            string requestJson = "";
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the url that the customer was redirected to
                    requestJson = Helper.StreamToString(stream);

                    // Setup the payment with PayPal
                    bool success = Andromeda.WebOrdering.PaymentProviders.PayPalPaymentProvider.PayPalCallback(domainConfiguration, orderId, requestJson, out responseJson);
                    
                    if (success)
                    {
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    }

                    // Let the caller know we're returning HTML
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                        "POST PayPalCallback/" + orderId +
                        " SourceIP:" + sourceIPAddress +
                        " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                        " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "POST", UriTemplate = "WebSites?key={key}")]
        public Stream AddUpdateWebSite(Stream stream, string key)
        {
            Stream response = null;
            string requestJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();
            WebSiteServicesData webSiteServicesData = null;

            try
            {
                // Get the url that the customer was redirected to
                requestJson = Helper.StreamToString(stream);

                // Send the order to ACS
                webSiteServicesData = new WebSiteServicesData() { Key = key, RequestJson = requestJson };
                string errorCode;
                bool success = Andromeda.WebOrdering.Services.WebSiteServices.AddUpdateWebSite(webSiteServicesData, out errorCode);

                if (!string.IsNullOrEmpty(errorCode) && errorCode == ((int)HttpStatusCode.Conflict).ToString())
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                    response = Helper.StringToStream(HttpStatusCode.Conflict.ToString() + "- Another publish is in progress");
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = webSiteServicesData.HttpStatusCode;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";

                    response = Helper.StringToStream(webSiteServicesData.ResultJson);

                    if (webSiteServicesData.HttpStatusCode != HttpStatusCode.Accepted &&
                        webSiteServicesData.HttpStatusCode != HttpStatusCode.Created)
                    {
                        Logger.Log.Error(webSiteServicesData.ResultJson);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response = Helper.StringToStream(exception.Message);
            }
            finally
            {
                Logger.Log.Debug(
                        "POST WebSites" +
                        " SourceIP:" + sourceIPAddress +
                        " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                        " Request:" + requestJson.Replace("\r\n", "") +
                        " Response:" + (webSiteServicesData != null && webSiteServicesData.ResultJson != null ? webSiteServicesData.ResultJson : ""));
            }

            return response;
        }

        [WebInvoke(Method = "PUT", UriTemplate = "Feedback?key={key}")]
        public Stream Feedback(Stream stream, string key)
        {
            return this.SiteFeedback(stream, "", key);
        }

        [WebInvoke(Method = "PUT", UriTemplate = "Feedback/{siteId}?key={key}")]
        public Stream SiteFeedback(Stream stream, string siteId, string key)
        {
            Stream response = null;
            string feedbackJson = "";
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the feedback from the caller
                    feedbackJson = Helper.StreamToString(stream);

                    // Send the Feedback to ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.FeedbackServices.Put(siteId, key, domainConfiguration, feedbackJson, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                        "PUT Feedback/" + siteId +
                        " SourceIP:" + sourceIPAddress +
                        " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                        " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "PUT", UriTemplate = "TelemetrySession/{siteId}?key={key}")]
        public Stream TelemetrySession(Stream stream, string siteId, string key)
        {
            Stream response = null;
            string telemetrySessionJson = "";
            string responseJson = "";
            string sourceIPAddress = Helper.GetClientIPAddressPortString();

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the payment from the caller
                    telemetrySessionJson = Helper.StreamToString(stream);

                    // Send the Telemtry session to ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.TelemetryServices.PutSession(siteId, key, domainConfiguration, telemetrySessionJson, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning JSON
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            finally
            {
                Logger.Log.Debug(
                        "PUT TelemetrySession/" + siteId +
                        " SourceIP:" + sourceIPAddress +
                        " Status: " + WebOperationContext.Current.OutgoingResponse.StatusCode.ToString() +
                        " Response:" + responseJson);
            }

            return response;
        }

        [WebInvoke(Method = "PUT", UriTemplate = "Telemetry/{siteId}?key={key}")]
        public Stream Telemetry(Stream stream, string siteId, string key)
        {
            Stream response = null;
            string telemetryJson = "";
            string responseJson = "";

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(null);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the telemetry data from the caller
                    telemetryJson = Helper.StreamToString(stream);

                    // Send the Telemetry to ACS
                    HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
                    bool success = Andromeda.WebOrdering.Services.TelemetryServices.Put(siteId, key, domainConfiguration, telemetryJson, out httpStatus, out responseJson);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = httpStatus;

                    // Let the caller know we're returning HTML
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}