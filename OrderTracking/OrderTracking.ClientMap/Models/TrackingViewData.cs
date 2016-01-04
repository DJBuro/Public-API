using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderTracking.ClientMap.Models
{
    public class TrackingViewData : SiteViewData
    {
        public class TrackViewData : TrackingViewData
        {
            public ClientMapService.ClientMapData ClientMapData;

            public string SiteName;
            public string Credentials;
        }
    }

    public interface ISiteViewData : IViewDataContainer { }

    public class SiteViewDataDictionary : ViewDataDictionary { }

    public class SiteViewData : SiteViewDataDictionary, ISiteViewData
    {
        public ViewDataDictionary ViewData { get; set; }
    }
}
