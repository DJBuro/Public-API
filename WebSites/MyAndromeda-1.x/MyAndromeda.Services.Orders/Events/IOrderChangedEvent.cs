using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromedaDataAccess.Domain;

namespace MyAndromeda.Services.Orders.Events
{
    public interface IOrderChangedEvent : ITransientDependency
    {
        void OrderStatusChanged(OrderHeader orderHeader, MyAndromeda.Data.DataWarehouse.Models.OrderStatu oldStatus);
    }
}
