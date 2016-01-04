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
        private void TestPutSiteOrderV2(string signpostServer, string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 PUT site order");

            ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).RamesesTestOrderId = null;

            // Use the number of seconds between now and 1/1/2000 as the order number (should be unique enogh)
            TimeSpan diff = DateTime.Now - new DateTime(2000, 1, 1);
            string orderNumber = "W" + ((int)diff.TotalSeconds).ToString();
            ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).ExternalOrderId = orderNumber;

            string siteOrderUrl = host + "/sites/7B10658E-622C-420F-997A-00A8C6690E59/orders/" + orderNumber + "?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

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
                        "\"title\": \"Mr\"," +
                        "\"firstName\": \"Test\"," +
                        "\"surname\": \"McTest\"," +
                        "\"contacts\": " +
                        "[" +
                            "{" +
                                "\"type\": \"Mobile\"," +
                                "\"value\": \"0123456789\"," +
                                "\"marketingLevel\": \"3rdParty\"" +
                            "}," +
                            "{" +
                                "\"type\": \"Email\"," +
                                "\"value\": \"" + ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername + "\"," +
                                "\"marketingLevel\": \"3rdParty\"" +
                            "}" +
                        "]," +
                        "\"address\": " +
                        "{" +
                            "\"prem1\": \"prem1\"," +
                            "\"prem2\": \"prem2\"," +
                            "\"prem3\": \"prem3\"," +
                            "\"prem4\": \"prem4\"," +
                            "\"prem5\": \"prem5\"," +
                            "\"prem6\": \"prem6\"," +
                            "\"org1\": \"org1\"," +
                            "\"org2\": \"org2\"," +
                            "\"org3\": \"org3\"," +
                            "\"roadNumber\": \"roadNum\"," +
                            "\"roadName\": \"roadName\"," +
                            "\"city\": \"city\"," +
                            "\"town\": \"town\"," +
                            "\"zipCode\": \"zipCode\"," +
                            "\"county\": \"county\"," +
                            "\"state\": \"state\"," +
                            "\"locality\": \"locality\"," +
                            "\"directions\": \"directions\"," +
                            "\"country\": United Kingdom\"country\"" +
                        "}," +
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
                            "\"instructions\": \"instructions\"," +
                            "\"person\": \"person\"," +
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
                            "\"instructions\": \"instructions\"," +
                            "\"person\": \"person\"," +
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
                    "<TimeToTake>218</TimeToTake>" +
                    "<EstimatedDeliveryTime>0</EstimatedDeliveryTime>" +
                    "<Customer>" +
                        "<Title>Mr</Title>" +
                        "<FirstName>Test</FirstName>" +
                        "<Surname>McTest</Surname>" +
                        "<Contacts>" +
                            "<Contact>" +
                                "<Type>Mobile</Type>" +
                                "<Value>0123456789</Value>" +
                                "<MarketingLevel>3rdParty</MarketingLevel>" +
                            "</Contact>" +
                            "<Contact>" +
                                "<Type>Email</Type>" +
                                "<Value>" + ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername + "</Value>" +
                                "<MarketingLevel>3rdParty</MarketingLevel>" +
                            "</Contact>" +
                        "</Contacts>" +
                        "<Address>" +
                            "<Prem1>prem1</Prem1>" +
                            "<Prem2>prem2</Prem2>" +
                            "<Prem3>prem3</Prem3>" +
                            "<Prem4>prem4</Prem4>" +
                            "<Prem5>prem5</Prem5>" +
                            "<Prem6>prem6</Prem6>" +
                            "<Org1>org1</Org1>" +
                            "<Org2>org2</Org2>" +
                            "<Org3>org3</Org3>" +
                            "<RoadNumber>roadNum</RoadNumber>" +
                            "<RoadName>roadName</RoadName>" +
                            "<City>city</City>" +
                            "<Town>town</Town>" +
                            "<ZipCode>zipCode</ZipCode>" +
                            "<County>county</County>" +
                            "<State>state</State>" +
                            "<Locality>locality</Locality>" +
                            "<Directions>directions</Directions>" +
                            "<Country>United Kingdom</Country>" +
                        "</Address>" +
                    "</Customer>" +
                    "<Pricing>" +
                        "<PriceBeforeDiscount>1390</PriceBeforeDiscount>" +
                        "<PricesIncludeTax>false</PricesIncludeTax>" +
                        "<PriceAfterDiscount>1390</PriceAfterDiscount>" +
                    "</Pricing>" +
                    "<OrderLines>" +
                        "<OrderLine>" +
                            "<ProductId>679</ProductId>" +
                            "<Quantity>1</Quantity>" +
                            "<Price>1230</Price>" +
                            "<Name>Harlem</Name>" +
                            "<OrderLineIndex>0</OrderLineIndex>" +
                            "<LineType>1</LineType>" +
                            "<Instructions>instructions</Instructions>" +
                            "<Person>person</Person>" +
                            "<AddToppings></AddToppings>" +
                            "<RemoveToppings>Oreo Shake</RemoveToppings>" +
                        "</OrderLine>" +
                        "<OrderLine>" +
                            "<ProductId>714</ProductId>" +
                            "<Quantity>1</Quantity>" +
                            "<Price>160</Price>" +
                            "<Name>Coca-Cola 33cl</Name>" +
                            "<OrderLineIndex>0</OrderLineIndex>" +
                            "<LineType>1</LineType>" +
                            "<Instructions>instructions</Instructions>" +
                            "<Person>person</Person>" +
                            "<AddToppings></AddToppings>" +
                            "<RemoveToppings>Oreo Shake</RemoveToppings>" +
                        "</OrderLine>" +
                    "</OrderLines>" +
                    "<OrderPayments>" +
                        "<Payment>" +
                            "<PaymentType>PayLater</PaymentType>" +
                            "<Value>1390</Value>" +
                            "<AuthCode></AuthCode>" +
                            "<CVVStatus></CVVStatus>" +
                            "<PaytypeName></PaytypeName>" +
                            "<LastFourDigits></LastFourDigits>" +
                            "<PayProcessor></PayProcessor>" +
                            "<PSPSpecificDetails></PSPSpecificDetails>" +
                        "</Payment>" +
                    "</OrderPayments>" +
                "</Order>";

            string responseXml = "";
            if (!CloudSync.HttpHelper.RestPut(siteOrderUrl, xml, null, out responseXml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteOrderUrl + (responseXml.Length > 0 ? " with the error " + responseXml : ""));
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Error, "ACS returned: " + System.Web.HttpUtility.HtmlEncode(responseXml));
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
                    if (!this.ValidateSiteOrderV2(orders[0]))
                    {
                        groupTestSuccess = false; 
                        return; 
                    }

                    // Keep hold of the order id so we can update the order status later
                    ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).RamesesTestOrderId = orders[0].StoreOrderId;
                }
                else
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Expected one order but got " + orders.Length);
                    groupTestSuccess = false;
                    return;
                }
            }

            ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, host).OrderStatus = "1";

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private bool ValidateSiteOrderV2(dynamic order)
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

        private void TestPostSiteOrderStatusUpdateV2(string signpostServer, string privateHost, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 POST status update");

            if (ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, privateHost).RamesesTestOrderId == null || ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, privateHost).RamesesTestOrderId.Length == 0)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "No order to update.  Unable to perform full diagnostics.");
                groupTestSuccess = false;
                return;
            }
            else
            {
                // Build the web service url
                string siteListUrl = privateHost + "/order/" + ConfigurationManager.AppSettings["DiagnosticsACSTestSiteId"] + "/" + ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, privateHost).RamesesTestOrderId + "?licenseKey=A24C92FE-92D1-4705-8E33-202F51BCE38D&hardwareKey=1234";

                // Call the web service to update the order status
                string xml = "";
                if (!CloudSync.HttpHelper.RestPost(siteListUrl, "<Order><Status>4</Status></Order>", out xml))
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteListUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                    groupTestSuccess = false;
                    return;
                }
            }

            ACSDiagnosticTests.GetACSDiagnosticTestData("V1_", signpostServer, privateHost).OrderStatus = "4";

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private void TestGetSiteOrderStatusV2(string signpostServer, string publicHost, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 GET order status");

            if (ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, publicHost).RamesesTestOrderId == null || ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, publicHost).RamesesTestOrderId.Length == 0)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "No order to update.  Unable to perform full diagnostics.");
                groupTestSuccess = false;
                return;
            }
            else
            {
                // Build the web service url
                string siteListUrl = publicHost + "/sites/7B10658E-622C-420F-997A-00A8C6690E59/order/" + ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, publicHost).ExternalOrderId + "?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

                // Call the web service to update the order status
                string xml = "";
                if (!CloudSync.HttpHelper.RestGet(siteListUrl, out xml))
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteListUrl + (xml.Length > 0 ? " with the error " + xml : ""));
                    groupTestSuccess = false;
                    return;
                }

                // Parse the returned xml
                XDocument xDocument = XDocument.Parse(xml);

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
                        if (!this.ValidateOrderStatusV2(orders[0], signpostServer, publicHost))
                        {
                            groupTestSuccess = false; 
                            return; 
                        }
                    }
                    else
                    {
                        acsDiagnosticTests.SendFailedTestMessage(true, "Expected one order but got " + orders.Length);
                        groupTestSuccess = false;
                        return;
                    }
                }
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private bool ValidateOrderStatusV2(dynamic orderStatus, string signpostServer, string host)
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
    }
}