﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudSyncModel
{
    public class Store
    {
        public string ExternalSiteName { get; set; }
        public int AndromedaSiteId { get; set; }
        public string ExternalSiteId { get; set; }
        public string StoreStatus { get; set; }
        public string Phone { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneInfoId { get; set; }
        public string UiCulture { get; set; }

        public Address Address { get; set; }
        public string StorePaymentProviderId { get; set; }
        public List<TimeSpanBlock> OpeningHours { get; set; }
    }

    public class StoreEdt 
    {
        public int AndromedaSiteId { get; set; }
        public int EstimatedTimeForDelivery { get; set; }
        public int? EstimatedCollectionTime { get; set; }
    }

}
