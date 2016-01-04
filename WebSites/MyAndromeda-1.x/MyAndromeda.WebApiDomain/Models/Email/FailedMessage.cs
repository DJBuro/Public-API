using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.WebApiServices.Models.Email
{
    public class FailedMessage : Postal.Email
    {
        public OrderHeader Order { get; set; }
        public Customer Customer { get; set; }
        public Contact Contact { get; set; }

        public string Message { get; set; }

        public MyAndromedaDataAccess.Domain.Site Site { get; set; }
    }
}