using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Orders.OrderMonitoring.Services
{
    public class OrderMonitoringService : IOrderMonitoringService
    {
        private readonly IOrderMonitoringDataService orderMonitoringDataService;

        public OrderMonitoringService(IOrderMonitoringDataService orderMonitoringDataService)
        {
            this.orderMonitoringDataService = orderMonitoringDataService;
        }

        public List<OrderHeader> GetOrders(double minutes, int status)
        {
            return this.orderMonitoringDataService.GetOrders(minutes, status);
        }

        public OrderHeader GetOrderById(Guid orderId)
        {
            return this.orderMonitoringDataService.GetOrderById(orderId);
        }

        public void Update(OrderHeader model)
        {
            this.orderMonitoringDataService.Update(model);
        }

        public IQueryable<OrderHeader> List(System.Linq.Expressions.Expression<Func<OrderHeader, bool>> predicate)
        {
            return this.orderMonitoringDataService.List(predicate);
        }
    }
}
