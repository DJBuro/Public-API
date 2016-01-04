using System.Collections.Generic;

namespace AndroCloudWebApiExternal.Models.Andromeda
{
    public class Customer
    {
        //public string title { get; set; }

        public string firstName { get; set; }

        public string surname { get; set; }

        public List<Contact> contacts { get; set; }

        public Address address { get; set; }
    }
}