using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDashboard.Mvc
{
    public enum ColumnsEnum
    {
        SalesPence = 1,  // DO NOT USE use 16 instead
        TotalDeliverySales = 2,
        TotalDeliveryOrders = 3,
        TotalNonDeliveryOrders = 4,
        DeliveredInLessThan30Min = 5,
        DeliveredInLessThan35Min = 6,
        DeliveredInLessThan45Min = 7,
        OTDInstore = 8,
        ToTheDoorTime = 9,
        AverageDriveTime = 10,
        Make = 11,
        DeliveredInLessThan20Min = 12,
        OrdersPerDeliveryDriver = 13,
        OrdersLastWeek = 14,
        OrdersLastYear = 15,
        FlashSalesNetVAT = 16,
        NetSalesLastWeek = 17,
        NetSalesLastYear = 18,
        FlashTickets = 19, // orders that have been taken, not necessarily cashed off
        TotalOrderCount = 20 // DO NOT USE use 19 instead
    }
}
