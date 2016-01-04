using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AndroCloudDataAccessEntityFramework;
using AndroCloudDataAccess;
using AndroCloudHelper;
using AndroCloudWCFServices.Services;
using AndroCloudWCFServices;
using AndroCloudServicesTests;

namespace AndroCloudServicesTests
{
    [TestClass]
    public class MenuGETUnitTests
    {
        [TestMethod]
        [Description("No partner id")]
        public void Menu_GET_NoPartnerId_TestMethod()
        {
            // Test data
            string partnerId = "";
            string applicationId = "";
            string siteId = "AC8A96E4-D0C3-4EC1-927B-36A0F6B0378E";
            string expectedXMLResponse =
                "<Error><ErrorCode>1000</ErrorCode><Message>Missing partnerId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1000,\"message\":\"Missing partnerId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = Menu.GetMenu(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = Menu.GetMenu(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("No site id")]
        public void Menu_GET_NositeId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "";
            string expectedXMLResponse =
                "<Error><ErrorCode>1006</ErrorCode><Message>Missing siteId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1006,\"message\":\"Missing siteId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = Menu.GetMenu(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = Menu.GetMenu(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown partner id")]
        public void Menu_GET_UnknownPartnerId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "AC8A96E4-D0C3-4EC1-927B-36A0F6B0378E";
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
            string response = Menu.GetMenu(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = Menu.GetMenu(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown site id")]
        public void Menu_GET_UnknownSiteId_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "AC8A96E4-D0C3-4EC1-927B-36A0F6B0378E";
            string expectedXMLResponse =
                "<Error><ErrorCode>1007</ErrorCode><Message>Unknown siteId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1007,\"message\":\"Unknown siteId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
//            TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, false, true, "", null);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = Menu.GetMenu(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = Menu.GetMenu(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Not allowed to access site")]
        public void Menu_GET_NotAllowedToAccessSite_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "AC8A96E4-D0C3-4EC1-927B-36A0F6B0378E";
            string expectedXMLResponse =
                "<Error><ErrorCode>1017</ErrorCode><Message>PartnerId is not authorized to access this siteId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1017,\"message\":\"PartnerId is not authorized to access this siteId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
 //           TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, true, false, "", null);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = Menu.GetMenu(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON get
            response = Menu.GetMenu(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("No menu for site")]
        public void Menu_GET_NoMenuForSiteGuid_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "AC8A96E4-D0C3-4EC1-927B-36A0F6B0378E";
            string expectedXMLResponse =
                "<Error><ErrorCode>1008</ErrorCode><Message>Menu not found</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1008,\"message\":\"Menu not found\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
//            TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, true, true, "", null);
            AndroCloudHelper.MockSiteMenusDataAccess(mock, false);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML get
            string response = Menu.GetMenu(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the XML get
            response = Menu.GetMenu(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);

        }

        [TestMethod]
        [Description("No problems")]
        public void Menu_GET_TestMethod()
        {
            // Test data
            string partnerId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string applicationId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string siteId = "AC8A96E4-D0C3-4EC1-927B-36A0F6B0378E";
            string expectedXMLResponse =
                "test menu data";
            string expectedJSONResponse =
                "test menu data";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = AndroCloudHelper.GetMockDataAccessFactory();
//            TestHelper.MockPartnersDataAccess(mock, true);
            AndroCloudHelper.MockSitesDataAccess(mock, true, true, "", null);
            AndroCloudHelper.MockSiteMenusDataAccess(mock, true);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML Get
            string actualResponse = Menu.GetMenu(new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, actualResponse);

            // Run the JSON Get
            actualResponse = Menu.GetMenu(new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON), siteId, partnerId, applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, actualResponse);
        }
    }
}

