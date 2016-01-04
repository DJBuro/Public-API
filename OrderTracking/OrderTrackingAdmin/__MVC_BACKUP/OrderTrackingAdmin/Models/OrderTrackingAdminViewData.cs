using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrderTracking.Dao.Domain;
using OrderTrackingAdmin.Mvc;

namespace OrderTrackingAdmin.Models
{
    public class OrderTrackingAdminViewData : SiteViewData
    {
        /// <summary>
        /// Masterpage, all other pages inherit from this.
        /// </summary>
        public class OrderTrackingAdminBaseViewData : SiteViewData
        {
            public string ErrorMessage;

            public Account Account;
            public Store Store;
            public IList<Account> Accounts;
            public IList<Log> Logs;
            public IList<Order> Orders;
            public IList<OrderStatus> OrderStatuses;
            public IList<Driver> Drivers;

        }

        public class IndexViewData : OrderTrackingAdminBaseViewData
        {
            
        }
    }
}
