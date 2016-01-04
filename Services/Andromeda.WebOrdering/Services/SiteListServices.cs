using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace Andromeda.WebOrdering.Services
{
    public class SiteListServices
    {
        /// <summary>
        /// Gets a list of sites that the application is allowed to access
        /// </summary>
        /// <param name="key"></param>
        /// <param name="domainConfiguration">Various configuration data</param>
        /// <param name="json">A JSON list of sites that the application is allowed to access</param>
        /// <returns>True when the call was successful</returns>
        public static bool GetSiteList(string deliveryZoneFilter, string key, DomainConfiguration domainConfiguration, out HttpStatusCode httpStatus, out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;
            json = "";

            // Call the signpost server to locate an ACS server we can use
            IEnumerable<string> serverUrls = null;
            success = HostServices.GetServerUrl(domainConfiguration, out httpStatus, out serverUrls);
            
            if (success)
            {
                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    // Use the ACS server url returned by the signpost server to build the ACS API call
                    string url = serverUrl + "/sites?deliveryZone=" + deliveryZoneFilter + "&applicationid=" + domainConfiguration.ApplicationId;

                    // Make the API call
                    success = HttpHelper.RestCall("GET", url, "Application/JSON", "Application/JSON", null, "", true, out httpStatus, out json);

                    // Do we need to try another ACS server?
                    if (success) break;
                }
            }

            return success;
        }
    }
}