using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroCloudWebApiExternal.Models.JustEat
{
    public class JustEatOrder
    {
        public string Id { get; set; }
        public int CustomerOrderId { get; set; }
        public Order Order { get; set; }
        public RestaurantInfo RestaurantInfo { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
        public BasketInfo BasketInfo { get; set; }
        public bool IsAMiniFistPumpOrder { get; set; }
    }
}