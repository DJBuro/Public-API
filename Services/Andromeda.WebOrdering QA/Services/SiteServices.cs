using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;


namespace Andromeda.WebOrdering.Services
{
    public class SiteServices
    {
        public static bool GetSite(string key, DomainConfiguration domainConfiguration, string siteId, int gotMenuVersion, bool? statusCheck, out HttpStatusCode httpStatus, out string originalJson, out string filteredJson)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;
            originalJson = "";
            filteredJson = "";

            // Call the signpost server to get a list of ACS servers
            IEnumerable<string> serverUrls = null;
            success = HostServices.GetServerUrl(domainConfiguration, out httpStatus, out serverUrls);

            if (success)
            {
                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    string url = serverUrl + "/sites/" + siteId + 
                        "?applicationid=" + domainConfiguration.ApplicationId + 
                        "&gotMenuVersion=" + gotMenuVersion.ToString() +
                        (statusCheck.HasValue ? "&statusCheck=" + statusCheck.ToString() : "");

                    // Do we need to log the call data?
                    bool log = true;
                    if (!bool.TryParse(ConfigurationManager.AppSettings["LogSiteCalls"], out log))
                    {
                        Global.Log.Error("LogMenuCalls setting missing or invalid");
                        log = true;
                    }

                    success = HttpHelper.RestCall("GET", url, "Application/JSON", "Application/JSON", null, "", log, out httpStatus, out originalJson);

                    if (success) break;
                }
            }

            // We will take the original JSON and strip out the payment details for security reasons
            filteredJson = originalJson;

            if (success)
            {
                // Strip out the payment client id
                string paymentClientId = "\"paymentClientId\":\"";
                int startIndex = filteredJson.IndexOf(paymentClientId);

                if (startIndex > -1)
                {
                    int endIndex = filteredJson.IndexOf("\"", startIndex + paymentClientId.Length);
                    if (endIndex == -1) endIndex = filteredJson.Length;

                    string before = filteredJson.Substring(0, startIndex - 1);
                    string after = filteredJson.Substring(endIndex + 1);
                    filteredJson = before + after;
                }

                // Strip out the payment password
                string paymentClientPassword = "\"paymentClientPassword\":\"";
                startIndex = filteredJson.IndexOf(paymentClientPassword);

                if (startIndex > -1)
                {
                    int endIndex = filteredJson.IndexOf("\"", startIndex + paymentClientPassword.Length);
                    if (endIndex == -1) endIndex = filteredJson.Length;

                    string before = filteredJson.Substring(0, startIndex - 1);
                    string after = filteredJson.Substring(endIndex + 1);
                    filteredJson = before + after;
                }
            }

            return success;
        }
    }
}