using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Orders.OrderMonitoring.Services
{
    public interface IOrderMonitoringWindowsService
    {
        List<Guid> GetOrderIds(double minutes, int bufferTimeMinutes, int status);
    }
}
