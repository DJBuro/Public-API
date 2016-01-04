using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace Andromeda.WebOrdering.Services
{
    public class MenuServices
    {
        public static bool GetSiteMenu(string key, DomainConfiguration domainConfiguration, string siteId, out HttpStatusCode httpStatus, out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;

            // Call the signpost server to get a list of ACS servers
            IEnumerable<string> serverUrls = null;
            success = HostServices.GetServerUrl(domainConfiguration, out httpStatus, out serverUrls);

            json = "";
            if (success)
            {
                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    string url = serverUrl + "/sites/" + siteId + "/menu?applicationid=" + domainConfiguration.ApplicationId;

                    // Do we need to log the call data?
                    bool log = true;
                    if (!bool.TryParse(ConfigurationManager.AppSettings["LogMenuCalls"], out log))
                    {
                        Logger.Log.Error("LogMenuCalls setting missing or invalid");
                        log = true; 
                    }

                    success = HttpHelper.RestCall("GET", url, "Application/JSON", "Application/JSON", null, "", log, out httpStatus, out json);

                    if (success) break;
                }
            }

            return success;
        }

        public static bool GetSiteMenuWithImages(DomainConfiguration domainConfiguration, string siteId, out HttpStatusCode httpStatus, out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;

            // Call the signpost server to get a list of ACS servers
            IEnumerable<string> serverUrls = null;
            success = HostServices.GetServerUrl(domainConfiguration, out httpStatus, out serverUrls);

            json = "";
            if (success)
            {
                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    string url = serverUrl + "/sites/" + siteId + "?applicationid=" + domainConfiguration.ApplicationId;

                    // Do we need to log the call data?
                    bool log = true;
                    if (!bool.TryParse(ConfigurationManager.AppSettings["LogMenuCalls"], out log))
                    {
                        Logger.Log.Error("LogMenuCalls setting missing or invalid");
                        log = true;
                    }

                    success = HttpHelper.RestCall("GET", url, "Application/JSON", "Application/JSON", null, "", log, out httpStatus, out json);

                    if (success) break;
                }
            }

            return success;
        }
    }
}