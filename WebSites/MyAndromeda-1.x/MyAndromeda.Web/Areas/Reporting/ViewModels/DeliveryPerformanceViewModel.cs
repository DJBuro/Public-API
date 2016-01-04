using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Reporting.ViewModels
{
    public class DeliveryPerformanceViewModel
    {
        public DateTime Time { get; set; }

        public long TotalDeliveryOrders { get; set; }
        public long Under15 { get; set; }
        public long Between15And20 { get; set; }
        public long Between20And25 { get; set; }
        public long Between25And30 { get; set; }
        public long Between30And35 { get; set; }
        public long Between35And45 { get; set; }
        public long Between45And60 { get; set; }
        public long Over60 { get; set; }
    }
}