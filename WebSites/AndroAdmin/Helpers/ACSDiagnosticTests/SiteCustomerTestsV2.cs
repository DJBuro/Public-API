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
        private void TestPutSiteCustomerV2(string signpostServer, string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 PUT customer");

            // Use the number of seconds between now and 1/1/2000 as the order number (should be unique enogh)
            TimeSpan diff = DateTime.Now - new DateTime(2000, 1, 1);
            string customerEmailAddress = ((int)diff.TotalSeconds).ToString() + "@test.com";
            ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername = customerEmailAddress;

            string siteCustomerUrl = 
                host + 
                "/customers/" + 
                ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername + 
                "?applicationid=" + ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            string xml =
                "<Customer>" +
                    "<Title>Mr</Title>" +
                    "<FirstName>Test</FirstName>" +
                    "<Surname>McTest</Surname>" + 
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
                        "<RoadNum>roadNum</RoadNum>" +
                        "<RoadName>roadName</RoadName>" +
                        "<City>city</City>" +
                        "<Town>town</Town>" +
                        "<ZipCode>zipCode</ZipCode>" +
                        "<Postcode>postcode</Postcode>" +
                        "<County>county</County>" +
                        "<State>state</State>" +
                        "<Locality>locality</Locality>" +
                        "<Country>United Kingdom</Country>" +
                        "<Directions>directions</Directions>" +
                    "</Address>" +
                    "<Contacts>" +
                        "<Contact>" +
                            "<Type>EMail</Type>" +
                            "<Value>" + ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername + "</Value>" +
                            "<MarketingLevel>3rdParty</MarketingLevel>"+
                        "</Contact>" +
                        "<Contact>" +
                            "<Type>Mobile</Type>" +
                            "<Value>0123456789</Value>" +
                            "<MarketingLevel>3rdParty</MarketingLevel>" +
                        "</Contact>" +
                    "</Contacts>" +
                "</Customer>";

            byte[] temp = System.Text.Encoding.UTF8.GetBytes("pass123");
            string encodedPassword = "Basic " + System.Convert.ToBase64String(temp);

            string responseXml = "";
            if (!CloudSync.HttpHelper.RestPut(siteCustomerUrl, xml, new Dictionary<string, string>() { { "Authorization", encodedPassword } }, out responseXml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteCustomerUrl + (responseXml.Length > 0 ? " with the error " + responseXml : ""));
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Error, "ACS returned: " + System.Web.HttpUtility.HtmlEncode(responseXml));
                groupTestSuccess = false;
                return;
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private void TestGetSiteCustomerV2(string signpostServer, string host, bool modifiedCustomer, string username, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 GET customer");

            string siteCustomerUrl = 
                host + 
                "/customers/" + 
                username + 
                "?applicationid=" + 
                ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            byte[] temp = System.Text.Encoding.UTF8.GetBytes("pass123");
            string encodedPassword = "Basic " + System.Convert.ToBase64String(temp);

            string responseXml = "";
            if (!CloudSync.HttpHelper.RestGet(siteCustomerUrl, new Dictionary<string, string>() { { "Authorization", encodedPassword } }, out responseXml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteCustomerUrl + (responseXml.Length > 0 ? " with the error " + responseXml : ""));
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Error, "ACS returned: " + System.Web.HttpUtility.HtmlEncode(responseXml));
                groupTestSuccess = false;
                return;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(responseXml);

            // Extract the customer details
            var data = from customer in xDocument.Descendants("Customer")
                select new
                {
                    Title = customer.Element("Title").Value,
                    FirstName = customer.Element("FirstName").Value,
                    Surname = customer.Element("Surname").Value,
                    Contacts = from contact in customer.Element("Contacts").Elements("Contact")
                        select new
                        {
                            Type = contact.Element("Type").Value,
                            Value = contact.Element("Value").Value,
                            MarketingLevel = contact.Element("MarketingLevel").Value
                        }
                };

            if (data == null)
            {
                groupTestSuccess = false;
                acsDiagnosticTests.SendFailedTestMessage(true, "No customer details!");
                return;
            }
            else
            {
                var customers = data.ToArray();

                if (customers.Length == 1)
                {
                    // Check the correct data was returned
                    if (modifiedCustomer)
                    {
                        if (!this.ValidateSiteCustomerModifiedV2(customers[0], username))
                        {
                            groupTestSuccess = false;
                            return;
                        }
                    }
                    else
                    {
                        if (!this.ValidateSiteCustomerV2(customers[0], username))
                        {
                            groupTestSuccess = false;
                            return;
                        }
                    }
                }
                else
                {
                    groupTestSuccess = false;
                    acsDiagnosticTests.SendFailedTestMessage(true, "Expected one customer but got " + customers.Length);
                    return;
                }
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private bool ValidateSiteCustomerV2(dynamic customer, string username)
        {
            if (customer.Title != "Mr")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected title Mr but got " + customer.title);
                return false;
            }

            if (customer.FirstName != "Test")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected firstName Test but got " + customer.firstName);
                return false;
            }

            if (customer.Surname != "McTest")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected surname McTest but got " + customer.surname);
                return false;
            }

            if (customer.Contacts == null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Null or missing contacts");
                return false;
            }
            else
            {
                bool foundMobile = false;
                bool foundEmail = false;

                foreach (var contact in customer.Contacts)
                {
                    if (contact.Type == "Mobile")
                    {
                        foundMobile = true;
                        if (contact.Value != "0123456789")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Expected mobile contact value 0123456789 but got " + contact.Value);
                            return false;
                        }

                        if (contact.MarketingLevel != "3rdParty")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Expected mobile contact marketingLevel 3rdParty but got " + contact.MarketingLevel);
                            return false;
                        }
                    }
                    else if (contact.Type == "Email")
                    {
                        foundEmail = true;
                        if (contact.Value != username)
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Expected email contact value " + username + " but got " + contact.Value);
                            return false;
                        }

                        if (contact.MarketingLevel != "3rdParty")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Expected email contact marketingLevel 3rdParty but got " + contact.MarketingLevel);
                            return false;
                        }
                    }
                }

                if (!foundMobile)
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Missing mobile contact");
                    return false;
                }

                if (!foundEmail)
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Missing email contact");
                    return false;
                }
            }

            return true;
        }

        private bool ValidateSiteCustomerModifiedV2(dynamic customer, string username)
        {
            if (customer.Title != "MrX")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected title MrX but got " + customer.title);
                return false;
            }

            if (customer.FirstName != "TestX")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected firstName TestX but got " + customer.firstName);
                return false;
            }

            if (customer.Surname != "McTestX")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected surname McTestX but got " + customer.surname);
                return false;
            }

            if (customer.Contacts == null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Null or missing contacts");
                return false;
            }
            else
            {
                bool foundMobile = false;
                bool foundEmail = false;

                foreach (var contact in customer.Contacts)
                {
                    if (contact.Type == "Mobile")
                    {
                        foundMobile = true;
                        if (contact.Value != "0123456789X")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Expected mobile contact value 0123456789X but got " + contact.Value);
                            return false;
                        }

                        if (contact.MarketingLevel != "3rdParty")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Expected mobile contact marketingLevel 3rdParty but got " + contact.MarketingLevel);
                            return false;
                        }
                    }
                    else if (contact.Type == "Email")
                    {
                        foundEmail = true;
                        if (contact.Value != username)
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Expected email contact value " + username + " but got " + contact.Value);
                            return false;
                        }

                        if (contact.MarketingLevel != "3rdParty")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Expected email contact marketingLevel 3rdParty but got " + contact.MarketingLevel);
                            return false;
                        }
                    }
                }

                if (!foundMobile)
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Missing mobile contact");
                    return false;
                }

                if (!foundEmail)
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Missing email contact");
                    return false;
                }
            }

            return true;
        }

        private void TestGetSiteCustomerOrdersV2(string signpostServer, string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 GET customer order headers");

            string siteCustomerUrl = 
                host + 
                "/customers/" + 
                ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername + 
                "/orders?applicationid=" + 
                ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            byte[] temp = System.Text.Encoding.UTF8.GetBytes("pass123");
            string encodedPassword = "Basic " + System.Convert.ToBase64String(temp);

            string responseXml = "";
            if (!CloudSync.HttpHelper.RestGet(siteCustomerUrl, new Dictionary<string, string>() { { "Authorization", encodedPassword } }, out responseXml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteCustomerUrl + (responseXml.Length > 0 ? " with the error " + responseXml : ""));
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Error, "ACS returned: " + System.Web.HttpUtility.HtmlEncode(responseXml));
                groupTestSuccess = false;
                return;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(responseXml);

            // Extract the order headers
            var data = from orderHeader in xDocument.Descendants("Orders").Elements("Order")
                select new
                {
                    Id = orderHeader.Element("Id").Value,
                    ForDateTime = orderHeader.Element("ForDateTime").Value,
                    OrderStatus = orderHeader.Element("OrderStatus").Value
                };

            if (data == null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "No order headers!");
                groupTestSuccess = false;
                return;
            }
            else
            {
                var orderHeaders = data.ToArray();

                if (orderHeaders.Length == 1)
                {
                    // Check the correct data was returned
                    if (!this.ValidateSiteCustomerOrdersV2(orderHeaders[0], signpostServer, host))
                    {
                        groupTestSuccess = false;
                        return;
                    }
                }
                else
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Expected one order header but got " + orderHeaders.Length);
                    groupTestSuccess = false;
                    return;
                }
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private bool ValidateSiteCustomerOrdersV2(dynamic orderHeader, string signpostServer, string host)
        {
            if (orderHeader.Id != ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).ExternalOrderId)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected id " + ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).ExternalOrderId + " but got " + orderHeader.Id);
                return false;
            }

            if (orderHeader.OrderStatus != "1")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected order status 1 Test but got " + orderHeader.OrderStatus);
                return false;
            }

            if (orderHeader.ForDateTime != "2014-04-22T16:36:32")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected ForDateTime 2014-04-22T16:36:32 but got " + orderHeader.ForDateTime);
                return false;
            }

            return true;
        }

        private void TestGetSiteCustomerOrderV2(string signpostServer, string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 GET customer order");

            string siteCustomerUrl = 
                host + 
                "/customers/" + 
                ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername + 
                "/orders/" + 
                ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).ExternalOrderId + 
                "?applicationid=" + 
                ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            byte[] temp = System.Text.Encoding.UTF8.GetBytes("pass123");
            string encodedPassword = "Basic " + System.Convert.ToBase64String(temp);

            string responseXml = "";
            if (!CloudSync.HttpHelper.RestGet(siteCustomerUrl, new Dictionary<string, string>() { { "Authorization", encodedPassword } }, out responseXml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteCustomerUrl + (responseXml.Length > 0 ? " with the error " + responseXml : ""));
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Error, "ACS returned: " + System.Web.HttpUtility.HtmlEncode(responseXml));
                groupTestSuccess = false;
                return;
            }

            // Parse the returned xml
            XDocument xDocument = XDocument.Parse(responseXml);

            // Extract the order
            var data = from order in xDocument.Descendants("Order")
                select new
                {
                    Id = order.Element("Id").Value,
                    ForDateTime = order.Element("ForDateTime").Value,
                    OrderStatus = order.Element("OrderStatus").Value,
                    OrderTotal = order.Element("OrderTotal").Value,
                    OrderLines = from orderLine in order.Element("OrderLines").Elements("OrderLine")
                        select new
                        {
                            MenuId = orderLine.Element("MenuId").Value,
                            ProductName = orderLine.Element("ProductName").Value,
                            Quantity = orderLine.Element("Quantity").Value,
                            UnitPrice = orderLine.Element("UnitPrice").Value,
                            ChefNotes = orderLine.Element("ChefNotes").Value,
                            Person = orderLine.Element("Person").Value
                        }
                };

            if (data == null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "No order lines!");
                groupTestSuccess = false;
                return;
            }
            else
            {
                var orderLines = data.ToArray();

                if (orderLines.Length == 1)
                {
                    // Check the correct data was returned
                    if (!this.ValidateSiteCustomerOrderV2(orderLines[0], signpostServer, host))
                    {
                        groupTestSuccess = false;
                        return;
                    }
                }
                else
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Expected one order line but got " + orderLines.Length);
                    groupTestSuccess = false;
                    return;
                }
            }

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }

        private bool ValidateSiteCustomerOrderV2(dynamic order, string signpostServer, string host)
        {
            if (order.Id != ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).ExternalOrderId)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected id " + ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).ExternalOrderId + " but got " + order.Id);
                return false;
            }

            if (order.OrderStatus != "1")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected order status 1 Test but got " + order.OrderStatus);
                return false;
            }

            if (order.ForDateTime != "2014-04-22T16:36:32")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected ForDateTime 2014-04-22T16:36:32 but got " + order.ForDateTime);
                return false;
            }

            if (order.OrderTotal != "13.9000")
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Expected OrderTotal 13.9000 but got " + order.OrderTotal);
                return false;
            }

            if (order.OrderLines == null)
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "Missing order lines");
                return false;
            }
            else
            {
                bool menuItem1Found = false;
                bool menuItem2Found = false;

                foreach (var orderLine in order.OrderLines)
                {
                    if (orderLine.MenuId == "679")
                    {
                        if (orderLine.ProductName != "Harlem")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Menu item 1 expected ProductName Harlem but got " + orderLine.ProductName);
                            return false;
                        }

                        if (orderLine.Quantity != "1")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Menu item 1 expected Quantity 1 but got " + orderLine.Quantity);
                            return false;
                        }

                        if (orderLine.UnitPrice != "1230")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Menu item 1 expected UnitPrice 1230 but got " + orderLine.UnitPrice);
                            return false;
                        }

                        if (orderLine.ChefNotes != "")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Menu item 1 expected empty ChefNotes but got " + orderLine.ChefNotes);
                            return false;
                        }

                        if (orderLine.Person != "person")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Menu item 1 expected Person \"person\" but got " + orderLine.Person);
                            return false;
                        }

                        menuItem1Found = true;
                    }
                    else if (orderLine.MenuId != "714")
                    {
                        if (orderLine.ProductName != "Coca-Cola 33cl")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Menu item 2 expected ProductName Coca-Cola 33cl but got " + orderLine.ProductName);
                            return false;
                        }

                        if (orderLine.Quantity != "1")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Menu item 2 expected Quantity 1 but got " + orderLine.Quantity);
                            return false;
                        }

                        if (orderLine.UnitPrice != "160")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Menu item 2 expected UnitPrice 160 but got " + orderLine.UnitPrice);
                            return false;
                        }

                        if (orderLine.ChefNotes != "")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Menu item 2 expected empty ChefNotes but got " + orderLine.ChefNotes);
                            return false;
                        }

                        if (orderLine.Person != "person")
                        {
                            acsDiagnosticTests.SendFailedTestMessage(true, "Menu item 2 expected Person \"person\" but got " + orderLine.Person);
                            return false;
                        }

                        menuItem2Found = true;
                    }
                }

                if (!menuItem1Found)
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Missing MenuId 679");
                    return false;
                }

                if (!menuItem2Found)
                {
                    acsDiagnosticTests.SendFailedTestMessage(true, "Expected MenuId 714");
                    return false;
                }
            }

            return true;
        }

        private void TestPostSiteCustomerV2(string signpostServer, string host, ref bool groupTestSuccess)
        {
            acsDiagnosticTests.SendMessage(true, ACSDiagnosticTests.MessageTypeEnum.StartTest, "Testing v2 POST customer");

            string siteCustomerUrl = 
                host + 
                "/customers/" + 
                ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername + 
                "?applicationid=" + 
                ConfigurationManager.AppSettings["DiagnosticsACSTestApplicationId"];

            string xml =
                "<Customer>" +
                    "<Title>MrX</Title>" +
                    "<FirstName>TestX</FirstName>" +
                    "<Surname>McTestX</Surname>" +
                    "<Address>" +
                        "<Prem1>prem1X</Prem1>" +
                        "<Prem2>prem2X</Prem2>" +
                        "<Prem3>prem3X</Prem3>" +
                        "<Prem4>prem4X</Prem4>" +
                        "<Prem5>prem5X</Prem5>" +
                        "<Prem6>prem6X</Prem6>" +
                        "<Org1>org1X</Org1>" +
                        "<Org2>org2X</Org2>" +
                        "<Org3>org3X</Org3>" +
                        "<RoadNum>roadNumX</RoadNum>" +
                        "<RoadName>roadNameX</RoadName>" +
                        "<City>cityX</City>" +
                        "<Town>townX</Town>" +
                        "<ZipCode>zipCodeX</ZipCode>" +
                        "<Postcode>postcodeX</Postcode>" +
                        "<County>countyX</County>" +
                        "<State>stateX</State>" +
                        "<Locality>localityX</Locality>" +
                        "<Country>United Kingdom</Country>" +
                        "<Directions>directionsX</Directions>" +
                    "</Address>" +
                    "<Contacts>" +
                        "<Contact>" +
                            "<Type>EMail</Type>" +
                            "<Value>" + ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername + "X</Value>" +
                            "<MarketingLevel>3rdParty</MarketingLevel>" +
                        "</Contact>" +
                        "<Contact>" +
                            "<Type>Mobile</Type>" +
                            "<Value>0123456789X</Value>" +
                            "<MarketingLevel>3rdParty</MarketingLevel>" +
                        "</Contact>" +
                    "</Contacts>" +
                "</Customer>";

            byte[] temp = System.Text.Encoding.UTF8.GetBytes("pass123");
            string encodedPassword = "Basic " + System.Convert.ToBase64String(temp);

            string responseXml = "";
            if (!CloudSync.HttpHelper.RestPost(siteCustomerUrl, xml, new Dictionary<string, string>() { { "Authorization", encodedPassword } }, out responseXml))
            {
                acsDiagnosticTests.SendFailedTestMessage(true, "REST call failed for: " + siteCustomerUrl + (responseXml.Length > 0 ? " with the error " + responseXml : ""));
                acsDiagnosticTests.SendMessage(false, ACSDiagnosticTests.MessageTypeEnum.Error, "ACS returned: " + System.Web.HttpUtility.HtmlEncode(responseXml));
                groupTestSuccess = false;
                return;
            }

            // Update the username
            ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername = ACSDiagnosticTests.GetACSDiagnosticTestData("V2_", signpostServer, host).CustomerUsername + "X";

            acsDiagnosticTests.SendSuccessfulTestMessage(true);
        }
    }
}