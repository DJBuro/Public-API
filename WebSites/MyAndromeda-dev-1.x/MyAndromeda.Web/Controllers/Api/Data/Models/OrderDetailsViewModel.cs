using System;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Web.Controllers.Api.Data.Models
{
    public class OrderDetailsViewModel
    {
        public static Expression<Func<OrderHeader, OrderDetailsViewModel>> FromOrder
        {
            get
            {
                return r => new OrderDetailsViewModel()
                {
                    StatusDescription = r.OrderStatu.Description,
                    DeliveryCharge = r.DeliveryCharge,
                    CardCharges = r.OrderPayments.Sum(k => k.PaymentCharge),
                    Tips = r.Tips,
                    BringgId = r.BringgTaskId,
                    IbsOrderId = r.IbsOrders.OrderByDescending(d => d.CreatedAtUtc).Select(o => o.IbsOrderId).FirstOrDefault(),
                    TicketNumber = r.TicketNumber,
                    OrderPostCode = r.CustomerAddress.ZipCode,
                    DriverName = r.DriverName,
                    CookingInstructions = r.CookingInstructions,
                    CustomerDirections = r.Customer.Address.Directions,
                    OrderAddressDirections = r.CustomerAddress.Directions,
                    OrderNotes = r.OrderNotes
                };
            }
        }

        public string StatusDescription { get; set; }

        public decimal DeliveryCharge { get; set; }

        public int? CardCharges { get; set; }

        public int? Tips { get; set; }

        public int? BringgId { get; set; }

        public long IbsOrderId { get; set; }

        public int? TicketNumber { get; set; }

        public string OrderPostCode { get; set; }

        public string DriverName { get; set; }

        public string CookingInstructions { get; set; }

        public string CustomerDirections { get; set; }

        public string OrderAddressDirections { get; set; }

        public string OrderNotes { get; set; }
    }
}
