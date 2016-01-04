using System;
using System.Collections.Generic;
using System.Web.Mvc;
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
            public Client Client;
            public IList<Account> Accounts;
            public IList<Log> Logs;
            public IList<Order> Orders;
            public IList<OrderStatus> OrderStatuses;
            public IList<Driver> Drivers;
        }

        public class StoreViewData : OrderTrackingAdminBaseViewData
        {
        }

        public class ClientViewData : OrderTrackingAdminBaseViewData
        {
            public string Url { get; set; }
            public bool HasTemplate { get; set; }
            public IEnumerable<Client> Clients;
            public IEnumerable<Store> Stores;
            public IEnumerable<SelectListItem> WebsiteTemplates;
            public Dictionary<string, List<InjectionPoint>> InjectionPoints;
        }

        public class GlobalViewData : OrderTrackingAdminBaseViewData
        {
            public IList<Apn> Apns;
            public IList<TrackerType> TrackerTypes;
            public Apn Apn;
            public SmsCredential SmsCredential;
            public String ViewString;
        }

        public class TrackerViewData : OrderTrackingAdminBaseViewData
        {
            public int LicenseCount;

            public Tracker Tracker;
            public IList<Tracker> Trackers;
            public IList<TrackerCommand> TrackerCommands;

            public IEnumerable<SelectListItem> TrackerTypeListItems;
            public IEnumerable<SelectListItem> ApnListItems;
            public IEnumerable<SelectListItem> StoreListItems;
        }
    }
}
