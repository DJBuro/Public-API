using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Andromeda.WebOrdering.Services
{
    public class DeliveryRoadsServices
    {
        internal static bool GetDeliveryRoads(string key, DomainConfiguration domainConfiguration, string postcode, out System.Net.HttpStatusCode httpStatus, out string json)
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
                    string url = serverUrl + "/deliveryroads/" + postcode + "?applicationid=" + domainConfiguration.ApplicationId;

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