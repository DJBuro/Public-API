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
        IQueryable<Customer> Query();

        //Customer GetCustomerByAcsOrderId(Guid acsOrderId);
    }

    public class AcsCustomerDataService : IAcsCustomerDataService 
    {
        private readonly DataWarehouseDbContext sharedDbContext;

        public AcsCustomerDataService(DataWarehouseDbContext sharedDbContext) 
        {
            this.sharedDbContext = sharedDbContext;
        }


        public IQueryable<Customer> Query()
        {
            var table =  this.sharedDbContext.Set<Customer>();

            return table;
        }
    }
}
