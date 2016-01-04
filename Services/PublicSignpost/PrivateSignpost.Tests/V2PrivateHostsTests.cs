using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrivateSignpost.Services;
using PrivateSignpost.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AndroCloudHelper;
using System.Net;
using SignpostDataAccessLayer;

namespace PrivateSignpost.Tests
{
    [TestClass]
    public class V2PrivateHostsTests
    {
        [TestMethod]
        public async Task BlankStoreId()
        {
            string andromedaSiteIdParameter = "";
            string licenseKey = "zzz";
            string hardwareKey = "zzz";

            DataTypes dataTypes = new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON);

            ResponseDetails responseDetails = await HostServices.GetHostsV2Async(
                new MockSignpostDataAccess(), dataTypes, andromedaSiteIdParameter, licenseKey, hardwareKey);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseDetails.httpStatusCode);
            Assert.AreEqual("{\"errorCode\":1006,\"message\":\"Missing siteId\"}", responseDetails.ResponseText);
        }

        [TestMethod]
        public async Task InvalidStoreId()
        {
            string andromedaSiteIdParameter = "zzz";
            string licenseKey = "zzz";
            string hardwareKey = "zzz";

            DataTypes dataTypes = new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON);

            ResponseDetails responseDetails = await HostServices.GetHostsV2Async(
                new MockSignpostDataAccess(), dataTypes, andromedaSiteIdParameter, licenseKey, hardwareKey);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseDetails.httpStatusCode);
            Assert.AreEqual("{\"errorCode\":1027,\"message\":\"Invalid site id\"}", responseDetails.ResponseText);
        }

        [TestMethod]
        public async Task BlankLicenseKey()
        {
            string andromedaSiteIdParameter = "123";
            string licenseKey = "";
            string hardwareKey = "zzz";

            DataTypes dataTypes = new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON);

            ResponseDetails responseDetails = await HostServices.GetHostsV2Async(
                new MockSignpostDataAccess(), dataTypes, andromedaSiteIdParameter, licenseKey, hardwareKey);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseDetails.httpStatusCode);
            Assert.AreEqual("{\"errorCode\":1009,\"message\":\"Missing licenseKey\"}", responseDetails.ResponseText);
        }

        [TestMethod]
        public async Task BlankHardwareKey()
        {
            string andromedaSiteIdParameter = "123";
            string licenseKey = "zzz";
            string hardwareKey = "";

            DataTypes dataTypes = new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON);

            ResponseDetails responseDetails = await HostServices.GetHostsV2Async(
                new MockSignpostDataAccess(), dataTypes, andromedaSiteIdParameter, licenseKey, hardwareKey);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseDetails.httpStatusCode);
            Assert.AreEqual("{\"errorCode\":1010,\"message\":\"Missing hardwareKey\"}", responseDetails.ResponseText);
        }

        [TestMethod]
        public async Task Valid()
        {
            string andromedaSiteIdParameter = "123";
            string licenseKey = "zzz";
            string hardwareKey = "zzz";

            DataTypes dataTypes = new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML);

            ResponseDetails responseDetails = await HostServices.GetHostsV2Async(
                new SignpostDataAccess(), dataTypes, andromedaSiteIdParameter, licenseKey, hardwareKey);

            Assert.AreEqual(HttpStatusCode.OK, responseDetails.httpStatusCode);

            // From mock database
            //  Assert.AreEqual("[{\"url\":\"y\",\"order\":0}]", responseDetails.ResponseText);

            // From real database
            Assert.AreEqual("<Hosts><Host><Type>EU Private ACS v1 Test 2</Type><Url>http://test2.androcloudservices.com/AndroCloudPrivateAPI/privateapi</Url><Version>1</Version><Order>1</Order></Host><Host><Type>EU Public ACS v1 Test 1</Type><Url>http://test1.androcloudservices.com/AndroCloudAPI/weborderapi</Url><Version>1</Version><Order>0</Order></Host><Host><Type>EU SignalR Server 1</Type><Url>http://test1.androcloudservices.com:8082/</Url><Version>0</Version><Order>0</Order></Host><Host><Type>EU Private ACS v1 Test 1</Type><Url>http://test1.androcloudservices.com/AndroCloudPrivateAPI/privateapi</Url><Version>1</Version><Order>0</Order></Host><Host><Type>EU Public ACS v2 Test 2</Type><Url>http://test2.androcloudservices.com/AndroCloudAPI/weborderapiv2</Url><Version>2</Version><Order>1</Order></Host><Host><Type>EU Private ACS v2 Test 1</Type><Url>http://test1.androcloudservices.com/AndroCloudPrivateAPI/privateapiv2</Url><Version>2</Version><Order>0</Order></Host><Host><Type>EU Public ACS v2 Test 1</Type><Url>http://test1.androcloudservices.com/AndroCloudAPI/weborderapiv2</Url><Version>2</Version><Order>0</Order></Host><Host><Type>EU Private ACS v2 Test 2</Type><Url>http://test2.androcloudservices.com/AndroCloudPrivateAPI/privateapiv2</Url><Version>2</Version><Order>1</Order></Host><Host><Type>EU Public ACS v1 Test 2</Type><Url>http://test2.androcloudservices.com/AndroCloudAPI/weborderapi</Url><Version>1</Version><Order>1</Order></Host><Host><Type>EU SignalR Server 2</Type><Url>http://test2.androcloudservices.com:8082/</Url><Version>0</Version><Order>1</Order></Host></Hosts>", responseDetails.ResponseText);
        }

    }
}
