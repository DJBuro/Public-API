using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Andromeda.WebOrderTracking;
using System.IO;

namespace WebOrderTrackingTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void InvalidCredentials()
        {
            string expectedResponse = "{\"error\":{\"errorCode\":\"2\"}}";
            string response = WebOrderTrackingMethods.TrackOrder("291763e95b6b4f99874fdc4749564c85", "xxxxxxxx");

            // The call should succeed
            Assert.AreEqual(expectedResponse, response, "Incorrect response received");
        }

        [TestMethod]
        public void ValidCredentials()
        {
            string expectedResponse = "{\"orders\": [{\"status\":\"1\", \"storeLat\":\"51.36037\", \"storeLon\":\"-0.15022\", \"custLat\":\"51.36476\", \"custLon\":\"-0.1569\", \"personProcessing\":\"\" }]}";
            string response = WebOrderTrackingMethods.TrackOrder("291763e95b6b4f99874fdc4749564c85", "OTTEST");

            // The call should succeed
            Assert.AreEqual(expectedResponse, response, "Incorrect response received");

            response = WebOrderTrackingMethods.TrackOrderLocation("291763e95b6b4f99874fdc4749564c85", "OTTEST");

            Assert.AreEqual("{\"orders\": [{\"status\":\"2\"}]}", response, "Incorrect response received");
        }
    }
}
