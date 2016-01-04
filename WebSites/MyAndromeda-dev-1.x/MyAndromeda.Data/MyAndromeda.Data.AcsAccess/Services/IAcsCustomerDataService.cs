using MyAndromeda.Core;
using MyAndromeda.Data.AcsOrders.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.AcsOrders.Services
{
    public interface IAcsCustomerDataService : IDependency
    {
        Customer GetCustomerByAcsOrderId(Guid acsOrderId);
    }

    public class AcsCustomerDataService : IAcsCustomerDataService 
    {
        private readonly IAcsOrdersDbContext sharedDbContext;

        public AcsCustomerDataService(IAcsOrdersDbContext sharedDbContext) 
        {
            this.sharedDbContext = sharedDbContext;
        }

        public Customer GetCustomerByAcsOrderId(Guid acsOrderId)
        {
            var table = this.sharedDbContext.GetContext().Customers;
            var query = table.Where(e=> e.OrderHeaders.Any(order => order.ACSOrderId == acsOrderId));
            var result = query.SingleOrDefault();

            return result;
        }
    }
}
