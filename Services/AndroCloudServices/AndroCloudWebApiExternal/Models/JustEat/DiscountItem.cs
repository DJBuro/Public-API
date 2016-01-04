namespace AndroCloudWebApiExternal.Models.JustEat
{
    public class DiscountItem
    {
        public int Id { get; set; }

        public decimal Discount { get; set; }

        public string DiscountType { get; set; }

        public decimal QualifyingValue { get; set; }
    }
}