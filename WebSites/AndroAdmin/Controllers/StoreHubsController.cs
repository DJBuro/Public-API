using System;
using System.Linq;
using System.Web.Mvc;
using AndroAdmin.Services;
using AndroAdmin.Helpers;
using AndroAdminDataAccess.DataAccess;
using CloudSync;

namespace AndroAdmin.Controllers
{
    [Security(Permissions = Permissions.ReadHubs)]
    public class StoreHubsController : BaseController
    {
        private readonly HubControllerService hubService = new HubControllerService();
        private readonly SiteHubControllerService storeHubService = new SiteHubControllerService();

        //private IStoreDAO storeDAO = null;

        public ActionResult Index(int id)
        {
            var store = this.storeHubService.GetStore(id);
            var allHubs = this.hubService.GetHubs();
            var selectedItems = this.storeHubService.GetSelectedHubs(id);
            
            var vm = new ViewModels.HubSelectionListViewModel() 
            { 
                StoreId = id,
                Name = store.Name,
                ExternalStoreId = store.ExternalSiteId,
                AllHubs = allHubs.ToList(),
                SelectedHubs = selectedItems.Select(e=> e.HubAddressId).ToList()
            };

            return View(vm);
        }

        [Security(Permissions = Permissions.EditHubs)]
        public ActionResult Edit(int id) 
        {
            var store = this.storeHubService.GetStore(id);
            var allHubs = this.hubService.GetHubs();
            var selectedItems = this.storeHubService.GetSelectedHubs(id);

            var vm = new ViewModels.HubSelectionListViewModel()
            {
                StoreId = id,
                Name = store.Name,
                ExternalStoreId = store.ExternalSiteId,
                AllHubs = allHubs.ToList(),
                SelectedHubs = selectedItems.Select(e => e.HubAddressId).ToList()
            };

            return View(vm);
        }

        [ActionName("Edit")]
        [HttpPost]
        [Security(Permissions = Permissions.EditHubs)]
        public ActionResult EditPost(int storeId) 
        {
            var store = this.storeHubService.GetStore(storeId);
            var allHubs = this.hubService.GetHubs();
            var previousHubs = this.storeHubService.GetSelectedHubs(storeId);

            var vm = new ViewModels.HubSelectionEditViewModel()
            {
                StoreId = store.Id,
                SelectedHubs = previousHubs.Select(e => e.HubAddressId).ToArray(),
                ExternalStoreId = store.ExternalSiteId,
                AllHubs = allHubs.ToList()
            };

            if (!TryUpdateModel(vm)) {
                return View(vm);
            }

            //select any that are not in the list anymore 
            var removeHubs = previousHubs.Where(storeHub => !vm.SelectedHubs.Any(selectedId => selectedId == storeHub.HubAddressId));
            foreach (var removeItem in removeHubs) 
            {
                this.storeHubService.RemoveHubFrom(store, removeItem);
            }

            //select any that are not in the database 
            var addHubs = vm.SelectedHubs.Where(id => !previousHubs.Any(storeHub => storeHub.HubAddressId == id));
            foreach (var newItem in addHubs) 
            {
                this.storeHubService.AddHubTo(store, newItem);
            }

            return RedirectToAction("Index", new { Id = vm.StoreId });
        }

        /// <summary>
        /// Maintenance reset.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <returns></returns>
        [Security(Permissions = Permissions.ResetHubHardwareKey)]
        public ActionResult MaintenanceReset(int storeId)
        {
            ActionResult result = null;

            try 
            {
                var store = this.storeHubService.GetStore(storeId);

                this.LogDAO.Add(new AndroAdminDataAccess.Domain.Log() { 
                    Created = DateTime.UtcNow,
                    Message = string.Format("hub hardware key reset. Store {0} (id: {1}; andro site id: {2})  has been updated by: {3}", 
                        store.Name, 
                        store.Id, 
                        store.AndromedaSiteId, 
                        this.User.Identity.Name),
                    StoreId = storeId.ToString(),
                    Source = "AndroAdmin.Controllers.MaintenanceReset",
                    Method = "MaintenanceReset",
                    Severity = "INFO",
                });

                this.storeHubService.ResetSiteHubHardwareKey(storeId);    
                // Success
                TempData["message"] = "Reset 'HardwareKey' has completed.";

                if (this.SyncCloud())
                {
                    result = RedirectToAction("Index", new { Id = storeId });
                }
            }
            catch (Exception exception)
            {
                TempData["errorMessage"] = "Reset 'HardwareKey' task failed";

                ErrorHelper.LogError("AndroAdmin.Controllers.StoreHubsController", exception);
            }

            return result == null ? 
                RedirectToAction("Index", "Error") : 
                result;
        }

        private bool SyncCloud() 
        {
            var response = true;
            try
            {
                // Push the change out to the ACS servers
                string errorMessage = SyncHelper.ServerSync();

                // Success
                if (errorMessage.Length == 0)
                {
                    TempData["message"] = "Cloud servers successfully updated";
                }
                else
                {
                    ErrorHelper.LogError("SyncCloud", new Exception(errorMessage));
                    TempData["errorMessage"] = "Failed to update cloud server(s)";
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("CloudServerController.Sync", exception);
                response = false;
            }

            return response;
        }

        public ActionResult Sync(int storeId) 
        {
            try
            {
                string errorMessage = SyncHelper.ServerSync();

                // Success
                if (errorMessage.Length == 0)
                {
                    TempData["message"] = "Cloud servers successfully updated";
                }
                else
                {
                    TempData["errorMessage"] = "Failed to update cloud servers";
                }

                return RedirectToAction("Index", new { Id = storeId });
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("CloudServerController.Sync", exception);
            }

            return RedirectToAction("Index", new { Id = storeId });
        }
    }
}
