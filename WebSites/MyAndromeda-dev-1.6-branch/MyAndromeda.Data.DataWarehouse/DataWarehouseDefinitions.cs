using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DataWarehouse
{
    public static class DataWarehouseDefinitions
    {
        public static class PayTypes 
        {
            public const string PayLater = "PAYLATER";
        }
        public static class OrderStatus 
        {
            public const string OrderCreated = "Order Created";
            public const string OrderHasBeenReceivedByTheStore = "Order has been received by the store";
            public const string OrderIsInOven = "Order is in oven";
            public const string OrderIsReadyForDispatch = "Order is ready for dispatch";
            public const string OrderIsOutForDelivery = "Order is out for delivery";
            public const string OrderHasBeenCompleted = "Order has been completed";
            public const string OrderHasBeenCancelled = "Order has been cancelled"; 
            public const string FutureOrder = "Future Order";
            public const string OrderRefusedStoreOffline = "Order Refused - Store Offline";
        }
    }

}
