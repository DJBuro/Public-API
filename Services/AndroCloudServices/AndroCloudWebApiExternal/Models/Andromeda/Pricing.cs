using System.Collections.Generic;

namespace AndroCloudWebApiExternal.Models.Andromeda
{
    public class Pricing
    {
        public int DeliveryCharge { get; set; }
        public int PriceBeforeDiscount { get; set; }
        //public int PriceAfterDiscount { get; set; }
        public int? FinalPrice { get; set; }

        public string DiscountType
        {
            get;
            set;
        }

        //public string InitialDiscountReason
        //{
        //    get;
        //    set;
        //}

        //public int DiscountTypeAmount { get; set; }
        public int DiscountAmount { get; set; }

        //public int TotalTax { get; set; }
        public bool PricesIncludeTax { get; set; }

        //public List<Discount> Discounts { get; set; }

        //public List<Tax> Taxes { get; set; }


        public decimal PaymentCharge { get; set; }
    }
}