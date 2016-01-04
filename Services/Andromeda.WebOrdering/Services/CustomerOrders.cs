using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Andromeda.WebOrdering.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Andromeda.WebOrdering.Services
{
    public class CustomerOrders
    {
        public static bool GetAll(string key, DomainConfiguration domainConfiguration, string username, string passwordHeader, out HttpStatusCode httpStatus, out string json)
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
                    string url = serverUrl + "/customers/" + username + "/orders?applicationid=" + domainConfiguration.ApplicationId;

                    success = HttpHelper.RestCall
                    (
                        "GET", 
                        url, 
                        "Application/JSON", 
                        "Application/JSON",
                        new Dictionary<string, string>() { {"Authorization", passwordHeader} }, 
                        "", 
                        true, 
                        out httpStatus,
                        out json
                    );

                    if (success) break;
                }
            }

            return success;
        }

        public static bool GetByOrderId(string key, DomainConfiguration domainConfiguration, string orderId, string username, string passwordHeader, out HttpStatusCode httpStatus, out string json)
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
                    string url = serverUrl + "/customers/" + username + "/orders/" + orderId + "?applicationid=" + domainConfiguration.ApplicationId;

                    success = HttpHelper.RestCall
                    (
                        "GET",
                        url,
                        "Application/JSON",
                        "Application/JSON",
                        new Dictionary<string, string>() { { "Authorization", passwordHeader } },
                        "",
                        true,
                        out httpStatus,
                        out json
                    );

                    if (success) break;
                }
            }

            return success;
        }
    }
}