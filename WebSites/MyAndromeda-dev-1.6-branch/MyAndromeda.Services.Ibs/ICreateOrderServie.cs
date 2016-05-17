using MyAndromeda.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Framework.Dates;
using MyAndromeda.Framework.Helpers;
using MyAndromeda.Services.Ibs.Events;
using MyAndromeda.Services.Ibs.Models;
using MyAndromeda.Data.DataWarehouse.Models;
using System.Data.Entity;

namespace MyAndromeda.Services.Ibs
{
    public interface ICreateOrderService : IDependency
    {
        Task<AddOrderRequest> CreateOrderRequestModelAsync(int andromedaSiteId, OrderHeader orderHeader, CustomerResultModel customer); 
    }
}
