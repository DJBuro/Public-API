using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using Andromeda.WebOrdering.Model;
using Andromeda.WebOrdering.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Andromeda.WebOrdering
{
    public class Helper
    {
        // Map host headers to application ids
        public static Dictionary<string, DomainConfiguration> DomainConfiguration { get; set; }

        // Static constructor
        static Helper()
        {
            // Host names must be uppercase so we can do case insensitive lookups
            Helper.DomainConfiguration = new Dictionary<string, DomainConfiguration>();
        }

        public static DomainConfiguration GetDomainConfiguration(string domainName)
        {
            // Get the host header
            //string hostHeader = "";

            // if domain is null - getting the host-header
            if (domainName == null)
            {
                // Not sure whether the host header name will always match case so do a case insensitive lookup just in case...
                foreach (string keyName in HttpContext.Current.Request.Headers.AllKeys)
                {
                    if (keyName.Equals("host", StringComparison.CurrentCultureIgnoreCase))
                    {
                        domainName = HttpContext.Current.Request.Headers[keyName];
                        if (!string.IsNullOrEmpty(domainName))
                        {
                            domainName = domainName.ToUpper().Replace("WWW.", "");
                        }
                        break;
                    }
                }
            }
            return Helper.GetDomainConfigurationForHost(domainName);
        }

        public static DomainConfiguration GetDomainConfigurationForHost(string hostHeader)
        {
            DomainConfiguration domainConfiguration = null;

            lock (Helper.DomainConfiguration)
            {
                Logger.Log.Debug("Getting mapping for " + hostHeader.ToUpper());

                // Look up the applicationid to use with this host header
                if (Helper.DomainConfiguration.TryGetValue(hostHeader.ToUpper(), out domainConfiguration))
                {
                    domainConfiguration.HostHeader = hostHeader;
                }
                else
                {
                    Logger.Log.Info("No mapping for " + hostHeader + " using default");

                    // No mapping for the host header.  Use default mapping
                    string defaultMapping = ConfigurationManager.AppSettings["DefaultMapping"];

                    if (String.IsNullOrEmpty(defaultMapping))
                    {
                        Logger.Log.Error("No default mapping in web.config");
                    }
                    // Get the default mapping - let it throw an exception if the default mapping doesn't exist
                    else if (Helper.DomainConfiguration.TryGetValue(defaultMapping.ToUpper(), out domainConfiguration))
                    {
                        domainConfiguration.HostHeader = defaultMapping;
                    }
                    else
                    {
                        Logger.Log.Error("Unable to find default mapping for " + defaultMapping);
                    }
                }

                // Did we find a mapping?
                if (domainConfiguration == null)
                {
                    Logger.Log.Error("Unable to find mapping for " + hostHeader);
                }
                else
                {
                    string signpostServersText = "";
                    foreach (string signPostServer in domainConfiguration.SignpostServers)
                    {
                        signpostServersText += signPostServer;
                    }

                    Logger.Log.Debug("Using appid: " + domainConfiguration.ApplicationId + ", signpost servers: " + signpostServersText);
                }
            }

            return domainConfiguration;
        }

        public static string GetHostHeader()
        {
            // Get the host header
            string hostHeader = "";

            // Not sure whether the host header name will always match case so do a case insensitive lookup just in case...
            foreach (string keyName in HttpContext.Current.Request.Headers.AllKeys)
            {
                if (keyName.Equals("host", StringComparison.CurrentCultureIgnoreCase))
                {
                    hostHeader = HttpContext.Current.Request.Headers[keyName];
                    int firstDotIndex = hostHeader.IndexOf('.');
                    if (firstDotIndex > -1)
                    {
                        hostHeader = hostHeader.Substring(0, firstDotIndex);
                    }
                    break;
                }
            }

            return hostHeader;
        }

        public static MemoryStream StringToStream(string text)
        {
            byte[] returnBytes = Encoding.UTF8.GetBytes(text);
            return new MemoryStream(returnBytes);
        }

        public static string StreamToString(Stream input)
        {
            string output = "";

            using (StreamReader streamReader = new StreamReader(input))
            {
                if (streamReader != null)
                {
                    output = streamReader.ReadToEnd();
                }
            }

            return output;
        }

        public static bool CheckOrderResult(ref string json, out bool isProvisional, out Model.ACSError error)
        {
            error = null;
            isProvisional = false;

            JObject jObject = JObject.Parse(json);

            // Was there an error?
            if (jObject["status"] != null && ((string)jObject["status"]).Length > 0 && (string)jObject["status"] != "1")
            {
                // Return the error code
                error = new ACSError() { ErrorCode = (string)jObject["status"] };
                json = "{ \"errorCode\" : \"" + (string)jObject["status"] + "\" }";

                return false;
            }
            else if (jObject["errorCode"] != null && ((string)jObject["errorCode"]).Length > 0)
            {
                // Return the error code
                error = new ACSError() { ErrorCode = (string)jObject["errorCode"] };
                json = "{ \"errorCode\" : \"" + (string)jObject["errorCode"] + "\" }";

                return false;
            }
            else
            {
                isProvisional = jObject["isProvisional"] == null ? false : (bool)jObject["isProvisional"];
            }

            return true;
        }

        public static bool CheckForError(ref string json, out Model.ACSError error)
        {
            error = null;

            // If it's empty or an array it's not an error
            if (json == null || json.Length == 0 || json.StartsWith("["))
            {
                return true;
            }

            JObject jObject = JObject.Parse(json);

            // Was there an error?
            if (jObject["status"] != null && ((string)jObject["status"]).Length > 0 && (string)jObject["status"] != "1")
            {
                // Return the error code
                error = new ACSError() { ErrorCode = (string)jObject["status"] };
                json = "{ \"errorCode\" : \"" + jObject["status"] + "\" }";

                return false;
            }
            else if (jObject["errorCode"] != null && ((string)jObject["errorCode"]).Length > 0)
            {
                // Return the error code
                error = new ACSError() { ErrorCode = (string)jObject["errorCode"] };
                json = "{ \"errorCode\" : \"" + (string)jObject["errorCode"] + "\" }";

                return false;
            }

            return true;
        }

        public static string GenerateOrderNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);

            DateTime dateTimeNow = DateTime.UtcNow;
            string orderNumber =
                "W" +
                dateTimeNow.Ticks.ToString() +
                randomNumber.ToString();

            return orderNumber;
        }

        public static Stream InternalError(Exception exception)
        {
            Logger.Log.Error("Unhandled exception", exception);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            return Helper.StringToStream("{ \"errorCode\": \"1\" }");
        }

        public static Stream InternalErrorNoLog(Exception exception)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            return Helper.StringToStream("{ \"errorCode\": \"1\" }");
        }

        public static string GetClientIPAddressPortString()
        {
            string result = "";
            if (OperationContext.Current != null)
            {
                MessageProperties messageProperties = OperationContext.Current.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpointProperty = (RemoteEndpointMessageProperty)messageProperties[RemoteEndpointMessageProperty.Name];

                result = endpointProperty.Address;
            }

            return result;
        }

        public static bool GetPaymentProviderDetails(string key, DomainConfiguration domainConfiguration, string siteId, OrderDetails orderDetails)
        {
            Logger.Log.Debug("GetPaymentProviderDetails");

            bool success = false;
            string originalSiteDetailsJson = "";
            string filteredSiteDetailsJson = "";
            HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;

            // Get the site details from ACS
            success = SiteDetailServices.GetSiteDetails(key, domainConfiguration, siteId, out httpStatus, out originalSiteDetailsJson, out filteredSiteDetailsJson);

            if (success)
            {
                // Parse the response json
                JObject jObject = JObject.Parse(originalSiteDetailsJson);

                JToken errorJToken = jObject["errorCode"];
                if (errorJToken != null)
                {
                    // There was a problem
                    Logger.Log.Error("GetPaymentProviderDetails GetSiteDetails returned an error.  Json: " + originalSiteDetailsJson);

                    success = false;
                }
                else
                {
                    // Is the site still online?
                    orderDetails.IsOnline = (bool)jObject["isOpen"];

                    // Get the payment provider details
                    orderDetails.PaymentProviderName = (string)jObject["paymentProvider"];
                    orderDetails.MerchantId = (string)jObject["paymentClientId"];
                    orderDetails.Password = (string)jObject["paymentClientPassword"];

                    success = true;
                }
            }

            return success;
        }
    }
}