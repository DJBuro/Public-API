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
    public class CustomerPUTUnitTests
    {
        private const string customerXML =
            "<Customer>" +
                "<Title>Mr</Title>" +
                "<Firstname>Test</Firstname>" +
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
                    "<Town>town</Town>" +
                    "<Postcode>postcode</Postcode>" +
                    "<County>county</County>" +
                    "<State>state</State>" +
                    "<Locality>locality</Locality>" +
                    "<Country>country</Country>" +
                "</Address>" +
                "<Contacts>" +
                "</Contacts>" +
            "</Customer>";

        private const string customerJSON = 
            "{" +
                "title: 'Mr', " +
                "firstname: 'Test', " +
                "surname: 'McTest'," +
                "address: " +
                "{" +
                    "prem1: 'prem1'," +
                    "prem2: 'prem2'," +
                    "prem3: 'prem3'," +
                    "prem4: 'prem4'," +
                    "prem5: 'prem5'," +
                    "prem6: 'prem6'," +
                    "org1: 'org1'," +
                    "org2: 'org2'," +
                    "org3: 'org3'," +
                    "roadNum: 'roadNum'," +
                    "roadName: 'roadName'," +
                    "town: 'town'," +
                    "postcode: 'postcode'," +
                    "county: 'county'," +
                    "state: 'state'," +
                    "locality: 'locality'," +
                    "country: 'country'" +
                "}," +
                "contacts:" +
                "[" +
                "]" +
            "}";

        [TestMethod]
        [Description("Missing username")]
        public void Customer_PUT_NoUsername_TestMethod()
        {
            // Test data
            string username = "";
            string password = "321";
            string applicationId = "12345";
            string expectedXMLResponse =
                "<Error><ErrorCode>1034</ErrorCode><Message>Missing username</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1034,\"message\":\"Missing username\"}";

            // Mock up the andro cloud data access
            Mock<AndroCloudDataAccess.IDataAccessFactory> androCloudMock = AndroCloudHelper.GetMockDataAccessFactory();

            // Mock up the data warehouse data access
            Mock<DataWarehouseDataAccess.IDataAccessFactory> dataWarehouseMock = DataWarehouseHelper.GetMockDataAccessFactory();
            DataWarehouseHelper.MockCustomerDataAccess(dataWarehouseMock, false);

            // Inject our mock data access factory into the service
            AndroCloudDataAccess.DataAccessHelper.DataAccessFactory = androCloudMock.Object;
            DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory = dataWarehouseMock.Object;

            // Run the XML post
            string response = AndroCloudWCFServices.Services.Customer.Put(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(CustomerPUTUnitTests.customerXML)),
                username,
                password,
                "",
                applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = AndroCloudWCFServices.Services.Customer.Put(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(CustomerPUTUnitTests.customerJSON)),
                "",
                username,
                password,
                applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Missing password")]
        public void Customer_PUT_NoPasword_TestMethod()
        {
            // Test data
            string username = "321";
            string password = "";
            string applicationId = "12345";
            string expectedXMLResponse =
                "<Error><ErrorCode>1035</ErrorCode><Message>Missing password</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1035,\"message\":\"Missing password\"}";

            // Mock up the andro cloud data access
            Mock<AndroCloudDataAccess.IDataAccessFactory> androCloudMock = AndroCloudHelper.GetMockDataAccessFactory();

            // Mock up the data warehouse data access
            Mock<DataWarehouseDataAccess.IDataAccessFactory> dataWarehouseMock = DataWarehouseHelper.GetMockDataAccessFactory();
            DataWarehouseHelper.MockCustomerDataAccess(dataWarehouseMock, false);

            // Inject our mock data access factory into the service
            AndroCloudDataAccess.DataAccessHelper.DataAccessFactory = androCloudMock.Object;
            DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory = dataWarehouseMock.Object;

            // Run the XML post
            string response = AndroCloudWCFServices.Services.Customer.Put(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(CustomerPUTUnitTests.customerXML)),
                "",
                username,
                password,
                applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = AndroCloudWCFServices.Services.Customer.Put(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(CustomerPUTUnitTests.customerJSON)),
                "",
                username,
                password,
                applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Missing applicationId")]
        public void Customer_PUT_NoApplicationId_TestMethod()
        {
            // Test data
            string username = "321";
            string password = "321";
            string applicationId = "";
            string expectedXMLResponse =
                "<Error><ErrorCode>1028</ErrorCode><Message>Missing applicationId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1028,\"message\":\"Missing applicationId\"}";

            // Mock up the andro cloud data access
            Mock<AndroCloudDataAccess.IDataAccessFactory> androCloudMock = AndroCloudHelper.GetMockDataAccessFactory();

            // Mock up the data warehouse data access
            Mock<DataWarehouseDataAccess.IDataAccessFactory> dataWarehouseMock = DataWarehouseHelper.GetMockDataAccessFactory();
            DataWarehouseHelper.MockCustomerDataAccess(dataWarehouseMock, false);

            // Inject our mock data access factory into the service
            AndroCloudDataAccess.DataAccessHelper.DataAccessFactory = androCloudMock.Object;
            DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory = dataWarehouseMock.Object;

            // Run the XML post
            string response = AndroCloudWCFServices.Services.Customer.Put(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(CustomerPUTUnitTests.customerXML)),
                "",
                username,
                password,
                applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = AndroCloudWCFServices.Services.Customer.Put(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(CustomerPUTUnitTests.customerJSON)),
                "",
                username,
                password,
                applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Unknown applicationId")]
        public void Customer_PUT_UnknownApplicationId_TestMethod()
        {
            // Test data
            string username = "321";
            string password = "321";
            string applicationId = "99999";
            string expectedXMLResponse =
                "<Error><ErrorCode>1029</ErrorCode><Message>Unknown applicationId</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1029,\"message\":\"Unknown applicationId\"}";

            // Mock up the andro cloud data access
            Mock<AndroCloudDataAccess.IDataAccessFactory> androCloudMock = AndroCloudHelper.GetMockDataAccessFactory();

            // Mock up the data warehouse data access
            Mock<DataWarehouseDataAccess.IDataAccessFactory> dataWarehouseMock = DataWarehouseHelper.GetMockDataAccessFactory();
            DataWarehouseHelper.MockCustomerDataAccess(dataWarehouseMock, false);

            // Inject our mock data access factory into the service
            AndroCloudDataAccess.DataAccessHelper.DataAccessFactory = androCloudMock.Object;
            DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory = dataWarehouseMock.Object;

            // Run the XML post
            string response = AndroCloudWCFServices.Services.Customer.Put(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(CustomerPUTUnitTests.customerXML)),
                "",
                username,
                password,
                applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = AndroCloudWCFServices.Services.Customer.Put(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(CustomerPUTUnitTests.customerJSON)),
                "",
                username,
                password,
                applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("Username already used")]
        public void Customer_PUT_UsernameAlreadyUsed_TestMethod()
        {
            // Test data
            string username = "321";
            string password = "321";
            string applicationId = "99999";
            string expectedXMLResponse =
                "<Error><ErrorCode>1041</ErrorCode><Message>Username already used</Message></Error>";
            string expectedJSONResponse =
                "{\"errorCode\":1041,\"message\":\"Username already used\"}";

            ACSApplication acsApplication = new ACSApplication()
            {
                ExternalApplicationId = "99999",
                Id = 1,
                Name = "Test ACS Application"
            };

            // Mock up the andro cloud data access
            Mock<AndroCloudDataAccess.IDataAccessFactory> androCloudMock = AndroCloudHelper.GetMockDataAccessFactory(acsApplication);

            // Mock up the data warehouse data access
            Mock<DataWarehouseDataAccess.IDataAccessFactory> dataWarehouseMock = DataWarehouseHelper.GetMockDataAccessFactory();
            DataWarehouseHelper.MockCustomerDataAccess(dataWarehouseMock, true);

            // Inject our mock data access factory into the service
            AndroCloudDataAccess.DataAccessHelper.DataAccessFactory = androCloudMock.Object;
            DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory = dataWarehouseMock.Object;

            // Run the XML post
            string response = AndroCloudWCFServices.Services.Customer.Put(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(CustomerPUTUnitTests.customerXML)),
                "",
                username,
                password,
                applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = AndroCloudWCFServices.Services.Customer.Put(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(CustomerPUTUnitTests.customerJSON)),
                "",
                username,
                password,
                applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }

        [TestMethod]
        [Description("No problems")]
        public void Customer_PUT_TestMethod()
        {
            // Test data
            string username = "321";
            string password = "321";
            string applicationId = "99999";
            string expectedXMLResponse = "";
            string expectedJSONResponse = "";

            ACSApplication acsApplication = new ACSApplication()
            {
                ExternalApplicationId = "99999",
                Id = 1,
                Name = "Test ACS Application"
            };

            // Mock up the andro cloud data access
            Mock<AndroCloudDataAccess.IDataAccessFactory> androCloudMock = AndroCloudHelper.GetMockDataAccessFactory(acsApplication);

            // Mock up the data warehouse data access
            Mock<DataWarehouseDataAccess.IDataAccessFactory> dataWarehouseMock = DataWarehouseHelper.GetMockDataAccessFactory();
            DataWarehouseHelper.MockCustomerDataAccess(dataWarehouseMock, false);

            // Inject our mock data access factory into the service
            AndroCloudDataAccess.DataAccessHelper.DataAccessFactory = androCloudMock.Object;
            DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory = dataWarehouseMock.Object;

            // Run the XML post
            string response = AndroCloudWCFServices.Services.Customer.Put(
                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
                new MemoryStream(UTF8Encoding.Default.GetBytes(CustomerPUTUnitTests.customerXML)),
                "",
                username,
                password,
                applicationId);

            // Success?
            Assert.AreEqual(expectedXMLResponse, response);

            // Run the JSON post
            response = AndroCloudWCFServices.Services.Customer.Put(
                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
                new MemoryStream(UTF8Encoding.Default.GetBytes(CustomerPUTUnitTests.customerJSON)),
                "",
                username,
                password,
                applicationId);

            // Success?
            Assert.AreEqual(expectedJSONResponse, response);
        }
    }
}
