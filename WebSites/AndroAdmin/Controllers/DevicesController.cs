using System.Linq;
using System.Web.Mvc;
using AndroAdmin.Helpers;
using AndroAdmin.ViewModels.ApiCredentials;
using AndroAdmin.ViewModels.StoreType;
using AndroAdminDataAccess.DataAccess;
using AndroAdminDataAccess.EntityFramework.DataAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using CloudSync;

namespace AndroAdmin.Controllers
{
    public class DevicesController : BaseController
    {
        private readonly IDevicesDataService devicesDataService;
        private readonly IStoreDevicesDataService storeDevicesDataService;
        private readonly IExternalApiDataService externalApiDataService; 

        public DevicesController()
        {
            this.devicesDataService = new DevicesDataService();
            this.externalApiDataService = new ExternalApiDataService();
            this.storeDevicesDataService = new StoreDevicesDataService();
        }

        public ActionResult Index()
        {
            var vm = new StoreDevicesIndexViewModel();
            vm.Apis = this.externalApiDataService.List().Select(e => e.ToSelectViewModel());

            return View(vm);
        }

        public ActionResult Read([DataSourceRequest]
                                 DataSourceRequest request)
        {
            var data = this.devicesDataService.List(e => true);
            var result = data.ToDataSourceResult(request, e => e.ToViewModel());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadOnly()
        {
            var data = this.devicesDataService.List(e => true);
            var results = data.Select(e => e.ToViewModel());

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create([DataSourceRequest]
                                   DataSourceRequest request, ViewModels.StoreType.DeviceViewModel model)
        {
            var dataModel = model.ToDataModel();

            model.Id = dataModel.Id;

            this.devicesDataService.Create(dataModel);

            // Push the change out to the ACS servers
            string errorMessage = SyncHelper.ServerSync();

            // Success
            if (errorMessage.Length == 0)
            {
                TempData["message"] = "Device " + dataModel.Name + " created successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
            }

            return Json(new[] { model }.ToDataSourceResult(request));
        }

        public ActionResult Update([DataSourceRequest]
                                   DataSourceRequest request, ViewModels.StoreType.DeviceViewModel model)
        {
            var dataModel = this.devicesDataService.Get(model.Id.GetValueOrDefault());
            
            dataModel.Name = model.Name;
            dataModel.ExternalApiId = model.ExternalApi == null ? null :
                                                                        model.ExternalApi.Id.HasValue ? model.ExternalApi.Id : null;

            this.devicesDataService.Update(dataModel);
            
            // Push the change out to the ACS servers
            string errorMessage = SyncHelper.ServerSync();

            // Success
            if (errorMessage.Length == 0)
            {
                TempData["message"] = "Device " + dataModel.Name + " updated successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
            }

            ActionResult actionResult = RedirectToAction("Update", new { Id = model.Id, edit = false });

            return Json(new[] { dataModel.ToViewModel() }.ToDataSourceResult(request));
        }

        public ActionResult Destroy([DataSourceRequest]
                                    DataSourceRequest request, ViewModels.StoreType.DeviceViewModel model)
        {
            var dataModel = this.devicesDataService.Get(model.Id.GetValueOrDefault());

            if (dataModel != null)
            {
                this.devicesDataService.Destroy(dataModel);
            }

            // Push the change out to the ACS servers
            string errorMessage = SyncHelper.ServerSync();

            // Success
            if (errorMessage.Length == 0)
            {
                TempData["message"] = "Device " + dataModel.Name + " deleted successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
            }
            return Json(new[] { dataModel.ToViewModel() }.ToDataSourceResult(request));
        }
    }
}