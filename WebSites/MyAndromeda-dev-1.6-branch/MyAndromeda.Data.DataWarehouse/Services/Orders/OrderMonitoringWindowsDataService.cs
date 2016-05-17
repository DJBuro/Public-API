using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public class OrderMonitoringWindowsDataService : IOrderMonitoringWindowsDataService
    {
        public List<Guid> GetOrderIds(double minutes, int bufferTime, int status)
        {
            using (DataWarehouseDbContext dataContext = new DataWarehouseDbContext())
            {
                DateTime dtTemp = DateTime.UtcNow.AddMinutes(minutes);
                
                List<Guid> orderList = dataContext.OrderHeaders
                    .Where(s => s.DestinationDevice.Equals("iBT8000", StringComparison.InvariantCultureIgnoreCase) && s.Status == status)
                    //.Where(s => s.OrderWantedTime < dtTemp)
                    //may be DbFunctions in a later version of EF :)
                    .Where(s => DbFunctions.AddMinutes(s.OrderPlacedTime, bufferTime) < DateTime.UtcNow)
                    .Select(x => x.ID)
                    .ToList();

                return orderList;
            }
        }
    }
}
