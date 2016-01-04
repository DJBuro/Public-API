using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public interface IOrderMonitoringWindowsDataService : ITransientDependency
    {
        /// <summary>
        /// Gets the order ids.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <param name="bufferTime">The buffer time. How long to ignore emails 'created date time' for before checking</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        List<Guid> GetOrderIds(double minutes, int bufferTime, int status);
    }
}
