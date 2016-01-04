using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdminDataAccess.Domain;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.DataAccess;
using AndroAdmin.Helpers;
using AndroAdmin.Model;
using CloudSync;
using Andromeda.GPSIntegration;
using Andromeda.GPSIntegration.Bringg;
using Newtonsoft.Json;
using Andromeda.GPSIntegration.Model;
using System.Configuration;

namespace AndroAdmin.Controllers
{
    [Authorize]
    [Security(Permissions = "ViewStores")]
    public class StoreController : BaseController
    {
        private static IGPSIntegrationServices GPSIntegrationServices = new BringgGPSIntegrationServices();

        public StoreController()
        {
            ViewBag.SelectedMenu = MenuItemEnum.Stores;
            ViewBag.SelectedStoreMenu = StoresMenuItemEnum.Stores;
        }

        [Security(Permissions = "ViewStores")]
        public ActionResult Index()
        {
            try
            {
                // Get all the stores
                IEnumerable<AndroAdminDataAccess.Domain.StoreListItem> stores = this.StoreDAO.GetAllStoreListItems();

                return View(stores);
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.Index", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [Security(Permissions = "AddStore")]
        public ActionResult Add()
        {
            ActionResult actionResult = null;

            try
            {
                // Get a list of store statuses
                IList<AndroAdminDataAccess.Domain.StoreStatus> storeStatuses = this.StoreStatusDAO.GetAll();

                if (storeStatuses == null)
                {
                    actionResult = RedirectToAction("Index");
                }

                // Get a list of countries
                List<AndroAdminDataAccess.Domain.Country> countries = this.CountryDAO.GetAll();
                if (actionResult == null)
                {
                    if (countries == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }

                    // Insert a blank country so we can force the user to select a country
                    countries.Insert(0, new Country() { CountryName = "", Id = -1, ISO3166_1_alpha_2 = "", ISO3166_1_numeric = -1 });
                }

                // Get a list of chains
                IList<AndroAdminDataAccess.Domain.Chain> chains = this.ChainDAO.GetAll();
                if (actionResult == null)
                {
                    if (chains == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }

                    // Insert a blank chain so we can force the user to select a chain
                    chains.Insert(0, new Chain() { Id = -1, Name = "", Description = "" });
                }

                if (actionResult == null)
                {
                    StoreStatus defaultStoreStatus = null;

                    // See if we can find the "live" status
                    foreach (StoreStatus storeStatus in storeStatuses)
                    {
                        if (storeStatus.Status == "Live")
                        {
                            defaultStoreStatus = storeStatus;
                        }
                    }

                    // Default to something if "live" status not found
                    if (defaultStoreStatus == null)
                    {
                        defaultStoreStatus = storeStatuses[0];
                    }

                    StoreModel storeModel = new StoreModel()
                    {
                        StoreStatuses = storeStatuses,
                        StoreStatusId = defaultStoreStatus.Id,
                        CountryId = -1,
                        Countries = countries,
                        Chains = chains,
                        ChainId = -1
                    };

                    actionResult = View(storeModel);
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.Add", exception);

                return RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [HttpPost]
        [Security(Permissions = "AddStore")]
        public ActionResult Add(StoreModel storeModel)
        {
            ActionResult actionResult = null;

            try
            {
                // Get a list of store statuses
                IList<AndroAdminDataAccess.Domain.StoreStatus> storeStatuses = null;
                if (actionResult == null)
                {
                    storeStatuses = this.StoreStatusDAO.GetAll();

                    if (storeStatuses == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }

                // Get a list of countries
                List<AndroAdminDataAccess.Domain.Country> countries = null;
                if (actionResult == null)
                {
                    countries = this.CountryDAO.GetAll();

                    if (countries == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }

                    // Insert a blank country so we can force the user to select a country
                    countries.Insert(0, new Country() { CountryName = "", Id = -1, ISO3166_1_alpha_2 = "", ISO3166_1_numeric = -1 });
                }

                // Get a list of chains
                IList<AndroAdminDataAccess.Domain.Chain> chains = this.ChainDAO.GetAll();
                if (actionResult == null)
                {
                    if (chains == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }

                // Insert a blank chain so we can force the user to select a chain
                chains.Insert(0, new Chain() { Id = -1, Name = "", Description = "" });

                // Check that a store name was entered
                if (storeModel.Store.Name == null || storeModel.Store.Name == "")
                {
                    ModelState.AddModelError("Store.Name", "Please enter a store name");
                    actionResult = this.ModelError(storeModel, storeStatuses, countries, chains);
                }

                // Check that a store id was entered

                // Check that a customer store id was entered
                if (storeModel.Store.CustomerSiteId == null || storeModel.Store.CustomerSiteId == "")
                {
                    ModelState.AddModelError("Store.CustomerSiteId", "Please enter a customer store id");
                    actionResult = this.ModelError(storeModel, storeStatuses, countries, chains);
                }

                // Check that a country was selected
                if (storeModel.CountryId == -1)
                {
                    ModelState.AddModelError("Store.Address.Country", "Please select a country");
                    actionResult = this.ModelError(storeModel, storeStatuses, countries, chains);
                }

                // Check that a chain was selected
                if (storeModel.ChainId == -1)
                {
                    ModelState.AddModelError("Store.Chain", "Please select a chain");
                    actionResult = this.ModelError(storeModel, storeStatuses, countries, chains);
                }

                // Validate model
                if (!ModelState.IsValid)
                {
                    actionResult = this.ModelError(storeModel, storeStatuses, countries, chains);
                }

                // Has the store name already been used?
                if (actionResult == null)
                {
                    Store existingStore = this.StoreDAO.GetByName(storeModel.Store.Name);
                    if (existingStore != null)
                    {
                        ModelState.AddModelError("Store.Name", "Store name already used");
                        actionResult = this.ModelError(storeModel, storeStatuses, countries, chains);
                    }
                }

                // Has the store id already been used?
                if (actionResult == null)
                {
                    if (this.StoreDAO.GetByAndromedaId(storeModel.Store.AndromedaSiteId) != null)
                    {
                        ModelState.AddModelError("Store.AndromedaSiteId", "Store id already used");
                        actionResult = this.ModelError(storeModel, storeStatuses, countries, chains);
                    }
                }

                // Find the selected status
                if (actionResult == null)
                {
                    foreach (StoreStatus storeStatus in storeStatuses)
                    {
                        if (storeStatus.Id == storeModel.StoreStatusId)
                        {
                            storeModel.Store.StoreStatus = storeStatus;
                            break;
                        }
                    }
                }

                // Find the selected country
                if (actionResult == null)
                {
                    foreach (Country country in countries)
                    {
                        if (country.Id == storeModel.CountryId)
                        {
                            storeModel.Store.Address = new AndroAdminDataAccess.Domain.Address();
                            storeModel.Store.Address.Country = country;
                            break;
                        }
                    }
                }

                // Find the selected chain
                if (actionResult == null)
                {
                    foreach (Chain chain in chains)
                    {
                        if (chain.Id == storeModel.ChainId)
                        {
                            storeModel.Store.Chain = chain;
                            break;
                        }
                    }
                }

                if (actionResult == null)
                {
                    storeModel.Store.CustomerSiteName = storeModel.Store.Name;
                    storeModel.Store.ExternalSiteName = storeModel.Store.Name;

                    storeModel.Store.TimeZone = "GMT"; // default list item selected
                    // Add the store
                    this.StoreDAO.Add(storeModel.Store);

                    // Push the change out to the ACS servers
                    string errorMessage = SyncHelper.ServerSync();

                    // Success
                    if (errorMessage.Length == 0)
                    {
                        TempData["message"] = "Store " + storeModel.Store.Name + " successfully added";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
                    }

                    // Success
                    actionResult = RedirectToAction("StoreDetails", new { Id = storeModel.Store.Id });
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.Add (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        private ActionResult ModelError(StoreModel storeModel, IList<AndroAdminDataAccess.Domain.StoreStatus> storeStatuses, List<AndroAdminDataAccess.Domain.Country> countries, IList<AndroAdminDataAccess.Domain.Chain> chains)
        {
            storeModel.StoreStatuses = storeStatuses; // The model returned by MVC no longer includes the store statuses
            storeModel.Countries = countries; // The model returned by MVC no longer includes the countries
            storeModel.Chains = chains; // The model returned by MVC no longer includes the chains
            return View(storeModel);
        }

        [Security(Permissions = "ViewStores")]
        public ActionResult StoreDetails(int? id, bool? edit)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Do we need the view to switch to edit mode?
                    ViewBag.Edit = edit.HasValue ? edit : false;

                    StoreModel storeModel = null;
                    if (this.GetStoreModel(id.Value, edit, out storeModel))
                    {
                        actionResult = View(storeModel);
                    }
                    else
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }
                else
                {
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.Update", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "ViewStores")]
        public ActionResult StoreAddress(int? id, bool? edit)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Do we need the view to switch to edit mode?
                    ViewBag.Edit = edit.HasValue ? edit : false;

                    StoreModel storeModel = null;
                    if (this.GetStoreModel(id.Value, edit, out storeModel))
                    {
                        actionResult = View(storeModel);
                    }
                    else
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }
                else
                {
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.Update", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "ViewStores")]
        public ActionResult StoreGPSlocation(int? id, bool? edit)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Do we need the view to switch to edit mode?
                    ViewBag.Edit = edit.HasValue ? edit : false;

                    StoreModel storeModel = null;
                    if (this.GetStoreModel(id.Value, edit, out storeModel))
                    {
                        actionResult = View(storeModel);
                    }
                    else
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }
                else
                {
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.Update", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "ViewStores")]
        public ActionResult StoreOpeningTimes(int? id, bool? edit)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Do we need the view to switch to edit mode?
                    ViewBag.Edit = edit.HasValue ? edit : false;

                    StoreModel storeModel = null;
                    if (this.GetStoreModel(id.Value, edit, out storeModel))
                    {
                        actionResult = View(storeModel);
                    }
                    else
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }
                else
                {
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.Update", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "ViewStores")]
        public ActionResult Bringg(int? id, bool? edit)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Do we need the view to switch to edit mode?
                    ViewBag.Edit = edit.HasValue ? edit : false;

                    StoreModel storeModel = null;
                    if (!this.GetStoreModel(id.Value, edit, out storeModel))
                    {
                        actionResult = RedirectToAction("Index");
                    }

                    // Get the Bringg store details
                    Andromeda.GPSIntegration.Model.StoreConfiguration storeSettings = null;
                    
                    // Get the GPS integration config
                    ResultEnum result = StoreController.GPSIntegrationServices.GetStoreByAndromedaStoreId(id.Value, out storeSettings);

                    // Deserialize the bringg config
                    if (result == ResultEnum.NoStoreSettings)
                    {
                        StoreBringgConfigModel storeBringgConfigModel = new StoreBringgConfigModel()
                        {
                            Store = storeModel.Store,
                            GPSConfig = StoreController.GetDefaultBringgConfig()
                        };

                        actionResult = View(storeBringgConfigModel);
                    }
                    else if (result == ResultEnum.OK && storeSettings != null && !String.IsNullOrEmpty(storeSettings.PartnerConfiguration))
                    {
                        StoreBringgConfigModel storeBringgConfigModel = new StoreBringgConfigModel()
                        {
                            Store = storeModel.Store,
                            GPSConfig = JsonConvert.DeserializeObject<GPSConfig>(storeSettings.PartnerConfiguration)
                        };

                        actionResult = View(storeBringgConfigModel);
                    }
                    else
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }
                else
                {
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.Update", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        private bool GetStoreModel(int id, bool? edit, out StoreModel storeModel)
        {
            storeModel = null;

            // Get the store
            AndroAdminDataAccess.Domain.Store store = this.StoreDAO.GetById(id);
            if (store == null)
            {
                return false;
            }

            // Get a list of store statuses
            IList<AndroAdminDataAccess.Domain.StoreStatus> storeStatuses = this.StoreStatusDAO.GetAll();
            if (storeStatuses == null)
            {
                return false;
            }

            // Get a list of countries
            List<AndroAdminDataAccess.Domain.Country> countries = this.CountryDAO.GetAll();
            if (countries == null)
            {
                return false;
            }

            // Get a list of payment types
            IList<AndroAdminDataAccess.Domain.StorePaymentProvider> paymentTypes = this.StorePaymentProviderDAO.GetAll();
            if (paymentTypes == null)
            {
                return false;
            }

            // A list of opening times for each day
            Dictionary<string, List<TimeSpanBlock>> openingTimesByDay = new Dictionary<string, List<TimeSpanBlock>>();

            // Get a list of opening times for each day
            foreach (TimeSpanBlock timeSpanBlock in store.OpeningHours)
            {
                List<TimeSpanBlock> dayTimeSpanBlocks = null;

                // Is there a list of opening times for this day?
                if (!openingTimesByDay.TryGetValue(timeSpanBlock.Day, out dayTimeSpanBlocks))
                {
                    // No opening times for this day - create a new list
                    dayTimeSpanBlocks = new List<TimeSpanBlock>();
                    openingTimesByDay.Add(timeSpanBlock.Day, dayTimeSpanBlocks);
                }

                // Add the opeing time to this day
                dayTimeSpanBlocks.Add(timeSpanBlock);
            }

            // Sort the opening times
            foreach (List<TimeSpanBlock> blocks in openingTimesByDay.Values)
            {
                blocks.Sort(new TimeSpanBlockComparer());
            }

            // Figure out what mode each day is set to
            Dictionary<string, bool> isOpen = new Dictionary<string, bool>();
            this.GetIsOpen("Monday", openingTimesByDay, isOpen);
            this.GetIsOpen("Tuesday", openingTimesByDay, isOpen);
            this.GetIsOpen("Wednesday", openingTimesByDay, isOpen);
            this.GetIsOpen("Thursday", openingTimesByDay, isOpen);
            this.GetIsOpen("Friday", openingTimesByDay, isOpen);
            this.GetIsOpen("Saturday", openingTimesByDay, isOpen);
            this.GetIsOpen("Sunday", openingTimesByDay, isOpen);

            // Get a list of chains
            IList<AndroAdminDataAccess.Domain.Chain> chains = this.ChainDAO.GetAll(); 
            if (chains == null)
            {
                return false;
            }

            // Build the model
            storeModel = new StoreModel()
            {
                Store = store,
                StoreStatuses = storeStatuses,
                StoreStatusId = store.StoreStatus.Id,
                CountryId = store.Address.Country.Id,
                Countries = countries,
                Address = store.Address,
                StorePaymentProviders = paymentTypes,
                PaymentProviderId = store.PaymentProvider == null ? -1 : store.PaymentProvider.Id,
                OpeningTimesByDay = openingTimesByDay,
                IsOpen = isOpen,
                Chains = chains,
                ChainId = store.Chain.Id
            };

            // Which mode should the store times gui run in?
            List<string> permissions = this.PermissionsDAO.GetNamesByUserName(User.Identity.Name);
            storeModel.ShowEditOpeningTimes = permissions.Contains("EditStore");

            return true;
        }

        private void GetIsOpen(string dayName, Dictionary<string, List<TimeSpanBlock>> openingTimesByDay, Dictionary<string, bool> isOpen)
        {
            List<TimeSpanBlock> dayData = null;

            // Are there any opening times for this day?
            if (openingTimesByDay.TryGetValue(dayName, out dayData))
            {
                if (dayData.Count > 0)
                {
                    // Store is open
                    isOpen.Add(dayName, true);
                }
            }
            else
            {
                // There are no opening times
                isOpen.Add(dayName, false);
            }
        }

        [HttpPost]
        [Security(Permissions = "EditStore")]
        public ActionResult StoreDetails(StoreModel storeModel)
        {
            ActionResult actionResult = null;

            try
            {
                // Get the existing store model
                StoreModel existingStoreModel = null;
                if (!this.GetStoreModel(storeModel.Store.Id, true, out existingStoreModel))
                {
                    actionResult = RedirectToAction("Index");
                }

                // Find the selected status
                StoreStatus storeStatus = null;
                if (actionResult == null)
                {
                    foreach (StoreStatus checkStoreStatus in existingStoreModel.StoreStatuses)
                    {
                        if (checkStoreStatus.Id == storeModel.StoreStatusId)
                        {
                            storeStatus = checkStoreStatus;
                            break;
                        }
                    }
                }

                // Find the selected country
                Country country = null;
                if (actionResult == null)
                {
                    foreach (Country checkCountry in existingStoreModel.Countries)
                    {
                        if (checkCountry.Id == storeModel.CountryId)
                        {
                            country = checkCountry;
                            break;
                        }
                    }
                }

                // Apply the changes
                if (actionResult == null)
                {
                    existingStoreModel.Store.Name = storeModel.Store.Name;
                    existingStoreModel.Store.StoreStatus = storeStatus;
                    existingStoreModel.Store.CustomerSiteName = storeModel.Store.CustomerSiteName;
                    existingStoreModel.Store.CustomerSiteId = storeModel.Store.CustomerSiteId;
                    existingStoreModel.Store.ExternalSiteName = storeModel.Store.ExternalSiteName;
                    existingStoreModel.Store.ExternalSiteId = storeModel.Store.ExternalSiteId;
                    existingStoreModel.Store.Telephone = storeModel.Store.Telephone;
                    existingStoreModel.Store.TimeZone = storeModel.Store.TimeZone;
                    existingStoreModel.Store.Address.Country = country;
                }

                // Store name
                if (existingStoreModel.Store.Name == null || existingStoreModel.Store.Name.Length == 0)
                {
                    ModelState.AddModelError("Store.Name", "Please enter a store name ");
                    TempData["errorMessage"] = "There is a problem with the store details";
                    ViewBag.Edit = true;
                    actionResult = View(existingStoreModel);
                }

                // Store status
                if (storeStatus == null)
                {
                    ModelState.AddModelError("Store.StoreStatus.Status", "Please select a store status");
                    TempData["errorMessage"] = "There is a problem with the store details";
                    ViewBag.Edit = true;
                    actionResult = View(existingStoreModel);
                }

                // Has the store name already been used?
                if (actionResult == null)
                {
                    Store existingStore = this.StoreDAO.GetByName(storeModel.Store.Name);
                    if (existingStore != null && existingStore.Id != storeModel.Store.Id)
                    {                   
                        ModelState.AddModelError("Store.Name", "Store name already used");
                        TempData["errorMessage"] = "There is a problem with the store details";
                        ViewBag.Edit = true;
                        actionResult = View(existingStoreModel);
                    }
                }

                // Customer store name
                if (existingStoreModel.Store.CustomerSiteName == null || existingStoreModel.Store.CustomerSiteName.Length == 0)
                {
                    ModelState.AddModelError("Store.CustomerSiteName", "Please enter a customer store name ");
                    TempData["errorMessage"] = "There is a problem with the store details";
                    ViewBag.Edit = true;
                    actionResult = View(existingStoreModel);
                }

                // External store name
                if (existingStoreModel.Store.ExternalSiteName == null || existingStoreModel.Store.ExternalSiteName.Length == 0)
                {
                    ModelState.AddModelError("Store.ExternalSiteName", "Please enter an external store name ");
                    TempData["errorMessage"] = "There is a problem with the store details";
                    ViewBag.Edit = true;
                    actionResult = View(existingStoreModel);
                }

                // External store id
                if (existingStoreModel.Store.ExternalSiteId == null || existingStoreModel.Store.ExternalSiteId.Length == 0)
                {
                    ModelState.AddModelError("Store.ExternalSiteId", "Please enter a customer store ID ");
                    TempData["errorMessage"] = "There is a problem with the store details";
                    ViewBag.Edit = true;
                    actionResult = View(existingStoreModel);
                }

                // Payment provider
                if (storeModel.PaymentProviderId == -1)
                {
                    existingStoreModel.Store.PaymentProvider = null;
                }
                else
                {
                    foreach (StorePaymentProvider paymentProvider in existingStoreModel.StorePaymentProviders)
                    {
                        if (paymentProvider.Id == storeModel.PaymentProviderId)
                        {
                            existingStoreModel.Store.PaymentProvider = paymentProvider;
                            break;
                        }
                    }
                }

                // Chain
                foreach (Chain chain in existingStoreModel.Chains)
                {
                    if (chain.Id == storeModel.ChainId)
                    {
                        existingStoreModel.Store.Chain = chain;
                        break;
                    }
                }

                if (actionResult == null)
                {
                    // Update the store
                    this.StoreDAO.Update(existingStoreModel.Store);

                    // Push the change out to the ACS servers
                    string errorMessage = SyncHelper.ServerSync();

                    // Success
                    if (errorMessage.Length == 0)
                    {
                        TempData["message"] = "Store " + existingStoreModel.Store.Name + " successfully updated";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
                    }

                    actionResult = RedirectToAction("StoreDetails", new { Id = storeModel.Store.Id, edit = false });
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.StoreDetails (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [HttpPost]
        [Security(Permissions = "EditStore")]
        public ActionResult StoreAddress(StoreModel storeModel)
        {
            ActionResult actionResult = null;

            try
            {
                // Get the existing store model
                StoreModel existingStoreModel = null;
                if (!this.GetStoreModel(storeModel.Store.Id, true, out existingStoreModel))
                {
                    actionResult = RedirectToAction("Index");
                }

                // Find the selected country
                Country country = null;
                if (actionResult == null)
                {
                    foreach (Country checkCountry in existingStoreModel.Countries)
                    {
                        if (checkCountry.Id == storeModel.CountryId)
                        {
                            country = checkCountry;
                            break;
                        }
                    }
                }

                // Apply the changes
                if (actionResult == null)
                {
                    existingStoreModel.Store.Address.Country = country;

                    existingStoreModel.Store.Address.Org1 = storeModel.Store.Address.Org1;
                    existingStoreModel.Store.Address.Org2 = storeModel.Store.Address.Org2;
                    existingStoreModel.Store.Address.Org3 = storeModel.Store.Address.Org3;
                    existingStoreModel.Store.Address.Prem1 = storeModel.Store.Address.Prem1;
                    existingStoreModel.Store.Address.Prem2 = storeModel.Store.Address.Prem2;
                    existingStoreModel.Store.Address.Prem3 = storeModel.Store.Address.Prem3;
                    existingStoreModel.Store.Address.Prem4 = storeModel.Store.Address.Prem4;
                    existingStoreModel.Store.Address.Prem5 = storeModel.Store.Address.Prem5;
                    existingStoreModel.Store.Address.Prem6 = storeModel.Store.Address.Prem6;
                    existingStoreModel.Store.Address.RoadNum = storeModel.Store.Address.RoadNum;
                    existingStoreModel.Store.Address.RoadName = storeModel.Store.Address.RoadName;
                    existingStoreModel.Store.Address.Town = storeModel.Store.Address.Town;
                    existingStoreModel.Store.Address.Locality = storeModel.Store.Address.Locality;
                    existingStoreModel.Store.Address.County = storeModel.Store.Address.County;
                    existingStoreModel.Store.Address.State = storeModel.Store.Address.State;
                    existingStoreModel.Store.Address.PostCode = storeModel.Store.Address.PostCode;
                    existingStoreModel.Store.Address.DPS = storeModel.Store.Address.DPS;
                }

                // Country
                if (existingStoreModel.Store.Address.Country == null)
                {
                    ModelState.AddModelError("Store.Address.Country", "Please select a country");
                    TempData["errorMessage"] = "There is a problem with the store details";
                    ViewBag.Edit = true;
                    actionResult = View(existingStoreModel);
                }

                if (actionResult == null)
                {
                    // Update the store
                    this.StoreDAO.Update(existingStoreModel.Store);

                    // Push the change out to the ACS servers
                    string errorMessage = SyncHelper.ServerSync();

                    // Success
                    if (errorMessage.Length == 0)
                    {
                        TempData["message"] = "Store " + existingStoreModel.Store.Name + " successfully updated";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
                    }

                    actionResult = RedirectToAction("StoreAddress", new { Id = storeModel.Store.Id, edit = false });
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.StoreAddress (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [HttpPost]
        [Security(Permissions = "EditStore")]
        public ActionResult StoreGPSLocation(StoreModel storeModel)
        {
            ActionResult actionResult = null;

            try
            {
                // Get the existing store model
                StoreModel existingStoreModel = null;
                if (!this.GetStoreModel(storeModel.Store.Id, true, out existingStoreModel))
                {
                    actionResult = RedirectToAction("Index");
                }

                // Apply the changes
                if (actionResult == null)
                {
                    existingStoreModel.Store.Address.Lat = storeModel.Store.Address.Lat.Length > 15 ? storeModel.Store.Address.Lat.Substring(0, 15) : storeModel.Store.Address.Lat;
                    existingStoreModel.Store.Address.Long = storeModel.Store.Address.Long.Length > 15 ? storeModel.Store.Address.Long.Substring(0, 15) : storeModel.Store.Address.Long;
                }

                if (actionResult == null)
                {
                    // Update the store
                    this.StoreDAO.Update(existingStoreModel.Store);

                    // Push the change out to the ACS servers
                    string errorMessage = SyncHelper.ServerSync();

                    // Success
                    if (errorMessage.Length == 0)
                    {
                        TempData["message"] = "Store " + existingStoreModel.Store.Name + " successfully updated";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
                    }

                    actionResult = RedirectToAction("StoreGPSLocation", new { Id = storeModel.Store.Id, edit = false });
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.StoreGPSLocation (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "EditStore")]
        public ActionResult DeleteOpeningHour(string id, string openingHoursId)
        {
            ActionResult actionResult = null;

            try
            {
                int openingHours = Int32.Parse(openingHoursId);
                int storeId = Int32.Parse(id);

                // Get the existing store model
                StoreModel storeModel = null;
                if (!this.GetStoreModel(storeId, true, out storeModel))
                {
                    actionResult = RedirectToAction("Index");
                }

                if (actionResult == null)
                {
                    // Delete the opening hour
                    this.OpeningHoursDAO.DeleteById(storeId, openingHours);

                    // Push the change out to the ACS servers
                    string errorMessage = SyncHelper.ServerSync();

                    // Success
                    if (errorMessage.Length == 0)
                    {
                        TempData["message"] = "Store " + storeModel.Store.Name + " successfully updated";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
                    }

                    actionResult = RedirectToAction("StoreOpeningTimes", new { Id = storeModel.Store.Id, edit = true });
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.DeleteOpeningHour", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        /// <summary>
        /// Open or close a store for the specified day
        /// </summary>
        /// <param name="siteModel"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpPost]
        [Security(Permissions = "EditStore")]
        public ActionResult OpenCloseStore(AndroAdmin.Model.StoreModel storeModel, string day)
        {
            ActionResult actionResult = null;

            try
            {
                // Get the site model from the db as the one returned in the view is missing stuff
                if (storeModel.ChangeIsOpen)
                {
                    // Changed this day to open
                    // Remove all opening times
                    this.OpeningHoursDAO.DeleteBySiteIdDay(storeModel.Store.Id, day);

                    TimeSpanBlock timeSpanBlock = new TimeSpanBlock();
                    timeSpanBlock.Day = day;
                    timeSpanBlock.StartTime = "";
                    timeSpanBlock.EndTime = "";
                    timeSpanBlock.OpenAllDay = true;

                    // Add a single opening time with the isOpenAllDay flag set
                    this.OpeningHoursDAO.Add(storeModel.Store.Id, timeSpanBlock);
                }
                else
                {
                    // Changed this day to closed
                    // Remove all opening times for the day
                    this.OpeningHoursDAO.DeleteBySiteIdDay(storeModel.Store.Id, day);
                }

                // Push the change out to the ACS servers
                string errorMessage = SyncHelper.ServerSync();

                // Success
                if (errorMessage.Length == 0)
                {
                    TempData["message"] = "Store " + storeModel.Store.Name + " successfully updated";
                }
                else
                {
                    TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
                }

                actionResult = RedirectToAction("StoreOpeningTimes", new { Id = storeModel.Store.Id, edit = true });
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.OpenCloseStore", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        /// <summary>
        /// Add a block of time when the store will be open for a specific day of the week
        /// </summary>
        /// <param name="siteModel"></param>
        /// <param name="id"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpPost]
        [Security(Permissions = "EditStore")]
        public ActionResult StoreOpeningTimes(AndroAdmin.Model.StoreModel storeModel, string day)
        {
            ActionResult actionResult = null;

            try
            {
                string errorMessage = "";

                // Did we receive a start time?
                if (errorMessage.Length == 0)
                {
                    if (storeModel.AddOpeningStartTime.Length == 0)
                    {
                        errorMessage = "Missing start time";
                    }
                }

                // Did we receive a end time?
                if (errorMessage.Length == 0)
                {
                    if (storeModel.AddOpeningEndTime.Length == 0)
                    {
                        errorMessage = "Missing end time";
                    }
                }

                // Parse the start time
                TimeSpan startTimeSpan = TimeSpan.Zero;
                if (errorMessage.Length == 0)
                {
                    if (!OpeningTimesHelper.ParseTime(storeModel.AddOpeningStartTime, out startTimeSpan))
                    {
                        errorMessage = "Invalid start time";
                    }
                }

                // Parse the end time
                TimeSpan endTimeSpan = TimeSpan.Zero;
                if (errorMessage.Length == 0)
                {
                    if (!OpeningTimesHelper.ParseTime(storeModel.AddOpeningEndTime, out endTimeSpan))
                    {
                        errorMessage = "Invalid end time";
                    }
                }

                // Get the existing store model
                StoreModel existingStoreModel = null;
                if (!this.GetStoreModel(storeModel.Store.Id, true, out existingStoreModel))
                {
                    actionResult = RedirectToAction("Index");
                }

                if (errorMessage.Length == 0)
                {
                    // Validate start and end times
                    storeModel.OpeningTimesByDay = existingStoreModel.OpeningTimesByDay;
                    errorMessage = OpeningTimesHelper.CheckNewOpeningTime(day, storeModel, startTimeSpan, endTimeSpan);
                }

                // Was there an error?
                if (errorMessage.Length > 0)
                {
                    // It's a bit of a kludge but I couldn't think of a better way
                    existingStoreModel.ErrorMessage = errorMessage;
                    existingStoreModel.ErrorDay = day;
                    existingStoreModel.AddOpeningStartTime = storeModel.AddOpeningStartTime;
                    existingStoreModel.AddOpeningEndTime = storeModel.AddOpeningEndTime;

                    TempData["errorMessage"] = errorMessage;
                    ViewBag.Edit = true;

                    actionResult = View(existingStoreModel);
                }
                else
                {
                    // Get the current opening times for the day being updated
                    List<TimeSpanBlock> dayOpeningTimes = null;
                    if (storeModel.OpeningTimesByDay.TryGetValue(day, out dayOpeningTimes))
                    {
                        // Is the store currently set to be open all day?
                        if (dayOpeningTimes.Count == 1 && dayOpeningTimes[0].OpenAllDay)
                        {
                            // There is a special opening time that signifies the store is open all day so remove all opening times for the day
                            this.OpeningHoursDAO.DeleteBySiteIdDay(storeModel.Store.Id, day);
                        }
                    }

                    // Add the opening time
                    TimeSpanBlock timeSpanBlock = new TimeSpanBlock();
                    timeSpanBlock.Day = day;
                    timeSpanBlock.OpenAllDay = false;
                    timeSpanBlock.StartTime = startTimeSpan.Hours.ToString("00") + ":" + startTimeSpan.Minutes.ToString("00");
                    timeSpanBlock.EndTime = endTimeSpan.Hours.ToString("00") + ":" + endTimeSpan.Minutes.ToString("00");

                    // Add the opening hour
                    this.OpeningHoursDAO.Add(storeModel.Store.Id, timeSpanBlock);

                    // Push the change out to the ACS servers
                    errorMessage = SyncHelper.ServerSync();

                    // Success
                    if (errorMessage.Length == 0)
                    {
                        TempData["message"] = "Store " + storeModel.Store.Name + " successfully updated";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to synchronise with one or more ACS cloud server";
                    }

                    // Done
                    actionResult = RedirectToAction("StoreOpeningTimes", new { Id = storeModel.Store.Id, edit = true });
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.StoreOpeningTimes POST", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "ViewAMSStores")]
        public ActionResult StoreAMSUpload(int? id)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Get the store
                    AndroAdminDataAccess.Domain.Store store = this.StoreDAO.GetById(id.Value);

                    if (store == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                    else
                    {
                        // Build a list of AMS servers to display
                        IList<AMSServer> amsServers = this.AMSServerDAO.GetByChainId(store.Chain.Id.Value);
                        SelectList amsServersSelectList = new SelectList(amsServers, "Id", "DisplayName");

                        // Build a list of FTP sites to display
                        IList<FTPSite> ftpSites = this.FTPSiteDAO.GetByChainId(store.Chain.Id.Value);
                        SelectList ftpSitesSelectList = new SelectList(ftpSites, "Id", "Name");

                        // Get the stores ams server & ftp sites
                        IEnumerable<StoreAMSServerFtpSite> storeAmsServerFtpSites = this.StoreAMSServerFTPSiteDAO.GetBySiteId(id.Value);

                        // AMS server & ftp sites for this store
                        Dictionary<int, List<StoreAMSServerFtpSite>> storeIndex = new Dictionary<int, List<StoreAMSServerFtpSite>>();

                        foreach (StoreAMSServerFtpSite storeAMSServerFtpSite in storeAmsServerFtpSites)
                        {
                            // Find the ftp sites for this store & AMS server
                            List<StoreAMSServerFtpSite> amsServerFtpSites = null;
                            if (!storeIndex.TryGetValue(storeAMSServerFtpSite.StoreAMSServer.AMSServer.Id, out amsServerFtpSites))
                            {
                                // No ftp sites found for this particular store & AMS server
                                amsServerFtpSites = new List<StoreAMSServerFtpSite>();

                                storeIndex.Add(storeAMSServerFtpSite.StoreAMSServer.AMSServer.Id, amsServerFtpSites);
                            }

                            // Append the ftp server to the store & AMS server
                            amsServerFtpSites.Add(storeAMSServerFtpSite);
                        }

                        // Model for the view only
                        UpdateAMSandFTPSitesModel updateAMSandFTPSites = new UpdateAMSandFTPSitesModel();

                        // AMS servers
                        updateAMSandFTPSites.AMSServers = amsServersSelectList;

                        // FTP sites
                        updateAMSandFTPSites.FTPSites = ftpSitesSelectList;

                        // Store AMS Server Ftp Sites
                        updateAMSandFTPSites.StoreAMSServerFtpSites = storeIndex;

                        updateAMSandFTPSites.Store = store;

                        actionResult = View(updateAMSandFTPSites);
                    }
                }
                else
                {
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("AMSStoreController.StoreAMSUpload", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [HttpPost]
        [Security(Permissions = "EditAMSStore")]
        public ActionResult StoreAMSUpload(int? id, UpdateAMSandFTPSitesModel updateAMSandFTPSites)
        {
            ActionResult actionResult = null;

            try
            {
                // Check if id or updateAMSandFTPSites are null

                // Check for ams server id
                int amsServerId = 0;
                if (!int.TryParse(updateAMSandFTPSites.AMSServerId, out amsServerId))
                {
                    actionResult = RedirectToAction("Index");
                }

                // Check for ftp site id
                int ftpSiteId = 0;
                if (!int.TryParse(updateAMSandFTPSites.FTPSiteId, out ftpSiteId))
                {
                    actionResult = RedirectToAction("Index");
                }

                // Get the store
                Store store = null;
                if (actionResult == null)
                {
                    store = this.StoreDAO.GetById(id.Value);

                    if (store == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }

                // Get the AMS server
                AMSServer amsServer = null;
                if (actionResult == null)
                {
                    amsServer = this.AMSServerDAO.GetById(amsServerId);

                    if (amsServer == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }

                // Get the FTP site
                FTPSite ftpSite = null;
                if (actionResult == null)
                {
                    ftpSite = this.FTPSiteDAO.GetById(ftpSiteId);

                    if (ftpSite == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }

                // Get StoreAMSServer 
                StoreAMSServer storeAMSServer = null;
                if (actionResult == null)
                {
                    storeAMSServer = this.StoreAMSServerDAO.GetByStoreIdAMServerId(store.Id, amsServer.Id);

                    // If not exist then create it
                    if (storeAMSServer == null)
                    {
                        storeAMSServer = new StoreAMSServer();
                        storeAMSServer.AMSServer = amsServer;
                        storeAMSServer.Store = store;
                        storeAMSServer.Priority = 0;

                        this.StoreAMSServerDAO.Add(storeAMSServer);
                    }
                    else
                    {
                        // Check to see if the StoreAMSServer already has the FTP Site
                        if (StoreAMSServerFTPSiteDAO.GetBySiteIdAMSServerIdFTPSiteId(storeAMSServer.Id, ftpSiteId) != null)
                        {
                            // Already used!
                            TempData["message"] = "Store is already uploading to that AMS Server and FTP Site!!";

                            // Redisplay the view with the error message
                            actionResult = RedirectToAction("StoreAMSUpload", new { Id = id });
                        }
                    }
                }

                // Create StoreAMSServerFTPSite
                if (actionResult == null)
                {
                    StoreAMSServerFtpSite storeAMSServerFtpSite = new StoreAMSServerFtpSite();
                    storeAMSServerFtpSite.FTPSite = ftpSite;
                    storeAMSServerFtpSite.StoreAMSServer = storeAMSServer;

                    StoreAMSServerFTPSiteDAO.Add(storeAMSServerFtpSite);

                    // Success
                    TempData["message"] = "AMS Server / FTP site successfully added";
                    actionResult = RedirectToAction("StoreAMSUpload", new { Id = id });
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.StoreAMSUpload (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        public ActionResult StoreAMSUploadDelete(int? siteId, int? id)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Get the StoreAMSServerFTPSite object we're about to delete as we'll need some of it's data afterwards
                    StoreAMSServerFtpSite storeAMSServerFtpSite = this.StoreAMSServerFTPSiteDAO.GetById(id.Value);

                    // Delete the StoreAMSServerFTPSite object
                    this.StoreAMSServerFTPSiteDAO.DeleteById(id.Value);

                    // Get the StoreAMSServerFTPSite object
                    IList<StoreAMSServerFtpSite> storeAMSServerFtpSites = this.StoreAMSServerFTPSiteDAO.GetByStoreAMSServerId(storeAMSServerFtpSite.StoreAMSServer.Id);

                    // Are there any other ftp sites being used to upload to the AMS server for the store
                    if (storeAMSServerFtpSites.Count == 0)
                    {
                        // No other ftp servers.  We need to delete the StoreAMSServer object
                        this.StoreAMSServerDAO.DeleteById(storeAMSServerFtpSite.StoreAMSServer.Id);
                    }

                    actionResult = RedirectToAction("StoreAMSUpload", new { id = siteId });

                    TempData["message"] = "Data will no longer be uploaded via the FTP site";
                }
                else
                {
                    actionResult = RedirectToAction("StoreAMSUpload", new { id = siteId });
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.StoreAMSUploadDelete", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [HttpPost]
        [Security(Permissions = "EditStore")]
        public ActionResult Bringg(StoreBringgConfigModel storeBringgConfigModel)
        {
            ActionResult actionResult = null;

            try
            {
                // Validation
                if (storeBringgConfigModel.GPSConfig.isEnabled)
                {
                    if (!storeBringgConfigModel.GPSConfig.partnerConfig.companyId.HasValue)
                    {
                        ModelState.AddModelError("GPSConfig.partnerConfig.companyId", "Please enter a company id");
                        TempData["errorMessage"] = "Please enter a company id";
                        ViewBag.Edit = true;
                        return View(storeBringgConfigModel);
                    }
                    else if (String.IsNullOrEmpty(storeBringgConfigModel.GPSConfig.partnerConfig.accessToken))
                    {
                        ModelState.AddModelError("GPSConfig.partnerConfig.accessToken", "Please enter the Bringg access token");
                        TempData["errorMessage"] = "Please enter the Bringg access token";
                        ViewBag.Edit = true;
                        return View(storeBringgConfigModel);
                    }
                    else if (String.IsNullOrEmpty(storeBringgConfigModel.GPSConfig.partnerConfig.secretKey))
                    {
                        ModelState.AddModelError("GPSConfig.partnerConfig.secretKey", "Please enter the Bringg secret key");
                        TempData["errorMessage"] = "Please enter the Bringg secret key";
                        ViewBag.Edit = true;
                        return View(storeBringgConfigModel);
                    }
                    else if (String.IsNullOrEmpty(storeBringgConfigModel.GPSConfig.partnerConfig.apiUrl))
                    {
                        ModelState.AddModelError("GPSConfig.partnerConfig.apiUrl", "Please enter the Bringg API url");
                        TempData["errorMessage"] = "Please enter the Bringg API url";
                        ViewBag.Edit = true;
                        return View(storeBringgConfigModel);
                    }
                }

                // Call Bringg to check the credentials
                if (!StoreController.CheckBringgConfig(storeBringgConfigModel))
                {
                    ModelState.AddModelError("GPSConfig.partnerConfig.apiUrl", "Please enter the Bringg API url");
                    TempData["errorMessage"] = "Please enter the Bringg API url";
                    ViewBag.Edit = true;
                    return View(storeBringgConfigModel);
                }

                // Get the Bringg store details
                Andromeda.GPSIntegration.Model.StoreConfiguration storeSettings = null;

                // Get the GPS integration config
                ResultEnum result = StoreController.GPSIntegrationServices.GetStoreByAndromedaStoreId(storeBringgConfigModel.Store.Id, out storeSettings);

                GPSConfig gpsConfig = null;
                if (result == ResultEnum.NoStoreSettings)
                {
                    gpsConfig = StoreController.GetDefaultBringgConfig();
                }
                else if (result == ResultEnum.OK)
                {
                    // Deserialize the bringg config
                    if (result == ResultEnum.OK && storeSettings != null && !String.IsNullOrEmpty(storeSettings.PartnerConfiguration))
                    {
                        gpsConfig = JsonConvert.DeserializeObject<GPSConfig>(storeSettings.PartnerConfiguration);
                    }
                    else
                    {
                        TempData["message"] = "Store successfully updated";
                        return RedirectToAction("Index");
                    }
                }

                // Update the GPS integration config
                gpsConfig.isEnabled = storeBringgConfigModel.GPSConfig.isEnabled;
                gpsConfig.partnerName = "Bringg";
                gpsConfig.partnerConfig.companyId = storeBringgConfigModel.GPSConfig.partnerConfig.companyId;
                gpsConfig.partnerConfig.accessToken = storeBringgConfigModel.GPSConfig.partnerConfig.accessToken;
                gpsConfig.partnerConfig.secretKey = storeBringgConfigModel.GPSConfig.partnerConfig.secretKey;
                gpsConfig.partnerConfig.apiUrl = storeBringgConfigModel.GPSConfig.partnerConfig.apiUrl;
                gpsConfig.partnerConfig.clockInAPIUrl = storeBringgConfigModel.GPSConfig.partnerConfig.clockInAPIUrl;

                Andromeda.GPSIntegration.Model.StoreConfiguration storeBringgConfig = new Andromeda.GPSIntegration.Model.StoreConfiguration()
                {
                    AndromedaStoreId = storeBringgConfigModel.Store.Id,
                    PartnerConfiguration = JsonConvert.SerializeObject(gpsConfig)
                };

                result = StoreController.GPSIntegrationServices.UpdateStore(storeBringgConfig);

                // Get the bringg config
                if (result == ResultEnum.OK)
                {
                    actionResult = RedirectToAction("Bringg", new { Id = storeBringgConfigModel.Store.Id, edit = false });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                AndroAdmin.Helpers.ErrorHelper.LogError("StoreController.Bring (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        private static GPSConfig GetDefaultBringgConfig()
        {
            return new GPSConfig()
                {
                    isEnabled = false,
                    partnerName = "Bringg",
                    partnerConfig = new BringgConfig()
                    {
                        accessToken = "",
                        apiCallsEnabled = true,
                        apiUrl = "http://developer-api.bringg.com/partner_api",
                        companyId = null,
                        secretKey = "",
                        testMode = false
                    }
                };
        }

        private static bool CheckBringgConfig(StoreBringgConfigModel storeBringgConfigModel)
        {
            bool success = false;

            string verifyBringgAccountDetails = ConfigurationManager.AppSettings["verifyBringgAccountDetails"];

            if (String.IsNullOrEmpty(verifyBringgAccountDetails) || verifyBringgAccountDetails.ToLower() != "true")
            {
                return true; // Don't verify the account details - assume they are true
            }

            if (storeBringgConfigModel.GPSConfig.isEnabled)
            {
                GPSConfig gpsConfig = StoreController.GetDefaultBringgConfig();

                gpsConfig.isEnabled = storeBringgConfigModel.GPSConfig.isEnabled;
                gpsConfig.partnerName = "Bringg";
                gpsConfig.partnerConfig.companyId = storeBringgConfigModel.GPSConfig.partnerConfig.companyId;
                gpsConfig.partnerConfig.accessToken = storeBringgConfigModel.GPSConfig.partnerConfig.accessToken;
                gpsConfig.partnerConfig.secretKey = storeBringgConfigModel.GPSConfig.partnerConfig.secretKey;
                gpsConfig.partnerConfig.apiUrl = storeBringgConfigModel.GPSConfig.partnerConfig.apiUrl;

                Andromeda.GPSIntegration.Model.StoreConfiguration storeBringgConfig = new Andromeda.GPSIntegration.Model.StoreConfiguration()
                {
                    AndromedaStoreId = storeBringgConfigModel.Store.Id,
                    PartnerConfiguration = JsonConvert.SerializeObject(gpsConfig)
                };

                ResultEnum result = StoreController.GPSIntegrationServices.ValidateCredentials(storeBringgConfig);

                success = result == ResultEnum.OK;
            }
            else
            {
                success = true;
            }

            return success;
        }
    }
}
