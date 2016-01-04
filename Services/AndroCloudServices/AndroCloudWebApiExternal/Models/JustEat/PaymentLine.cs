namespace AndroCloudWebApiExternal.Models.JustEat
{
    public class PaymentLine
    {
        public string Type { get; set; }

        public decimal CardFee { get; set; }

        public decimal Value { get; set; }
    }
}