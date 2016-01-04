using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using System.Xml.Linq;

namespace AndroAdmin.Helpers.ACSDiagnosticTests
{
    public class ACSDiagnosticTestsV1
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
                this.TestPrivateSignpostServerV1_Phase1(signpostServer);

                // Test the public signpost server
                this.TestPublicSignpostServerV1_Phase2(signpostServer);

                // Test the private signpost server (phase 2)
                this.TestPrivateSignpostServerV1_Phase3(signpostServer);

                // Test the public signpost server (phase 2)
                this.TestPublicSignpostServerV1_Phase4(signpostServer);
            }
        }

        private bool TestPrivateSignpostServerV1_Phase1(string signpostServer)
        {
            bool success = true;

            List<string> hosts = this.GetHosts(signpostServer, ref success);

            if (success)
            {
                foreach (string host in hosts)
                {
                    this.TestPrivateHostV1_Phase1(signpostServer, host);
                }
            }

            return success;
        }

        private List<string> GetHosts(string signpostServer, ref bool success)
        {
            // Get the v1 hosts list from the signpost server
            string hostsUrl = signpostServer + "/AndroCloudprivateAPI/privateapi/host/" + ConfigurationManager.AppSettings["DiagnosticsACSTestSiteId"] + "?licenseKey=A24C92FE-92D1-4705-8E33-202F51BCE38D&hardwareKey=1234";
            List<string> hosts = new List<string>();
            string xml = "";

            // Call the web service to get the hosts
            if (!CloudSync.HttpHelper.RestGet(hostsUrl, out xml))
            {
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Error ,"REST call failed for: " + hostsUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                success = false;
                return hosts;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(xml);

            // Extract the host urls
            var data = from item in xDocument.Descendants("Host")
                select new
                {
                    url = item.Element("Url").Value
                };

            if (data != null)
            {
                // Test each host
                foreach (var host in data)
                {
                    hosts.Add(host.url);
                }
            }

            return hosts;
        }

        private void TestPrivateHostV1_Phase1(string signpostServer, string host)
        {
            bool success = true;

            acsDiagnosticTests.SendBlank();
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.Info, "<span style=\"font-weight:bold;\">Testing PRIVATE v1 ACS SERVICES on " + host + " (PHASE 1 OF 4):</span>");
            acsDiagnosticTests.SendBlank();

            // Check menu upload
            this.TestSiteMenuUploadV1_Phase1(host, ref success);

            // Test ETD update
            this.TestPostSiteETDUploadV1_Phase1(signpostServer, host, ref success);

            this.SendTestResultMessage(success);
        }

        private void TestSiteMenuUploadV1_Phase1(string host, ref bool success)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v1 POST site menu");

            // Send the v1 site menu
            string siteListUrl = host + "/menu/" + ConfigurationManager.AppSettings["DiagnosticsACSTestSiteId"] + "?licenseKey=A24C92FE-92D1-4705-8E33-202F51BCE38D&version=5&hardwareKey=1234";

            // Call the web service to send the site menu
            string xml = "";
            if (!CloudSync.HttpHelper.RestPost(siteListUrl, "THIS IS A TEST MENU", out xml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteListUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                success = false;
                return;
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private void TestPostSiteETDUploadV1_Phase1(string signpostServer, string host, ref bool success)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v1 POST ETD");

            if (ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, host).TestEtd == 0)
            {
                Random random = new Random();
                ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, host).TestEtd = random.Next(1, 1000);
            }

            // Update the v1 etd
            string siteListUrl = host + "/site/" + ConfigurationManager.AppSettings["DiagnosticsACSTestSiteId"] + "?licenseKey=A24C92FE-92D1-4705-8E33-202F51BCE38D&hardwareKey=1234";

            // Call the web service to send the new etd
            string xml = "";
            if (!CloudSync.HttpHelper.RestPost(siteListUrl, "<Site><ETD>" + ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, host).TestEtd.ToString() + "</ETD></Site>", out xml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteListUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                success = false;
                return;
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private bool TestPublicSignpostServerV1_Phase2(string signpostServer)
        {
            // Get the v1 hosts list from the signpost server
            string hostsUrl = signpostServer + "/AndroCloudAPI/weborderapi/host?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            string xml = "";

            // Call the web service to get the hosts
            if (!CloudSync.HttpHelper.RestGet(hostsUrl, out xml))
            {
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Error ,"REST call failed for: " + hostsUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                return false;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(xml);

            // Extract the host urls
            var data = from item in xDocument.Descendants("Host")
                select new
                {
                    url = item.Element("Url").Value
                };

            if (data != null)
            {
                // Test each host
                foreach (var host in data)
                {
                    this.TestPublicHostV1_Phase2(signpostServer, host.url);
                }
            }

            return true;
        }

        private void TestPublicHostV1_Phase2(string signpostServer, string host)
        {
            bool success = true;

            acsDiagnosticTests.SendBlank();
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.Info, "<span style=\"font-weight:bold;\">Testing PUBLIC v1 ACS SERVICES on " + host + " (PHASE 2 OF 4):</span>");
            acsDiagnosticTests.SendBlank();

            // Check site list
            this.TestGetSiteListV1(host, ref success);

            // Check site details
            this.TestGetSiteDetailsV1(signpostServer, host, ref success);

            // Check site menu
            this.TestGetSiteMenuV1(host, ref success);

            // Send an order to the test store
            this.TestPutSiteOrderV1(signpostServer, host, ref success);

            this.SendTestResultMessage(success);
        }

        private void TestGetSiteListV1(string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v1 GET site list");

            // Get the v1 sites list
            string siteListUrl = host + "/site?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            // Call the web service to get the sites
            string xml = "";
            if (!CloudSync.HttpHelper.RestGet(siteListUrl, out xml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteListUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                groupTestSuccess = false;
                return;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(xml);

            // Extract the sites
            var data = from item in xDocument.Descendants("Site")
                       select new
                       {
                           SiteId = item.Element("SiteId").Value,
                           Name = item.Element("Name").Value,
                           MenuVersion = item.Element("MenuVersion").Value,
                           IsOpen = item.Element("IsOpen").Value,
                           EstDelivTime = item.Element("EstDelivTime").Value
                       };
            if (data == null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "No sites!");
                groupTestSuccess = false;
                return;
            }
            else
            {
                var sites = data.ToArray();

                if (sites.Length == 1)
                {
                    // Check the correct data was returned
                    if (!this.ValidateSiteListSiteV1(sites[0]))
                    {
                        groupTestSuccess = false;  
                        return; 
                    }
                }
                else
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Expected one site but got " + sites.Length);
                    groupTestSuccess = false;
                    return;
                }
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private bool ValidateSiteListSiteV1(dynamic site)
        {
            if (site.SiteId != "7B10658E-622C-420F-997A-00A8C6690E59")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected site id 7B10658E-622C-420F-997A-00A8C6690E59 but got " + site.SiteId);
                return false;
            }

            if (site.Name != "ACS Diagnostics")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected site name \"ACS Diagnostics\" but got " + site.Name);
                return false;
            }

            if (site.IsOpen.ToUpper() != "TRUE")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Diagnostics Rameses VM is offline.  Unable to perform full diagnostics.");
                return false;
            }

            return true;
        }

        private void TestGetSiteDetailsV1(string signpostServer, string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v1 GET site details");

            // Get the v1 site details
            string siteDetailsUrl = host + "/sitedetails/7B10658E-622C-420F-997A-00A8C6690E59?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            // Call the web service to get the site details
            string xml = "";
            if (!CloudSync.HttpHelper.RestGet(siteDetailsUrl, out xml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteDetailsUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                groupTestSuccess = false;
                return;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(xml);

            // Extract the site details
            var data = from item in xDocument.Descendants("SiteDetails")
                select new
                {
                    SiteId = item.Element("SiteId").Value,
                    Name = item.Element("Name").Value,
                    MenuVersion = item.Element("MenuVersion").Value,
                    IsOpen = item.Element("IsOpen").Value,
                    EstDelivTime = item.Element("EstDelivTime").Value,
                    TimeZone = item.Element("TimeZone").Value,
                    Phone = item.Element("Phone").Value,
                    PaymentProvider = item.Element("PaymentProvider").Value,
                    PaymentClientId = item.Element("PaymentClientId").Value,
                    PaymentClientPassword = item.Element("PaymentClientPassword").Value,
                    Address = new
                    {
                        Long = item.Element("Address").Element("Long") == null ? null : item.Element("Address").Element("Long").Value,
                        Lat = item.Element("Address").Element("Lat") == null ? null : item.Element("Address").Element("Lat").Value,
                        Prem1 = item.Element("Address").Element("Prem1") == null ? null : item.Element("Address").Element("Prem1").Value,
                        Prem2 = item.Element("Address").Element("Prem2") == null ? null : item.Element("Address").Element("Prem2").Value,
                        Prem3 = item.Element("Address").Element("Prem3") == null ? null : item.Element("Address").Element("Prem3").Value,
                        Prem4 = item.Element("Address").Element("Prem4") == null ? null : item.Element("Address").Element("Prem4").Value,
                        Prem5 = item.Element("Address").Element("Prem5") == null ? null : item.Element("Address").Element("Prem5").Value,
                        Prem6 = item.Element("Address").Element("Prem6") == null ? null : item.Element("Address").Element("Prem6").Value,
                        Org1 = item.Element("Address").Element("Org1") == null ? null : item.Element("Address").Element("Org1").Value,
                        Org2 = item.Element("Address").Element("Org2") == null ? null : item.Element("Address").Element("Org2").Value,
                        Org3 = item.Element("Address").Element("Org3") == null ? null : item.Element("Address").Element("Org3").Value,
                        RoadName = item.Element("Address").Element("RoadName") == null ? null : item.Element("Address").Element("RoadName").Value,
                        Postcode = item.Element("Address").Element("Postcode") == null ? null : item.Element("Address").Element("Postcode").Value,
                        Country = item.Element("Address").Element("Country") == null ? null : item.Element("Address").Element("Country").Value
                    }
                };

            bool etdWarning = false;
            if (data == null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "No site details!");
                groupTestSuccess = false;
                return;
            }
            else
            {
                var sites = data.ToArray();

                if (sites.Length == 1)
                {
                    // Check the correct data was returned
                    if (!this.ValidateSiteDetailsV1(sites[0], signpostServer, host, ref etdWarning))
                    {
                        groupTestSuccess = false;
                        return;
                    }
                }
                else
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Expected one site but got " + sites.Length);
                    groupTestSuccess = false;
                    return;
                }
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);

            if (etdWarning)
            {
                acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.Error, "Rameses has updated the ETD so the automated ETD check is not valid.  You will need to rerun the tests");
            }
        }

        private bool ValidateSiteDetailsV1(dynamic site, string signpostServer, string host, ref bool etdWarning)
        {
            if (site.SiteId != "7B10658E-622C-420F-997A-00A8C6690E59")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected site id 7B10658E-622C-420F-997A-00A8C6690E59 but got " + site.SiteId);
                return false;
            }

            if (site.Name != "ACS Diagnostics")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected site name \"ACS Diagnostics\" but got " + site.Name);
                return false;
            }

            if (site.IsOpen.ToUpper() != "TRUE")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Diagnostics Rameses VM is offline.  Unable to perform full diagnostics.");
                return false;
            }

            if (site.TimeZone != "GMT")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected TimeZone GMT but got " + site.TimeZone);
                return false;
            }

            if (site.Phone != "0123456789")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected Phone 0123456789 but got " + site.Phone);
                return false;
            }

            if (site.PaymentProvider != "DataCashTest")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected PaymentProvider DataCashTest but got " + site.PaymentProvider);
                return false;
            }

            if (site.PaymentClientId != "99005860")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected PaymentClientId 99005860 but got " + site.PaymentClientId);
                return false;
            }

            if (site.PaymentClientPassword != "HVaEGxTM")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected PaymentClientPassword HVaEGxTM but got " + site.PaymentClientPassword);
                return false;
            }

            if (site.Address.Long != "-0.151209600000")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected Address.Long -0.151209600000 but got " + site.Address.Long);
                return false;
            }

            if (site.Address.Lat != "51.360660499999")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected Address.Lat 51.360660499999 but got " + site.Address.Lat);
                return false;
            }

            if (site.Address.Prem1 != null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected no Address.Prem1 but got " + site.Address.Prem1);
                return false;
            }

            if (site.Address.Prem2 != null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected no Address.Prem2 but got " + site.Address.Prem2);
                return false;
            }

            if (site.Address.Prem3 != "")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected blank Address.Prem3 but got " + site.Address.Prem3);
                return false;
            }

            if (site.Address.Prem4 != "")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected blank Address.Prem4 but got " + site.Address.Prem4);
                return false;
            }

            if (site.Address.Prem5 != "")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected blank Address.Prem5 but got " + site.Address.Prem5);
                return false;
            }

            if (site.Address.Prem6 != "")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected blank Address.Prem6 but got " + site.Address.Prem6);
                return false;
            }

            if (site.Address.Org1 != null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected no Address.Org1 but got " + site.Address.Org1);
                return false;
            }

            if (site.Address.Org2 != "Andromeda")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected Address.Org2 Andromeda but got " + site.Address.Org2);
                return false;
            }

            if (site.Address.Org3 != "")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected blank Address.Org3 but got " + site.Address.Org3);
                return false;
            }

            if (site.Address.RoadName != "test address")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected Address.RoadName test address but got " + site.Address.RoadName);
                return false;
            }

            if (site.Address.Postcode != "SM6 0DZ")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected Address.Postcode SM6 0DZ but got " + site.Address.Postcode);
                return false;
            }

            if (site.Address.Country != "United Kingdom")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected Address.Country United Kingdom but got " + site.Address.Country);
                return false;
            }

            if (site.EstDelivTime != ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, host).TestEtd.ToString())
            {
                if (site.EstDelivTime == "99")
                {
                    etdWarning = true;
                }
                else
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Expected ETD of " + ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, host).TestEtd + " but got " + site.EstDelivTime);
                    return false;
                }
            }

            return true;
        }

        private void TestGetSiteMenuV1(string host, ref bool success)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v1 GET site menu");

            // Get the v1 site menu
            string siteDetailsUrl = host + "/menu/7B10658E-622C-420F-997A-00A8C6690E59?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            // Call the web service to get the site details
            string xml = "";
            if (!CloudSync.HttpHelper.RestGet(siteDetailsUrl, out xml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteDetailsUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                success = false;
                return;
            }

            if (xml != "THIS IS A TEST MENU")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected menu text \"THIS IS A TEST MENU\" but got " + (xml.Length > 1000 ? " what looks like a real menu" : xml));
                success = false;
                return;
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private void TestPutSiteOrderV1(string signpostServer, string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v1 PUT site order");

            ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, host).RamesesTestOrderId = null;
            
            // Use the number of seconds between now and 1/1/2000 as the order number (should be unique enogh)
            TimeSpan diff = DateTime.Now - new DateTime(2000, 1, 1);
            string orderNumber = ((int)diff.TotalSeconds).ToString();
            if (ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, host).ExternalOrderId == null)
            {
                ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, host).ExternalOrderId = orderNumber;
            }

            string siteOrderUrl = host + "/order/7B10658E-622C-420F-997A-00A8C6690E59/" + orderNumber + "?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            // Call the web service to send the order to the test store
            string json =
                "{" +
                    "\"partnerReference\": \"" + orderNumber + "\"," +
                    "\"type\": \"delivery\"," +
                    "\"orderTimeType\": \"TIMED\"," +
                    "\"orderWantedTime\": \"2014-04-23T11:15:38Z\"," +
                    "\"orderPlacedTime\": \"2014-04-23T10:54:38Z\"," +
                    "\"timeToTake\": 218," +
                    "\"chefNotes\": \"\"," +
                    "\"oneOffDirections\": \"\"," +
                    "\"estimatedDeliveryTime\": 0," +
                    "\"customer\": " +
                    "{" +
                        "\"title\": \"TEST\"," +
                        "\"firstName\": \"TEST\"," +
                        "\"surname\": \"TEST\"," +
                        "\"contacts\": " +
                        "[" +
                            "{" +
                                "\"type\": \"Mobile\"," +
                                    "\"value\": \"TEST\"," +
                            "\"marketingLevel\": \"None\"" +
                            "}," +
                            "{" +
                                "\"type\": \"Email\"," +
                                "\"value\": \"TEST@TEST.com\"," +
                                "\"marketingLevel\": \"None\"" +
                            "}" +
                        "]," +
                        "\"address\": " +
                        "{" +
                            "\"prem1\": \"\"," +
                            "\"prem2\": \"\"," +
                            "\"prem3\": \"\"," +
                            "\"prem4\": \"\"," +
                            "\"prem5\": \"\"," +
                            "\"prem6\": \"\"," +
                            "\"org1\": \"\"," +
                            "\"org2\": \"\"," +
                            "\"org3\": \"\"," +
                            "\"roadNumber\": \"196\"," +
                            "\"roadName\": \"AVENUE DE VERSAILLES\"," +
                            "\"city\": \"PARIS\"," +
                            "\"town\": \"PARIS\"," +
                            "\"zipCode\": \"75016\"," +
                            "\"county\": \"\"," +
                            "\"state\": \"\"," +
                            "\"locality\": \"\"," +
                            "\"directions\": \"premier escalier a gauche dernier etage (pas d'ascenseur) \"," +
                            "\"userLocality1\": \"Etg. 7\"," +
                            "\"userLocality2\": \"Digi. A2684\"," +
                            "\"userLocality3\": \"\"," +
                            "\"country\": \"FR\"" +
                        "}," +
                        "\"accountNumber\": \"\"" +
                    "}," +
                    "\"pricing\": " +
                    "{" +
                        "\"priceBeforeDiscount\": 1390," +
                        "\"pricesIncludeTax\": \"false\"," +
                        "\"priceAfterDiscount\": 1390" +
                    "}," +
                    "\"orderLines\": " +
                    "[" +
                        "{" +
                            "\"productId\": 679," +
                            "\"quantity\": \"1\"," +
                            "\"price\": 1230," +
                            "\"name\": \"Harlem\"," +
                            "\"orderLineIndex\": 0," +
                            "\"lineType\": 1," +
                            "\"instructions\": \"\"," +
                            "\"person\": \"\"," +
                            "\"inDealFlag\": false," +
                            "\"addToppings\": []," +
                            "\"removeToppings\": []" +
                        "}," +
                        "{" +
                            "\"productId\": 714," +
                            "\"quantity\": \"1\"," +
                            "\"price\": 160," +
                            "\"name\": \"Coca-Cola 33cl\"," +
                            "\"orderLineIndex\": 1," +
                            "\"lineType\": 1," +
                            "\"instructions\": \"\"," +
                            "\"person\": \"\"," +
                            "\"inDealFlag\": false," +
                            "\"addToppings\": []," +
                            "\"removeToppings\": []" +
                        "}" +
                    "]," +
                    "\"orderPayments\": " +
                    "[" +
                        "{" +
                            "\"PaymentType\": \"PayLater\"," +
                            "\"Value\": \"1390\"," +
                            "\"AuthCode\": null," +
                            "\"CVVStatus\": null," +
                            "\"PaytypeName\": null," +
                            "\"LastFourDigits\": null," +
                            "\"PayProcessor\": null," +
                            "\"PSPSpecificDetails\": null" +
                        "}" +
                    "]" +
                "}";

            string xml =
                "<Order>" +
                    "<PartnerReference>" + orderNumber + "</PartnerReference>" +
                    "<Type>Delivery</Type>" +
                    "<OrderTimeType>ASAP</OrderTimeType>" +
                    "<OrderWantedTime>2014-04-22T16:36:32Z</OrderWantedTime>" +
                    "<OrderPlacedTime>2014-04-22T16:36:32Z</OrderPlacedTime>" +
                    "<TimeToTake>0</TimeToTake>" +
                    "<EstimatedDeliveryTime>0</EstimatedDeliveryTime>" +
                    "<Customer>" +
                        "<Title>TEST</Title>" +
                        "<FirstName>TEST</FirstName>" +
                        "<Surname>TEST</Surname>" +
                        "<Contacts>" +
                            "<Contact>" +
                                "<Type>Telephone</Type>" +
                                "<Value>TEST</Value>" +
                                "<MarketingLevel>None</MarketingLevel>" +
                            "</Contact>" +
                            "<Contact>" +
                                "<Type>Mobile</Type>" +
                                "<Value></Value>" +
                                "<MarketingLevel>None</MarketingLevel>" +
                            "</Contact>" +
                            "<Contact>" +
                                "<Type>Email</Type>" +
                                "<Value>TEST@TEST.com</Value>" +
                                "<MarketingLevel>None</MarketingLevel>" +
                            "</Contact>" +
                        "</Contacts>" +
                        "<Address>" +
                            "<Org1></Org1>" +
                            "<RoadNumber>TEST</RoadNumber>" +
                            "<RoadName>TEST</RoadName>" +
                            "<Town>TEST</Town>" +
                            "<ZipCode>TEST</ZipCode>" +
                            "<County></County>" +
                            "<Country>GB</Country>" +
                        "</Address>" +
                    "</Customer>" +
                    "<Pricing>" +
                        "<PriceBeforeDiscount>350</PriceBeforeDiscount>" +
                        "<FinalPrice>350</FinalPrice>" +
                        "<PricesIncludeTax>true</PricesIncludeTax>" +
                        "<DeliveryCharge>200</DeliveryCharge>" +
                    "</Pricing>" +
                    "<OrderLines>" +
                        "<OrderLine>" +
                            "<OrderLineIndex>0</OrderLineIndex>" +
                            "<ProductId>61</ProductId>" +
                            "<Quantity>1</Quantity>" +
                            "<Name>Oreo Shake</Name>" +
                            "<InDealFlag>false</InDealFlag>" +
                            "<LineType>1</LineType>" +
                            "<MaximumParts>1</MaximumParts>" +
                            "<Price>350</Price>" +
                        "</OrderLine>" +
                    "</OrderLines>" +
                    "<OrderPayments>" +
                        "<Payment>" +
                            "<Value>350</Value>" +
                            "<Currency>GBP</Currency>" +
                            "<PaymentType>PayLater</PaymentType>" +
                            "<PaytypeName></PaytypeName>" +
                            "<AuthCode></AuthCode>" +
                            "<Last4digits></Last4digits>" +
                        "</Payment>" +
                    "</OrderPayments>" +
                "</Order>";

            string responseXml = "";
            if (!CloudSync.HttpHelper.RestPut(siteOrderUrl, xml, null, out responseXml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteOrderUrl + (responseXml.Length > 0 ? " with the error " + responseXml : ""));
                groupTestSuccess = false;
                return;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(responseXml);

            // Extract the order details
            var data = from item in xDocument.Descendants("Order")
                select new
                {
                    Status = item.Element("Status").Value,
                    StoreOrderId = item.Element("storeOrderId").Value
                };

            if (data == null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "No order details!");
                groupTestSuccess = false;
                return;
            }
            else
            {
                var orders = data.ToArray();

                if (orders.Length == 1)
                {
                    // Check the correct data was returned
                    if (!this.ValidateSiteOrderV1(orders[0]))
                    {
                        groupTestSuccess = false;
                        return;
                    }

                    // Keep hold of the order id so we can update the order status later
                    ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, host).RamesesTestOrderId = orders[0].StoreOrderId;
                }
                else
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Expected one order but got " + orders.Length);
                    groupTestSuccess = false;
                    return;
                }
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private bool ValidateSiteOrderV1(dynamic order)
        {
            if (order.Status != "1")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected status 1 but got " + order.Status);
                return false;
            }

            if (order.StoreOrderId == null || order.StoreOrderId.Length == 0)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Blank or missing store order id");
                return false;
            }

            return true;
        }

        private bool TestPrivateSignpostServerV1_Phase3(string signpostServer)
        {
            bool success = true;

            List<string> hosts = this.GetHosts(signpostServer, ref success);

            if (success)
            {
                foreach (string privateHost in hosts)
                {
                    this.TestPrivateHostV1_Phase3(signpostServer, privateHost);
                }
            }

            return success;
        }

        private void TestPrivateHostV1_Phase3(string signpostServer, string privateHost)
        {
            bool success = true;

            acsDiagnosticTests.SendBlank();
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.Info, "<span style=\"font-weight:bold;\">Testing PRIVATE v1 ACS SERVICES on " + privateHost + " (PHASE 3 OF 4):</span>");
            acsDiagnosticTests.SendBlank();

            // Check order status update
            this.TestPostSiteOrderStatusUpdateV1(signpostServer, privateHost, ref success);

            this.SendTestResultMessage(success);
        }

        private void TestPostSiteOrderStatusUpdateV1(string signpostServer, string privateHost, ref bool success)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v1 POST status update");


            if (ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, privateHost).RamesesTestOrderId == null ||
                ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, privateHost).RamesesTestOrderId.Length == 0)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "No order to update. Unable to perform full diagnostics.");
                success = false;
                return;
            }
            else
            {
                // Build the web service url
                string siteListUrl = privateHost + "/order/" + ConfigurationManager.AppSettings["DiagnosticsACSTestSiteId"] + "/" + ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, privateHost).RamesesTestOrderId + "?licenseKey=A24C92FE-92D1-4705-8E33-202F51BCE38D&hardwareKey=1234";

                // Call the web service to update the order status
                string xml = "";
                if (!CloudSync.HttpHelper.RestPost(siteListUrl, "<Order><Status>4</Status></Order>", out xml))
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteListUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                    success = false;
                    return;
                }
            }

            ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, privateHost).OrderStatus = "4";

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private bool TestPublicSignpostServerV1_Phase4(string signpostServer)
        {
            // Get the v1 hosts list from the signpost server
            string hostsUrl = signpostServer + "/AndroCloudAPI/weborderapi/host?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

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
                    url = item.Element("Url").Value
                };

            if (data != null)
            {
                // Test each host
                foreach (var host in data)
                {
                    this.TestPublicHostV1_Phase4(signpostServer, host.url);
                }
            }

            return true;
        }

        private void TestPublicHostV1_Phase4(string signpostServer, string publicHost)
        {
            bool success = true;

            acsDiagnosticTests.SendBlank();
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.Info, "<span style=\"font-weight:bold;\">Testing PUBLIC v1 ACS SERVICES on " + publicHost + " (PHASE 4 OF 4):</span>");
            acsDiagnosticTests.SendBlank();

            // Check get order status 
            this.TestGetSiteOrderStatusV1(signpostServer, publicHost, ref success);

            this.SendTestResultMessage(success);
        }

        private void TestGetSiteOrderStatusV1(string signpostServer, string publicHost, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v1 GET order status");
            //       https://cloud1.androcloudservices.com/AndroCloudAPI/weborderapi/order/682218BA-87ED-44C3-8ABF-489A8434BA5B/2E538453-0449-4994-8751-54AA1EB3AACC?applicationid=D713ADDC-5F36-4E5D-916E-324130E80AA3

            // Build the web service url
            string siteListUrl = publicHost + "/order/7B10658E-622C-420F-997A-00A8C6690E59/" + ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, publicHost).ExternalOrderId + "?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            // Call the web service to update the order status
            string responseXml = "";
            if (!CloudSync.HttpHelper.RestGet(siteListUrl, out responseXml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteListUrl + (responseXml.Length > 0 ? " with the error " + responseXml : ""));
                groupTestSuccess = false;
                return;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(responseXml);

            // Extract the order details
            var data = from item in xDocument.Descendants("Order")
                select new
                {
                    Status = item.Element("Status").Value,
                    StoreOrderId = item.Element("storeOrderId").Value
                };

            if (data == null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "No order details!");
                groupTestSuccess = false;
                return;
            }
            else
            {
                var orders = data.ToArray();

                if (orders.Length == 1)
                {
                    // Check the correct data was returned
                    if (!this.ValidateOrderStatusV1(orders[0], signpostServer, publicHost))
                    {
                        groupTestSuccess = false;
                        return;
                    }
                }
                else
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Expected one order but got " + orders.Length.ToString());
                    groupTestSuccess = false;
                    return;
                }
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private bool ValidateOrderStatusV1(dynamic orderStatus, string signpostServer, string host)
        {
            string expectedStatus = ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, host).OrderStatus;
            if (orderStatus.Status != expectedStatus)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected status " + expectedStatus + " but got " + orderStatus.Status);
                return false;
            }

            if (orderStatus.StoreOrderId == null || orderStatus.StoreOrderId.Length == 0)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Blank or missing store order id");
                return false;
            }

            return true;
        }

        private void SendTestResultMessage(bool success)
        {
            if (success)
            {
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Success, "Test completed successfully");
            }
            else
            {
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Error, "One or more test failed");
            }
        }
    }
}