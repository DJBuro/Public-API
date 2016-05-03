using System;
using System.Collections.Generic;

namespace MyAndromeda.Web.Controllers.Api.Data
{

    public class OrderViewModel 
    {
        public CustomerViewModel Customer { get; set; }
        public DriverViewModel Driver { get; set; }
        public Guid Id { get; set; }

        public int ItemCount { get; set; }
        public decimal FinalPrice { get; set; }

        public DateTime? OrderPlacedTime { get; set; }
        public DateTime? OrderWantedTime { get; set; }
        
        public int? BringgId { get; set; }
        public OrderAddressViewModel OrderAddress { get; set; }

        public IEnumerable<OrderItem> Items { get; set; }

        public string StatusDescription { get; set; }

        public long IbsOrderId { get; set; }

        public IEnumerable<PaymentLine> PaymentLines { get; set; }

        public int? Tips { get; set; }

        public decimal DeliveryCharge { get; set; }

        public string CookingInstructions { get; set; }

        public string OrderNotes { get; set; }

        public string ExternalOrderRef { get; set; }
        public int? TicketNumber { get; set; }

        public List<OrderStatus> OrderStatusHistory { get; set; }

        public int? CardCharges { get; set; }
    }

}