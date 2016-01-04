using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using AndroCloudDataAccess;
using AndroCloudWCFServices.Services;
using AndroCloudHelper;
using AndroCloudWCFServices;

namespace AndroCloudServicesTests
{
    [TestClass]
    public class SiteUnitTests
    {
        [TestMethod]
        [Description("No partner id")]
        public void Sites_GET_NoPartnerId_TestMethod()
        {
            // Test data
            string partnerId = "";
            string applicationId = "";
            string maxDistance = null;
            string longitude = null;
            string latitude = null;
            string expectedXMLResponse =
                "<Error><ErrorCode>1000</ErrorCode><Message>Missing partnerId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1000,\"message\":\"Missing partnerId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown partner id")]
        public void Sites_GET_UnknownPartnerId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string maxDistance = null;
            string longitude = null;
            string latitude = null;
            string expectedXMLResponse =
                "<Error><ErrorCode>1004</ErrorCode><Message>Unknown partnerId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1004,\"message\":\"Unknown partnerId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
 //           TestHelper.MockPartnersDataAccess(mock, false);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown chain id")]
        public void Sites_GET_UnknownChainId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string maxDistance = null;
            string longitude = null;
            string latitude = null;
            string expectedXMLResponse =
                "<Error><ErrorCode>1005</ErrorCode><Message>Unknown groupId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1005,\"message\":\"Unknown groupId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
 //           TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, true, true, "", null);
 //           TestHelper.MockGroupsDataAccess(mock, false);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Max distance no longitude")]
        public void Sites_GET_MaxDistanceNoLongitude_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string maxDistance = "35";
            string longitude = null;
            string latitude = "4.5";
            string expectedXMLResponse =
                "<Error><ErrorCode>1001</ErrorCode><Message>Missing longitude</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1001,\"message\":\"Missing longitude\"}";

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Max distance no latitude")]
        public void Sites_GET_MaxDistanceNoLatitude_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string maxDistance = "35";
            string longitude = "4.5";
            string latitude = null;
            string expectedXMLResponse =
                "<Error><ErrorCode>1002</ErrorCode><Message>Missing latitude</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1002,\"message\":\"Missing latitude\"}";

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Longitude no max distance")]
        public void Sites_GET_LongitudeNoMaxDistance_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string maxDistance = null;
            string longitude = "0.5";
            string latitude = null;
            string expectedXMLResponse =
                "<Error><ErrorCode>1003</ErrorCode><Message>Missing maxDistance</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1003,\"message\":\"Missing maxDistance\"}";

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Latitide no max distance")]
        public void Sites_GET_LatitudeNoMaxDistance_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string maxDistance = null;
            string longitude = null;
            string latitude = "0.5";
            string expectedXMLResponse =
                "<Error><ErrorCode>1003</ErrorCode><Message>Missing maxDistance</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1003,\"message\":\"Missing maxDistance\"}";

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("No problem")]
        public void Sites_GET_NoFiltersTestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string maxDistance = null;
            string longitude = null;
            string latitude = null;
            string expectedXMLResponse =
                "<Sites>" +
                    "<Site>" +
                        "<SiteId>15a8ac10-0ac0-444f-9b47-7db2cc342d10</SiteId>" +
                        "<Name>test site</Name>" +
                        "<MenuVersion>43</MenuVersion>" +
                        "<IsOpen>true</IsOpen>" +
                        "<EstDelivTime>30</EstDelivTime>" +
                    "</Site>" +
                "</Sites>";
            string expectedJSONResponse =
                "[" +
                    "{" +
                        "\"siteId\":\"15a8ac10-0ac0-444f-9b47-7db2cc342d10\"," +
                        "\"name\":\"test site\"," +
                        "\"menuVersion\":43," +
                        "\"isOpen\":true," +
                        "\"estDelivTime\":30" +
                    "}" +
                "]";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
 //           TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, true, true, "", null);
 //           TestHelper.MockGroupsDataAccess(mock, true);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.Site.GetSites(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, maxDistance, longitude, latitude, null, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("No problem")]
        public void Sites_GET_ClosestSiteFiltersTestMethod()
        {
            // Bugger - can't test this as the distance calcs are done in the data access code which we'll be mocking......

            //// Test data
            //string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            //string chainId = null;
            //string maxDistance = "5";
            //string longitude = "-0.151222";
            //string latitude = "51.360671d";
            //string expectedXMLResponse =
            //    "<Sites>" +
            //        "<Site>" +
            //            "<SiteId>15a8ac10-0ac0-444f-9b47-7db2cc342d10</SiteId>" +
            //            "<Name>test site</Name>" +
            //            "<MenuVersion>43</MenuVersion>" +
            //            "<IsOpen>true</IsOpen>" +
            //            "<EstDelivTime>30</EstDelivTime>" +
            //        "</Site>" +
            //    "</Sites>";
            //string expectedJSONResponse =
            //    "[" +
            //        "{" +
            //            "\"siteId\":\"15a8ac10-0ac0-444f-9b47-7db2cc342d10\"," +
            //            "\"name\":\"test site\"," +
            //            "\"menuVersion\":43," +
            //            "\"isOpen\":true," +
            //            "\"estDelivTime\":30" +
            //        "}" +
            //    "]";

            //// wal post office
            //// lon -0.149688d
            //// lat 51.359873d

            //// palace
            //// lon -0.1416d
            //// lat 51.501019d

            //AndroCloudDataAccess.Domain.Site site = new AndroCloudDataAccess.Domain.Site();
            //site.EstDelivTime = 30;
            //site.IsOpen = true;
            //site.MenuVersion = 43;
            //site.Name = "test site";
            //site.LicenceKey = "";
            //site.ExternalId = "15a8ac10-0ac0-444f-9b47-7db2cc342d10";

            //AndroCloudDataAccess.Domain.Site site2 = new AndroCloudDataAccess.Domain.Site();
            //site2.EstDelivTime = 30;
            //site2.IsOpen = true;
            //site2.MenuVersion = 43;
            //site2.Name = "test site";
            //site2.LicenceKey = "";
            //site2.ExternalId = "15a8ac10-0ac0-444f-9b47-7db2cc342d10";

            //List<AndroCloudDataAccess.Domain.Site> sites = new List<AndroCloudDataAccess.Domain.Site>() { site, site2 };

            //// Mock up the data access
            //Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();
            //TestHelper.MockPartnersDataAccess(mock, true);
            //TestHelper.MockSitesDataAccess(mock, true, true, "", sites);
            //TestHelper.MockGroupsDataAccess(mock, true);

            //// Inject our mock data access factory into the service
            //DataAccessHelper.DataAccessFactory = mock.Object;

            //// Run the XML get
            //string response = AndroCloudWCFServices.Services.Site.GetSite(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, maxDistance, longitude, latitude);

            //// Success?
            //Assert.AreEqual(expectedXMLResponse, response);

            //// Run the JSON get
            //response = AndroCloudWCFServices.Services.Site.GetSite(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, maxDistance, longitude, latitude);

            //// Success?
            //Assert.AreEqual(expectedJSONResponse, response);
        }
    }
}
