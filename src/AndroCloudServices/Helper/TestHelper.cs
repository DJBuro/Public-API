using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using AndroCloudDataAccess.Domain;
using AndroCloudHelper;

namespace AndroCloudServices.Helper
{
    public class TestHelper
    {
        /// <summary>
        /// Create order
        /// </summary>
        /// <param name="externalSiteId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static Response CheckForStoreSignalRBypass(string externalSiteId, DataTypeEnum dataType)
        {
            Response response = null;

            if (TestHelper.Bypass(externalSiteId))
            {
                // Create a fake order to return to the caller
                Order order = new Order();
                order.StoreOrderId = Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", ""); // Return this so that the caller (iPhone app or whatever) can pass it back when they want to check the order status
                order.RamesesStatusId = 1; // Initally the order status will be "Order taken"

                // Serialize
                response = new Response(SerializeHelper.Serialize<Order>(order, dataType));
            }

            return response;
        }

        /// <summary>
        /// Order status check
        /// </summary>
        /// <param name="externalSiteId"></param>
        /// <param name="orderId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static Response CheckForStoreSignalRBypass(string externalSiteId, string orderId, DataTypeEnum dataType)
        {
            Response response = null;

            // Do we need to bypass for this store?
            if (TestHelper.Bypass(externalSiteId))
            {
                // Create a fake order to return to the caller
                Order order = new Order();
                order.StoreOrderId = orderId;
                order.RamesesStatusId = TestHelper.OrderStatus; // Cycle through order statuses for testing purposes

                // Serialize
                response = new Response(SerializeHelper.Serialize<Order>(order, dataType));
            }

            return response;
        }

        private static bool Bypass(string externalSiteId)
        {
            bool bypass = false;

            string signalRBypass = ConfigurationManager.AppSettings["SignalRBypass"];

            if (signalRBypass != null)
            {
                string[] signalRBypassStoresArray = signalRBypass.Split('|');
                List<string> signalRBypassStores = new List<string>(signalRBypassStoresArray);

                // Do we need to bypass for this store?
                if ((signalRBypassStores.Count == 1 && signalRBypassStores[0].Equals("ALL", StringComparison.CurrentCultureIgnoreCase)) ||
                    signalRBypassStores.Contains(externalSiteId))
                {
                    bypass = true;
                }
            }

            return bypass;
        }

        private static int orderStatus = 1;
        public static int OrderStatus 
        {
            get
            {
                if (TestHelper.orderStatus > 5)
                {
                    TestHelper.orderStatus = 1;
                }
                else
                {
                    TestHelper.orderStatus++;
                }

                return TestHelper.orderStatus;
            }
        }
    }
}
