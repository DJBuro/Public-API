using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Data.Domain.Marketing;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.SendGridService.EmailServices;

namespace MyAndromeda.Web.Areas.Marketing.Controllers
{
    [MyAndromedaAuthorize]
    public class EmailSettingsController : Controller
    {
        private readonly IAuthorizer authorizer;
        private readonly IEmailSettingsService emailSettingsService;
        private readonly INotifier notifier;
        private readonly WorkContextWrapper workContextWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSettingsController" /> class.
        /// </summary>
        /// <param name="workContextWrapper">The work context wrapper.</param>
        /// <param name="authorizer">The authorizer.</param>
        /// <param name="emailSettingsService">The email settings service.</param>
        /// <param name="notifier">The notifier.</param>
        public EmailSettingsController(
            WorkContextWrapper workContextWrapper,
            IAuthorizer authorizer,
            IEmailSettingsService emailSettingsService,
            INotifier notifier)
        {
            this.workContextWrapper = workContextWrapper;
            this.notifier = notifier;
            this.emailSettingsService = emailSettingsService;
            this.authorizer = authorizer;
        }

        public ActionResult Index()
        {
            if (!this.authorizer.Authorize(UserPermissions.ChangeEmailSettings))
                return new HttpUnauthorizedResult("You do not have permissions to change the settings");

            var viewModel = new ViewModels.EditEmailSettingsViewModel();

            var settings = this.emailSettingsService.GetBestSettings(this.workContextWrapper.Current.CurrentChain.Chain.Id);
            if (settings.Id > 0) { viewModel.Settings = settings; }

            return View(viewModel);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost() 
        {
            if (!this.authorizer.Authorize(UserPermissions.ChangeEmailSettings))
                return new HttpUnauthorizedResult("You do not have permissions to change the settings");

            var settings = this.emailSettingsService.GetBestSettings(this.workContextWrapper.Current.CurrentChain.Chain.Id);
            var viewModel = new ViewModels.EditEmailSettingsViewModel();
            viewModel.Settings = settings;

            this.TryUpdateModel(viewModel);

            if (!this.ModelState.IsValid) 
            {
                return View("Index", viewModel);
            }

            this.emailSettingsService.Update(settings);

            this.notifier.Notify("Email Settings have been updated");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create() 
        {
            if (!this.authorizer.Authorize(UserPermissions.ChangeEmailSettings))
                return new HttpUnauthorizedResult("You do not have permissions to change the settings");

            var settings = this.emailSettingsService.GetBestSettings(this.workContextWrapper.Current.CurrentChain.Chain.Id);
            var viewModel = new ViewModels.EditEmailSettingsViewModel(settings.Id > 0 ? settings : new EmailSettings());

            return View(viewModel);
        }

        [ActionName("Create"), HttpPost]
        public ActionResult CreatePost(ViewModels.EditEmailSettingsViewModel model) 
        {
            if (!this.authorizer.Authorize(UserPermissions.ChangeEmailSettings))
                return new HttpUnauthorizedResult("You do not have permissions to change the settings");

            if(!this.ModelState.IsValid)
                return View(model);

            model.Settings.ChainId = this.workContextWrapper.Current.CurrentChain.Chain.Id;
            this.emailSettingsService.Create(model.Settings);

            this.notifier.Notify("Email Settings have been updated");
            return RedirectToAction("Index");
        }

        public ActionResult Destroy(int id) 
        {
            if (!this.authorizer.Authorize(UserPermissions.ChangeEmailSettings))
                return new HttpUnauthorizedResult("You do not have permissions to change the settings");

            var model = this.emailSettingsService.Get(this.workContextWrapper.Current.CurrentSite.Site.Id);
            if (model != null) 
            { 
                this.emailSettingsService.Destroy(id);
            }

            this.notifier.Notify("The email settings have been removed");
            return RedirectToAction("Index");
        }

        //public ActionResult Edit() 
        //{
        //    if (!this.authorizer.Authorize(Permissions.ChangeEmailSettings))
        //        return new HttpUnauthorizedResult("You do not have permissions to change the settings");

        //    return View();
        //}

        //[ActionName("Edit")]
        //public ActionResult EditPost()
        //{
        //    if (!this.authorizer.Authorize(Permissions.ChangeEmailSettings))
        //        return new HttpUnauthorizedResult("You do not have permissions to change the settings");

        //    return View();
        //}

    }
}
