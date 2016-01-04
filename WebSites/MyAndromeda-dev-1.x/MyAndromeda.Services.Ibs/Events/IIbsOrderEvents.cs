using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Services.Ibs.Models;

namespace MyAndromeda.Services.Ibs.Events
{
    public interface IIbsOrderEvents : IDependency
    {
        Task OrderCreatingAsync(OrderHeader orderHeader, Models.CustomerResultModel customerResult);

        Task OrderCreatingFailedAsync(OrderHeader orderHeader, Models.CustomerResultModel customerResult, Exception ex);

        Task OrderRequestCreatedAsync(OrderHeader orderHeader, Models.CustomerResultModel customerResult, AddOrderRequest orderRequest);

        Task OrderSendingAsync(Models.LocationResult locationResult,
            Models.CustomerResultModel customerResult,
            Models.AddOrderRequest addOrderRequest);
        
        Task OrderSentAsync(
            Models.LocationResult locationResult, 
            Models.CustomerResultModel customerResult, 
            Models.AddOrderResult orderResult);

        Task OrderFailedAsync(
            Models.LocationResult locationResult, 
            Models.CustomerResultModel customerResult,
            Models.AddOrderFailure orderFailure
            );
    }

}
