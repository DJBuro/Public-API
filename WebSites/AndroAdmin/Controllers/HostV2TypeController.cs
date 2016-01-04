using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdminDataAccess.DataAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using AndroAdminDataAccess.EntityFramework;
using AndroAdmin.DataAccess;
using AndroAdmin.Helpers;
using AndroAdmin.ViewModels.HostV2;

namespace AndroAdmin.Controllers
{
    public class HostV2TypeController : BaseController
    {
        private readonly IHostTypesDataService hostTypesDataService = AndroAdminDataAccessFactory.GetHostTypeDataService();
        private readonly IHostV2DataService hostV2DataService = AndroAdminDataAccessFactory.GetHostV2DataService();
        
        public HostV2TypeController() { }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var results = this.hostTypesDataService.List().ToDataSourceResult(request, e=> new {
                e.Id, 
                e.Name
            });

            return Json(results);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, ViewModels.HostV2.NewHostTypeViewModel model) 
        {
            var newModel = new HostType() { Id = Guid.NewGuid(), Name = model.Name };
            hostTypesDataService.Add(newModel);

            return Json(new[] { newModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, HostType model)
        {
            hostTypesDataService.Update(model);

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, HostType model)
        {
            if (model != null)
            {
                hostTypesDataService.Destroy(model);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReadHostsByTypeId([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            var hostList = hostV2DataService.List(e => e.HostTypeId == id);

            return Json(hostList.ToDataSourceResult(request, e =>
                new HostTypeDetailViewModel
                {
                    Id = e.Id,
                    OptInOnly = e.OptInOnly,
                    Enabled = e.Enabled,
                    Public = e.Public,
                    Url = e.Url,
                    Version = e.Version,
                    HostTypeId = e.HostTypeId
                })
            );
        }
    }
}
