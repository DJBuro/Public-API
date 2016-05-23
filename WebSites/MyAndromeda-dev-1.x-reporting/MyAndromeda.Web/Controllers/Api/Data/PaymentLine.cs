using System;

namespace MyAndromeda.Web.Controllers.Api.Data
{

    public class PaymentLine 
    {
        public Guid Id { get; set; }

        public string PayTypeName { get; set; }

        public int Value { get; set; }

        public string PaymentType { get; set; }
        public int? Charge { get; set; }
    }

}