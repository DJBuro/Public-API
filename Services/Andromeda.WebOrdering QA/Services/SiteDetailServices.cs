using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;


namespace Andromeda.WebOrdering.Services
{
    public class SiteDetailServices
    {
        public static bool GetSiteDetails(string key, DomainConfiguration domainConfiguration, string siteId, out HttpStatusCode httpStatus, out string originalSiteDetailsJson, out string filteredSiteDetailsJson)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;
            originalSiteDetailsJson = "";
            filteredSiteDetailsJson = "";

            // Call the signpost server to get a list of ACS servers
            IEnumerable<string> serverUrls = null;
            success = HostServices.GetServerUrl(domainConfiguration, out httpStatus, out serverUrls);

            if (success)
            {
                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    string url = serverUrl + "/sites/" + siteId + "/details?applicationid=" + domainConfiguration.ApplicationId;

                    success = HttpHelper.RestCall("GET", url, "Application/JSON", "Application/JSON", null, "", true, out httpStatus, out originalSiteDetailsJson);

                    if (success) break;
                }
            }

            // We will take the original JSON and strip out the payment details for security reasons
            filteredSiteDetailsJson = originalSiteDetailsJson;

            if (success)
            {
                // Strip out the payment client id
                string paymentClientId = "\"paymentClientId\":\"";
                int startIndex = filteredSiteDetailsJson.IndexOf(paymentClientId);

                if (startIndex > -1)
                {
                    int endIndex = filteredSiteDetailsJson.IndexOf("\"", startIndex + paymentClientId.Length);
                    if (endIndex == -1) endIndex = filteredSiteDetailsJson.Length;

                    string before = filteredSiteDetailsJson.Substring(0, startIndex - 1);
                    string after = filteredSiteDetailsJson.Substring(endIndex + 1);
                    filteredSiteDetailsJson = before + after;
                }

                // Strip out the payment password
                string paymentClientPassword = "\"paymentClientPassword\":\"";
                startIndex = filteredSiteDetailsJson.IndexOf(paymentClientPassword);

                if (startIndex > -1)
                {
                    int endIndex = filteredSiteDetailsJson.IndexOf("\"", startIndex + paymentClientPassword.Length);
                    if (endIndex == -1) endIndex = filteredSiteDetailsJson.Length;

                    string before = filteredSiteDetailsJson.Substring(0, startIndex - 1);
                    string after = filteredSiteDetailsJson.Substring(endIndex + 1);
                    filteredSiteDetailsJson = before + after;
                }
            }

            return success;
        }
    }
}