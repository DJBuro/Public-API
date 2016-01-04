using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdmin.ViewModels.ApiCredentials;
using AndroAdminDataAccess.EntityFramework;

namespace AndroAdmin.ViewModels.StoreType
{
    public class DeviceViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        //public Guid? ExternalApiId { get; set; }
        public ExternalApiSelectViewModel ExternalApi { get; set; }
    }

    public static class StoreTypeViewModelExtensions 
    {
        public static DeviceViewModel ToViewModel(this AndroAdminDataAccess.EntityFramework.Device model)
        {
            return new DeviceViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                ExternalApi = model.ExternalApiId.HasValue ? 
                    model.ExternalApi.ToSelectViewModel() : 
                    new ExternalApiSelectViewModel() { Name = "None" }
                //ExternalApiId = model.ExternalApiId
            };
        }

        public static AndroAdminDataAccess.EntityFramework.Device ToDataModel(this DeviceViewModel viewModel) 
        {
            var dataModel = new AndroAdminDataAccess.EntityFramework.Device()
            {
                Id = viewModel.Id.GetValueOrDefault(Guid.NewGuid()),
                Name = viewModel.Name,
            };


            if (viewModel.ExternalApi != null && viewModel.ExternalApi.Id.HasValue)
            {
                dataModel.ExternalApiId = viewModel.ExternalApi.Id;
            }
            else 
            {
                dataModel.ExternalApiId = null;
            }
            
            return dataModel;
        }
        //public static StoreDeviceViewModel ToStoreTypeViewModel(this AndroAdminDataAccess.EntityFramework.Store model) 
        //{
        //    return new StoreDeviceViewModel()
        //    {
        //        Id = model.Id,
        //        ClientSiteName = model.ClientSiteName,
        //        AndromedaSiteId = model.AndromedaSiteId,
        //        Chain = model.Chain.Name
        //    };
        //}
    }
}