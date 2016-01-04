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
    public class HostGETUnitTests
    {
        [TestMethod]
        [Description("No site id")]
        public void Host_GET_NoSiteId_TestMethod()
        {
            // Test data
            string siteId = "";
            string licenseKey = "123";
            string hardwareKey = "123";
            string expectedXMLResponse =
                "<Error><ErrorCode>1006</ErrorCode><Message>Missing siteId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1006,\"message\":\"Missing siteId\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = Hosts.GetHosts(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML), 
                siteId,
                licenseKey,
                hardwareKey);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Hosts.GetHosts(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                siteId,
                licenseKey,
                hardwareKey);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Missing license key")]
        public void Host_GET_NoLicenseKey_TestMethod()
        {
            // Test data
            string siteId = "3559D139-E4F4-4BB1-B0FC-A774AB42DA27";
            string licenseKey = "";
            string hardwareKey = "123";
            string expectedXMLResponse =
                "<Error><ErrorCode>1009</ErrorCode><Message>Missing licenseKey</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1009,\"message\":\"Missing licenseKey\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = Hosts.GetHosts(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                siteId,
                licenseKey,
                hardwareKey);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Hosts.GetHosts(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                siteId,
                licenseKey,
                hardwareKey);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Missing hardware key")]
        public void Host_GET_NoHardwareKey_TestMethod()
        {
            // Test data
            string siteId = "3559D139-E4F4-4BB1-B0FC-A774AB42DA27";
            string licenseKey = "123";
            string hardwareKey = "";
            string expectedXMLResponse =
                "<Error><ErrorCode>1010</ErrorCode><Message>Missing hardwareKey</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1010,\"message\":\"Missing hardwareKey\"}";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = Hosts.GetHosts(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                siteId,
                licenseKey,
                hardwareKey);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Hosts.GetHosts(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                siteId,
                licenseKey,
                hardwareKey);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown site id")]
        public void Host_GET_UnknownSiteGuid_TestMethod()
        {
            // Test data
            string siteId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string licenseKey = "123";
            string hardwareKey = "zzz";
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
            string response = Hosts.GetHosts(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                siteId,
                licenseKey,
                hardwareKey);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Hosts.GetHosts(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                siteId,
                licenseKey,
                hardwareKey);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Invalid license key")]
        public void Host_GET_UnknownLicenseKey_TestMethod()
        {
            // Test data
            string siteId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string licenseKey = "asd";
            string hardwareKey = "zzz";
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
            string response = Hosts.GetHosts(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                siteId,
                licenseKey,
                hardwareKey);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Hosts.GetHosts(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                siteId,
                licenseKey,
                hardwareKey);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("No problems")]
        public void Host_GET_TestMethod()
        {
            // Test data
            string siteId = "A4423DA7-00A5-4290-8783-2246F1B3E4CD";
            string licenseKey = "123";
            string hardwareKey = "zzz";
            string expectedXMLResponse = "<Hosts><Host><Url>thisisatesturl</Url><Order>1</Order></Host></Hosts>";
            string expectedJSONResponse = "[{\"url\":\"thisisatesturl\",\"order\":1}]";

            // Mock up the data access
            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();
            TestHelper.MockSitesDataAccess(mock, true, true, "123", null);
            TestHelper.MockSiteMenusDataAccess(mock, true);
            TestHelper.MockHostDataAccess(mock);

            // Inject our mock data access factory into the service
            DataAccessHelper.DataAccessFactory = mock.Object;

            // Run the XML post
            string response = Hosts.GetHosts(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                siteId,
                licenseKey,
                hardwareKey);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = Hosts.GetHosts(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                siteId,
                licenseKey,
                hardwareKey);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }
    }
}

