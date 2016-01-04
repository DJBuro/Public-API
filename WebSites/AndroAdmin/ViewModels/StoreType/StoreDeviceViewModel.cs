using AndroAdminDataAccess.EntityFramework;
using System;
using System.Linq;

namespace AndroAdmin.ViewModels.StoreType
{

    public class StoreDeviceViewModel
    {
        public Guid? Id { get; set; }
        public int StoreId { get; set; }
        public string ClientSiteName { get; set; }
        public DeviceViewModel Device { get; set; }
        
    }

    public static class StoreDeviceViewModelExtensions
    {
        public static StoreDeviceViewModel ToViewModel(this StoreDevice storeDevice) 
        {
            return new StoreDeviceViewModel()
            {
                Id = storeDevice.Id,
                Device = storeDevice.Device.ToViewModel(),
                StoreId = storeDevice.StoreId,
                ClientSiteName = storeDevice.Store.ClientSiteName
            };
        }
    }
}