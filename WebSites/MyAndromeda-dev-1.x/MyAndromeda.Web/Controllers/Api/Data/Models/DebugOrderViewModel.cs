using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Web.Controllers.Api.Data.Models
{
    public class DebugOrderViewModel
    {
        public static Expression<Func<OrderHeader, DebugOrderViewModel>> FromOrder
        {
            get
            {
                return r => new DebugOrderViewModel()
                {
                    StatusDescription = r.OrderStatu.Description,
                    Id = r.ID,
                    Items = r.OrderLines.Count(),
                    FinalPrice = r.FinalPrice,
                    OrderPlacedTime = r.OrderPlacedTime,
                    OrderWantedTime = r.OrderWantedTime,
                    CustomerName = r.Customer.FirstName + " " + r.Customer.LastName
                };
            }
        }

        public string StatusDescription { get; set; }

        public Guid Id { get; set; }

        public int Items { get; set; }

        public decimal FinalPrice { get; set; }

        public DateTime? OrderPlacedTime { get; set; }

        public DateTime? OrderWantedTime { get; set; }

        public string CustomerName { get; set; }
    }
}
