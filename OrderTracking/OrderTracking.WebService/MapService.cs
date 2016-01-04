using System;
using System.Collections.Generic;
using OrderTracking.Core;
using OrderTracking.Dao;
using OrderTracking.Dao.Domain;
using OrderTracking.WebService.Dao;
using OrderTracking.WebService.Gps;

namespace OrderTracking
{
    public class MapService : IMapTracking
    {
        public IAccountDao AccountDao { get; set; }
        public ICustomerDao CustomerDao { get; set; }
        public IDriverDao DriverDao { get; set; }
        public IOrderDao OrderDao { get; set; }

        #region IMapTracking Members

        public ClientMapData GetClientOrderByCredentials(string credentials)
        {
            var clientMapData = new ClientMapData();
            var customer = CustomerDao.FindByUserCredentialsOnly(credentials);

           if (customer == null)
                return clientMapData;

            //if this isn't 1, we have multiple matches... which are bad...
            if (customer.CustomerOrders.Count == 1)
            {
                foreach (CustomerOrder customerOrder in customer.CustomerOrders)
                {
                    var acc = AccountDao.FindByStoreRamesesId(customerOrder.Order.Store.ExternalStoreId);

                    clientMapData = GetClientOrder(acc.UserName, acc.Password, credentials);
                }
            }

            return clientMapData;
        }

        public ClientMapData GetClientOrder(string userName, string password, string credentials)
        {
            var clientMapData = new ClientMapData();

            Account account;
            if (AccountDao.Verify(userName, password, out account))
            {
                var customer = CustomerDao.FindByUserCredentials(credentials, account.Store);

                if (customer == null)
                    return null;

                if (customer.Coordinates == null)
                    customer.Coordinates = new Coordinates();

                clientMapData.Customer = new DeliveryCustomer(customer.Name,customer.Coordinates.Longitude,customer.Coordinates.Latitude, customer.Credentials);
                clientMapData.Premises = new DeliveryPremises(account.Store.ExternalStoreId,account.Store.Name,account.Store.Coordinates.Longitude,account.Store.Coordinates.Latitude);

                Driver driver;

                clientMapData.DeliveryOrders = new List<DeliveryOrder>();

                foreach (CustomerOrder customerOrder in customer.CustomerOrders)
                {
                    // Get the order status
                    var orderStatus = (OrderStatus)customerOrder.Order.OrderStatus[0];
                    
                    // Is the order proximity delivered or cashed off?
                    if (customerOrder.Order.ProximityDelivered != null || orderStatus.Status.Id > 4)
                    {
                        // Yep - skip the order
                        continue;
                    }
                
                    DeliveryOrder deliveryOrder = new DeliveryOrder {Items = null};

                    deliveryOrder.OrderStatusTime = string.Format("{0:f}", orderStatus.Time);
                    deliveryOrder.OrderStatus = orderStatus.Status.Id.Value.ToString();
                    deliveryOrder.OrderProcessor = orderStatus.Processor == null ? "" : orderStatus.Processor;
                    deliveryOrder.StatusUpdates = new List<DeliveryStatusUpdate>();
                    
                    // Add all status updates for this order
                    foreach (OrderStatus orderStatusUpdate in customerOrder.Order.OrderStatus)
                    {
                        DeliveryStatusUpdate deliveryStatusUpdate = new DeliveryStatusUpdate();
                        deliveryStatusUpdate.Processor = orderStatusUpdate.Processor;
                        deliveryStatusUpdate.StatusId = orderStatusUpdate.Status.Id.Value;
                        deliveryStatusUpdate.Time = orderStatusUpdate.Time;
                        
                        deliveryOrder.StatusUpdates.Add(deliveryStatusUpdate);
                    }

                    // Add the order lines
                    if (orderStatus.Order.Items.Count > 0)
                    {
                        deliveryOrder.Items = new List<string>(orderStatus.Order.Items.Count);

                        foreach (Item item in orderStatus.Order.Items)
                        {
                            deliveryOrder.Items.Add(item.Name);
                        }
                    }
                       
                    // Get the driver details - we need the details of the tracker assigned to the driver
                    driver = DriverDao.FindByOrder(customerOrder.Order, account.Store);

                    // Is a driver assigned to this order?                        
                    if (driver != null)
                    {
                        var tracker = (Tracker)driver.Trackers[0];
                        tracker = account.GetTracker(tracker.Name);

                        if (tracker != null)
                        {
                            clientMapData.DeliveryDriver = new DeliveryDriver();
                            clientMapData.DeliveryDriver.HasFix = (tracker.Status.Id == 1) ? true : false;
                            clientMapData.DeliveryDriver.Longitude = tracker.Coordinates.Longitude;
                            clientMapData.DeliveryDriver.Latitude = tracker.Coordinates.Latitude;
                        }
                    }
                    
                    // Work out whether the order has been proximity delivered
                    deliveryOrder.ProximityDelivered = false;

                    var order = customerOrder.Order;
                    
                    if (order.ProximityDelivered != null)
                    {
                        // Set all proximity delivered orders to delivered.
                        this.UpdateOrderAsProximityDelivered(order, account.Store);

                        deliveryOrder.ProximityDelivered = true;
                        clientMapData.OrderStatusTime = string.Format("{0:f}",order.ProximityDelivered);
                    }
        
                    clientMapData.DeliveryOrders.Add(deliveryOrder);
                }
            }

            // Does the client have any orders?
            if (clientMapData.DeliveryOrders.Count > 0)
            {
                // Return the first order as the current order
                // This is required by the customer facing order tracking website
                clientMapData.DeliveryOrder = clientMapData.DeliveryOrders[0];
                clientMapData.OrderStatus = clientMapData.DeliveryOrder.OrderStatus;
                clientMapData.OrderProcessor = clientMapData.DeliveryOrder.OrderProcessor;
                clientMapData.OrderStatusTime = clientMapData.DeliveryOrder.OrderStatusTime;
            }

            return clientMapData;
        }

        public Store GetStore(string userName, string password)
        {
            Account account;
            if (AccountDao.Verify(userName, password, out account))
            {
                account.Store.Id = null;
                account.Store.Drivers = null;
                account.Store.Orders = null;
                account.Store.Trackers = null;
                account.Store.Coordinates.Id = null;
                
                return account.Store;
            }

            return null;
        }


        public StoreMapData GetPremisesData(string userName, string password)
        {
            var storeMapData = new StoreMapData();
            var proximityDelivered = false;

            Account account;
            if (AccountDao.Verify(userName, password, out account))
            {
                storeMapData.PremisesDrivers = new List<PremisesDriver>();

                var drivers = account.Store.Drivers;

                var premDrivers = new List<PremisesDriver>();

                var trackers = account.GetTrackers();

                foreach (Driver driver in drivers)
                {
                    // if (driver.Trackers == null || driver.Trackers.Count == 0 || !account.GpsEnabled) continue;
                    if (account.GpsEnabled)
                    {
                        if (driver.Trackers != null && driver.Trackers.Count > 0)
                        {
                            var tracker = (Tracker)driver.Trackers[0];

                            var tracker2 = trackers.Find(c => c.Name == tracker.Name);

                            if (tracker2 != null)
                            {
                                premDrivers.Add(new PremisesDriver
                                                    {
                                                        ExternalId = driver.ExternalDriverId,
                                                        HasFix = (tracker2.Status.Id == 1) ? true : false,
                                                        BatteryLevel = tracker2.BatteryLevel,
                                                        LastUpdate = tracker2.LastUpdate,
                                                        Speed = tracker2.Speed,
                                                        Longitude =
                                                            (tracker2.Coordinates == null)
                                                                ? 0
                                                                : tracker2.Coordinates.Longitude,
                                                        Latitude =
                                                            (tracker2.Coordinates == null)
                                                                ? 0
                                                                : tracker2.Coordinates.Latitude
                                                    });
                            }
                        }
                    }
                }

                storeMapData.PremisesDrivers = premDrivers;

                foreach (var driver in premDrivers)
                {
                    var orders = DriverDao.FindOrders(driver, account.Store);

                    if (orders == null) continue;
                    foreach (var order in orders)
                    {
                        var customerOrder = (CustomerOrder)order.CustomerOrder[0];
                        
                        if(driver.Deliveries == null)
                        {
                            driver.Deliveries = new List<Delivery>();
                        }

                        // Get the dispatched date/time
                        long orderDispatchedTicks = 0;
                        foreach (OrderStatus status in order.OrderStatus)
                        {
                            if (status.DispatchedDateTime.HasValue)
                            {
                                orderDispatchedTicks = status.DispatchedDateTime.Value.Ticks;
                            }
                        }

                        if (customerOrder.Order.ProximityDelivered == null)
                        {
                            if (driver.HasFix)
                            {
                                var delivered = driver.ProximityDelivered(customerOrder.Customer.Coordinates);

                                if (delivered)
                                {   //set order to proximity delivered
                                    UpdateOrderAsProximityDelivered(order, account.Store);
                                    proximityDelivered = true;
                                }
                            }
                        }
                        else
                        {
                            proximityDelivered = true;
                        }

                        driver.Deliveries.Add(new Delivery(order.ExternalOrderId, proximityDelivered, 4, orderDispatchedTicks));

                        proximityDelivered = false;
                    }
                }

                var outstanding = OrderDao.FindUndelivered(account.Store);

                foreach (var order in outstanding)
                {
                    if (storeMapData.OutStandingDeliveries == null)
                        storeMapData.OutStandingDeliveries = new List<Delivery>();

                    var orderStatus = (OrderStatus) order.OrderStatus[0];
                    
                    // Get the despatched date/time
                    long orderDispatchedTicks = 0;
                    foreach (OrderStatus status in order.OrderStatus)
                    {
                        if (status.DispatchedDateTime.HasValue)
                        {
                            orderDispatchedTicks = status.DispatchedDateTime.Value.Ticks;
                        }
                    }

                    storeMapData.OutStandingDeliveries.Add(new Delivery(order.ExternalOrderId, false, orderStatus.Status.Id.Value, orderDispatchedTicks));
                }
            }
            return storeMapData;
        }



        private void UpdateOrderAsProximityDelivered(Order order, Store store)
        {
            var ord = OrderDao.FindByExternalId(order.ExternalOrderId, store);

            var orderStatus = (OrderStatus)ord.OrderStatus[0];

            //only updates orders with a driver;
            if (orderStatus.Status.Id != 4) return;

            ord.ProximityDelivered = order.ProximityDelivered;
            ord.ProximityDelivered = DateTime.Now;
            OrderDao.Update(ord);
        }

        #endregion

    }
}
