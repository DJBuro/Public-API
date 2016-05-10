using System;
using System.Collections.Generic;
using MyAndromeda.Services.Orders.Models;
using MyAndromeda.Core;

namespace MyAndromeda.Services.Orders.Services
{
    public interface IOrderSupportService : IDependency
    {
        void UpdateOrders(IEnumerable<Guid> ids);

        IEnumerable<OrderMapModel> List(IEnumerable<Guid> ids);
        
    }
}