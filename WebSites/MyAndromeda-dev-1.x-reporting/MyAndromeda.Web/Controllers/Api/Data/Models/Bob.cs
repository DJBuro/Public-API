using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Controllers.Api.Data.Models
{
    public class MapResults
    {
        public int OrderCount { get; set; }

        public decimal Total { get; set; }

        public List<OrderSummary> Orders { get; set; }
        public CustomerViewModel Customer { get; set; }
        public OrderAddressViewModel OrderAddress { get; set; }
    }

}