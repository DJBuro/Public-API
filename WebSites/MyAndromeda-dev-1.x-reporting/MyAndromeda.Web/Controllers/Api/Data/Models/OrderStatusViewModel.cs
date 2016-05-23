using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Web.Controllers.Api.Data.Models
{
    public class OrderStatusViewModel
    {
        public static Expression<Func<OrderHeader, OrderStatusViewModel>> FromOrder
        {
            get
            {
                return r => new OrderStatusViewModel()
                {
                    OrderStatusHistory = r.OrderStatusHistories.Select(s => new OrderStatus()
                    {
                        Id = s.Status,
                        Description = s.OrderStatu.Description,
                        ChangeDateTime = s.ChangedDateTime
                    })
                    .OrderByDescending(s => s.ChangeDateTime)
                    .ToList()
                };
            }
        }

        public List<OrderStatus> OrderStatusHistory { get; set; }

    }
}
