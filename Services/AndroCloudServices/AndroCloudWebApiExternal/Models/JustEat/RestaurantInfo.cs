using System.Collections.Generic;

namespace AndroCloudWebApiExternal.Models.JustEat
{
    public class RestaurantInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public List<string> AddressLines { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        public string DispatchMethod { get; set; }

        public string EmailAddress { get; set; }
    }
}