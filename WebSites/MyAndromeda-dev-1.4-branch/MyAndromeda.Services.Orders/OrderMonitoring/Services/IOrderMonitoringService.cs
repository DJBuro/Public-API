using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Orders.OrderMonitoring.Services
{
    public interface IOrderMonitoringService : IDependency
    {
        /// <summary>
        /// Returns orders having OrderWantedTime(UTC) less-than specified number of minutes.
        /// </summary>
        /// <param name="minutes">Minutes</param>
        /// <param name="status">Status less-than 0 returns all orders</param>
        /// <returns></returns>
        List<OrderHeader> GetOrders(double minutes, int status);

        /// <summary>
        /// Lists the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IQueryable<OrderHeader> List(Expression<Func<OrderHeader, bool>> predicate);
        
        /// <summary>
        /// Gets the order by id.
        /// </summary>
        /// <param name="orderId">The order id.</param>
        /// <returns></returns>
        OrderHeader GetOrderById(Guid orderId);
        
        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Update(OrderHeader model);
    }
}
