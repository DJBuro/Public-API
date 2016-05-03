using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Authorization.Management;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Web.Areas.Authorization.ViewModels;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using MyAndromeda.Core.Services;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework;
using System.Collections.Generic;

namespace MyAndromeda.Web.Areas.Authorization.Controllers
{
    public class StoreEnrolmentPermissionController : Controller
    {
        private readonly IPermissionManager permissionManager;
        private readonly IEnrolmentService enrolementService;

        private readonly ISiteDataService siteDataService;
        private readonly INotifier notifier;
        private readonly IAuthorizer authorizer;
        private readonly ITranslator translator; 

        public StoreEnrolmentPermissionController(IPermissionManager permissionManager, IEnrolmentService enrolementService, INotifier notifier, ISiteDataService siteDataService, IAuthorizer authorizer, ITranslator translator)
        { 
            this.translator = translator;
            this.authorizer = authorizer;
            this.siteDataService = siteDataService;
            this.notifier = notifier;
            this.enrolementService = enrolementService;
            this.permissionManager = permissionManager;
        }

        public ActionResult Index()
        {
            if (!authorizer.Authorize(SiteEnrollmentUserPermissions.ViewSiteEnrollment))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedView));
                return new HttpUnauthorizedResult();
            }

            return View();
        }

        public ActionResult Levels() 
        {
            if (!authorizer.Authorize(SiteEnrollmentUserPermissions.EditSiteEnrollment)) 
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedView));
                return new HttpUnauthorizedResult();
            }

            IEnumerable<Core.Site.IEnrolmentLevel> levels = enrolementService.ListEnrolmentLevels();

            return View(levels);
        }

        public ActionResult Create() 
        {
            if (!authorizer.Authorize(SiteEnrollmentUserPermissions.EditSiteEnrollment))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));
                return new HttpUnauthorizedResult();
            }

            var viewModel = new EnrolmentLevelViewModel();

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult CreatePost(EnrolmentLevelViewModel viewModel) 
        {
            if (!authorizer.Authorize(SiteEnrollmentUserPermissions.EditSiteEnrollment))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));
                return new HttpUnauthorizedResult();
            }

            if (!ModelState.IsValid) 
            {
                return this.View(viewModel);
            }

            enrolementService.CreateOrUpdate(viewModel);
            this.notifier.Notify(string.Format(format: "Create: {0}", arg0: viewModel.Name));

            return RedirectToAction(actionName: "Levels");
        }

        public ActionResult EditEnrolement(int storeId) 
        {
            if (!authorizer.Authorize(SiteEnrollmentUserPermissions.EditSiteEnrollment))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));
                return new HttpUnauthorizedResult();
            }

            Data.Domain.Site store = this.siteDataService.List(e => e.Id == storeId).SingleOrDefault();
            IEnumerable<Core.Authorization.IPermission> currentPermissions = this.permissionManager.GetEffectivePermissionsForSite(store);

            var enrolementModel = new StoreEnrolmentViewModel() 
            { 
                StoreId = storeId,
                Site = store,
                Options = enrolementService.ListEnrolmentLevels(),
                SelectedOptionId = enrolementService.GetEnrolmentLevels(store).Select(e => e.Id).ToArray(),
                CurrentPermissions = currentPermissions
            };

            return View(enrolementModel);
        }

        [HttpPost]
        [ActionName("EditEnrolement")]
        public ActionResult EditEnrolementPost(StoreEnrolmentViewModel viewModel) 
        {
            if (!authorizer.Authorize(SiteEnrollmentUserPermissions.EditSiteEnrollment))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));
                return new HttpUnauthorizedResult();
            }

            Data.Domain.Site store = this.siteDataService.List(e => e.Id == viewModel.StoreId).SingleOrDefault();

            viewModel.Site = store;
            viewModel.Options = enrolementService.ListEnrolmentLevels();  

            if (!this.ModelState.IsValid) 
            {
                IEnumerable<Core.Authorization.IPermission> currentPermissions = this.permissionManager.GetEffectivePermissionsForSite(store);
                
                viewModel.CurrentPermissions = currentPermissions;
                return this.View(viewModel);
            }

            enrolementService.RemoveStoreEnrollments(store);

            if (viewModel.SelectedOptionId.Length > 0 )
            {
                foreach (var selected in viewModel.SelectedOptionId) { 
                    Core.Site.IEnrolmentLevel level = viewModel.Options.FirstOrDefault(e => e.Id == selected);
                    enrolementService.AddStoreEnrolment(store, level);
                }
            }

            this.notifier.Notify(message: "Saved");

            return RedirectToAction(actionName: "EditEnrolement", routeValues: new { storeId = store.Id });
            //return RedirectToAction("Index", "Site", new { externalSiteId = viewModel.Site.ExternalSiteId, ChainId = viewModel.Site.ChainId, Area = "Store" });
        }

        [HttpGet]
        public ActionResult Delete(string name) 
        {
            if (!authorizer.Authorize(SiteEnrollmentUserPermissions.EditSiteEnrollment))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));
                return new HttpUnauthorizedResult();
            }

            Core.Site.IEnrolmentLevel enrollment = this.enrolementService.GetEnrolmentLevel(name);

            if (enrollment != null) { this.enrolementService.Delete(enrollment); }

            this.notifier.Notify(translator.T(text: "The enrollment has been deleted."));

            return RedirectToAction(actionName: "Levels");
        }
    }
}