using System.Collections.Generic;

namespace AndroCloudWebApiExternal.Models.JustEat
{
    public class PaymentInfo
    {
        public List<PaymentLine> PaymentLines { get; set; }

        public decimal Total { get; set; }

        public decimal TotalComplementary { get; set; }

        public string PaidDate { get; set; }

        public bool CashOnDelivery { get; set; }
    }
}