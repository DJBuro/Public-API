using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.CloudSynchronization.Services;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Web.Areas.Store.Models;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using System.Globalization;

namespace MyAndromeda.Web.Areas.Store.Controllers
{
    public class LocalizationController : Controller
    {
        private readonly ICurrentSite currentSite;
        private readonly IStoreDataService storeDataService;
        private readonly ISynchronizationTaskService synchronizationTaskService;
        private readonly ICurrentUser currentUser;
        private readonly INotifier notifier;
        private readonly IAuthorizer authorizer;

        public LocalizationController(ICurrentSite currentSite, IStoreDataService storeDataService, INotifier notifier, ISynchronizationTaskService synchronizationTaskService, ICurrentUser currentUser, IAuthorizer authorizer) 
        {
            this.authorizer = authorizer;
            this.currentUser = currentUser;
            this.synchronizationTaskService = synchronizationTaskService;
            this.storeDataService = storeDataService;
            this.notifier = notifier;
            this.currentSite = currentSite;
        }

        //
        // GET: /Store/Localization/
        public ActionResult Index()
        {
            if (!authorizer.Authorize(LocalizationPermissions.ViewAndEditLocalization))
            {
                return new HttpUnauthorizedResult();
            }

            var model = this.CreateViewModel();

            model.SelectedCultureType = this.currentSite.Store.UiCulture;
            model.SelectedTimeZoneId = this.currentSite.Store.TimeZoneInfoId;

            return View(model);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost() 
        {
            if (!authorizer.Authorize(LocalizationPermissions.ViewAndEditLocalization))
            {
                return new HttpUnauthorizedResult();
            }

            var model = this.CreateViewModel();
            this.UpdateModel(model);

            if (!this.ModelState.IsValid) 
            {
                this.notifier.Error("The localization was not saved.");
                return View(model);   
            }

            var store = this.storeDataService.Get(e => e.Id == this.currentSite.Store.Id);

            store.UiCulture = model.SelectedCultureType;
            store.TimeZoneInfoId = model.SelectedTimeZoneId;

            this.storeDataService.Update(store);

            this.notifier.Notify("The localization has been saved.");


            //or run me without competing with other websites calling the same database.
            //uncomment
            //var errorMessage = SyncHelper.ServerSync();

            this.synchronizationTaskService.CreateTask(new CloudSynchronizationTask()
            {
                Completed = false,
                Description = "Site localization has been updated",
                Name = "Site Update",
                Timestamp = DateTime.UtcNow,
                StoreId = this.currentSite.AndromediaSiteId,
                InvokedByUserId = this.currentUser.User.Id,
                InvokedByUserName = this.currentUser.User.Username
            });

            return View(model);
        }

        private Models.LocalizationViewModel CreateViewModel() 
        {
            var model= new Models.LocalizationViewModel();
            
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            IOrderedEnumerable<CultureChoiceViewModel> uiCultures = cultures.Select(e => new CultureChoiceViewModel
            {
                Name = CultureInfo.CreateSpecificCulture(e.Name).Name,
                EnglishName = e.EnglishName
            }).OrderBy(e=> e.Name);

            IOrderedEnumerable<TimeZoneViewModel> timeZones = TimeZoneInfo.GetSystemTimeZones()
                .Select(e=> new TimeZoneViewModel(){
                    DisplayName = e.DisplayName,
                    StandardName = e.StandardName,
                    Id = e.Id,
                    //e.BaseUtcOffset
                })
                .OrderBy(e => e.DisplayName);

            model.CultureChoices = uiCultures;
            model.TimezoneChoices = timeZones;


            return model;
        }
	}
}