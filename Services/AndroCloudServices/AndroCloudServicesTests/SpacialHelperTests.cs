using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AndroCloudWCFHelper;

namespace AndroCloudServicesTests
{
    [TestClass]
    public class SpacialHelperTests
    {
        [TestMethod]
        public void CalcDistanceBetweenTwoPoints()
        {
            // Andro Wallington to Buckingham Palace
            double distanceInKm = SpacialHelper.CalcDistanceBetweenTwoPoints(-0.151222, 51.360671d, -0.1416d, 51.501019d);

            Assert.AreEqual(15.62, distanceInKm);
        }
    }
}
