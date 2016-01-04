using AndroAdmin.DataAccess;
using AndroAdmin.Helpers;
using AndroAdmin.ViewModels.HostV2;
using AndroAdminDataAccess.DataAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AndroAdmin.Controllers
{
    [Security(Permissions = Permissions.EditServicesHostList.ReadApplicationHostList)]
    public class HostV2ForApplicationController : BaseController
    {
        //
        // GET: /HostV2ForApplication/

        private readonly IHostV2DataService hostV2DataService = AndroAdminDataAccessFactory.GetHostV2DataService();
        private readonly IHostV2ForApplicationDataService hostV2ForApplicationDataService = AndroAdminDataAccessFactory.GetHostV2ForApplicationDataService();
        private readonly IACSApplicationDAO applicationDataLayer = AndroAdminDataAccessFactory.GetACSApplicationDAO();

        public ActionResult Index(int id)
        {
            var application = applicationDataLayer.GetById(id);
            var applicationServerList = hostV2ForApplicationDataService.ListConnectedHostsForApplication(id);
            
            var allHostsAvailable = hostV2DataService
                .List()
                .GroupBy(e => e.HostType.Name)
                .ToDictionary(e => e.Key, e => e.ToList());

            var vm = new HostApplicationSelectionViewModel()
            {
                Application = application,
                ApplicationId = application.Id,
                ApplicationHosts = applicationServerList,

                PossibleHosts = allHostsAvailable,
                UserSelectedAppplicationHosts = applicationServerList.Select(e => e.Id)
            };

            return View(vm);
        }

        [ActionName("Index")]
        [HttpPost]
        [Security(Permissions = Permissions.EditServicesHostList.EditApplicationHostList)]
        public ActionResult IndexPost(HostApplicationSelectionViewModel model)
        {
            model.PossibleHosts = hostV2DataService
                .List()
                .GroupBy(e => e.HostType.Name)
                .ToDictionary(e => e.Key, e => e.ToList());

            if (model.UserSelectedAppplicationHosts != null && model.UserSelectedAppplicationHosts.Any())
            { 
                //check that at least one is selected from each group
                foreach (var group in model.PossibleHosts)
                {
                    bool selectedfromGroup = group.Value.Any(e => model.UserSelectedAppplicationHosts.Any(selectedId => selectedId == e.Id));

                    if (!selectedfromGroup)
                    {
                        this.ModelState.AddModelError(group.Key, string.Format("Please select a host from the {0} group. Please note that the private services may respond with this list for the stores.", group.Key));
                        break;
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                model.Application = applicationDataLayer.GetById(model.ApplicationId);

                return View(model);
            
            }

            if (model.UserSelectedAppplicationHosts != null && model.UserSelectedAppplicationHosts.Any())
            {
                this.hostV2ForApplicationDataService.AddCompleteRange(model.ApplicationId, model.UserSelectedAppplicationHosts);
            }
            else 
            {
                this.hostV2ForApplicationDataService.ClearAll(model.ApplicationId);
            }

            this.ValidateSync();

            if (ModelState.IsValid) 
            {
                this.Notifier.Notify("The hosts for this application have been synced with ACS.");

                return RedirectToAction("Index", new { Id = model.ApplicationId });
            }

            model.Application = applicationDataLayer.GetById(model.ApplicationId);
            return View(model);
        }

        public JsonResult ListApplicationByHostV2Id([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            var data = this.hostV2ForApplicationDataService.ListByHostId(id);
            return Json(data.ToDataSourceResult(request, e => new { e.Id, e.Name, e.ExternalApplicationId }));
        }
    }
}
