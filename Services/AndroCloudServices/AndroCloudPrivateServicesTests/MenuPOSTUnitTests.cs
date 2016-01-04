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
using AndroCloudPrivateWCFServices.Services;
using AndroCloudHelper;
using System.IO;
using AndroCloudPrivateWCFServices;

namespace AndroCloudPrivateServicesTests
{
    [TestClass]
    public class MenuPOSTUnitTests
    {
        [TestMethod]
        [Description("No site id")]
        public void Menu_POST_NoSiteId_TestMethod()
        {
            // Test data
            string siteId = "";
            string licenseKey = "123";
            string hardwareKey = "123";
            string version = "1";
            string menu = "<menu>test</menu>";
            string expectedXMLResponse =
                "<Error><ErrorCode>1006</ErrorCode><Message>Missing siteId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1006,\"message\":\"Missing siteId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), 
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Missing license key")]
        public void Menu_POST_NoLicenseKey_TestMethod()
        {
            // Test data
            string siteId = "3559D139-E4F4-4BB1-B0FC-A774AB42DA27";
            string licenseKey = "";
            string hardwareKey = "123";
            string version = "1";
            string menu = "<menu>test</menu>";
            string expectedXMLResponse =
                "<Error><ErrorCode>1009</ErrorCode><Message>Missing licenseKey</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1009,\"message\":\"Missing licenseKey\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Missing hardware key")]
        public void Menu_POST_NoHardwareKey_TestMethod()
        {
            // Test data
            string siteId = "3559D139-E4F4-4BB1-B0FC-A774AB42DA27";
            string licenseKey = "123";
            string hardwareKey = "";
            string version = "1";
            string menu = "<menu>test</menu>";
            string expectedXMLResponse =
                "<Error><ErrorCode>1010</ErrorCode><Message>Missing hardwareKey</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1010,\"message\":\"Missing hardwareKey\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Missing version")]
        public void Menu_POST_NoVersion_TestMethod()
        {
            // Test data
            string siteId = "3559D139-E4F4-4BB1-B0FC-A774AB42DA27";
            string licenseKey = "123";
            string hardwareKey = "zzz";
            string version = "";
            string menu = "<menu>test</menu>";
            string expectedXMLResponse =
                "<Error><ErrorCode>1011</ErrorCode><Message>Missing version</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1011,\"message\":\"Missing version\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Invalid version")]
        public void Menu_POST_InvalidVersion_TestMethod()
        {
            // Test data
            string siteId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string licenseKey = "123";
            string hardwareKey = "zzz";
            string version = "zzz";
            string menu = "<menu>test</menu>";
            string expectedXMLResponse =
                "<Error><ErrorCode>1012</ErrorCode><Message>Version is not a valid Integer</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1012,\"message\":\"Version is not a valid Integer\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown site id")]
        public void Menu_POST_UnknownSiteGuid_TestMethod()
        {
            // Test data
            string siteId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string licenseKey = "123";
            string hardwareKey = "zzz";
            string version = "12";
            string menu = "<menu>test</menu>";
            string expectedXMLResponse =
                "<Error><ErrorCode>1007</ErrorCode><Message>Unknown siteId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1007,\"message\":\"Unknown siteId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();
            TestHelper.MockSitesDataAccess(mock, false, true, "", null);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Invalid license key")]
        public void Menu_POST_UnknownLicenseKey_TestMethod()
        {
            // Test data
            string siteId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string licenseKey = "asd";
            string hardwareKey = "zzz";
            string version = "12";
            string menu = "<menu>test</menu>";
            string expectedXMLResponse =
                "<Error><ErrorCode>1013</ErrorCode><Message>Invalid licenceKey</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1013,\"message\":\"Invalid licenceKey\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();
            TestHelper.MockSitesDataAccess(mock, true, true, "fffff", null);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("No problems")]
        public void Menu_POST_TestMethod()
        {
            // Test data
            string siteId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string licenseKey = "123";
            string hardwareKey = "zzz";
            string version = "12";
            string menu = "<menu>test</menu>";
            string expectedXMLResponse = "";
            string expectedJSONResponse = "";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();
            TestHelper.MockSitesDataAccess(mock, true, true, "123", null);
            TestHelper.MockSiteMenusDataAccess(mock, true);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Menus.PostMenu(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(menu)),
                siteId,
                licenseKey,
                hardwareKey,
                version);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }
    }
}

