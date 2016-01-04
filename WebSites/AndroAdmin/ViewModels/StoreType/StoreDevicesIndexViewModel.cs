using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdmin.ViewModels.ApiCredentials;

namespace AndroAdmin.ViewModels.StoreType
{
    public class StoreDevicesIndexViewModel
    {
        public StoreDevicesIndexViewModel() { }

        public IEnumerable<ExternalApiSelectViewModel> Apis { get; set; }
    }
}