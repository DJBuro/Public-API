using System;
using System.Collections.Generic;
using OrderTracking.Dao.Domain;

namespace OrderTracking.WebService.Dao
{

    public interface IOrderTracking 
    {

        //Tracker Functions
        Results GetTrackerNames(string userName, string password, out string[] trackers);

        Results GetTrackers(string userName, string password, out List<Tracker> trackers);

        // Driver Functions
        Results AddUpdateDriver(string userName, string password, 
            string externalDriverId, string driverName, string vehicle, string trackerName);

        Results GetDriver(string userName, string password, string externalDriverId, out Driver driver);//note: return order associated with drivers

        Results GetDrivers(string userName, string password, out List<Driver> drivers);

        //Order Functions
        Results AddOrderToDriver(string userName, string password, string externalOrderId, string externalDriverId, DateTime dateTimeAdded);

        Results RemoveOrderFromDriver(string userName, string password, string externalOrderId, string externalDriverId);

        Results AddOrder(string userName, string password, string orderName, string ticketNumber, string externalOrderId, string orderProcessor, string extraInformation, DateTime dateTimeCreated, List<OrderItem> orderItems, CustomerDetails customerDetails);
        
        Results GetAllStatuses(string userName, string password, out List<Status> status);

        Results UpdateOrderStatus(string userName, string password, string externalOrderId, int status, string processor, DateTime dateTimeChanged);

        Results GetUndeliveredOrders(string userName, string password, out List<Order> orders);

        Results GetOrder(
            string userName, 
            string password, 
            string externalOrderId, 
            out Order order, 
            out DateTime? orderTaken,
            out DateTime? orderDispatched);

        Results GetOrders(string userName, string password, out List<Order> orders);

        //Site Functions
        Results ClearStore(string userName, string password);

        //Log Functions
        Results Last20Logs(string userName, string password, out List<Log> logs);


        void zTEST();
    }

    public interface IOrderTrackingAdmin
    {
        Results AddTrackersToStore(string userName, string password);
        Results AddUpdateStore(string userName, string password, StoreDetails storeDetails);
        Results AddUpdateAccount(StoreDetails storeDetails);
    }

    public interface IMapTracking
    {
        ClientMapData GetClientOrder(string userName, string password, string credentials);
        ClientMapData GetClientOrderByCredentials(string credentials);
        StoreMapData GetPremisesData(string userName, string password);
        Store GetStore(string userName, string password);
    }
}
