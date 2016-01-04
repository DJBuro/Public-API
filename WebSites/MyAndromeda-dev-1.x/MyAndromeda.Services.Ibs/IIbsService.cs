using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Services.Ibs.Models;

namespace MyAndromeda.Services.Ibs
{
    public interface IIbsService : IDependency
    {
        /// <summary>
        /// Gets the customer.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="orderHeader">The order header.</param>
        /// <returns></returns>
        Task<CustomerResultModel> GetCustomerAsync(int andromedaSiteId, OrderHeader orderHeader);

        /// <summary>
        /// Adds the customer.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="orderHeader">The order header.</param>
        /// <returns></returns>
        Task<CustomerResultModel> AddCustomerAsync(int andromedaSiteId, OrderHeader orderHeader);

        /// <summary>
        /// Adds the order async.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="orderHeader">The order header.</param>
        /// <param name="orderRequest">The order request.</param>
        /// <returns></returns>
        Task<AddOrderResult> AddOrderAsync(int andromedaSiteId, OrderHeader orderHeader, CustomerResultModel customer, AddOrderRequest orderRequest);

        /// <summary>
        /// Creates the order data.
        /// </summary>
        /// <param name="orderHeader">The order header.</param>
        /// <returns></returns>
        Task<AddOrderRequest> CreateOrderData(int andromedaSiteId, OrderHeader orderHeader, CustomerResultModel customer);

        /// <summary>
        /// Gets the menu async.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="orderHeader">The order header.</param>
        /// <returns></returns>
        Task<MenuResult> GetMenuAsync(int andromedaSiteId);

        /// <summary>
        /// Gets the locations.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <returns></returns>
        Task<Locations> GetLocations(int andromedaSiteId);
    }
}