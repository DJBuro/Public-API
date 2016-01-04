using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AndroCloudDataAccess;
using AndroCloudHelper;
using AndroCloudDataAccess.Domain;

namespace AndroCloudServicesTests
{
    [TestClass]
    public class SiteDetailsGETUnitTests
    {
        [TestMethod]
        [Description("No partner id")]
        public void SiteDetails_GET_NoPartnerId_TestMethod()
        {
            // Test data
            string partnerId = "";
            string applicationId = "";
            string siteId = null;
            string expectedXMLResponse =
                "<Error><ErrorCode>1000</ErrorCode><Message>Missing partnerId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1000,\"message\":\"Missing partnerId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, siteId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, siteId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown partner id")]
        public void SiteDetails_GET_UnknownPartnerId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "123";
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
            string response = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, siteId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, siteId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Missing site id")]
        public void SiteDetails_GET_MissingSiteId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = null;
            string expectedXMLResponse =
                "<Error><ErrorCode>1006</ErrorCode><Message>Missing siteId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1006,\"message\":\"Missing siteId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
 //           TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, false, true, "", null);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, siteId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, siteId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown site id")]
        public void SiteDetails_GET_UnknownSiteId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "123";
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

            // Run the XML get
            string response = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, siteId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, siteId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unauthorized siteId")]
        public void SiteDetails_GET_UnauthorizedSiteId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "123";
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

            // Run the XML get
            string response = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, siteId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, siteId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("No problem")]
        public void SiteDetails_GET_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "123";
            string expectedXMLResponse =
                "<SiteDetails>" +
                    "<SiteId>15a8ac10-0ac0-444f-9b47-7db2cc342d10</SiteId>" +
                    "<Name>Test Site</Name>" +
                    "<MenuVersion>95</MenuVersion>" +
                    "<IsOpen>true</IsOpen>" +
                    "<EstDelivTime>30</EstDelivTime>" +
                    "<TimeZone>UTC+1</TimeZone>" +
                    "<Phone>0123456789</Phone>" +
                    "<Address>" +
                        "<Long>654.321</Long>" +
                        "<Lat>123.456</Lat>" +
                        "<Prem1>test prem1</Prem1>" +
                        "<Prem2>test prem2</Prem2>" +
                        "<Prem3>test prem3</Prem3>" +
                        "<Prem4>test prem4</Prem4>" +
                        "<Prem5>test prem5</Prem5>" +
                        "<Prem6>test prem6</Prem6>" +
                        "<Org1>test org1</Org1>" +
                        "<Org2>test org2</Org2>" +
                        "<Org3>test org3</Org3>" +
                        "<RoadNum>test roadnum</RoadNum>" +
                        "<RoadName>test roadname</RoadName>" +
                        "<Town>test town</Town>" +
                        "<Postcode>test postcode</Postcode>" +
                        "<Dps>zzz</Dps>" +
                        "<County>Surrey</County>" +
                        "<Locality>test locality</Locality>" +
                        "<Country>UK</Country>" +
                   "</Address>" +
               "</SiteDetails>";
            string expectedJSONResponse =
                "{" +
                    "\"siteId\":\"15a8ac10-0ac0-444f-9b47-7db2cc342d10\"," +
                    "\"name\":\"Test Site\"," +
                    "\"menuVersion\":95," +
                    "\"isOpen\":true," +
                    "\"estDelivTime\":30," +
                    "\"timeZone\":\"UTC+1\"," +
                    "\"phone\":\"0123456789\"," +
                    "\"address\":" +
                        "{" +
                            "\"long\":\"654.321\"," +
                            "\"lat\":\"123.456\"," +
                            "\"prem1\":\"test prem1\"," +
                            "\"prem2\":\"test prem2\"," +
                            "\"prem3\":\"test prem3\"," +
                            "\"prem4\":\"test prem4\"," +
                            "\"prem5\":\"test prem5\"," +
                            "\"prem6\":\"test prem6\"," +
                            "\"org1\":\"test org1\"," +
                            "\"org2\":\"test org2\"," +
                            "\"org3\":\"test org3\"," +
                            "\"roadNum\":\"test roadnum\"," +
                            "\"roadName\":\"test roadname\"," +
                            "\"town\":\"test town\"," +
                            "\"postcode\":\"test postcode\"," +
                            "\"dps\":\"zzz\"," +
                            "\"county\":\"Surrey\"," +
                            "\"state\":null," +
                            "\"locality\":\"test locality\"," +
                            "\"country\":\"UK\"" +                            
                        "}," +
                    "\"paymentProvider\":null," +
                    "\"paymentClientId\":null," +
                    "\"paymentClientPassword\":null" +
               "}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
//            TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, true, true, "", null);
//            TestHelper.MockGroupsDataAccess(mock, true);
            AndroCloudHelper.MockSiteDetailsDataAccess(mock);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), partnerId, siteId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = AndroCloudWCFServices.Services.SiteDetails.GetSiteDetails(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), partnerId, siteId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }
    }
}
