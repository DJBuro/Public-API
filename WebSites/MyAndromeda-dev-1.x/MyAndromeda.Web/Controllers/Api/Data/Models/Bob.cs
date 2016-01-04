using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Controllers.Api.Data.Models
{
    public class Bob
    {
        public int OrderCount { get; set; }

        public decimal Total { get; set; }

        public List<OrderSummary> Orders { get; set; }
        public CustomerViewModel Customer { get; set; }
        public OrderAddressViewModel OrderAddress { get; set; }
    }

    public class OrderSummary 
    {
        public decimal FinalPrice { get; set; }
        public DateTime OrderWantedTime { get; set; }
    }

    public class StoreOrdersSummary 
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
        public int ReceivedByStore { get; set; }
        public int InOven { get; set; }
        public int ReadyToDispatch { get; set; }
        public int OutForDelivery { get; set; }
    }
}