using MyAndromeda.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Services.Orders.Events
{
    public interface IOrderChangedEvent : ITransientDependency
    {
        string Name { get; }
        Task OrderStatusChanged(OrderHeader orderHeader, MyAndromeda.Data.DataWarehouse.Models.OrderStatu oldStatus);
    }
}
