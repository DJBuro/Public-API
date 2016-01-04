using System;
using MyAndromeda.Core;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Orders
{
    public interface IOrderUpdateService : IDependency 
    {
        /// <summary>
        /// Updates the order wanted time.
        /// </summary>
        /// <param name="externalSiteId">The external site id.</param>
        /// <param name="ramesesOrderId">The rameses order id.</param>
        void UpdateOrderWantedTime(string externalSiteId, int ramesesOrderId, DateTime toUniversalTime);

        /// <summary>
        /// Updates the order status.
        /// </summary>
        /// <param name="externalSiteId">The external site id.</param>
        /// <param name="orderId">The order id.</param>
        /// <param name="success">The success.</param>
        /// <param name="msg">The MSG.</param>
        Task UpdateOrderStatus(int andromeadSiteId, string externalSiteId, int orderId, bool success, string msg);
    }
}