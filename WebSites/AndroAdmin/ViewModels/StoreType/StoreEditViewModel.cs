using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroAdmin.ViewModels.StoreType
{
    public class StoreEditViewModel
    {
        public int StoreId { get; set; }

        public IEnumerable<DeviceViewModel> Devices { get; set; }

        public Guid[] SelectedStoreTypes { get; set; }
    }


}