using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public interface IOrderMonitoringDataService :  IDataProvider<OrderHeader>, IDependency
    {
        List<OrderHeader> GetOrders(double minutes, int status);
        OrderHeader GetOrderById(Guid orderId);
        void Update(Models.OrderHeader model);
    }
}
