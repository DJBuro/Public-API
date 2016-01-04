using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OrderTracking.Dao;
using OrderTracking.Dao.Domain;
using OrderTrackingAdmin.Models;
using OrderTrackingAdmin.Mvc;
using OrderTrackingAdmin.Mvc.Filters;

namespace OrderTrackingAdmin.Controllers
{
    public class StoreController : SiteController
    {
        public ITrackerDao TrackerDao;
        public ILogDao LogDao;
        public IOrderDao OrderDao;
        public IDriverDao DriverDao;
        public IOrderStatusDao OrderStatusDao;
        public ICoordinatesDao CoordinatesDao;
        public ICustomerDao CustomerDao;
       
        public void Translate(string id)
        {
            //note this is an empty call only to fire off the SiteBaseController to change the language
        }

        [RequiresAuthorisation]
        public ActionResult Index(string id)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData();

            if(!string.IsNullOrEmpty(id))
            {
                data.Account = AccountDao.FindByStoreRamesesId(id);
            }

            return (View(StoreControllerViews.Index, data));
        }
        
        [RequiresAuthorisation]
        public ActionResult All()
        {
            var data = new OrderTrackingAdminViewData.StoreViewData
                           {
                               Accounts = AccountDao.FindAll().OrderBy(c => c.Store.Name).ToList()
                           };

            return (View(StoreControllerViews.All, data));
        }

        [RequiresAuthorisation]
        public ActionResult FindById(Store store)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData {Account = new Account()};

            data.Account = AccountDao.FindByStoreRamesesId(store.ExternalStoreId);

            if(data.Account == null)
            {
                data.ErrorMessage = "Store could not be found";
                return (View(StoreControllerViews.Index, data));
            }

            return (View(StoreControllerViews.Result, data));

        }

        [RequiresAuthorisation]
        public ActionResult FindByExternalId(string id)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData {Account = new Account()};

            data.Account = AccountDao.FindByStoreRamesesId(id);

            if (data.Account == null)
            {
                data.ErrorMessage = "Store could not be found";
                return (View(StoreControllerViews.Index, data));
            }

            return (View(StoreControllerViews.Result, data));

        }
        [RequiresAuthorisation]
        public ActionResult FindByName(Store store)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData
                           {
                               Store = StoreDao.FindByExternalId(store.ExternalStoreId)
                           };

            if(data.Store == null)
            {
                data.ErrorMessage = "Store could not be found";
                return (View(StoreControllerViews.Index, data));
            }

            return (View(StoreControllerViews.Result, data));
        }

        //Add new store
        [RequiresAuthorisation]
        public ActionResult Add()
        {
            var data = new OrderTrackingAdminViewData.StoreViewData();

            return (View(StoreControllerViews.Add, data));
        }

        //View Store trackers
        [RequiresAuthorisation]
        public ActionResult Trackers(string id)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData {Account = AccountDao.FindByStoreRamesesId(id)};

            var trackers = TrackerDao.FindByStore(data.Account.Store);

            if (trackers.Count > 0)
            {
                Reconcile.TrackerRecon.Recon(ref trackers);
            }

            return (View(StoreControllerViews.Trackers, data));
        }

        //View Store on Map
        [RequiresAuthorisation]
        public ActionResult Map(string id)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData {Account = new Account()};

            data.Account = AccountDao.FindByStoreRamesesId(id);

            if (data.Account.Store == null)
            {
                data.ErrorMessage = "Store could not be found";
                return (View(StoreControllerViews.Index, data));
            }

            return (View(StoreControllerViews.Map, data));
        }

        
        //View Store Orders
        [RequiresAuthorisation]
        public ActionResult Orders(string id)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData {Account = AccountDao.FindByStoreRamesesId(id)};

            // data.Orders = OrderDao.FindUndelivered(data.Account.Store.Id.Value).OrderByDescending(c=> c.Id ).ToList();
            //note: should be faster than above, reduces casting on view
            data.OrderStatuses = OrderStatusDao.GetOrdersByStore(data.Account.Store).OrderByDescending(c => c.Order.Id.Value).ToList();

            return (View(StoreControllerViews.Orders, data));
        }

        //View Store Drivers
        [RequiresAuthorisation]
        public ActionResult Drivers(string id)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData {Account = AccountDao.FindByStoreRamesesId(id)};

            data.Drivers = DriverDao.FindByStore(data.Account.Store);

            return (View(StoreControllerViews.Drivers, data));
        }

        [RequiresAuthorisation]
        public ActionResult TodaysLogs(string id)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData
                           {
                               Account = AccountDao.FindByStoreRamesesId(id),
                               Logs = LogDao.TodaysLogs(id)
                           };

            return (View(StoreControllerViews.Logs, data));
        }

        [RequiresAuthorisation]
        public ActionResult WeeksLogs(string id)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData
                           {
                               Account = AccountDao.FindByStoreRamesesId(id),
                               Logs = LogDao.WeeksLogs(id)
                           };

            return (View(StoreControllerViews.Logs, data));
        }

        [RequiresAuthorisation]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddAccount(OrderTrackingAdminWS.StoreDetails storeDetails)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData();

            var orderTrackingAdminWs = new OrderTrackingAdminWS.OrderTrackingAdmin();

            var results = orderTrackingAdminWs.AddUpdateAccount(storeDetails);

            if(!results.Success)
            {
                data.ErrorMessage = "Error:";

                foreach (var error in results.ErrorMessages)
                {
                    data.ErrorMessage = data.ErrorMessage + error + "<br>";
                }
            }

            return (View(StoreControllerViews.Index, data));
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult UpdateStore(Store store)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData
                           {
                               Store = StoreDao.FindByExternalId(store.ExternalStoreId)
                           };

            if (UpdateModel(data.Store))
            {
                StoreDao.Update(data.Store);
                data.Account = AccountDao.FindByStoreRamesesId(data.Store.ExternalStoreId);
                data.Account.Store = StoreDao.FindByExternalId(data.Store.ExternalStoreId);

                return (View(StoreControllerViews.Result, data));
            }

            data.ErrorMessage = "Updating Store error";
            return (View(StoreControllerViews.Index, data));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult ClearMonitor(Store store)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData
                           {
                               Store = StoreDao.FindByExternalId(store.ExternalStoreId)
                           };

            if (UpdateModel(data.Store))
            {
                data.Account = AccountDao.FindByStoreRamesesId(data.Store.ExternalStoreId);

                var drivers = DriverDao.FindByStore(data.Account.Store);

                foreach (var driver in drivers)
                {
                    if (driver.Trackers.Count > 0)
                    {
                        TrackerDao.RemoveTrackerFromDriver(driver);
                    }
                }

                DriverDao.DeleteAll(drivers);

                var customers = CustomerDao.FindByStore(data.Account.Store);

                var coordinates = new List<Coordinates>();

                foreach (var customer in customers)
                {
                    if (customer.Coordinates != null)
                        coordinates.Add(customer.Coordinates);
                }

                CustomerDao.DeleteAll(customers);
                CoordinatesDao.DeleteAll(coordinates);

                var orders = OrderDao.FindByStore(data.Account.Store);
                OrderDao.DeleteAll(orders);

                data.Account.Store = StoreDao.FindByExternalId(data.Store.ExternalStoreId);

                Logging.LogNotice("ClearStore", data.Store.ExternalStoreId, "warning", "none", "Clear Store manually called from Andro Admin");

                return (View(StoreControllerViews.Result, data));
            }

            data.ErrorMessage = "Clear Monitor error";
            return (View(StoreControllerViews.Index, data));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult ClearAccountLogs(Store store)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData();

            var storeLogs = LogDao.AllAccountLogs(store.ExternalStoreId);
            LogDao.DeleteAll(storeLogs);

            data.Store = StoreDao.FindByExternalId(store.ExternalStoreId);

            data.Account = AccountDao.FindByStoreRamesesId(data.Store.ExternalStoreId);
            data.Account.Store = StoreDao.FindByExternalId(data.Store.ExternalStoreId);

            return (View(StoreControllerViews.Result, data));
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult UpdateCoordinates(Coordinates coordinates)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData();

            if(coordinates.Latitude != 0 | coordinates.Longitude != 0)
            {
                CoordinatesDao.Update(coordinates);
            }

            return (View(StoreControllerViews.Index, data));
        }

        
        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult UpdateAccount(Store store)
        {
            var data = new OrderTrackingAdminViewData.StoreViewData
                           {
                               Account = AccountDao.FindByStoreRamesesId(store.Id.Value.ToString())
                           };

            if (UpdateModel(data.Account))
            {
                AccountDao.Update(data.Account);

                data.Account = AccountDao.FindByStoreRamesesId(data.Account.Store.ExternalStoreId);
                data.Account.Store = StoreDao.FindByExternalId(data.Account.Store.ExternalStoreId);

                return (View(StoreControllerViews.Result, data));
            }

            data.ErrorMessage = "Updating Account error";
            return (View(StoreControllerViews.Index, data));
        }

    }
}
