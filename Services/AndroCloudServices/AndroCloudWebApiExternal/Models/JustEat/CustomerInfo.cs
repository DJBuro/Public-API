namespace AndroCloudWebApiExternal.Models.JustEat
{
    public class CustomerInfo
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        public string PhoneNumber { get; set; }

        public string TimeZone { get; set; }

        public int PreviousRestuarantOrderCount { get; set; }
    }

}