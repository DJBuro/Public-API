using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Orders.OrderMonitoring.Models
{
    public class OrderList
    {
        public DateTime? Created { get; set; }

        public List<Guid> OrderIds { get; set; }
    }
}
