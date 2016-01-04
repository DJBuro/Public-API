using AndroAdmin.ViewModels.ApiCredentials;
using AndroAdminDataAccess.DataAccess;
using AndroAdminDataAccess.EntityFramework.DataAccess;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Linq;
using System.Web.Mvc;
using AndroAdmin.ViewModels.StoreType;
using AndroAdmin.Helpers;
using AndroAdmin.Services.Api;
using System.Collections.Generic;
using CloudSync;

namespace AndroAdmin.Controllers
{
    public class StoreDevicesController : BaseController 
    {
        private readonly IDevicesDataService devicesDataService;
        private readonly IStoreDevicesDataService storeDevicesDataService;
        private readonly IExternalApiDataService externalApiDataService;
        private readonly ExternalApiService externalApiService;

        public StoreDevicesController()
        {
            this.storeDevicesDataService = new StoreDevicesDataService();
            this.devicesDataService = new DevicesDataService();
            this.externalApiDataService = new ExternalApiDataService();
            this.externalApiService = new ExternalApiService();
        }

        public ActionResult Index(int storeId)
        {
            var selectedStoreTypes = this.devicesDataService
                .List(e => e.StoreDevices.Any(storeDevice => storeDevice.StoreId == storeId))
                .Select(e => e.ToViewModel());

            var viewModel = new StoreEditViewModel()
            {
                StoreId = storeId,
                Devices = selectedStoreTypes
            };

            return View(viewModel);
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request, int id) 
        {
            var data = this.devicesDataService.ListStoreDevice(e=> e.StoreId == id);
            var results = data.ToDataSourceResult(request, e => e.ToViewModel());

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadStoreConnections([DataSourceRequest]DataSourceRequest request, Guid id)
        {
            var data = this.devicesDataService.ListStoreDevice(e => e.DeviceId == id);
            var results = data.ToDataSourceResult(request, e => e.ToViewModel());

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadForStore([DataSourceRequest]DataSourceRequest request, string text)
        {
            var data = this.devicesDataService.List(e => e.Name.Contains(text));
            var result = data.Select(e => e.ToViewModel()); //data.ToDataSourceResult(request, e => e.ToViewModel());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadForExternalApi([DataSourceRequest]DataSourceRequest request, Guid externalApiId)
        {
            var data = this.devicesDataService.ListStoreDevice(e => e.Device.ExternalApiId == externalApiId);
            var result = data.Select(e => e.ToViewModel());

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, StoreDeviceViewModel viewModel)
        {
            if (viewModel.Device.Id.HasValue) { 
                var device = this.devicesDataService.Get(viewModel.Device.Id.GetValueOrDefault());
                viewModel.Device = device.ToViewModel();
            }

            var storeDevices = this.storeDevicesDataService.ListEnabled(e => e.StoreId == viewModel.StoreId).ToArray();

            if (storeDevices.Any(e => e.DeviceId == viewModel.Device.Id)) 
            {
                this.ModelState.AddModelError("Device", "This device has already been selected.");
            }

            if (!ModelState.IsValid) 
            {
                return Json(new[] { viewModel }.ToDataSourceResult(request, this.ModelState));
            }

            var model = this.storeDevicesDataService.New();
            model.StoreId = viewModel.StoreId;

            if (viewModel.Device.Id.HasValue) 
            {
                model.DeviceId = viewModel.Device.Id.GetValueOrDefault();
            }

            this.storeDevicesDataService.Create(model);

            try
            {
                // Push the change out to the ACS servers
                string errorMessage = SyncHelper.ServerSync();

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    this.Notifier.Notify(errorMessage);
                    //this.ModelState.AddModelError("Id", new Exception(errorMessage));
                }
            }
            catch (Exception e) { }

            return Json(new[] { model.ToViewModel() }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, StoreDeviceViewModel viewModel) 
        {
            var model = this.storeDevicesDataService.Get(viewModel.Id.GetValueOrDefault());

            if (viewModel.Device.Id.HasValue)
            {
                var device = this.devicesDataService.Get(viewModel.Device.Id.GetValueOrDefault());
                model.DeviceId = viewModel.Device.Id.GetValueOrDefault();
                model.Device = device;

                viewModel.Device = device.ToViewModel();
            }

            this.storeDevicesDataService.Update(model);

            // Push the change out to the ACS servers
            string errorMessage = SyncHelper.ServerSync();

            // Success
            if (errorMessage.Length == 0)
            {
                TempData["message"] = "Store " + viewModel.ClientSiteName + " updated successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
            }

            return Json(new[] { model.ToViewModel() }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, StoreDeviceViewModel viewModel) 
        {
            var model = this.storeDevicesDataService.Get(viewModel.Id.GetValueOrDefault());
            if (model != null) 
            {
                this.storeDevicesDataService.Delete(model);
            }
            
            // Push the change out to the ACS servers
            string errorMessage = SyncHelper.ServerSync();

            // Success
            if (errorMessage.Length == 0)
            {
                TempData["message"] = "Store " + viewModel.ClientSiteName + " deleted successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
            }

            return Json(new[] { model.ToViewModel() }.ToDataSourceResult(request, this.ModelState)); 
        }

        [HttpPost]
        public ActionResult ReadScheama([DataSourceRequest]DataSourceRequest request, Guid id) 
        {
            var storeDevice = this.storeDevicesDataService.Get(id);

            if (!storeDevice.Device.ExternalApiId.HasValue) 
            {
                Json(Enumerable.Empty<ApiCredentialKeyPairViewModel>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            var externalAPIDefinitionParams = storeDevice.Device.ExternalApi == null ? string.Empty : storeDevice.Device.ExternalApi.DefinitionParameters;
            var models = this.externalApiService.ReadStoreDeviceSchema(externalAPIDefinitionParams, storeDevice.Parameters);

            return Json(models.ToDataSourceResult(request), JsonRequestBehavior.AllowGet); 
        }

        [HttpPost]
        public ActionResult UpdateSchema([DataSourceRequest]DataSourceRequest request, Guid id, ApiCredentialKeyPairViewModel model) 
        {
            var models = new[] { model };
            var storeDevice = this.storeDevicesDataService.Get(id);
            storeDevice.GenerateParameters(models);

            this.storeDevicesDataService.Update(storeDevice);

            // Push the change out to the ACS servers
            string errorMessage = SyncHelper.ServerSync();

            // Success
            if (errorMessage.Length == 0)
            {
                TempData["message"] = "Store schema " + model.Key + " updated successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
            }

            return Json(models.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
    
    //public class DevicesController : BaseController
    //{
    //    private readonly IDevicesDataService devicesDataService;
    //    private readonly IExternalApiDataService externalApiDataService; 

    //    public DevicesController()
    //    {
    //        this.devicesDataService = new DevicesDataService();
    //        this.externalApiDataService = new ExternalApiDataService();
    //    }

    //    public ActionResult Index()
    //    {
    //        var vm = new StoreDevicesIndexViewModel();
    //        vm.Apis = this.externalApiDataService.List().Select(e => e.ToSelectViewModel());

    //        return View(vm);
    //    }

        

    //    public ActionResult Read([DataSourceRequest]DataSourceRequest request)
    //    {
    //        var data = this.devicesDataService.List(e => true);
    //        var result = data.ToDataSourceResult(request, e => e.ToViewModel());

    //        return Json(result, JsonRequestBehavior.AllowGet);
    //    }



    //    public ActionResult ReadOnly()
    //    {
    //        var data = this.devicesDataService.List(e => true);
    //        var results = data.Select(e => e.ToViewModel());

    //        return Json(results, JsonRequestBehavior.AllowGet);
    //    }

    //    public ActionResult Create([DataSourceRequest]DataSourceRequest request, ViewModels.StoreType.DeviceViewModel model)
    //    {
    //        var dataModel = model.ToDataModel();

    //        model.Id = dataModel.Id;

    //        this.devicesDataService.Create(dataModel);

    //        return Json(new[] { model }.ToDataSourceResult(request));
    //    }

    //    public ActionResult Update([DataSourceRequest]DataSourceRequest request, ViewModels.StoreType.DeviceViewModel model)
    //    {
    //        var dataModel = this.devicesDataService.Get(model.Id.GetValueOrDefault());
            
    //        dataModel.Name = model.Name;
    //        dataModel.ExternalApiId = model.ExternalApi == null ? null :
    //            model.ExternalApi.Id.HasValue ? model.ExternalApi.Id : null;

    //        this.devicesDataService.Update(dataModel);

    //        return Json(new[] { dataModel.ToViewModel() }.ToDataSourceResult(request));
    //    }

    //    public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, ViewModels.StoreType.DeviceViewModel model)
    //    {
    //        var dataModel = this.devicesDataService.Get(model.Id.GetValueOrDefault());

    //        if (dataModel != null)
    //        {
    //            this.devicesDataService.Destroy(dataModel);
    //        }

    //        return Json(new[] { dataModel.ToViewModel() }.ToDataSourceResult(request));
    //    }
    //}
}
