namespace AndroCloudWebApiExternal.Models.JustEat
{
    public class GroupedBasketItem
    {
        public BasketItem BasketItem { get; set; }

        public int Quantity { get; set; }

        public double CombinedPrice { get; set; }

        public int OrderSubId { get; set; }

        public object MenuCardNumber { get; set; }
    }
}