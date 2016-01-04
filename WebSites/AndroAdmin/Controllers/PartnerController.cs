using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.Helpers;
using AndroAdmin.Model;
using CloudSync;
using System.Configuration;

namespace AndroAdmin.Controllers
{
    [Authorize]
    [Security(Permissions="ViewACSPartners")]
    public class PartnerController : BaseController
    {
        public PartnerController()
        {
            ViewBag.SelectedMenu = MenuItemEnum.OnlineOrdering;
            ViewBag.SelectedWebOrderingMenu = WebOrderingMenuItemEnum.Partners;
        }

        public ActionResult Index()
        {
            try
            {
                // Get all the partners
                IList<AndroAdminDataAccess.Domain.Partner> partners = this.PartnerDAO.GetAll();

                return View(partners);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PartnerController.Index", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [Security(Permissions = "AddACSPartner")]
        public ActionResult Add()
        {
            AndroAdminDataAccess.Domain.Partner partner = new AndroAdminDataAccess.Domain.Partner();
            partner.ExternalId = Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").ToUpper();

            return View(partner);
        }

        [HttpPost]
        [Security(Permissions = "AddACSPartner")]
        public ActionResult Add(AndroAdminDataAccess.Domain.Partner partner)
        {
            ActionResult actionResult = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    actionResult = View(partner);
                }

                // Has the partner name already been used?
                if (actionResult == null)
                {
                    AndroAdminDataAccess.Domain.Partner existingPartner = this.PartnerDAO.GetByName(partner.Name);
                    if (existingPartner != null)
                    {
                        ModelState.AddModelError("Name", "Partner name already used");
                        actionResult = View(partner);
                    }
                }

                // Has the partner external id already been used?
                if (actionResult == null)
                {
                    AndroAdminDataAccess.Domain.Partner existingPartner = this.PartnerDAO.GetByExternalId(partner.ExternalId);
                    if (existingPartner != null)
                    {
                        ModelState.AddModelError("ExternalId", "External id already used");
                        actionResult = View(partner);
                    }
                }

                if (actionResult == null)
                {
                    // Add the store
                    this.PartnerDAO.Add(partner);

                    // Push the change out to the ACS servers
                    string errorMessage = SyncHelper.ServerSync();

                    // Success?
                    if (errorMessage.Length == 0)
                    {
                        TempData["message"] = "Partner successfully added";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to add partner";
                    }

                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PartnerController.Add (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "ViewACSPartners")]
        public ActionResult Details(int? id, bool? edit)
        {
            try
            {
                // Do we need to run in edit mode?
                if (edit.HasValue && edit.Value)
                {
                    ViewBag.Edit = true;
                }
                else
                {
                    ViewBag.Edit = null;
                }

                // Build the model to send to the view
                Model.PartnerModel partnerModel = this.GetPartnerModel(id.Value);

                return View(partnerModel);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PartnerController.Details", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        private Model.PartnerModel GetPartnerModel(int id)
        {
            // Get the partner
            AndroAdminDataAccess.Domain.Partner partner = this.PartnerDAO.GetById(id);

            // Get the partners applications
            IList<AndroAdminDataAccess.Domain.ACSApplication> acsApplications = this.ACSApplicationDAO.GetByPartnerId(id);

            // Build the model to send to the view
            Model.PartnerModel partnerModel = new Model.PartnerModel();
            partnerModel.Partner = partner;
            partnerModel.ACSApplications = acsApplications;

            return partnerModel;
        }

        [HttpPost]
        [Security(Permissions = "EditACSPartner")]
        public ActionResult Details(Model.PartnerModel partnerModel)
        {
            ActionResult actionResult = null;

            try
            {
                // Get the partner details from the database
                Model.PartnerModel existingPartnerModel = this.GetPartnerModel(partnerModel.Partner.Id);

                // The post back doesn't include the partner applications
                partnerModel.ACSApplications = existingPartnerModel.ACSApplications;

                if (!ModelState.IsValid)
                {
                    actionResult = View(partnerModel);
                }

                // Has the partner name already been used?
                if (actionResult == null)
                {
                    // Has the user changed the partner name?
                    if (existingPartnerModel.Partner.Name != partnerModel.Partner.Name)
                    {
                        AndroAdminDataAccess.Domain.Partner existingPartner = this.PartnerDAO.GetByName(partnerModel.Partner.Name);
                        if (existingPartner != null)
                        {
                            ModelState.AddModelError("Partner.Name", "Partner name already used");
                            actionResult = View(partnerModel);
                        }
                    }
                }

                // Has the partner external id already been used?
                if (actionResult == null)
                {
                    // Has the user changed the external id?
                    if (existingPartnerModel.Partner.ExternalId != partnerModel.Partner.ExternalId)
                    {
                        AndroAdminDataAccess.Domain.Partner existingPartner = this.PartnerDAO.GetByExternalId(partnerModel.Partner.ExternalId);
                        if (existingPartner != null)
                        {
                            ModelState.AddModelError("Partner.ExternalId", "External id already used");
                            actionResult = View(partnerModel);
                        }
                    }
                }

                if (actionResult == null)
                {
                    // Update the partner
                    this.PartnerDAO.Update(partnerModel.Partner);

                    // Push the change out to the ACS servers
                    string errorMessage = SyncHelper.ServerSync();

                    ViewBag.Edit = null;

                    // Success?
                    if (errorMessage.Length == 0)
                    {
                        TempData["message"] = "Partner successfully updated";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to update partner";
                    }

                    actionResult = View(partnerModel);
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PartnerController.Details (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "ViewACSPartners")]
        public ActionResult Application(int? id, bool? edit)
        {
            try
            {
                // Do we need to run in edit mode?
                if (edit.HasValue && edit.Value)
                {
                    ViewBag.Edit = true;
                }
                else
                {
                    ViewBag.Edit = null;
                }

                // Get the application model for the view
                Model.ACSApplicationModel acsApplicationModel = this.GetApplicationModel(id.Value, false);

                return View(acsApplicationModel);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PartnerController.Application", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        private Model.ACSApplicationModel GetApplicationModel(int id, bool getAllStores)
        {
            // Get the application
            AndroAdminDataAccess.Domain.ACSApplication acsApplication = this.ACSApplicationDAO.GetById(id);

            // Get the partner
            AndroAdminDataAccess.Domain.Partner partner = this.PartnerDAO.GetById(acsApplication.PartnerId);

            // Get the applications sites
            IList<AndroAdminDataAccess.Domain.Store> stores = null;
            IList<AndroAdminDataAccess.Domain.Store> appStores = null;
            IList<AndroAdminDataAccess.Domain.Store> allStores = null;

            Dictionary<int, AndroAdminDataAccess.Domain.Store> appStoresIndex = new Dictionary<int,AndroAdminDataAccess.Domain.Store>();

            appStores = this.StoreDAO.GetByACSApplicationId(acsApplication.Id);

            if (getAllStores)
            {
                allStores = this.StoreDAO.GetAll();
                stores = allStores;

                foreach (AndroAdminDataAccess.Domain.Store store in appStores)
                {
                    appStoresIndex.Add(store.Id, store);
                }
            }
            else
            {
                stores = appStores;
            }

            // Build the model to send to the view
            Model.ACSApplicationModel acsApplicationModel = new Model.ACSApplicationModel();
            acsApplicationModel.ACSApplication = acsApplication;
            acsApplicationModel.Partner = partner;
            acsApplicationModel.EnvironmentsList = this.EnvironmentDAO.GetAll();

            foreach (AndroAdminDataAccess.Domain.Store store in stores)
            {
                Model.StoreModel storeModel = new Model.StoreModel();
                storeModel.Store = store;
                acsApplicationModel.Stores.Add(storeModel);

                if (getAllStores)
                {
                    if (appStoresIndex.ContainsKey(store.Id))
                    {
                        storeModel.Selected = true;
                    }
                }
            }

            return acsApplicationModel;
        }

        [Security(Permissions = "ViewACSPartners")]
        public ActionResult ApplicationDiagnostics(int? id)
        {
            try
            {
                // Get the application model for the view
                Model.ACSApplicationModel acsApplicationModel = this.GetApplicationModel(id.Value, false);

                // Get hosts for the application
                string xml = "";
                string hostListUrl = ConfigurationManager.AppSettings["DiagnosticsACSSignpostServer"] + "host?applicationId=" + acsApplicationModel.ACSApplication.ExternalApplicationId;
                ViewBag.HostListUrl = hostListUrl;

                bool success = HttpHelper.RestGet(hostListUrl, out xml);
                if (success)
                {
                    ViewBag.HostList = XMLFormatter.FormatXml(xml);
                }
                else
                {
                    ViewBag.HostList = "Web service call failed";
                }

                // Get the site list for the application
                string siteListUrl = ConfigurationManager.AppSettings["DiagnosticsACSServer"] + "site?applicationId=" + acsApplicationModel.ACSApplication.ExternalApplicationId;
                success = HttpHelper.RestGet(siteListUrl, out xml);
                ViewBag.SiteListUrl = siteListUrl;

                if (success)
                {
                    ViewBag.SiteList = XMLFormatter.FormatXml(xml);
                }
                else
                {
                    ViewBag.HostList = "Web service call failed";
                }

                return View(acsApplicationModel);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PartnerController.ApplicationDiagnostics", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        [Security(Permissions = "EditACSPartner")]
        public ActionResult Application(Model.ACSApplicationModel acsApplicationModel)
        {
            ActionResult actionResult = null;

            try
            {
                // Get the application model for the view
                Model.ACSApplicationModel existingACSApplicationModel = this.GetApplicationModel(acsApplicationModel.ACSApplication.Id, false);

                // The postback doesn't include the application stores or partner
                acsApplicationModel.Stores = existingACSApplicationModel.Stores;
                acsApplicationModel.Partner = existingACSApplicationModel.Partner;
                acsApplicationModel.ACSApplication.EnvironmentName = existingACSApplicationModel.ACSApplication.EnvironmentName;

                if (!ModelState.IsValid)
                {
                    actionResult = View(acsApplicationModel);
                }

                // Has the application name already been used?
                if (actionResult == null)
                {
                    // Has the user changed the application name?
                    if (existingACSApplicationModel.ACSApplication.Name != acsApplicationModel.ACSApplication.Name)
                    {
                        AndroAdminDataAccess.Domain.ACSApplication existingACSApplication = this.ACSApplicationDAO.GetByName(acsApplicationModel.ACSApplication.Name);
                        if (existingACSApplication != null)
                        {
                            ModelState.AddModelError("ACSApplication.Name", "Application name already used");
                            actionResult = View(acsApplicationModel);
                        }
                    }
                }

                // Has the application external id already been used?
                if (actionResult == null)
                {
                    // Has the user changed the external id?
                    if (existingACSApplicationModel.ACSApplication.ExternalApplicationId != acsApplicationModel.ACSApplication.ExternalApplicationId)
                    {
                        AndroAdminDataAccess.Domain.ACSApplication existingACSApplication = this.ACSApplicationDAO.GetByExternalId(acsApplicationModel.ACSApplication.ExternalApplicationId);
                        if (existingACSApplication != null)
                        {
                            ModelState.AddModelError("ACSApplication.ExternalId", "Application id already used");
                            actionResult = View(acsApplicationModel);
                        }
                    }
                }

                if (actionResult == null)
                {
                    // Update the application
                    this.ACSApplicationDAO.Update(acsApplicationModel.ACSApplication);

                    ViewBag.Edit = null;

                    // Push the change out to the ACS servers
                    string errorMessage = SyncHelper.ServerSync();

                    if (String.IsNullOrEmpty(errorMessage))
                    {
                        // Push the change out to the signpost server
                        errorMessage = SignpostServerSync.ServerSync();
                    }

                    // Success?
                    if (String.IsNullOrEmpty(errorMessage))
                    {
                        TempData["message"] = "Application successfully updated";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to update Application";
                    }

                    actionResult = View(acsApplicationModel);
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PartnerController.Application (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "EditACSPartner")]
        public ActionResult AddApplication(int? id)
        {
            try
            {
                // Get the application model for the view
                Model.ACSApplicationModel acsApplicationModel = new ACSApplicationModel();
                acsApplicationModel.ACSApplication = new AndroAdminDataAccess.Domain.ACSApplication();
                acsApplicationModel.ACSApplication.ExternalApplicationId = Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").ToUpper();
                acsApplicationModel.Partner = this.PartnerDAO.GetById(id.Value);

                // Get a list of environments
                acsApplicationModel.EnvironmentsList = this.EnvironmentDAO.GetAll();

                return View(acsApplicationModel);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PartnerController.AddApplication", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        [Security(Permissions = "EditACSPartner")]
        public ActionResult AddApplication(Model.ACSApplicationModel acsApplicationModel)
        {
            ActionResult actionResult = null;

            try
            {
                // The postback doesn't include the partner
                acsApplicationModel.Partner = this.PartnerDAO.GetById(acsApplicationModel.Partner.Id);

                // Has the application name already been used?
                if (actionResult == null)
                {
                    // Has the user changed the application name?
                    AndroAdminDataAccess.Domain.ACSApplication existingACSApplication = this.ACSApplicationDAO.GetByName(acsApplicationModel.ACSApplication.Name);
                    if (existingACSApplication != null)
                    {
                        ModelState.AddModelError("ACSApplication.Name", "Application name already used");
                        actionResult = View(acsApplicationModel);
                    }
                }

                // Has the application external id already been used?
                if (actionResult == null)
                {
                    // Has the user changed the external id?
                    AndroAdminDataAccess.Domain.ACSApplication existingACSApplication = this.ACSApplicationDAO.GetByExternalId(acsApplicationModel.ACSApplication.ExternalApplicationId);
                    if (existingACSApplication != null)
                    {
                        ModelState.AddModelError("ACSApplication.ExternalId", "Application id already used");
                        actionResult = View(acsApplicationModel);
                    }
                }

                if (actionResult == null)
                {
                    acsApplicationModel.ACSApplication.PartnerId = acsApplicationModel.Partner.Id;

                    // Update the application
                    this.ACSApplicationDAO.Add(acsApplicationModel.ACSApplication);

                    ViewBag.Edit = null;

                    // Push the change out to the ACS servers
                    string errorMessage = SyncHelper.ServerSync();

                    // Success?
                    if (errorMessage.Length == 0)
                    {
                        TempData["message"] = "Application successfully added";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to add Application";
                    }

                    actionResult = RedirectToAction("Details", new { id = acsApplicationModel.Partner.Id });
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PartnerController.AddApplication (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "EditACSPartner")]
        public ActionResult ApplicationStores(int? id)
        {
            try
            {
                // Get the application model for the view
                Model.ACSApplicationModel acsApplicationModel = this.GetApplicationModel(id.Value, true);

                return View(acsApplicationModel);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PartnerController.ApplicationStores", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        [Security(Permissions = "EditACSPartner")]
        public ActionResult ApplicationStores(Model.ACSApplicationModel acsApplicationModel)
        {
            ActionResult actionResult = null;

            try
            {
                // Get the existing application model for the view so we can see how it's changed
                Model.ACSApplicationModel existingACSApplicationModel = this.GetApplicationModel(acsApplicationModel.ACSApplication.Id, false);

                // Build a dictionary of stores so we can do fast lookups
                Dictionary<int, StoreModel> existingStoreModels = new Dictionary<int,StoreModel>();
                foreach (StoreModel storeModel in existingACSApplicationModel.Stores)
                {
                    existingStoreModels.Add(storeModel.Store.Id, storeModel);
                }

                // Stores that need to be associated with the partner
                List<StoreModel> addStores = new List<StoreModel>();

                // Stores that need to be un-associated with the partner
                List<StoreModel> removeStores = new List<StoreModel>();

                // Figure out which stores have changed
                foreach (StoreModel storeModel in acsApplicationModel.Stores)
                {
                    // Does the user want the store to be associated with the partner?
                    if (storeModel.Selected)
                    {
                        // Is the store already allocated to the partner?
                        StoreModel existingStoreModel = null;
                        if (!existingStoreModels.TryGetValue(storeModel.Store.Id, out existingStoreModel))
                        {
                            // This store needs to be associated with the partner
                            addStores.Add(storeModel);
                        }                    
                    }
                    else
                    {
                        // Is the store already allocated to the partner?
                        StoreModel existingStoreModel = null;
                        if (existingStoreModels.TryGetValue(storeModel.Store.Id, out existingStoreModel))
                        {
                            // This store needs to be un-assocated with the partner
                            removeStores.Add(existingStoreModel);
                        }
                    }
                }

                // Associate stores with the partner
                foreach (StoreModel addStoreModel in addStores)
                {
                    this.ACSApplicationDAO.AddStore(addStoreModel.Store.Id, acsApplicationModel.ACSApplication.Id);
                }

                // Un-associate stores with the partner
                foreach (StoreModel removeStoreModel in removeStores)
                {
                    this.ACSApplicationDAO.RemoveStore(removeStoreModel.Store.Id, acsApplicationModel.ACSApplication.Id);
                }

                // Push the change out to the ACS servers
                string errorMessage = SyncHelper.ServerSync();

                ViewBag.Edit = null;

                // Success?
                if (errorMessage.Length == 0)
                {
                    TempData["message"] = "ApplicationStores successfully updated";
                }
                else
                {
                    TempData["errorMessage"] = "Failed to update Application stores";
                }

                actionResult = RedirectToAction("Application", new { id = acsApplicationModel.ACSApplication.Id, edit = false });
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PartnerController.ApplicationStores (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }
    }
}
