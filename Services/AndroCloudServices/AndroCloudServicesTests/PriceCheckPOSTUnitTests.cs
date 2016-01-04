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
using System.IO;
using AndroCloudWCFServices;

namespace AndroCloudServicesTests
{
    [TestClass]
    public class PriceCheckPOSTUnitTests
    {
        [TestMethod]
        [Description("Missing partnerId")]
        public void PriceCheck_POST_NoPartnerId_TestMethod()
        {
            // Test data
            string partnerId = "";
            string applicationId = "";
            string siteId = "123";
            string order = "<order>test</order>";
            string expectedXMLResponse =
                "<Error><ErrorCode>1000</ErrorCode><Message>Missing partnerId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1000,\"message\":\"Missing partnerId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = PriceCheck.PutPriceCheck(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
                siteId,
                partnerId,
                applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = PriceCheck.PutPriceCheck(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
                siteId,
                partnerId,
                applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Missing siteId")]
        public void PriceCheck_POST_NoSiteId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "";
            string order = "<order>test</order>";
            string expectedXMLResponse =
                "<Error><ErrorCode>1006</ErrorCode><Message>Missing siteId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1006,\"message\":\"Missing siteId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = PriceCheck.PutPriceCheck(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
                siteId,
                partnerId,
                applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = PriceCheck.PutPriceCheck(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
                siteId,
                partnerId,
                applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown partnerId")]
        public void PriceCheck_POST_UnknownPartnerId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string order = "<order>test</order>";
            string expectedXMLResponse =
                "<Error><ErrorCode>1004</ErrorCode><Message>Unknown partnerId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1004,\"message\":\"Unknown partnerId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
 //           TestHelper.MockPartnersDataAccess(mock, false);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = PriceCheck.PutPriceCheck(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
                siteId,
                partnerId,
                applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = PriceCheck.PutPriceCheck(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
                siteId,
                partnerId,
                applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown siteId")]
        public void PriceCheck_POST_UnknownSiteId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "123";
            string order = "<order>test</order>";
            string expectedXMLResponse =
                "<Error><ErrorCode>1007</ErrorCode><Message>Unknown siteId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1007,\"message\":\"Unknown siteId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
 //           TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, false, true, "", null);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = PriceCheck.PutPriceCheck(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
                siteId,
                partnerId,
                applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = PriceCheck.PutPriceCheck(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
                siteId,
                partnerId,
                applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unauthorized siteId")]
        public void PriceCheck_POST_UnauthorizedSiteId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "123";
            string order = "<order>test</order>";
            string expectedXMLResponse =
                "<Error><ErrorCode>1017</ErrorCode><Message>PartnerId is not authorized to access this siteId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1017,\"message\":\"PartnerId is not authorized to access this siteId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
//            TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, true, false, "", null);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = PriceCheck.PutPriceCheck(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
                siteId,
                partnerId,
                applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = PriceCheck.PutPriceCheck(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
                siteId,
                partnerId,
                applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("No problems")]
        public void PriceCheck_POST_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "123";
            string order = "<order>test</order>";
            string expectedXMLResponse = "";
            string expectedJSONResponse = "";

// TODO Mock the signalr bit and return ok?

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
//            TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, true, true, "", null);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = PriceCheck.PutPriceCheck(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
                siteId,
                partnerId,
                applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = PriceCheck.PutPriceCheck(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
                siteId,
                partnerId,
                applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }
    }
}
