using MyAndromeda.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Services.Orders.Events
{
    public enum WorkLevel 
    {
        NoWork,
        CompletedWork
    }
    
    public interface IOrderChangedEvent : IDependency
    {
        string Name { get; }

        Task<WorkLevel> OrderStatusChangedAsync(int andromedaSiteId, OrderHeader orderHeader, MyAndromeda.Data.DataWarehouse.Models.OrderStatu oldStatus);
    }
}
