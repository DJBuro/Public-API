using AndroAdmin.Helpers;
using AndroAdmin.Services.Api;
using AndroAdmin.ViewModels.ApiCredentials;
using AndroAdminDataAccess.EntityFramework.DataAccess;
using CloudSync;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;


namespace AndroAdmin.Controllers
{
    public class ExternalApiController : BaseController
    {
        readonly IExternalApiDataService externalApiDataService = new ExternalApiDataService();
        readonly ExternalApiService externalApiService = new ExternalApiService();

        public ExternalApiController()
        {
        }

        public ActionResult Index() 
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var items = externalApiDataService.List();

            return this.Json(items.ToDataSourceResult(request, e => new ExernalApiViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                DefinitionParameters = e.DefinitionParameters
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadOnly([DataSourceRequest]DataSourceRequest request)
        {
            var items = externalApiDataService.List();

            var results = items.Select(e => e.ToViewModel());

            var fake = new[] { new ExernalApiViewModel() { Name = "None" } };

            return Json(fake.Union(results), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, ExernalApiViewModel model) 
        {
            if (externalApiDataService.List(e => e.Name == model.Name).Any()) 
            {
                this.ModelState.AddModelError("Name", "Api already exists");

                return Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
            }

            var dbModel = externalApiDataService.New();

            dbModel.Name = model.Name;
            dbModel.Parameters = string.Empty;
            dbModel.DefinitionParameters = string.Empty;

            externalApiDataService.Create(dbModel);
            //update the grid datasource for later updates
            model.Id = dbModel.Id;
            
            // Push the change out to the ACS servers
            string errorMessage = SyncHelper.ServerSync();

            // Success
            if (errorMessage.Length == 0)
            {
                TempData["message"] = "API " + model.Name + " created successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
            }

            return Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ExernalApiViewModel model)
        {
            var dbItem = externalApiDataService.Get(model.Id.GetValueOrDefault());
            dbItem.Name = model.Name;
            this.externalApiDataService.Update(dbItem);
            
            // Push the change out to the ACS servers
            string errorMessage = SyncHelper.ServerSync();

            // Success
            if (errorMessage.Length == 0)
            {
                TempData["message"] = "API " + model.Name + " updated successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
            }

            ActionResult actionResult = RedirectToAction("Update", new { Id = model.Id, edit = false });

            return Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, ExernalApiViewModel model)
        {
            var dbItem = externalApiDataService.Get(model.Id.GetValueOrDefault());

            externalApiDataService.Delete(dbItem);

            // Push the change out to the ACS servers
            string errorMessage = SyncHelper.ServerSync();

            // Success
            if (errorMessage.Length == 0)
            {
                TempData["message"] = "API " + model.Name + " deleted successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
            }

            return Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        public ActionResult ReadTemplate([DataSourceRequest]DataSourceRequest request, Guid id)
        {
            var dbItem = externalApiDataService.Get(id);
            var data = externalApiService.ReadSchema(dbItem);   

            return Json(data.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult CreateTemplate([DataSourceRequest]DataSourceRequest request, Guid id, ApiCredentialKeyPairViewModel model)
        {
            var models = new[] { model };
            var dbItem = externalApiDataService.Get(id);
            this.externalApiService.CreateSchema(dbItem, models);

            // Push the change out to the ACS servers
            string errorMessage = SyncHelper.ServerSync();

            // Success
            if (errorMessage.Length == 0)
            {
                TempData["message"] = "API key " + model.Key + " created successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
            }

            return Json(models.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult UpdateTemplate([DataSourceRequest]DataSourceRequest request, Guid id, ApiCredentialKeyPairViewModel model)
        {
            var models = new[] { model };
            var dbItem = externalApiDataService.Get(id);
            this.externalApiService.UpdateSchema(dbItem, models);

            // Push the change out to the ACS servers
            string errorMessage = SyncHelper.ServerSync();

            // Success
            if (errorMessage.Length == 0)
            {
                TempData["message"] = "API key " + model.Key + " updated successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
            }

            return Json(models.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult DeleteTemplate([DataSourceRequest]DataSourceRequest request, Guid id, ApiCredentialKeyPairViewModel model)
        {
            var models = new[] { model };
            var dbItem = externalApiDataService.Get(id);

            this.externalApiService.DeleteSchema(dbItem, models);

            // Push the change out to the ACS servers
            string errorMessage = SyncHelper.ServerSync();

            // Success
            if (errorMessage.Length == 0)
            {
                TempData["message"] = "API key " + model.Key + " deleted successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
            }

            return Json(models.ToDataSourceResult(request, this.ModelState));
        }
    }
}
