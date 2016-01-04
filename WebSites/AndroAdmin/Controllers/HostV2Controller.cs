using AndroAdmin.DataAccess;
using AndroAdmin.Helpers;
using AndroAdmin.ViewModels.HostV2;
using AndroAdminDataAccess.DataAccess;
using AndroAdminDataAccess.EntityFramework;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AndroAdmin.Controllers
{
    [Security(Permissions = Permissions.EditServicesHostList.ReadHostList)]
    public class HostV2Controller : BaseController
    {
        private readonly IHostTypesDataService hostTypesDataService = AndroAdminDataAccessFactory.GetHostTypeDataService();
        private readonly IHostV2DataService hostV2dataService = AndroAdminDataAccessFactory.GetHostV2DataService(); 
             
        public ActionResult Index()
        {
            var types = hostTypesDataService.List();

            return View(types);
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request) 
        {
            var results = hostV2dataService.List().ToDataSourceResult(request, e =>
                new
                {
                    e.Id,
                    e.DataVersion,
                    e.Enabled,
                    e.OptInOnly,
                    e.Order,
                    e.Public,
                    e.Url,
                    e.Version,
                    e.HostTypeId,
                    e.LastUpdateUtc
                }
            );

            return Json(results);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Security(Permissions = Permissions.EditServicesHostList.EditHostList)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, NewHostViewModel model)
        {
            var newModel = new HostV2() 
            {
                Id = Guid.NewGuid(),
                Enabled = model.Enabled,
                HostTypeId = model.HostTypeId,
                OptInOnly = model.OptInOnly,
                Order = model.Order,
                Public = model.Public,
                Url = model.Url,
                Version = model.Version,
                LastUpdateUtc = DateTime.UtcNow
            };

            if (ModelState.IsValid) 
            { 
                hostV2dataService.Add(newModel);

                this.ValidateSync();
            }

            return Json(new[] { newModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Security(Permissions = Permissions.EditServicesHostList.EditHostList)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, EditHostViewModel model) 
        {
            var entity = hostV2dataService.List(e=> e.Id == model.Id).Single();

            if (ModelState.IsValid) 
            {
                entity.LastUpdateUtc = DateTime.UtcNow;
                entity.Enabled = model.Enabled;
                entity.HostTypeId = model.HostTypeId;
                entity.OptInOnly = model.OptInOnly;
                entity.Order = model.Order;
                entity.Public = model.Public;
                entity.Url = model.Url;
                entity.Version = model.Version;

                hostV2dataService.Update(entity);

                this.ValidateSync();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Security(Permissions = Permissions.EditServicesHostList.EditHostList)]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, HostV2 model)
        {
            if (model != null)
            {
                model.LastUpdateUtc = DateTime.UtcNow;
                hostV2dataService.Destroy(model);

                this.ValidateSync();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [Security(Permissions = Permissions.EditServicesHostList.EditHostList)]
        public ActionResult UpdateAll() 
        {
            hostV2dataService.UpdateVersionForAll();

            this.ValidateSync();

            if(this.ModelState.IsValid )
            {
                return RedirectToAction("Index") ;
            }

            return View();
        }


    }
}
