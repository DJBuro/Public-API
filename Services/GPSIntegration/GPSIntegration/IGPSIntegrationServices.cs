using Andromeda.GPSIntegration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration
{
    /// <summary>
    /// Not very useful errors - not that you can do much if there is an error
    /// </summary>
    public enum ResultEnum
    {
        OK = 0,
        UnknownError = 1,
        NoStoreSettings = 100,
        Disabled = 200
    }

    public interface IGPSIntegrationServices
    {
        /// <summary>
        /// A simple call to the third party provider to check that the login details work
        /// </summary>
        /// <param name="storeConfiguration">The login details to test</param>
        /// <returns></returns>
        ResultEnum ValidateCredentials(StoreConfiguration storeConfiguration);

        /// <summary>
        /// Gets a stores third party provider configuration details for the specified store from the AndroAdmin db
        /// </summary>
        /// <param name="andromedaStoreId">An Andromeda store id</param>
        /// <param name="store">The third party provider login details</param>
        /// <returns></returns>
        ResultEnum GetStoreByAndromedaStoreId(int andromedaStoreId, out StoreConfiguration store);

        /// <summary>
        /// Updates the third party providers configuration in the AndroAdmin db
        /// </summary>
        /// <param name="store">The third party provider login details to update</param>
        /// <returns></returns>
        ResultEnum UpdateStore(StoreConfiguration store);

        /// <summary>
        /// Sends an order to the third party provider
        /// </summary>
        /// <param name="andromedaStoreId">The Andromeda store id at which the order was placed</param>
        /// <param name="customer">Details of the customer that placed the order</param>
        /// <param name="newOrder">The order that was placed</param>
        /// <returns></returns>
        ResultEnum CustomerPlacedOrder(int andromedaStoreId, Customer customer, Order newOrder, Action<string, DebugLevel> log);

        /// <summary>
        /// Assigns a driver to an existing order in the third party provider
        /// </summary>
        /// <param name="andromedaStoreId">The Andromeda store id at which the order was placed</param>
        /// <param name="externalOrderId">The order id returned by the third party provider when the order was originally created</param>
        /// <param name="driver">The driver that should be assigned to the order</param>
        /// <returns></returns>
        ResultEnum AssignDriverToOrder(int andromedaStoreId, string externalOrderId, int? bags, Driver driver, Action<string, DebugLevel> log);

        /// <summary>
        /// Cancels an order in the third party provider
        /// </summary>
        /// <param name="externalOrderId">The order id returned by the third party provider when the order was originally created</param>
        /// <returns></returns>
        ResultEnum CancelOrder(int andromedaStoreId, string bringgTaskId);
    }

    public enum DebugLevel 
    {
        Notify,
        Error
    }
}
