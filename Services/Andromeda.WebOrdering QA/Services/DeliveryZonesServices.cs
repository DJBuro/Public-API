using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;


namespace Andromeda.WebOrdering.Services
{
    public class DeliveryZonesServices
    {
        public static bool GetDeliveryZones(string key, DomainConfiguration domainConfiguration, string siteId, out HttpStatusCode httpStatus, out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;
            json = "";

            // Call the signpost server to get a list of ACS servers
            IEnumerable<string> serverUrls = null;
            success = HostServices.GetServerUrl(domainConfiguration, out httpStatus, out serverUrls);

            if (success)
            {
                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    string url = serverUrl + "/sites/" + siteId + "/deliveryzones?applicationid=" + domainConfiguration.ApplicationId;

                    success = HttpHelper.RestCall("GET", url, "Application/JSON", "Application/JSON", null, "", true, out httpStatus, out json);

                    if (success) break;
                }
            }

            return success;
        }
    }
}