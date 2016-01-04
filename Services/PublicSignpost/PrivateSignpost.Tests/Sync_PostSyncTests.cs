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
    public class Sync_PostSyncTests
    {
        [TestMethod]
        public async Task BlankSecretKey()
        {
            //string secretKey = "";
            //string syncJSON = "{}";

            //ResponseDetails responseDetails = await SyncServices.SyncAsync(new SignpostDataAccess(), secretKey, syncJSON);

            //Assert.AreEqual(HttpStatusCode.BadRequest, responseDetails.httpStatusCode);
            //Assert.AreEqual("{\"errorCode\":5000,\"message\":\"Invalid secret key\"}", responseDetails.ResponseText);
        }

        [TestMethod]
        public async Task InvalidSecretKey()
        {
            //string secretKey = "sdafasdfsdfsadfsad";
            //string syncJSON = "{}";

            //ResponseDetails responseDetails = await SyncServices.SyncAsync(new SignpostDataAccess(), secretKey, syncJSON);

            //Assert.AreEqual(HttpStatusCode.BadRequest, responseDetails.httpStatusCode);
            //Assert.AreEqual("{\"errorCode\":5000,\"message\":\"Invalid secret key\"}", responseDetails.ResponseText);
        }

        [TestMethod]
        public async Task ValidSecretKey_BlankJSON()
        {
            //string secretKey = "CDEF9B9357CD47B7A87CDF510674C327";
            //string syncJSON = "";

            //ResponseDetails responseDetails = await SyncServices.SyncAsync(new SignpostDataAccess(), secretKey, syncJSON);

            //Assert.AreEqual(HttpStatusCode.BadRequest, responseDetails.httpStatusCode);
            //Assert.AreEqual("{\"errorCode\":5001,\"message\":\"Missing sync JSON\"}", responseDetails.ResponseText);
        }

        [TestMethod]
        public async Task ValidSecretKey_InvalidJSON()
        {
            //string secretKey = "CDEF9B9357CD47B7A87CDF510674C327";
            //string syncJSON = "sdfsadfsafsadf";

            //ResponseDetails responseDetails = await SyncServices.SyncAsync(new SignpostDataAccess(), secretKey, syncJSON);

            //Assert.AreEqual(HttpStatusCode.BadRequest, responseDetails.httpStatusCode);
            //Assert.AreEqual("{\"errorCode\":5002,\"message\":\"Invalid sync JSON\"}", responseDetails.ResponseText);
        }

        [TestMethod]
        public async Task Valid()
        {
            //Random random = new Random();

            //string secretKey = "CDEF9B9357CD47B7A87CDF510674C327";
            //string syncJSON = 
            //    "{" +
            //        "\"fromVersion\":0," +
            //        "\"toVersion\":2," +
            //        "\"acsApplicationSyncs\":" +
            //        "[" +
            //            "{" +
            //                "\"id\": 1," +
            //                "\"name\":\"qa1.androtest.co.uk " + Guid.NewGuid().ToString() + "\"," +
            //                "\"environmentId\":\"35AE2862-94F4-42C1-A2C3-D2D7CCF98EB9\"," +
            //                "\"externalApplicationId\":\"123 " + Guid.NewGuid().ToString() + "\"," +
            //                "\"acsApplicationSites\":" +
            //                "[" +
            //                    random.Next(0, 999).ToString() + "," +
            //                    random.Next(0, 999).ToString() + "," +
            //                    random.Next(0, 999).ToString() +
            //                "]" +
            //            "}" +
            //        "]" +
            //    "}";

            //ResponseDetails responseDetails = await SyncServices.SyncAsync(new SignpostDataAccess(), secretKey, syncJSON);

            //Assert.AreEqual(HttpStatusCode.OK, responseDetails.httpStatusCode);
//            Assert.AreEqual("{\"errorCode\":5002,\"message\":\"Invalid sync JSON\"}", responseDetails.ResponseText);
        }
    }
}
