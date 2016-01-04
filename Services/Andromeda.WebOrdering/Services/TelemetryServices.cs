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
    public class TelemetryServices
    {
        public static bool PutSession
        (
            string siteId,
            string key, 
            DomainConfiguration domainConfiguration, 
            string telemetrySessionJson, 
            out HttpStatusCode httpStatus, 
            out string json
        )
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
                    string url = serverUrl + "/sites/" + siteId + "/telemetrySession?applicationid=" + domainConfiguration.ApplicationId;

                    success = HttpHelper.RestCall
                    (
                        "Put", 
                        url, 
                        "Application/JSON", 
                        "Application/JSON",
                        null,
                        telemetrySessionJson, 
                        true, 
                        out httpStatus,
                        out json
                    );

                    if (success) break;
                }

                if (!success) httpStatus = HttpStatusCode.InternalServerError;
            }

            return success;
        }

        public static bool Put
        (
            string siteId,
            string key,
            DomainConfiguration domainConfiguration,
            string telemetryJson,
            out HttpStatusCode httpStatus,
            out string json
        )
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
                    string url = serverUrl + "/sites/" + siteId + "/telemetry?applicationid=" + domainConfiguration.ApplicationId;

                    success = HttpHelper.RestCall
                    (
                        "Put",
                        url,
                        "Application/JSON",
                        "Application/JSON",
                        null,
                        telemetryJson,
                        true,
                        out httpStatus,
                        out json
                    );

                    if (success) break;
                }

                if (!success) httpStatus = HttpStatusCode.InternalServerError;
            }

            return success;
        }
    }
}