using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicSignpost.Services;
using PublicSignpost.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AndroCloudHelper;
using System.Net;
using SignpostDataAccessLayer;

namespace PublicSignpost.Tests
{
    [TestClass]
    public class V1PublicHostsTests
    {
        [TestMethod]
        public async Task BlankApplicationId()
        {
            string partnerId = "";
            string applicationId = "";

            DataTypes dataTypes = new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON);

            ResponseDetails responseDetails = await HostServices.GetHostsV1Async(new MockSignpostDataAccess(), dataTypes, partnerId, applicationId);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseDetails.httpStatusCode);
            Assert.AreEqual("{\"errorCode\":1028,\"message\":\"Missing applicationId\"}", responseDetails.ResponseText);
        }

        [TestMethod]
        public async Task InvalidApplicationId()
        {
            string partnerId = "";
            string applicationId = "1111111111";

            DataTypes dataTypes = new DataTypes(DataTypeEnum.JSON, DataTypeEnum.JSON);

            ResponseDetails responseDetails = await HostServices.GetHostsV1Async(new MockSignpostDataAccess(), dataTypes, partnerId, applicationId);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseDetails.httpStatusCode);
            Assert.AreEqual("{\"errorCode\":1029,\"message\":\"Unknown applicationId\"}", responseDetails.ResponseText);
        }

        [TestMethod]
        public async Task Valid()
        {
            string partnerId = "";
            string applicationId = "123";

            DataTypes dataTypes = new DataTypes(DataTypeEnum.XML, DataTypeEnum.XML);

            ResponseDetails responseDetails = await HostServices.GetHostsV1Async(new SignpostDataAccess(), dataTypes, partnerId, applicationId);

            Assert.AreEqual(HttpStatusCode.OK, responseDetails.httpStatusCode);

            // From mock database
          //  Assert.AreEqual("[{\"url\":\"y\",\"order\":0}]", responseDetails.ResponseText);

            // From real database
            Assert.AreEqual("[{\"url\":\"\",\"order\":0}]", responseDetails.ResponseText);
        }

    }
}
