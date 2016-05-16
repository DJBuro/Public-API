using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Web.Controllers.Api.Data.Models
{
    public class OrderFoodViewModel
    {
        public static Expression<Func<OrderHeader, OrderFoodViewModel>> FromOrder
        {
            get
            {
                return r => new OrderFoodViewModel()
                {
                    Items = r.OrderLines.Select(item => new OrderItem()
                    {
                        Id = item.ID,
                        Name = item.Description,
                        Qty = item.Qty,
                        Price = item.Price,
                        Person = item.Person
                    }),
                    SubTotal = r.OrderLines.Select(item => item.Price).Sum()
                };
            }
        }

        public IEnumerable<OrderItem> Items { get; set; }

        public int? SubTotal { get; set; }
    }
}
