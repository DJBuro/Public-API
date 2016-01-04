using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Services.Orders.Models;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using MyAndromeda.Core;

namespace MyAndromeda.Services.Orders.Services
{
    public interface IOrderSupportService : IDependency
    {
        void UpdateOrders(IEnumerable<Guid> ids);

        IEnumerable<OrderMapModel> List(IEnumerable<Guid> ids);
        
    }
}