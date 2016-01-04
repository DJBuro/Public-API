using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.OrderMonitoring.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            #if (!DEBUG)
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new OrderMonitoringService() 
            };
            ServiceBase.Run(ServicesToRun);
            # else
            OrderMonitoringService service = new OrderMonitoringService();
            service.ProcessOrders();
            #endif
        }
    }
}
