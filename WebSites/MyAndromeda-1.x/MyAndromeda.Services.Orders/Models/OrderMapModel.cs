using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;

namespace MyAndromeda.Services.Orders.Models
{
    public class OrderMapModel
    {
        public Store Store { get; set; }
        public OrderHeader[] Orders { get; set; }
    }
}
