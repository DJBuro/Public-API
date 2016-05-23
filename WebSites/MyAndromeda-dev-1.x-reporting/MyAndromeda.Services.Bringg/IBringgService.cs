using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Services.Bringg
{
    public interface IBringgService : IDependency
    {
        /// <summary>
        /// Determines whether bringg is configured for the specified andromeda site id.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <returns></returns>
        Task<bool> IsBringgConfigured(int andromedaSiteId);

        /// <summary>
        /// Whens to send order.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <returns></returns>
        Task<bool> ShallWeSendOrder(int andromedaSiteId, UsefulOrderStatus currentState);

        /// <summary>
        /// Adds the order async.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="orderId">The order id.</param>
        /// <returns></returns>
        Task AddOrderAsync(int andromedaSiteId, Guid orderId, bool addNotes);

        /// <summary>
        /// Updates the driver async.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="internalOrderId">The internal order id.</param>
        /// <param name="externalOrderId">The external order id.</param>
        /// <returns></returns>
        Task<UpdateDriverResult> UpdateDriverAsync(int andromedaSiteId, Guid? internalOrderId, string externalOrderId);

        /// <summary>
        /// Cancels the order.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="internalOrderId">The internal order id.</param>
        /// <param name="externalOrderId">The external order id.</param>
        /// <returns></returns>
        Task<bool> CancelOrder(int andromedaSiteId, Guid? internalOrderId, string externalOrderId);
    }
}
