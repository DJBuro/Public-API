using System;

namespace MyAndromeda.Web.Controllers.Api.Data.Models
{
    public class CustomerOrderViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Postcode { get; set; }
    }
}
