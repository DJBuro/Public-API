using AndroAdmin.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;
using AndroAdmin.ViewModels.HostV2;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.DataAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace AndroAdmin.Controllers
{
    [Security(Permissions = Permissions.EditServicesHostList.ReadStoreHostList)]
    public class HostV2ForStoresController : BaseController
    {
        private readonly IStoreDAO storeDataLayer = AndroAdminDataAccessFactory.GetStoreDAO();
        private readonly IHostV2DataService hostV2DataService = AndroAdminDataAccessFactory.GetHostV2DataService();  
        private readonly IHostV2ForStoreDataService hostV2ForStoreDataService = AndroAdminDataAccessFactory.GetHostV2ForStoreDataService(); 

        public ActionResult Index(int id)
        {
            var store = storeDataLayer.GetById(id);
            var storeServerList = hostV2ForStoreDataService.ListConnectedHostsForSite(id);
            var validateAgainast = hostV2DataService.List().GroupBy(e => e.HostType.Name).ToDictionary(e => e.Key, e => e.ToList());

            var vm = new HostStoreSelectionViewModel()
            {
                Store = store,
                StoreId = store.Id,
                StoreHosts = storeServerList,
                Applications = base.ACSApplicationDAO.GetByStoreId(store.Id),
                PossibleHosts = validateAgainast,
                UserSelectedStoreHosts = storeServerList.Select(e=> e.Id)
            };

            return View(vm);
        }

        [ActionName("Index")]
        [HttpPost]
        [Security(Permissions = Permissions.EditServicesHostList.EditStoreHostList)]
        public ActionResult IndexPost(HostStoreSelectionViewModel model)
        {
            model.PossibleHosts = hostV2DataService.List().GroupBy(e => e.HostType.Name).ToDictionary(e=> e.Key, e=> e.ToList());

            if (model.UserSelectedStoreHosts != null && model.UserSelectedStoreHosts.Any()) 
            { 
                foreach (var group in model.PossibleHosts) 
                {
                    bool selectedfromGroup = group.Value.Any(e => model.UserSelectedStoreHosts.Any(selectedId => selectedId == e.Id));

                    if (selectedfromGroup) 
                    {
                        continue; 
                    }

                    this.ModelState.AddModelError(group.Key, string.Format("Please select a host from the {0} group", group.Key));
                    //SKIP LOOKING OVER ANYTHING ELSE, 
                    //USER NEEDS TO DO SOMETHING

                    break; 
                }
            }

            //if broke, fix it 
            if (!ModelState.IsValid)
            {
                model.Store = storeDataLayer.GetById(model.StoreId);
                return View(model);
            }

            //finish and move on 
            if (model.UserSelectedStoreHosts != null && model.UserSelectedStoreHosts.Any())
            {
                //Adding store specific hosts
                this.hostV2ForStoreDataService.AddCompleteRange(model.StoreId, model.UserSelectedStoreHosts);
            }
            else 
            {
                //no hosts selected
                this.hostV2ForStoreDataService.ClearAll(model.StoreId);
            }

            //off you go mr sync and tell me if anything went wrong 
            this.ValidateSync();

            if (!ModelState.IsValid) 
            {
                model.Store = storeDataLayer.GetById(model.StoreId);
                return View(model);
            }

            this.Notifier.Notify("The hosts for this store have been synced with ACS.");
            return RedirectToAction("Index", new { Id = model.StoreId });
        }

        public JsonResult ListStoreByHostV2Id([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            var data = this.hostV2ForStoreDataService.ListByHostId(id);

            return Json(data.ToDataSourceResult(request, e => new { e.Id, e.Name }));
        }
    }
}