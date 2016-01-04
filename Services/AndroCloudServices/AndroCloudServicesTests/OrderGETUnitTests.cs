using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AndroCloudDataAccess.Domain;
using Moq;
using AndroCloudDataAccess;
using AndroCloudWCFServices.Services;
using AndroCloudHelper;
using AndroCloudWCFServices;

namespace AndroCloudServicesTests
{
    [TestClass]
    public class OrderGETUnitTests
    {
        [TestMethod]
        [Description("Missing partner id")]
        public void Order_GET_NoPartnerId_TestMethod()
        {
            // Test data
            string partnerId = "";
            string applicationId = "";
            string externalSiteId = "";
            string externalOrderId = "01370541-97A6-4FBE-89D9-004A6331B46A";
            string expectedXMLResponse =
                "<Error><ErrorCode>1000</ErrorCode><Message>Missing partnerId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1000,\"message\":\"Missing partnerId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), externalSiteId, externalOrderId, partnerId, applicationId); 

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Missing order id")]
        public void Order_GET_NoOrderId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string externalOrderId = "";
            string externalSiteId = "456";
            string expectedXMLResponse =
                "<Error><ErrorCode>1014</ErrorCode><Message>Missing orderId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1014,\"message\":\"Missing orderId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown partner id")]
        public void Order_GET_UnknownPartnerId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string externalOrderId = "01370541-97A6-4FBE-89D9-004A6331B46A";
            string externalSiteId = "456";
            string expectedXMLResponse =
                "<Error><ErrorCode>1004</ErrorCode><Message>Unknown partnerId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1004,\"message\":\"Unknown partnerId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
//            TestHelper.MockPartnersDataAccess(mock, false);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Missing site id")]
        public void Order_GET_MissingSiteId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string externalOrderId = "01370541-97A6-4FBE-89D9-004A6331B46A";
            string externalSiteId = "";
            string expectedXMLResponse =
                "<Error><ErrorCode>1006</ErrorCode><Message>Missing siteId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1006,\"message\":\"Missing siteId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
 //           TestHelper.MockPartnersDataAccess(mock, false);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown order id")]
        public void Order_GET_UnknownOrderId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string externalOrderId = "123";
            string externalSiteId = "456";
            string expectedXMLResponse =
                "<Error><ErrorCode>1016</ErrorCode><Message>Unknown orderId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1016,\"message\":\"Unknown orderId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
//            TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, true, true, "", null);
            AndroCloudHelper.MockOrdersDataAccess(mock, false, true, externalOrderId, "0");

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Not allowed to access site")]
        public void Order_GET_NotAllowedToAccessSite_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string externalOrderId = "123";
            string externalSiteId = "456";
            string expectedXMLResponse =
                "<Error><ErrorCode>1017</ErrorCode><Message>PartnerId is not authorized to access this siteId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1017,\"message\":\"PartnerId is not authorized to access this siteId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
 //           TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, true, true, "", null);
            AndroCloudHelper.MockOrdersDataAccess(mock, true, false, externalOrderId, "0");

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("No problems")]
        public void Order_GET_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string externalOrderId = "123";
            string externalSiteId = "456";
            string status = "3";
            string expectedXMLResponse =
                "<Order><Status>" + status + "</Status><OrderId>" + externalOrderId + "</OrderId></Order>";
            string expectedJSONResponse =
                "{\"status\":" + status  + ",\"orderId\":\"" + externalOrderId + "\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
//            TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, true, true, "", null);
            AndroCloudHelper.MockOrdersDataAccess(mock, true, true, externalOrderId, status);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Order.GetOrder(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), externalSiteId, externalOrderId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }
    }
}
