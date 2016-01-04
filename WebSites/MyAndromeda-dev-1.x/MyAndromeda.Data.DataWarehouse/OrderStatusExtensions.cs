using System;
using System.Linq;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Data.DataWarehouse
{
    public static class OrderStatusExtensions
    {
        public static UsefulOrderStatus GetState(int statusId) 
        {
            switch (statusId)
            {
                case 0: return UsefulOrderStatus.OrderCreated;
                case 1: return UsefulOrderStatus.OrderHasBeenReceivedByTheStore;
                case 2: return UsefulOrderStatus.OrderIsInOven;
                case 3: return UsefulOrderStatus.OrderIsReadyForDispatch;
                case 4: return UsefulOrderStatus.OrderIsOutForDelivery;
                case 5: return UsefulOrderStatus.OrderHasBeenCompleted;
                case 6: return UsefulOrderStatus.OrderHasBeenCancelled;
                case 8: return UsefulOrderStatus.FutureOrder;
                case 9: return UsefulOrderStatus.OrderRefusedStoreOffline;
                case 1000: return UsefulOrderStatus.HelpDeskHaveIt;
            }

            if (statusId > 1000)
            {
                return UsefulOrderStatus.PrinterRejection;
            }

            return UsefulOrderStatus.NoIdea;
        }

        public static UsefulOrderStatus GetState(this OrderHeader header) 
        {
            return GetState(header.Status);
        }

        public static string Describe(this UsefulOrderStatus status) 
        {
            switch ((int)status)
            {
                case 0: return "Order Created";
                case 1: return "Order has been received by the store";
                case 2: return "Order is in oven";
                case 3: return "Order is ready for dispatch";
                case 4: return "Order is out for delivery";
                case 5: return "Order has been completed";
                case 6: return "Order has been cancelled";
                case 8: return "Future Order";
                case 9: return "Order Refused - Store Offline";
                case 1000: return "At Support";
            }

            if ((int)status > 1000)
            {
                return "GPRS Printer rejection";
            }

            return "Unknown";
        }

        
    }
}
