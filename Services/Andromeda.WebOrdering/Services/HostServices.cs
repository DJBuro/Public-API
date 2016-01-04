using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace Andromeda.WebOrdering.Services
{
    public class HostServices
    {
        /// <summary>
        /// Gets the url of an ACS server
        /// </summary>
        /// <param name="domainConfiguration">Configuration data</param>
        /// <param name="hostUrls">The url of an ACS server</param>
        /// <returns>True when successful</returns>
        public static bool GetServerUrl(DomainConfiguration domainConfiguration, out HttpStatusCode httpStatus, out IEnumerable<string> hostUrls)
        {
            //<Hosts>
            //    <Host>
            //        <Url>http://ahost.com/apiservices<Url>
            //        <Order>1<Order>
            //    </Host>
            //    <Host>
            //        <Url>http://anotherhost.com/apiservices/v1<Url>
            //        <Order>2<Order>
            //    </Host>
            //</Hosts>

            // We don't have any ACS server urls yet
            bool success = false;
            hostUrls = new List<string>();
            httpStatus = HttpStatusCode.InternalServerError;

            foreach (string signpostServer in domainConfiguration.SignpostServers)
            {
                // Build the url of the signpost server that we can use to get the ACS server url
                string url = signpostServer + "?applicationid=" + domainConfiguration.ApplicationId;

                // Call the signpost server to get a list of available ACS servers
                string xml = "";
                success = HttpHelper.RestCall("GET", url, "Application/XML", "Application/XML", null, "", true, out httpStatus, out xml);

                XDocument xmlDocument = null;
                if (success)
                {
                    // Parse the XML returned by the signpost server
                    xmlDocument = XDocument.Parse(xml);

                    success = xmlDocument != null;
                }

                if (success)
                {
                    // Extract the ACS server urls
                    hostUrls = from item
                               in xmlDocument.Descendants("Host")
                               where item.Element("Type").Value == "WebOrderingAPI"
                               && item.Element("Version").Value == "2"
                               orderby item.Element("Order").Value
                               select item.Element("Url").Value;
                }

                if (success) break;
            }

            return success;
        }
    }
}