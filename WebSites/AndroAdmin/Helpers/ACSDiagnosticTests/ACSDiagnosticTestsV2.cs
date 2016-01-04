using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using System.Xml.Linq;

namespace AndroAdmin.Helpers.ACSDiagnosticTests
{
    public partial class ACSDiagnosticTestsV2
    {
        private ACSDiagnosticTests acsDiagnosticTests { get; set; }

        internal void TestSignPostServers(ACSDiagnosticTests acsDiagnosticTests)
        {
            this.acsDiagnosticTests = acsDiagnosticTests;

            // Get the signpost servers to use
            string signpostServersSetting = ConfigurationManager.AppSettings["DiagnosticsACSSignpostServers"];
            string[] signpostServers = signpostServersSetting.Split(new char[] { ',' });

            // Test each signpost server
            foreach (string signpostServer in signpostServers)
            {
                acsDiagnosticTests.SendBlank();
                acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.Info, "<span style=\"font-weight:bold;\">Testing signpost server " + signpostServer + "</span>");

                // Test the private signpost server (phase 1)
                this.TestPrivateSignpostServerV2_Phase1(signpostServer);

                // Test the public signpost server (phase 2)
                this.TestPublicSignpostServerV2_Phase2(signpostServer);

                // Test the private signpost server (phase 3)
                this.TestPrivateSignpostServerV2_Phase3(signpostServer);

                // Test the public signpost server (phase 4)
                this.TestPublicSignpostServerV2_Phase4(signpostServer);
            }
        }

        private bool TestPrivateSignpostServerV2_Phase1(string signpostServer)
        {
            bool groupTestSuccess = true;

            // Get the hosts list from the signpost server
            List<string> hosts = this.TestPrivateSignpostServerV2_Phase1(signpostServer, ref groupTestSuccess);

            // Test each host
            if (groupTestSuccess)
            {
                int hostCount = 1;
                foreach (string host in hosts)
                {
                    this.TestPrivateHostV2_Phase1(signpostServer, host);

                    hostCount++;
                }
            }

            return groupTestSuccess;
        }

        private List<string> TestPrivateSignpostServerV2_Phase1(string signpostServer, ref bool groupTestSuccess)
        {
            // Get the v2 hosts list from the signpost server
            string hostsUrl = signpostServer + "/AndroCloudprivateAPI/privateapiv2/host/" + ConfigurationManager.AppSettings["DiagnosticsACSTestSiteId"] + "?licenseKey=A24C92FE-92D1-4705-8E33-202F51BCE38D&hardwareKey=1234";
            List<string> hosts = new List<string>();
            string xml = "";

            // Call the web service to get the hosts
            if (!CloudSync.HttpHelper.RestGet(hostsUrl, out xml))
            {
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Error, "REST call failed for: " + hostsUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                groupTestSuccess = false;
                return hosts;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(xml);

            // Extract the host urls
            var data = from item in xDocument.Descendants("Host")
                where item.Element("Version").Value == "2"
                select new
                {
                    type = item.Element("Type").Value,
                    url = item.Element("Url").Value,
                    version = item.Element("Version").Value
                };

            if (data != null)
            {
                // Test each host
                foreach (var host in data)
                {
                    if (host.type == "PrivateWebOrderingAPI" && host.version == "2")
                    {
                        hosts.Add(host.url);
                    }
                }
            }

            return hosts;
        }

        private void TestPrivateHostV2_Phase1(string signpostServer, string host)
        {
            bool groupTestSuccess = true;

            acsDiagnosticTests.SendBlank();
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.Info, "<span style=\"font-weight:bold;\">Testing PRIVATE v2 ACS SERVICES on " + host + " (PHASE 1 OF 4):</span>");
            acsDiagnosticTests.SendBlank();

            // Check menu upload
            this.TestSiteMenuUploadV2(host, ref groupTestSuccess);

            // Test ETD update
            this.TestSiteETDUploadV2(signpostServer, host, ref groupTestSuccess);

            if (groupTestSuccess)
            {
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Success, "Test completed successfully");
            }
        }

        private bool TestPublicSignpostServerV2_Phase2(string signpostServer)
        {
            acsDiagnosticTests.SendBlank();
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.Info, "<span style=\"font-weight:bold;\">Testing v2 hosts PUBLIC v2 ACS SERVICES on " + signpostServer + " (PHASE 2 OF 4):</span>");

            // Get the v2 hosts list from the signpost server
            string hostsUrl = signpostServer + "/AndroCloudAPI/weborderapiv2/host?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            string xml = "";

            // Call the web service to get the hosts
            if (!CloudSync.HttpHelper.RestGet(hostsUrl, out xml))
            {
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Error, "REST call failed for: " + hostsUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                return false;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(xml);

            // Extract the host urls
            var data = from item in xDocument.Descendants("Host")
                select new
                {
                    type = item.Element("Type").Value,
                    url = item.Element("Url").Value,
                    version = item.Element("Version").Value
                };

            if (data != null)
            {
                // Test each host
                foreach (var host in data)
                {
                    if (host.type == "WebOrderingAPI" && host.version == "2")
                    {
                        this.TestPublicHostV2_Phase2(signpostServer, host.url);
                    }
                }
            }

            return true;
        }

        private void TestPublicHostV2_Phase2(string signpostServer, string host)
        {
            acsDiagnosticTests.SendBlank();
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.Info, "<span style=\"font-weight:bold;\">Testing PUBLIC v2 ACS SERVICES on " + host + " (PHASE 2 OF 4):</span>");
            acsDiagnosticTests.SendBlank();

            bool groupTestSuccess = true;

            // Check site list
            this.TestSiteListV2(host, ref groupTestSuccess);

            // Check site details
            this.TestSiteDetailsV2(signpostServer, host, ref groupTestSuccess);

            // Check site menu
            this.TestGetSiteMenuV2(host, ref groupTestSuccess);

            // Put customer
            this.TestPutSiteCustomerV2(signpostServer, host, ref groupTestSuccess);

            // Send an order to the test store
            this.TestPutSiteOrderV2(signpostServer, host, ref groupTestSuccess);

         //   Get menu images

         //   Get delivery zones
            this.TestGetSiteDeliveryZonesV2(signpostServer, host, ref groupTestSuccess);

         //   Get delivery towns
         //   Get delivery roads

            // Get customer
            this.TestGetSiteCustomerV2(signpostServer, host, false, ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername, ref groupTestSuccess);

            // Get customer orders
            this.TestGetSiteCustomerOrdersV2(signpostServer, host, ref groupTestSuccess);

            // Get customer order
            this.TestGetSiteCustomerOrderV2(signpostServer, host, ref groupTestSuccess);

            // Post customer
            this.TestPostSiteCustomerV2(signpostServer, host, ref groupTestSuccess);

            // Get modified customer 
            this.TestGetSiteCustomerV2(signpostServer, host, true, ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername, ref groupTestSuccess);

        //    Put password reset request
        //    Post password reset request

            if (groupTestSuccess)
            {
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Success, "Test completed successfully");
            }
        }

        private bool TestPrivateSignpostServerV2_Phase3(string signpostServer)
        {
            bool groupTestSuccess = true;

            List<string> hosts = this.TestPrivateSignpostServerV2_Phase1(signpostServer, ref groupTestSuccess);

            if (groupTestSuccess)
            {
                foreach (string privateHost in hosts)
                {
                    this.TestPrivateHostV2_Phase3(signpostServer, privateHost);
                }
            }

            return groupTestSuccess;
        }

        private void TestPrivateHostV2_Phase3(string signpostServer, string privateHost)
        {
            bool groupTestSuccess = true;

            acsDiagnosticTests.SendBlank();
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.Info, "<span style=\"font-weight:bold;\">Testing PRIVATE v2 ACS SERVICES on " + privateHost + " (PHASE 3 OF 4):</span>");
            acsDiagnosticTests.SendBlank();

            // Check order status update
            this.TestPostSiteOrderStatusUpdateV2(signpostServer, privateHost, ref groupTestSuccess);

            if (groupTestSuccess)
            {
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Success, "Test completed successfully");
            }
        }

        private bool TestPublicSignpostServerV2_Phase4(string signpostServer)
        {
            // Get the v2 hosts list from the signpost server
            string hostsUrl = signpostServer + "/AndroCloudAPI/weborderapiv2/host?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            string xml = "";

            // Call the web service to get the hosts
            if (!CloudSync.HttpHelper.RestGet(hostsUrl, out xml))
            {
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Error, "REST call failed for: " + hostsUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                return false;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(xml);

            // Extract the host urls
            var data = from item in xDocument.Descendants("Host")
                select new
                {
                    type = item.Element("Type").Value,
                    url = item.Element("Url").Value,
                    version = item.Element("Version").Value
                };

            if (data != null)
            {
                // Test each host
                foreach (var host in data)
                {
                    if (host.type == "WebOrderingAPI" && host.version == "2")
                    {
                        this.TestPublicHostV2_Phase4(signpostServer, host.url);
                    }
                }
            }

            return true;
        }

        private void TestPublicHostV2_Phase4(string signpostServer, string publicHost)
        {
            bool groupTestSuccess = true;

            acsDiagnosticTests.SendBlank();
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.Info, "<span style=\"font-weight:bold;\">Testing PUBLIC v2 ACS SERVICES on " + publicHost + " (PHASE 4 OF 4):</span>");
            acsDiagnosticTests.SendBlank();

            // Check get order status 
            this.TestGetSiteOrderStatusV2(signpostServer, publicHost, ref groupTestSuccess);

            if (groupTestSuccess)
            {
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Success, "Test completed successfully");
            }
        }
    }
}