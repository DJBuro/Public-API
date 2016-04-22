using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using OrderTracking.Core;
using OrderTracking.Dao;

using OrderTracking.WebService.Dao;
using OrderTracking.WebService.Gps;
using OrderTracking.Dao.Domain;

using Order = OrderTracking.Dao.Domain.Order;
using Driver = OrderTracking.Dao.Domain.Driver;
using OrderStatus=OrderTracking.Dao.Domain.OrderStatus;
using XmlSerializer=OrderTracking.Core.XmlSerializer;
using OrderTracking.WebService.Tracking;

namespace OrderTracking
{
    public class Service : IOrderTracking
    {
        #region IDAOs

        public IOrderDao OrderDao { get; set; }
        public IOrderStatusDao OrderStatusDao { get; set; }
        public IAccountDao AccountDao { get; set; }
        public IStoreDao StoreDao { get; set; }
        public IDriverDao DriverDao { get; set; }
        public IDriverOrderDao DriverOrderDao { get; set; }
        public ITrackerDao TrackerDao { get; set; }
        public ITrackerStatusDao TrackerStatusDao { get; set; }
        public IStatusDao StatusDao { get; set; }
        public IItemDao ItemDao { get; set; }
        public ICustomerDao CustomerDao { get; set; }
        public ILogDao LogDao { get; set; }
        public ICoordinatesDao CoordinatesDao { get; set; }

        #endregion
        #region IOrderTracking Members

        public Results GetTrackerNames(string userName, string password, out string[] trackers)
        {
            trackers = new string[0];
            var results = new Results { Success = true };
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    if (account.GpsEnabled)
                    {
                        var storeTrackers = TrackerDao.FindByStore(account.Store);

                        trackers = new string[storeTrackers.Count];

                        for (var i = 0; i < storeTrackers.Count; i++)
                        {
                            trackers[i] = storeTrackers[i].Name;
                        }
                    }
                }
                else
                {
                    results = results.FailedLogin(userName, password, "GetTrackerNames");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("GetTrackerNames", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("GetTrackerNames", storeId, "");
            }

            return results;
        }

        public Results GetTrackers(string userName, string password, out List<Tracker> trackers)
        {
            trackers = new List<Tracker>();
            var results = new Results { Success = true };
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    if (account.GpsEnabled)
                    {
                        var storeTrackers = account.GetTrackers();

                        foreach (var storeTracker in storeTrackers)
                        {
                            var trackerss = TrackerDao.FindByName(storeTracker.Name, account.Store);

                            if (trackerss!= null && trackerss.Driver != null)
                            {
                                storeTracker.Driver = trackerss.Driver;
                            }
                        }

                        trackers = storeTrackers.XmlSerialize().Where(c => c.Active).ToList();
                    }
                }
                else
                {
                    results = results.FailedLogin(userName, password, "GetTrackers");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("GetTrackers", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("GetTrackers", storeId, "");
            }

            return results;
        }

        public Results AddUpdateDriver(string userName, string password, string externalDriverId, string driverName, string vehicle, string trackerName)
        {
            var results = new Results {Success = true};

            if(trackerName ==  null)
                trackerName ="";

            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try 
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    //results.AddHocDebug("AddUpdateDriver", "AddUpdateDriver: " + driverName + " id: " + externalDriverId, account.Store.ExternalStoreId);

                    if (driverName.Length == 0)
                        return results.AddHocError("AddUpdateDriver", "Driver needs a name", account.Store);

                    var tracker = TrackerDao.FindByName(trackerName, account.Store);

                    if (tracker == null && trackerName.Length > 0)
                        results.ItemCannotBeFoundError("AddUpdateDriver", "tracker", trackerName, account.Store);

                    if (results.Success)
                    {
                        var driver = DriverDao.FindByExternalId(externalDriverId, account.Store);

                        if (tracker == null && trackerName.Length == 0 && driver != null)
                        {
                            TrackerDao.RemoveTrackerFromDriver(ref driver);
                        }

                        if (driver != null)
                        {
                            if (driver.Trackers.Count > 0 && !tracker.Equals(driver.Trackers[0]))
                            {
                                tracker.Driver = driver;
                                TrackerDao.Save(tracker);

                                var trackerRef = (Tracker)driver.Trackers[0];
                                TrackerDao.RemoveTrackerFromDriver(ref trackerRef);
                            }
                        }

                        if (driver == null)
                        {
                            driver = new Driver();
                        }

                        driver.ExternalDriverId = externalDriverId;

                        driver.Name = driverName;
                        driver.Vehicle = vehicle;

                        driver.Store = account.Store;

                        if (tracker != null && driver.Trackers.Count == 0)
                        {
                            driver.Trackers.Add(tracker);
                            tracker.Driver = driver;

                            if (driver.Id.HasValue)
                                TrackerDao.Save(tracker);
                        }

                        DriverDao.Save(driver);
                    }
                }
                else
                {
                    results = results.FailedLogin(userName, password, "AddUpdateDriver");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("AddUpdateDriver", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("AddUpdateDriver", storeId, "");
            }

            return results;
        }

        [XmlInclude(typeof(Item))]
        [XmlInclude(typeof(OrderStatus))]
        [XmlInclude(typeof(Tracker))]
        [XmlInclude(typeof(DriverOrder))]
        public Results GetDriver(string userName, string password, string externalDriverId, out Driver driver)
        {
            driver = new Driver();
            var results = new Results {Success = true};
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    var dr = DriverDao.FindByExternalId(externalDriverId, account.Store);

                    if (dr == null)
                        results.ItemCannotBeFoundError("GetDriver", "driver", externalDriverId,
                                                       account.Store);

                    if (results.Success)
                    {
                        //the driver has a tracker assigned
                        if(dr.Trackers !=null)
                        {
                            if (dr.Trackers.Count > 0 && account.GpsEnabled)
                            {
                                var tracker = (Tracker)dr.Trackers[0];
                                dr.Trackers[0] = tracker.Get();
                            }
                        }
                    
                        driver = dr.XmlSerialize();
                    }
                }
                else
                {
                    results = results.FailedLogin(userName, password, "GetDriver");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("GetDriver", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("GetDriver", storeId, "");
            }

            return results;
        }

        [XmlInclude(typeof(Item))]
        [XmlInclude(typeof(OrderStatus))]
        [XmlInclude(typeof(Tracker))]
        [XmlInclude(typeof(DriverOrder))]
        public Results GetDrivers(string userName, string password, out List<Driver> drivers)
        {
            var dr = new List<Driver>();
            drivers = new List<Driver>();

            var results = new Results {Success = true};

            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    drivers = DriverDao.FindByStore(account.Store).ToList();
                
                    if (drivers.Count == 0)
                    {
                        results.AddHocError("GetDrivers", "There are no drivers found for that store",
                                            account.Store);
                    }

                    if (results.Success)
                    {
                        List<Tracker> trackers = (List<Tracker>)account.GetTrackers();

                        foreach (var driver in drivers)
                        {
                            //TODO: horribly bad... needs cleaning... use linq?
                            foreach (Tracker tracker in driver.Trackers)
                            {
                                foreach (var outTracker in trackers)
                                {
                                    if (outTracker.Name != tracker.Name) continue;

                                    if(tracker.Coordinates == null)
                                        tracker.Coordinates = new Coordinates();

                                    tracker.Coordinates.Longitude = outTracker.Coordinates.Longitude;
                                    tracker.Coordinates.Latitude = outTracker.Coordinates.Latitude;
                                    tracker.BatteryLevel = outTracker.BatteryLevel;
                                    tracker.Status = outTracker.Status;
                                }
                            }

                            dr.Add(driver.XmlSerialize());
                        }

                        drivers.Clear();
                        drivers = dr;
                    }
                }
                else
                {
                    results = results.FailedLogin(userName, password, "GetDrivers");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("GetDrivers", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("GetDrivers", storeId, "");
            }

            return results;
        }

        public Results AddOrderToDriver(string userName, string password, string externalOrderId, string externalDriverId, DateTime dateTimeAdded)
        {
            var results = new Results {Success = true};
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    //results.AddHocDebug("AddOrderToDriver", "ExternalOrderId: " + externalOrderId + " ExternalDriverId: " + externalDriverId, account.Store.ExternalStoreId);

                    var driver = DriverDao.FindByExternalId(externalDriverId, account.Store);

                    if (driver == null)
                        results.ItemCannotBeFoundError("AddOrderToDriver", "driver", externalDriverId,
                                                       account.Store);

                    //set the status to 4 as it has been assigned to a driver.
                    if (results.Success)
                    {
                        //note: grabs the first (not .Single()), in extreme cases (eg. session manager hydration with multiple milli-second calls) it may be possible to add same order twice.
                        var order = OrderDao.FindUndelivered(account.Store).Where(c => c.ExternalOrderId == externalOrderId).First();

                        if (order.Id == null)
                        {
                            results.ItemCannotBeFoundError("AddOrderToDriver", "order", externalOrderId,
                                                           account.Store);
                        }

                        if (results.Success)
                        {
                            var driverOrder = new DriverOrder(driver, order);

                            driver.DriverOrder.Add(driverOrder);

                            DriverDao.Save(driver);
                      
                            results = UpdateOrderStatus(userName, password, externalOrderId, 4, driver.Name, dateTimeAdded);
                        }
                    }
                }
                else
                {
                    results = results.FailedLogin(userName, password, "AddOrderToDriver");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("AddOrderToDriver", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("AddOrderToDriver", storeId, externalOrderId + " > " + externalDriverId);
            }

            return results;
        }

        public Results RemoveOrderFromDriver(string userName, string password, string externalOrderId, string externalDriverId)
        {
            var results = new Results {Success = true};
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    var order = OrderDao.FindByExternalId(externalOrderId, account.Store);

                    var driver = DriverDao.FindByExternalId(externalDriverId, account.Store);

                    if (order == null)
                        results.ItemCannotBeFoundError("RemoveOrderFromDriver", "order", externalOrderId,
                                                       account.Store);
                    if (driver == null)
                        results.ItemCannotBeFoundError("RemoveOrderFromDriver", "driver", externalDriverId,
                                                       account.Store);

                    if (results.Success)
                    {
                        if (driver.DriverOrder.Count > 0)
                        {
                            foreach (DriverOrder driverOrder in driver.DriverOrder)
                            {
                                if (driverOrder.Order.Equals(order))
                                {
                                    driver.DriverOrder.Remove(driverOrder);

                                    var status = (OrderStatus)order.OrderStatus[0];

                                    if(status.Status.Id == 4)
                                    {   //note: if the driver was mistakenly assigned the order... send back to 3...
                                        UpdateOrderStatus(account.UserName, account.Password, order.ExternalOrderId, 3,
                                                          "Removed to re-assign order", DateTime.Now);
                                    }

                                    DriverDao.Update(driver);

                                    break;
                                }
                            }
                        }
                        else
                        {
                            results.ItemCannotBeFoundError("RemoveOrderFromDriver", "Order: '"+ order.ExternalOrderId +"' for driver:",
                                                           externalDriverId, account.Store);
                        }
                    }
                }
                else
                {
                    results = results.FailedLogin(userName, password, "RemoveOrderFromDriver");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("RemoveOrderFromDriver", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("RemoveOrderFromDriver", storeId, externalOrderId + " > " + externalDriverId);
            }

            return results;
        }

        public Results AddOrder(
            string userName, 
            string password, 
            string orderName, 
            string ticketNumber, 
            string externalOrderId, 
            string orderProcessor, 
            string extraInformation, 
            DateTime dateTimeCreated, 
            List<OrderItem> orderItems, 
            CustomerDetails customerDetails)
        {
            var results = new Results {Success = true};
            var coordinates = new Coordinates();
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    var order = OrderDao.FindByExternalId(externalOrderId, account.Store);

                    if (order != null)
                    {
                        Logging.LogError("AddOrder", storeId, string.Format("Order Id: {0} already exists and cannot be added again", externalOrderId));

                        results.AddHocError("AddOrder",
                                            string.Format("Order Id: {0} already exists and cannot be added again",
                                                          externalOrderId), account.Store);
                    }
                    else
                    {
                        if (account.GpsEnabled)
                        {
                            coordinates = CoordinatesDao.Create(new Coordinates());

                            bool geocodeSuccess = true;
                            try
                            {
                                Logging.LogInfo("AddOrder", storeId, "Geocode required");

                                results = customerDetails.Geocode(
                                    account.Store,
                                    results,
                                    ref coordinates,
                                    ConfigurationManager.AppSettings.Get("Deployed"));
                            }
                            catch (Exception exception)
                            {
                                geocodeSuccess = false;
                                Logging.LogError("AddOrder", storeId, "Geocode error:" + exception.Message);
                            }

                            if (geocodeSuccess)
                            {
                                if (results.Success)
                                {
                                    var withinDeliveryRadius = account.Store.WithinDeliveryRadius(ref coordinates);

                                    //NOTE: if they are outside the delivery Radius, it won't be on the map
                                    if (!withinDeliveryRadius)
                                    {
                                        Logging.LogInfo
                                        (
                                            "AddOrder", 
                                            storeId, 
                                            "Order Id:" + externalOrderId + " : outside delivery bounds " +
                                                          customerDetails.HouseNumber + " " + customerDetails.RoadName + " " +
                                                          customerDetails.TownCity + " " + customerDetails.PostCode + 
                                                          " long:" + coordinates.Longitude + 
                                                          " lat:" + coordinates.Latitude);

                                        results.AddHocWarning("AddOrder", "Order Id:" + externalOrderId + " : outside delivery bounds " +
                                                              customerDetails.HouseNumber + " " + customerDetails.RoadName + " " +
                                                              customerDetails.TownCity + " " + customerDetails.PostCode + " long:" + coordinates.Longitude + " lat:" + coordinates.Latitude, account.Store);

                                        coordinates.Latitude = 0;//note: setting to 0 - if outside the delivery area...
                                        coordinates.Longitude = 0;
                                    }
                                }
                                else if (geocodeSuccess)
                                {
                                    Logging.LogError("AddOrder", storeId, "Geocode failed");
                                }
                            }

                            CoordinatesDao.Save(coordinates);
                        }
                        else
                        {
                            Logging.LogInfo("AddOrder", storeId, "GPS disabled");
                        }

                        if (order == null)
                            order = new Order();

                        order.Name = orderName;
                        order.ExternalOrderId = externalOrderId;
                        order.TicketNumber = ticketNumber;
                        order.ExtraInformation = extraInformation;

                        OrderStatus ordStatus = new OrderStatus
                                            {
                                                Order = order,
                                                Processor = orderProcessor,
                                                Status = StatusDao.FindById(1),
                                                Time = dateTimeCreated
                                            };

                        order.OrderStatus.Add(ordStatus);

                        foreach (var item in orderItems)
                        {
                            Item ordItem = new Item { Name = item.Name, Quantity = item.Quantity, Order = order };
                            order.Items.Add(ordItem);
                        }

                        order.Store = account.Store;

                        OrderDao.Save(order);

                        var customer = new Customer
                                           {
                                               Name = customerDetails.Name,
                                               ExternalId = customerDetails.ExternalId,
                                               Credentials = customerDetails.Credentials,
                                               BuildingName = customerDetails.BuildingName,
                                               Country = customerDetails.Country,
                                               HouseNumber = customerDetails.HouseNumber,
                                               PostCode = customerDetails.PostCode,
                                               RoadName = customerDetails.RoadName,
                                               TownCity = customerDetails.TownCity
                                           };

                        CustomerOrder cusOrder = new CustomerOrder(order, customer);
                        customer.CustomerOrders.Add(cusOrder);

                        if (account.GpsEnabled && coordinates != null)
                        {
                            customer.Coordinates = coordinates;
                        }

                        CustomerDao.Save(customer);

                        order.CustomerOrder.Add(cusOrder);
                        OrderDao.Update(order);
                    }
                }
                else
                {
                    results = results.FailedLogin(userName, password, "AddOrder");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("AddOrder", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("AddOrder", storeId, externalOrderId);
            }

            return results;
        }

        public Results UpdateOrderStatus(string userName, string password, string externalOrderId, int status, string processor, DateTime dateTimeChanged)
        {
            var results = new Results {Success = true};
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    var order = OrderDao.FindByExternalId(externalOrderId, account.Store);
                    var newStatus = StatusDao.FindById(status);

                    if (order == null)
                    {
                        results.ItemCannotBeFoundError("UpdateOrderStatus", "order", externalOrderId,
                                                       account.Store);

                        return results;
                    }
                    if(newStatus == null) 
                    {
                        results.ItemCannotBeFoundError("UpdateOrderStatus","status", status.ToString(),
                                                       account.Store);
                    }

                    if (results.Success)
                    {
                        var orderStatus = (OrderStatus)order.OrderStatus[0];

                        orderStatus.Status = newStatus;
                        orderStatus.Time = dateTimeChanged;
                        orderStatus.Processor = processor;

                        // Has the order created?
                        if (status == 1)
                        {
                            // Record the time that the order was created
                            orderStatus.CreatedDateTime = dateTimeChanged;
                        }
                        // has the order been dispatched?
                        else if (status == 4)
                        {
                            // Record the time that the order was dispatched
                            orderStatus.DispatchedDateTime = dateTimeChanged;
                        }
                        else if (status == 5 | status == 6 && account.GpsEnabled)
                        {
                            var driver = DriverDao.FindByOrder(order, account.Store);

                            if (driver != null)
                            {
                                RemoveOrderFromDriver(account.UserName, account.Password, order.ExternalOrderId,
                                                      driver.ExternalDriverId);
                            }
                        }

                        OrderDao.Update(order);
                
                    }
                }
                else
                {
                    results = results.FailedLogin(userName, password, "UpdateOrderStatus");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("UpdateOrderStatus", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("UpdateOrderStatus", storeId, externalOrderId + " > " + status);
            }

            return results;
        }

        public Results GetUndeliveredOrders(string userName, string password, out List<Order> orders)
        {
            var results = new Results{Success = true};
            orders = new List<Order>();
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    orders = OrderDao.FindUndelivered(account.Store).ToList();

                    orders = orders.XmlSerialize().ToList();
                }
                else
                {
                    results = results.FailedLogin(userName, password, "GetUndeliveredOrders");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("GetUndeliveredOrders", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("GetUndeliveredOrders", storeId, "");
            }

            return results;
        }

        public Results GetOrder(
            string userName, 
            string password, 
            string externalOrderId, 
            out Order order, 
            out DateTime? orderTaken, 
            out DateTime? orderDispatched)
        {
            var results = new Results{Success = true};

            order = new Order();
            orderTaken = null;
            orderDispatched = null;
            
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    order = OrderDao.FindByExternalId(externalOrderId, account.Store);

                    if (order != null && order.Id != null)
                    {
                        // Get the status 1 (order taken) status updates for this order - there should only be one
                        List<OrderStatus> orderStatuses = (List<OrderStatus>)OrderStatusDao.GetByOrder(order.Id.Value);

                        // Did we successfully get the order taken date/time?                
                        if (orderStatuses != null && orderStatuses.Count == 1)
                        {
                            orderTaken = orderStatuses[0].CreatedDateTime;
                            orderDispatched = orderStatuses[0].DispatchedDateTime;
                        }
                    }

                    order = order.XmlSerialize();
                }
                else
                {
                    results = results.FailedLogin(userName, password, "GetOrder");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("GetOrder", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("GetOrder", storeId, externalOrderId);
            }

            return results;
        }

        [XmlInclude(typeof(Item))]
        [XmlInclude(typeof(OrderStatus))]
        [XmlInclude(typeof(Tracker))]
        [XmlInclude(typeof(DriverOrder))]
        [XmlInclude(typeof(CustomerOrder))]
        public Results GetOrders(string userName, string password, out List<Order> orders)
        {
            var results = new Results{Success = true};
            orders = new List<Order>();
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    orders = OrderDao.FindByStore(account.Store).ToList();

                    orders = orders.XmlSerialize().ToList();
                }
                else
                {
                    results = results.FailedLogin(userName, password, "GetOrders");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("GetOrders", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("GetOrders", storeId, "");
            }

            return results;
        }

        public Results GetAllStatuses(string userName, string password, out List<Status> status)
        {
            var results = new Results {Success = true};
            status = new List<Status>();
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    status = StatusDao.FindAll().ToList();
                }
                else
                {
                    results = results.FailedLogin(userName, password, "GetAllStatuses");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("GetAllStatuses", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("GetAllStatuses", storeId, "");
            }

            return results;
        }

        public Results ClearStore(string userName, string password)
        {
            var results = new Results {Success = true};
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {

                    var drivers = DriverDao.FindByStore(account.Store);

                    foreach (var driver in drivers)
                    {
                        if(driver.Trackers.Count > 0)
                        {
                            TrackerDao.RemoveTrackerFromDriver(driver);
                        }
                    }


                    DriverDao.DeleteAll(drivers);

                    var customers = CustomerDao.FindByStore(account.Store);

                    var coordinates = new List<Coordinates>();

                    foreach (var customer in customers)
                    {
                        if(customer.Coordinates != null)
                        coordinates.Add(customer.Coordinates);
                    }

                    CustomerDao.DeleteAll(customers);
                    CoordinatesDao.DeleteAll(coordinates);

                    var orders = OrderDao.FindByStore(account.Store);
                    OrderDao.DeleteAll(orders);

                    results.AddHocWarning("ClearStore", "success", account.Store);
                }
                else
                {
                    results = results.FailedLogin(userName, password, "ClearStore");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("ClearStore", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("ClearStore", storeId, "");
            }

            return results;
        }

        public Results Last20Logs(string userName, string password, out List<Log> logs)
        {
            var results = new Results();
            logs = new List<Log>();
            Account account = null;
            string storeId = Logging.GetStoreId(userName);

            try
            {
                if (AccountDao.Verify(userName, password, out account))
                {
                    results.AddHocDebug("Last20Logs", "Last20Logs called by: " + account.Store.ExternalStoreId, account.Store);

                    logs = LogDao.Last20Logs(account.Store.ExternalStoreId).ToList();

                    if (logs.Count > 20)
                        logs = logs.GetRange(0, 20);
                }
                else
                {
                    results = results.FailedLogin(userName, password, "Last20Logs");
                }
            }
            catch (Exception exception)
            {
                Logging.LogError("Last20Logs", storeId, exception.Message);
            }
            finally
            {
                Logging.LogInfo("Last20Logs", storeId, "");
            }
            
            return results;
        }

        #endregion
        #region IOrderTracking Members

        public void zTEST()
        {
            List<Tracker> trackers;
            this.GetTrackers("MONITOR267", "Pass267", out trackers);


            Driver driver;
            this.GetDriver("MONITOR268", "Pass268", "6", out driver);

            var orderItems = new List<OrderItem>();
            //95 MONKSFIELD WAY SLOUGH SL2 1QN
            //[11:45:44] Ben Cole: 1 Abbey Gardens, London, SW1P 3SE
           // var cust = new CustomerDetails("externalId", "name", "creds", "1", "", "Abbey Gardens", "London",
            
                
           //var cust = new CustomerDetails("externalId", "name", "creds", "36", "", "Chapter Street", "London",
           //                                "SW1P 4NS", "UNITED KINGDOM");


            //36 CHAPTER STREET LONDON SW1P 4NS

           // //var api_url = encodeURIComponent(document.URL);

           //// document.write("<scr" + "ipt src='http://openspace.ordnancesurvey.co.uk/osmapapi/openspace.js?key=86E022425610D81BE0405F0ACA60422B&v=1.0.1&url=" + api_url + "'></scri" + "pt>");

           // //http://openspace.ordnancesurvey.co.uk/osmapapi/openspace.js?key=86E022425610D81BE0405F0ACA60422B

           // var request = WebRequest.Create("http://www.uk-postcodes.com/postcode/SK65JY.json");
           // var response = request.GetResponse();
           // var resStream = response.GetResponseStream();
           // int count;

           // //build input
           // var sb = new StringBuilder();

           // // used on each read operation
           // var buf = new byte[8192];

           // do
           // {
           //     // fill the buffer
           //     count = resStream.Read(buf, 0, buf.Length);

           //     if (count != 0)
           //     {
           //         // translate from bytes to ASCII text
           //         sb.Append(Encoding.ASCII.GetString(buf, 0, count));
           //     }
           // }
           // while (count > 0);

           // var googleReturn = sb.ToString().Split(',');


            //this.AddOrder("MONITOR60", "Pass60", "orderName1", "ticketnumber1", "exOrderId1", "processor", "", DateTime.Now,
            //              orderItems, cust);
          /* */
        }

        #endregion
    }
}