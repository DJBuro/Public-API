using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.WebApiServices.Models.Email
{
    public class SuccessMessage : Postal.Email 
    {
        public Customer Customer { get; set; }
        public Contact Contact { get; set; }

        public string Message { get; set; }
        public string DeliveryTime { get; set; }

        public MyAndromedaDataAccess.Domain.Site Site { get; set; }

        public OrderHeader Order { get; set; }
    }
}