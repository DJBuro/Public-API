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
    public class Sync_GetDataVersionTests
    {
        [TestMethod]
        public async Task BlankSecretKey()
        {
            string secretKey = "";

            ResponseDetails responseDetails = await SyncServices.GetDataVersionAsync(new SignpostDataAccess(), secretKey);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseDetails.httpStatusCode);
            Assert.AreEqual("{\"errorCode\":5000,\"message\":\"Invalid secret key\"}", responseDetails.ResponseText);
        }

        [TestMethod]
        public async Task InvalidSecretKey()
        {
            string secretKey = "sdafasdfsdfsadfsad";

            ResponseDetails responseDetails = await SyncServices.GetDataVersionAsync(new SignpostDataAccess(), secretKey);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseDetails.httpStatusCode);
            Assert.AreEqual("{\"errorCode\":5000,\"message\":\"Invalid secret key\"}", responseDetails.ResponseText);
        }

        [TestMethod]
        public async Task Valid()
        {
            string secretKey = "CDEF9B9357CD47B7A87CDF510674C327";

            ResponseDetails responseDetails = await SyncServices.GetDataVersionAsync(new SignpostDataAccess(), secretKey);

            Assert.AreEqual(HttpStatusCode.OK, responseDetails.httpStatusCode);
            Assert.AreEqual("{ \"version\":666 }", responseDetails.ResponseText);
        }
    }
}
