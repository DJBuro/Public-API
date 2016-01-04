using System.Collections.Generic;

namespace AndroCloudWebApiExternal.Models.JustEat
{
    public class BasketInfo
    {
        public string BasketId { get; set; }

        public int MenuId { get; set; }

        public List<DiscountItem> Discounts { get; set; }

        public decimal SubTotal { get; set; }

        public decimal ToSpend { get; set; }

        public decimal MultiBuyDiscount { get; set; }

        public decimal Discount { get; set; }

        public decimal DeliveryCharge { get; set; }

        public double Total { get; set; }

        public List<GroupedBasketItem> GroupedBasketItems { get; set; }
    }
}