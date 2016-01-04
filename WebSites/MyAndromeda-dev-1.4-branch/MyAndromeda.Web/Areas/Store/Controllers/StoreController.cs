using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Monads;
using MyAndromeda.CloudSynchronization.Services;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Web.Models;
using MyAndromedaDataAccess.Domain;
using System.Web.Routing;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Logging;
using MyAndromedaDataAccess.DataAccess;

namespace MyAndromeda.Web.Areas.Store.Controllers
{
    [MyAndromedaAuthorize]
    public class StoreController : Controller
    {
        private readonly WorkContextWrapper workContextWrapper;
        private readonly INotifier notifier;
        private readonly IAuthorizer authorizer;
        private readonly IUserAccessDataService userAccessDataService;
        private readonly ISiteDataService siteDataService;
        private readonly IStoreDataService storeDataService;

        private readonly ISiteDetailsDataAccess siteDetailsDataAccess;
        private readonly ICountryDataAccess countryDataAccess;
        private readonly IAddressDataAccess addressDataAccess;

        private readonly ISynchronizationTaskService synchronizationTaskService;

        public StoreController(WorkContextWrapper workContextWrapper, INotifier notifier, IAuthorizer authorizer, 
            IUserAccessDataService userAccessDataService, 
            ISiteDataService siteDataService, 
            IMyAndromedaLogger logger, 
            ISynchronizationTaskService synchronizationTaskService, 
            IStoreDataService storeDataService, 
            ISiteDetailsDataAccess siteDetailsDataAccess, 
            ICountryDataAccess countryDataAccess, 
            IAddressDataAccess addressDataAccess)
        {
            this.addressDataAccess = addressDataAccess;
            this.countryDataAccess = countryDataAccess;
            this.siteDetailsDataAccess = siteDetailsDataAccess;
            this.storeDataService = storeDataService;
            this.synchronizationTaskService = synchronizationTaskService;
            this.siteDataService = siteDataService;
            this.userAccessDataService = userAccessDataService;
            this.authorizer = authorizer;
            this.notifier = notifier;
            this.Logger = logger;
            this.workContextWrapper = workContextWrapper;
        }

        /// <summary>
        /// Gets or sets the data access factory.
        /// </summary>
        /// <value>The data access factory.</value>
        //public IDataAccessFactory DataAccessFactory { get; private set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public IMyAndromedaLogger Logger { get; private set; }

        public IAddressDataAccess AddressDataAccess
        {
            get
            {
                return this.addressDataAccess;
            }
        }

        public ActionResult Index(bool? edit)
        {
            if (!this.authorizer.AuthorizeAny(StoreUserPermissions.ViewEdtTime, StoreUserPermissions.ViewOpeningTimes, StoreUserPermissions.ViewStoreAddress, StoreUserPermissions.ViewStoreDetails)) 
            {
                return new HttpUnauthorizedResult();
            }

            if (!this.workContextWrapper.Current.CurrentSite.Available)
            {
                return RedirectToRoute("Error", new { message = "There is a problem accessing the site", Area = string.Empty });
            }

            var site = this.workContextWrapper.Current.CurrentSite.Site;

            SiteModel siteModel = null;
            this.GetSiteModel(site.Id, out siteModel);

            if (edit.HasValue && edit.Value)
            {
                siteModel.Editable = true;
            }
            else
            {
                siteModel.Editable = false;
            }

            return View(siteModel);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost(SiteModel siteModel)
        {
            ActionResult actionResult = null;

            if (!this.authorizer.AuthorizeAny(StoreUserPermissions.EditEdtTime, StoreUserPermissions.EditOpeningTimes, StoreUserPermissions.EditStoreAddress, StoreUserPermissions.EditStoreDetails)) 
            {
                return new HttpUnauthorizedResult();
            }

            try
            {
                var store = this.siteDataService.List(e => e.ExternalId == siteModel.ExternalSiteId).SingleOrDefault();
                store.CheckNull("store doesn't exist");

                if (!this.userAccessDataService.IsTheUserAssociatedWithStore(this.workContextWrapper.Current.CurrentUser.User.Id, store.Id))
                {
                    this.Logger.Debug("GET site");
                    actionResult = RedirectToRoute("Error", new { message = "You do not have permission to access this site" });
                }

                if (actionResult == null)
                {
                    // Get the site details
                    SiteModel updatedSiteModel = null;
                    string errorMessage = this.GetSiteModel(store.Id, out updatedSiteModel);

                    if (errorMessage.Length > 0)
                    {
                        actionResult = RedirectToRoute("Error", new { message = errorMessage });
                    }

                    // Make the changes to the object
                    updatedSiteModel.SiteDetails.ClientSiteName = siteModel.SiteDetails.ClientSiteName;
                    updatedSiteModel.SiteDetails.ExternalSiteName = siteModel.SiteDetails.ExternalSiteName;
                    updatedSiteModel.SiteDetails.CustomerSiteId = siteModel.SiteDetails.CustomerSiteId;
                    updatedSiteModel.SiteDetails.Phone = siteModel.SiteDetails.Phone;

                    // Validation
                    if (actionResult == null)
                    {
                        if (siteModel.SiteDetails.ClientSiteName == null || siteModel.SiteDetails.ClientSiteName.Length == 0)
                        {
                            ViewBag.Error = "Please enter a site name";
                            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
                            routeValueDictionary.Add("edit", "True");
                            updatedSiteModel.Editable = true;
                            actionResult = View(updatedSiteModel);
                        }
                    }

                    if (actionResult == null)
                    {
                        if (siteModel.SiteDetails.ExternalSiteName == null || siteModel.SiteDetails.ExternalSiteName.Length == 0)
                        {
                            ViewBag.Error = "Please enter an external site name";
                            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
                            routeValueDictionary.Add("edit", "True");
                            updatedSiteModel.Editable = true;
                            actionResult = View(updatedSiteModel);
                        }
                    }

                    if (actionResult == null)
                    {
                        if (siteModel.SiteDetails.CustomerSiteId == null || siteModel.SiteDetails.CustomerSiteId.Length == 0)
                        {
                            ViewBag.Error = "Please enter a site id";
                            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
                            routeValueDictionary.Add("edit", "True");
                            updatedSiteModel.Editable = true;
                            actionResult = View(updatedSiteModel);
                        }
                    }

                    if (actionResult == null)
                    {
                        // Make the changes to the db
                        this.siteDetailsDataAccess.Update(store.Id, updatedSiteModel.SiteDetails);

                        actionResult = View(updatedSiteModel);
                    }
                }
            }
            catch (Exception exception)
            {
                this.Logger.Error("POST site", exception);
                actionResult = RedirectToRoute("Error", new { message = "Internal error" });
            }

            this.synchronizationTaskService.CreateTask(new MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.CloudSynchronizationTask()
            {
                Completed = false,
                Description = "Site Details have been updated",
                Name = "Site Update",
                Timestamp = DateTime.UtcNow,
                StoreId = this.workContextWrapper.Current.CurrentSite.AndromediaSiteId,
                InvokedByUserId = this.workContextWrapper.Current.CurrentUser.User.Id,
                InvokedByUserName = this.workContextWrapper.Current.CurrentUser.User.Username
            });

            this.notifier.Notify("Your changes have been saved.");

            return actionResult;
        }

        private string GetSiteModel(int siteId, out SiteModel siteModel)
        {
            siteModel = null;

            // Get the site details
            SiteDetails siteDetails = null;
            this.siteDetailsDataAccess.GetBySiteId(siteId, out siteDetails);

            if (siteDetails == null)
            {
                return "Unknown site";
            }

            //// A list of opening times for each day
            //Dictionary<string, List<TimeSpanBlock>> openingTimesByDay = new Dictionary<string, List<TimeSpanBlock>>();

            //// Get a list of opening times for each day
            //foreach (TimeSpanBlock timeSpanBlock in siteDetails.OpeningHours)
            //{
            //    List<TimeSpanBlock> dayTimeSpanBlocks = null;

            //    // Is there a list of opening times for this day?
            //    if (!openingTimesByDay.TryGetValue(timeSpanBlock.Day, out dayTimeSpanBlocks))
            //    {
            //        // No opening times for this day - create a new list
            //        dayTimeSpanBlocks = new List<TimeSpanBlock>();
            //        openingTimesByDay.Add(timeSpanBlock.Day, dayTimeSpanBlocks);
            //    }

            //    // Add the opening time to this day
            //    dayTimeSpanBlocks.Add(timeSpanBlock);
            //}

            //// Sort the opening times
            //foreach (List<TimeSpanBlock> blocks in openingTimesByDay.Values)
            //{
            //    blocks.Sort(new TimeSpanBlockComparer());
            //}

            // Figure out what mode each day is set to
            //Dictionary<string, bool> isOpen = new Dictionary<string, bool>();

            //this.GetIsOpen("Monday", openingTimesByDay, isOpen);
            //this.GetIsOpen("Tuesday", openingTimesByDay, isOpen);
            //this.GetIsOpen("Wednesday", openingTimesByDay, isOpen);
            //this.GetIsOpen("Thursday", openingTimesByDay, isOpen);
            //this.GetIsOpen("Friday", openingTimesByDay, isOpen);
            //this.GetIsOpen("Saturday", openingTimesByDay, isOpen);
            //this.GetIsOpen("Sunday", openingTimesByDay, isOpen);

            // Get a list of employees
            //List<Employee> employees = null;
            //this.DataAccessFactory.EmployeeDataAccess.GetBySiteId(siteDetails.Id, out employees);

            // Build the model that the view will use
            siteModel = new SiteModel();
            siteModel.SiteDetails = siteDetails;
            siteModel.ExternalSiteId = siteModel.SiteDetails.ExternalSiteId;
            //siteModel.Employees = employees;
            //siteModel.OpeningTimesByDay = openingTimesByDay;
            //siteModel.IsOpen = isOpen;


            return "";
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

        public ActionResult ChangeAddress(string externalSiteId)
        {
            if (!this.authorizer.AuthorizeAny(StoreUserPermissions.ViewStoreAddress, StoreUserPermissions.EditStoreAddress))
            {
                return new HttpUnauthorizedResult();
            }

            ActionResult actionResult = null;

            var store = this.siteDataService.List(e => e.ExternalId == externalSiteId).SingleOrDefault();
            store.CheckNull("store doesn't exist");

            if (!this.userAccessDataService.IsTheUserAssociatedWithStore(this.workContextWrapper.Current.CurrentUser.User.Id, store.Id))
            {
                this.Logger.Error("GET site");
                return RedirectToRoute("Error", new { message = "You do not have permission to access this site" });
            }

            try
            {
                
                SiteDetails siteDetails = null;
                if (actionResult == null)
                {
                    // Show the users name
                    ViewBag.Firstname = this.workContextWrapper.Current.CurrentUser.User.Firstname; // myAndromedaUser.Firstname;

                    this.siteDetailsDataAccess.GetBySiteId(store.Id, out siteDetails);

                    if (siteDetails == null)
                    {
                        actionResult = RedirectToRoute("Error", new { message = "Unknown site" });
                    }
                }

                // Get a list of countries
                List<MyAndromedaDataAccess.Domain.Country> countries = null;
                if (actionResult == null)
                {
                    this.countryDataAccess.GetAll(out countries);

                    if (countries == null)
                    {
                        actionResult = RedirectToRoute("Error", new { message = "Unknown site" });
                    }
                }

                if (actionResult == null)
                {
                    MyAndromeda.Web.Models.AddressModel addressModel = new MyAndromeda.Web.Models.AddressModel()
                    {
                        Address = siteDetails.Address,
                        ClientSiteName = siteDetails.ClientSiteName,
                        Countries = countries,
                        CountryId = siteDetails.Address.Country.Id,
                        ExternalSiteId = siteDetails.ExternalSiteId
                    };

                    actionResult = View(addressModel);
                }
            }
            catch (Exception exception)
            {
                this.Logger.Error("ChangeAddress", exception);
                actionResult = RedirectToRoute("Error", new { message = "Internal error" });
            }

            return actionResult;
        }

        [HttpPost]
        public ActionResult ChangeAddress(MyAndromeda.Web.Models.AddressModel addressModel)
        {
            if (!this.authorizer.AuthorizeAny(StoreUserPermissions.EditStoreAddress))
            {
                return new HttpUnauthorizedResult();
            }

            ActionResult actionResult = null;

            var store = this.siteDataService.List(e => e.ExternalId == addressModel.ExternalSiteId).SingleOrDefault();
            store.CheckNull("store doesn't exist");

            if (!this.userAccessDataService.IsTheUserAssociatedWithStore(this.workContextWrapper.Current.CurrentUser.User.Id, store.Id))
            {
                this.Logger.Debug("GET site");
                return RedirectToRoute("Error", new { message = "You do not have permission to access this site" });
            }


            try
            {

                if (ModelState.IsValid)
                {
                    // Find the selected country
                    addressModel.Address.CountryId = addressModel.CountryId;

                    this.AddressDataAccess.UpsertBySiteId(store.Id, addressModel.Address);

                    this.notifier.Notify("Address has been updated");

                    this.synchronizationTaskService.CreateTask(new MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.CloudSynchronizationTask()
                    {
                        Completed = false,
                        Description = "Site address have been updated",
                        Name = "Site Update",
                        Timestamp = DateTime.UtcNow,
                        StoreId = this.workContextWrapper.Current.CurrentSite.AndromediaSiteId,
                        InvokedByUserId = this.workContextWrapper.Current.CurrentUser.User.Id,
                        InvokedByUserName = this.workContextWrapper.Current.CurrentUser.User.Username
                    });


                    return this.RedirectToAction("Index");
                }

                
                // Get a list of countries
                List<MyAndromedaDataAccess.Domain.Country> countries = null;
                if (actionResult == null)
                {
                    this.countryDataAccess.GetAll(out countries);

                    if (countries == null)
                    {
                        actionResult = RedirectToRoute("Error", new { message = "Unknown site" });
                    }
                }

                SiteDetails siteDetails = null;
                this.siteDetailsDataAccess.GetBySiteId(store.Id, out siteDetails);

                if (addressModel == null)
                {
                    actionResult = RedirectToRoute("Error", new { message = "Unknown site" });
                }
                else
                {
                    addressModel.Countries = countries;
                    addressModel.ClientSiteName = siteDetails.ClientSiteName;

                    actionResult = View(addressModel);
                }

            }
            catch (Exception exception)
            {
                this.Logger.Error("ChangeAddress", exception);
                actionResult = RedirectToRoute("Error", new { message = "Internal error" });

                return actionResult;
            }

            

            return actionResult;
        }

        public ActionResult ChangeOpeningHours()
        {
            if (!this.authorizer.AuthorizeAny(StoreUserPermissions.ViewOpeningTimes, StoreUserPermissions.EditOpeningTimes))
            {
                return new HttpUnauthorizedResult();
            }


            ActionResult actionResult = null;

            try
            {
                // Does the user have permission to access this store?
                var store = this.workContextWrapper.Current.CurrentSite.Site;
                store.CheckNull("store doesn't exist");


                if (actionResult == null)
                {
                    SiteModel siteModel = null;
                    string errorMessage = this.GetSiteModel(store.Id, out siteModel);

                    if (errorMessage.Length > 0)
                    {
                        actionResult = RedirectToRoute("Error", new { message = errorMessage });
                    }
                    else
                    {
                        actionResult = View(siteModel);
                    }
                }
            }
            catch (Exception exception)
            {
                this.Logger.Error("ChangeOpeningHours", exception);
                actionResult = RedirectToRoute("Error", new { message = "Internal error" });

                return actionResult;
            }



            return actionResult;
        }


        [ChildActionOnly]
        public PartialViewResult ShowEdt() 
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult ChangeEdt() 
        {
            if (!this.authorizer.AuthorizeAny(StoreUserPermissions.ViewEdtTime, StoreUserPermissions.ViewEdtTime))
            {
                return new HttpUnauthorizedResult();
            }

            var model = this.storeDataService.Get(e => e.Id == this.workContextWrapper.Current.CurrentSite.SiteId);

            return View(model);
        }

        [HttpPost, ActionName("ChangeEdt")]
        public ActionResult ChangeEdtPost(int edt) 
        {
            if (!this.authorizer.AuthorizeAny(StoreUserPermissions.EditEdtTime))
            {
                return new HttpUnauthorizedResult();
            }

            var model = this.storeDataService.Get(e => e.Id == this.workContextWrapper.Current.CurrentSite.SiteId);
            model.EstimatedDeliveryTime = edt;

            this.storeDataService.Update(model);

            this.synchronizationTaskService.CreateTask(new MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.CloudSynchronizationTask()
            {
                Completed = false,
                Description = "EDT has been updated",
                Name = "EDT Update",
                Timestamp = DateTime.UtcNow,
                StoreId = this.workContextWrapper.Current.CurrentSite.AndromediaSiteId,
                InvokedByUserId = this.workContextWrapper.Current.CurrentUser.User.Id,
                InvokedByUserName = this.workContextWrapper.Current.CurrentUser.User.Username
            });


            this.notifier.Notify("Saved");
            this.notifier.Notify("Published");

            return RedirectToAction("Index");
        }
    }
}
