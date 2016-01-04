using MyAndromeda.Data.DataWarehouse.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Orders.OrderMonitoring.Services
{
    public class OrderMonitoringWindowsService : IOrderMonitoringWindowsService
    {
        private readonly IOrderMonitoringWindowsDataService orderMonitoringWindowsDataService;

        public OrderMonitoringWindowsService()
        {
            this.orderMonitoringWindowsDataService = new MyAndromeda.Data.DataWarehouse.Services.Orders.OrderMonitoringWindowsDataService();
        }

        public OrderMonitoringWindowsService(IOrderMonitoringWindowsDataService orderMonitoringWindowsDataService)
        {
            this.orderMonitoringWindowsDataService = orderMonitoringWindowsDataService;
        }

        public List<Guid> GetOrderIds(double minutes, int bufferTimeMinutes, int status)
        {
            return this.orderMonitoringWindowsDataService.GetOrderIds(minutes, bufferTimeMinutes, status);
        }
    }
}
