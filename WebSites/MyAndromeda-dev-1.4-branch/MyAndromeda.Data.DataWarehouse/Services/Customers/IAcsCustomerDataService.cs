using MyAndromeda.Core;
//using MyAndromeda.Data.AcsOrders.Model;
using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DataWarehouse.Services.Customers
{
    public interface IAcsCustomerDataService : IDependency
    {
        Customer GetCustomerByAcsOrderId(Guid acsOrderId);
    }

    public class AcsCustomerDataService : IAcsCustomerDataService 
    {
        private readonly DataWarehouseEntities sharedDbContext;

        public AcsCustomerDataService(DataWarehouseEntities sharedDbContext) 
        {
            this.sharedDbContext = sharedDbContext;
        }

        public Customer GetCustomerByAcsOrderId(Guid acsOrderId)
        {
            var table = this.sharedDbContext.Customers;
            var query = table.Where(e=> e.OrderHeaders.Any(order => order.ACSOrderId == acsOrderId));
            var result = query.SingleOrDefault();

            return result;
        }
    }
}
