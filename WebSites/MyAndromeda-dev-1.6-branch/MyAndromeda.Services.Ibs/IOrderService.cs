using System;
using System.Threading.Tasks;
using MyAndromeda.Services.Ibs.Models;
using MyAndromeda.Core;

namespace MyAndromeda.Services.Ibs
{
    public interface IOrderService : ITransientDependency
    {
        Task<AddOrderResult> SendOrder(
            int andromedaSiteId,
            TokenResult token, 
            LocationResult location, 
            Models.CustomerResultModel customer,
            AddOrderRequest request);
    }
}
