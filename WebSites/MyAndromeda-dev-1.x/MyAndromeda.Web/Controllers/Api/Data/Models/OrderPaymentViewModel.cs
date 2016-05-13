using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Web.Controllers.Api.Data.Models
{
    public class OrderPaymentViewModel
    {
        public static Expression<Func<OrderHeader, OrderPaymentViewModel>> FromOrder
        {
            get
            {
                return r => new OrderPaymentViewModel()
                {

                    PaymentLines = r.OrderPayments.Select(item => new PaymentLine()
                    {
                        Id = item.ID,
                        PayTypeName = item.PayTypeName,
                        PaymentType = item.PaymentType,
                        Value = item.Value,
                        Charge = item.PaymentCharge
                    }),
                };
            }
        }

        public IEnumerable<PaymentLine> PaymentLines { get; set; }
    }
}
