using System;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Web.Controllers.Api.Data.Models
{
    public class DebugOrderViewModel
    {
        public static Expression<Func<OrderHeader, DebugOrderViewModel>> FromOrder
        {
            get
            {
                return r => new DebugOrderViewModel()
                {
                    StatusDescription = r.OrderStatu.Description,
                    ExternalOrderRef = r.ExternalOrderRef,
                    Id = r.ID,
                    Items = r.OrderLines.Count(),
                    FinalPrice = r.FinalPrice,
                    OrderPlacedTime = r.OrderPlacedTime,
                    OrderWantedTime = r.OrderWantedTime,
                    Customer = new CustomerOrderViewModel()
                    {
                        Id = r.Customer.ID,
                        Name = r.Customer.FirstName + " " + r.Customer.LastName,
                        Email = r.Customer.Contacts.Where(c => c.ContactTypeId == 0).Select(c => c.Value).FirstOrDefault(),
                        Phone = r.Customer.Contacts.Where(c => c.ContactTypeId == 1).Select(c => c.Value).FirstOrDefault(),
                        Latitude = r.Customer.Address.Lat,
                        Longitude = r.Customer.Address.Long,
                        Postcode = r.Customer.Address.PostCode
                    }
                };
            }
        }

        public string StatusDescription { get; set; }

        public string ExternalOrderRef { get; set; }

        public Guid Id { get; set; }

        public int Items { get; set; }

        public decimal FinalPrice { get; set; }

        public DateTime? OrderPlacedTime { get; set; }

        public DateTime? OrderWantedTime { get; set; }

        public CustomerOrderViewModel Customer { get; set; }
    }
}
