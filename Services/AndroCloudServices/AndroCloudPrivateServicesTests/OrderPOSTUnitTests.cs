//using System;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using AndroCloudServices.Domain;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using AndroCloudDataAccess.Domain;
//using Moq;
//using AndroCloudDataAccess;
//using AndroCloudHelper;
//using System.IO;
//using AndroCloudPrivateWCFServices;
//using AndroCloudPrivateWCFServices.Services;

//namespace AndroCloudPrivateServicesTests
//{
//    [TestClass]
//    public class OrderPOSTUnitTests
//    {
//        [TestMethod]
//        [Description("Missing order")]
//        public void OrderStatus_POST_NoOrder_TestMethod()
//        {
//            // Test data
//            string siteId = "123";
//            string internetOrderNumber = "123";
//            string licenseKey = "123";
//            string hardwareKey = "123";
//            string order = "";
//            string expectedXMLResponse =
//                "<Error><ErrorCode>1019</ErrorCode><Message>Missing order</Message></Error>";
//            string expectedJSONResponse =
//                "{\"errorCode\":1019,\"message\":\"Missing order\"}";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;

//            OrderStatusUpdate update = null;
//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update);

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;
//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(order)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey, 
//                out update2);

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }

//        [TestMethod]
//        [Description("Missing siteId")]
//        public void OrderStatus_POST_NoSiteId_TestMethod()
//        {
//            // Test data
//            string siteId = "";
//            string internetOrderNumber = "123";
//            string licenseKey = "123";
//            string hardwareKey = "123";
//            string orderXml = "<Order><Status>1</Status></Order>";
//            string orderJson = "{\"status\":1}";
//            string expectedXMLResponse =
//                "<Error><ErrorCode>1006</ErrorCode><Message>Missing siteId</Message></Error>";
//            string expectedJSONResponse =
//                "{\"errorCode\":1006,\"message\":\"Missing siteId\"}";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;

//            OrderStatusUpdate update = null;
//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderXml)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey, 
//                out update);

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;
//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderJson)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update2);

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }

//        [TestMethod]
//        [Description("Missing orderId")]
//        public void OrderStatus_POST_NoOrderId_TestMethod()
//        {
//            // Test data
//            string siteId = "123";
//            string internetOrderNumber = "";
//            string licenseKey = "123";
//            string hardwareKey = "123";
//            string orderXml = "<Order><Status>1</Status></Order>";
//            string orderJson = "{\"status\":1}";
//            string expectedXMLResponse =
//                "<Error><ErrorCode>1014</ErrorCode><Message>Missing orderId</Message></Error>";
//            string expectedJSONResponse =
//                "{\"errorCode\":1014,\"message\":\"Missing orderId\"}";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;

//            OrderStatusUpdate update = null;
//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderXml)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update);

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;
//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderJson)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update2);

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }

//        [TestMethod]
//        [Description("Invalid orderId")]
//        public void OrderStatus_POST_InvalidOrderId_TestMethod()
//        {
//            // Test data
//            string siteId = "123";
//            string internetOrderNumber = "zzz";
//            string licenseKey = "123";
//            string hardwareKey = "123";
//            string orderXml = "<Order><Status>1</Status></Order>";
//            string orderJson = "{\"status\":1}";
//            string expectedXMLResponse =
//                "<Error><ErrorCode>1015</ErrorCode><Message>Invalid orderId</Message></Error>";
//            string expectedJSONResponse =
//                "{\"errorCode\":1015,\"message\":\"Invalid orderId\"}";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;

//            OrderStatusUpdate update = null;
//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderXml)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update
//                );

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;
//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderJson)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update2
//                );

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }

//        [TestMethod]
//        [Description("Missing licenseKey")]
//        public void OrderStatus_POST_NoLicenseKey_TestMethod()
//        {
//            // Test data
//            string siteId = "123";
//            string internetOrderNumber = "123";
//            string licenseKey = "";
//            string hardwareKey = "123";
//            string orderXml = "<Order><Status>1</Status></Order>";
//            string orderJson = "{\"status\":1}";
//            string expectedXMLResponse =
//                "<Error><ErrorCode>1009</ErrorCode><Message>Missing licenseKey</Message></Error>";
//            string expectedJSONResponse =
//                "{\"errorCode\":1009,\"message\":\"Missing licenseKey\"}";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;

//            OrderStatusUpdate update = null;
//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderXml)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update);

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;
//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderJson)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update2);

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }

//        [TestMethod]
//        [Description("Missing hardwareKey")]
//        public void OrderStatus_POST_NoHardwareKey_TestMethod()
//        {
//            // Test data
//            string siteId = "123";
//            string internetOrderNumber = "123";
//            string licenseKey = "123";
//            string hardwareKey = "";
//            string orderXml = "<Order><Status>1</Status></Order>";
//            string orderJson = "{\"status\":1}";
//            string expectedXMLResponse =
//                "<Error><ErrorCode>1010</ErrorCode><Message>Missing hardwareKey</Message></Error>";
//            string expectedJSONResponse =
//                "{\"errorCode\":1010,\"message\":\"Missing hardwareKey\"}";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;

//            OrderStatusUpdate update = null;
//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderXml)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update);

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;
//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderJson)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update2);

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }

//        [TestMethod]
//        [Description("Unknown siteId")]
//        public void OrderStatus_POST_UnknownSiteId_TestMethod()
//        {
//            // Test data
//            string siteId = "123";
//            string internetOrderNumber = "123";
//            string licenseKey = "123";
//            string hardwareKey = "123";
//            string orderXml = "<Order><Status>1</Status></Order>";
//            string orderJson = "{\"status\":1}";
//            string expectedXMLResponse =
//                "<Error><ErrorCode>1007</ErrorCode><Message>Unknown siteId</Message></Error>";
//            string expectedJSONResponse =
//                "{\"errorCode\":1007,\"message\":\"Unknown siteId\"}";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;
//            TestHelper.MockSitesDataAccess(mock, false, true, "", null);

//            OrderStatusUpdate update = null;
//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderXml)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update);

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;
//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderJson)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update2);

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }

//        [TestMethod]
//        [Description("Unknown orderId")]
//        public void OrderStatus_POST_UnknownOrderId_TestMethod()
//        {
//            // Test data
//            string siteId = "123";
//            string internetOrderNumber = "999";
//            string licenseKey = "123";
//            string hardwareKey = "123";
//            string orderXml = "<Order><Status>1</Status></Order>";
//            string orderJson = "{\"status\":1}";
//            string expectedXMLResponse =
//                "<Error><ErrorCode>1016</ErrorCode><Message>Unknown orderId</Message></Error>";
//            string expectedJSONResponse =
//                "{\"errorCode\":1016,\"message\":\"Unknown orderId\"}";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;
//            TestHelper.MockSitesDataAccess(mock, true, true, licenseKey, null);
//            //TestHelper.MockOrdersDataAccess(mock, false, true, internetOrderNumber, "0");

//            OrderStatusUpdate update = null;
//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderXml)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update);

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;
//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderJson)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update2);

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }

//        [TestMethod]
//        [Description("Unknown licenseKey")]
//        public void OrderStatus_POST_UnknownLicenseKey_TestMethod()
//        {
//            // Test data
//            string siteId = "123";
//            string internetOrderNumber = "123";
//            string licenseKey = "123";
//            string hardwareKey = "123";
//            string orderXml = "<Order><Status>1</Status></Order>";
//            string orderJson = "{\"status\":1}";
//            string expectedXMLResponse =
//                "<Error><ErrorCode>1018</ErrorCode><Message>Unknown licenseKey</Message></Error>";
//            string expectedJSONResponse =
//                "{\"errorCode\":1018,\"message\":\"Unknown licenseKey\"}";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;
//            TestHelper.MockSitesDataAccess(mock, true, true, "", null);
//            //TestHelper.MockOrdersDataAccess(mock, true,true, internetOrderNumber, "0");

//            OrderStatusUpdate update = null;

//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderXml)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update);

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;

//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderJson)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update2);

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }

//        [TestMethod]
//        [Description("Invalid order status")]
//        public void OrderStatus_POST_InvalidOrderStatus_TestMethod()
//        {
//            // Test data
//            string siteId = "123";
//            string internetOrderNumber = "123";
//            string licenseKey = "123";
//            string hardwareKey = "123";
//            string ramesesOrderStatusId = "zzz";
//            string orderXml = "<Order><Status>" + ramesesOrderStatusId + "</Status></Order>";
//            string orderJson = "{\"status\":\"" + ramesesOrderStatusId + "\"}";
//            string expectedXMLResponse =
//                "<Error><ErrorCode>1021</ErrorCode><Message>Invalid orderStatusId</Message></Error>";
//            string expectedJSONResponse =
//                "{\"errorCode\":1021,\"message\":\"Invalid orderStatusId\"}";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;
//            TestHelper.MockSitesDataAccess(mock, true, true, "", null);
//            //TestHelper.MockOrdersDataAccess(mock, true, true, internetOrderNumber, "0");
//            //TestHelper.MockOrderStatusDataAccess(mock, true, "0");

//            OrderStatusUpdate update = null;
//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderXml)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey, 
//                out update);

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;
//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderJson)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey, 
//                out update2);

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }

//        [TestMethod]
//        [Description("Unknown order status")]
//        public void OrderStatus_POST_UnknownOrderStatus_TestMethod()
//        {
//            // Test data
//            string siteId = "123";
//            string internetOrderNumber = "123";
//            string licenseKey = "123";
//            string hardwareKey = "123";
//            string ramesesOrderStatusId = "999";
//            string orderXml = "<Order><Status>" + ramesesOrderStatusId + "</Status></Order>";
//            string orderJson = "{\"status\":" + ramesesOrderStatusId + "}";
//            string expectedXMLResponse =
//                "<Error><ErrorCode>1022</ErrorCode><Message>Unknown orderStatusId</Message></Error>";
//            string expectedJSONResponse =
//                "{\"errorCode\":1022,\"message\":\"Unknown orderStatusId\"}";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;
//            TestHelper.MockSitesDataAccess(mock, true, true, licenseKey, null);
//            //TestHelper.MockOrdersDataAccess(mock, true, true, internetOrderNumber, "0");
//            //TestHelper.MockOrderStatusDataAccess(mock, false, ramesesOrderStatusId);

//            OrderStatusUpdate update = null;
//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderXml)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update);

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;
//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderJson)),
//                siteId,
//                internetOrderNumber,
//                licenseKey,
//                hardwareKey, 
//                out update2);

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }

//        [TestMethod]
//        [Description("Dodgy data")]
//        public void OrderStatus_POST_DodgyData_TestMethod()
//        {
//            // Test data
//            string siteId = "123";
//            string externalOrderNumber = "123";
//            string licenseKey = "123";
//            string hardwareKey = "123";
//            string ramesesOrderStatusId = "1";
//            string orderXml = "<Order<Status>" + ramesesOrderStatusId + "</Status></Order>";
//            string orderJson = "{\"status:" + ramesesOrderStatusId + "}";
//            string expectedXMLResponse =
//                "<Error><ErrorCode>1023</ErrorCode><Message>Bad data: The '<' character, hexadecimal value 0x3C, cannot be included in a name. Line 1, position 7.</Message></Error>";
//            string expectedJSONResponse =
//                "{\"errorCode\":1023,\"message\":\"Bad data: Unterminated string. Expected delimiter: \". Path '', line 1, position 11.\"}";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();
// //           TestHelper.MockPartnersDataAccess(mock, true);
//            TestHelper.MockSitesDataAccess(mock, true, true, licenseKey, null);
//            //TestHelper.MockOrdersDataAccess(mock, true, true, externalOrderNumber, "0");
//            //TestHelper.MockOrderStatusDataAccess(mock, true, ramesesOrderStatusId);

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;

//            OrderStatusUpdate update = null;
//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderXml)),
//                siteId,
//                externalOrderNumber,
//                licenseKey,
//                hardwareKey, 
//                out update);

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;
//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderJson)),
//                siteId,
//                externalOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update2);

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }

//        [TestMethod]
//        [Description("No problems")]
//        public void OrderStatus_POST_TestMethod()
//        {
//            // Test data
//            string siteId = "123";
//            string externalOrderNumber = "123";
//            string licenseKey = "123";
//            string hardwareKey = "123";
//            string ramesesOrderStatusId = "1";
//            string orderXml = "<Order><Status>" + ramesesOrderStatusId + "</Status></Order>";
//            string orderJson = "{\"status\":" + ramesesOrderStatusId + "}";
//            string expectedXMLResponse = "";
//            string expectedJSONResponse = "";

//            // Mock up the data access
//            Mock<IDataAccessFactory> mock = TestHelper.GetMockDataAccessFactory();
// //           TestHelper.MockPartnersDataAccess(mock, true);
//            TestHelper.MockSitesDataAccess(mock, true, true, licenseKey, null);
//            //TestHelper.MockOrdersDataAccess(mock, true, true, externalOrderNumber, "0");
//            //TestHelper.MockOrderStatusDataAccess(mock, true, ramesesOrderStatusId);

//            // Inject our mock data access factory into the service
//            DataAccessHelper.DataAccessFactory = mock.Object;

//            OrderStatusUpdate update = null;
//            // Run the XML post
//            string response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderXml)),
//                siteId,
//                externalOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update);

//            // Success?
//            Assert.AreEqual(expectedXMLResponse, response);

//            OrderStatusUpdate update2 = null;
//            // Run the JSON post
//            response = Orders.PostOrder(
//                new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON),
//                new MemoryStream(UTF8Encoding.Default.GetBytes(orderJson)),
//                siteId,
//                externalOrderNumber,
//                licenseKey,
//                hardwareKey,
//                out update2);

//            // Success?
//            Assert.AreEqual(expectedJSONResponse, response);
//        }
//    }
//}
