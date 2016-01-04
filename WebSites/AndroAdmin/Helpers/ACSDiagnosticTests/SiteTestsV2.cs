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
        private void TestSiteMenuUploadV2(string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 POST site menu");

            // Send the v2 site menu
            string siteListUrl = host + "/menu/" + ConfigurationManager.AppSettings["DiagnosticsACSTestSiteId"] + "?licenseKey=A24C92FE-92D1-4705-8E33-202F51BCE38D&version=5&hardwareKey=1234";

            // Call the web service to send the site menu
            string xml = "";
            if (!CloudSync.HttpHelper.RestPost(siteListUrl, "THIS IS A TEST MENU", out xml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteListUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                groupTestSuccess = false;
                return;
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private void TestSiteETDUploadV2(string signpostServer, string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 POST ETD");

            if (ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).TestEtd == 0)
            {
                Random random = new Random();
                ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).TestEtd = random.Next(1, 1000);
            }

            // Update the v2 etd
            string siteListUrl = host + "/site/" + ConfigurationManager.AppSettings["DiagnosticsACSTestSiteId"] + "?licenseKey=A24C92FE-92D1-4705-8E33-202F51BCE38D&hardwareKey=1234";

            // Call the web service to send the new etd
            string xml = "";
            if (!CloudSync.HttpHelper.RestPost(siteListUrl, "<Site><ETD>" + ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, host).TestEtd.ToString() + "</ETD></Site>", out xml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteListUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                groupTestSuccess = false;
                return;
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private void TestSiteListV2(string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 GET site list");

            // Get the v2 sites list
            string siteListUrl = host + "/sites?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

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
                    if (!this.ValidateSiteListSiteV2(sites[0]))
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

        private bool ValidateSiteListSiteV2(dynamic site)
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

        private void TestGetSiteMenuV2(string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 GET site menu");

            // Get the v2 site menu
            string siteDetailsUrl = host + "/sites/7B10658E-622C-420F-997A-00A8C6690E59/menu?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            // Call the web service to get the site details
            string xml = "";
            if (!CloudSync.HttpHelper.RestGet(siteDetailsUrl, out xml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteDetailsUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                groupTestSuccess = false;
                return;
            }

            if (xml != "THIS IS A TEST MENU")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected menu text \"THIS IS A TEST MENU\" but got " + (xml.Length > 1000 ? " what looks like a real menu" : xml));
                groupTestSuccess = false;
                return;
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private void TestSiteDetailsV2(string signpostServer, string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 GET site details");

            // Get the v2 site details
            string siteDetailsUrl = host + "/sites/7B10658E-622C-420F-997A-00A8C6690E59/details?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

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
                    if (!this.ValidateSiteDetailsV2(sites[0], signpostServer, host, ref etdWarning))
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

        private bool ValidateSiteDetailsV2(dynamic site, string signpostServer, string host, ref bool etdWarning)
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

            if (site.EstDelivTime != ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).TestEtd.ToString())
            {
                if (site.EstDelivTime == "99")
                {
                    etdWarning = true;
                }
                else
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Expected ETD of " + ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).TestEtd + " but got " + site.EstDelivTime);
                    return false;
                }
            }

            return true;
        }

        private void TestGetSiteDeliveryZonesV2(string signpostServer, string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 GET site delivery zones");

            // Get the v2 site delivery zones
            string siteDetailsUrl = host + "/sites/7B10658E-622C-420F-997A-00A8C6690E59/deliveryzones?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            // Call the web service to get the site delivery zones
            string xml = "";
            if (!CloudSync.HttpHelper.RestGet(siteDetailsUrl, out xml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteDetailsUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                groupTestSuccess = false;
                return;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(xml);

            // Extract the site delivery zones
            var data = from item in xDocument.Descendants("DeliveryZones")
                select new
                {
                    Zone = item.Element("Zone").Value
                };

            if (data == null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "No site delivery zones!");
                groupTestSuccess = false;
                return;
            }
            else
            {
                var siteDeliveryZones = data.ToArray();

                if (siteDeliveryZones.Length == 1)
                {
                    // Check the correct data was returned
                    if (siteDeliveryZones[0].Zone != "sm60")
                    {
                        acsDiagnosticTests.SendFailedTestMessage(true, "Expected site delivery zone \"sm60\" but got " + siteDeliveryZones[0].Zone);
                        groupTestSuccess = false;
                        return;
                    }
                }
                else
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Expected one site delivery zone but got " + siteDeliveryZones.Length);
                    groupTestSuccess = false;
                    return;
                }
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }
    }
}