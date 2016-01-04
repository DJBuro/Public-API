using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Services.Email;
using MyAndromeda.Web.Areas.Marketing.Services;
using MyAndromeda.Web.Areas.Marketing.ViewModels;
using MyAndromedaDataAccess.Domain.Marketing;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAndromeda.Framework.Logging;
using MyAndromeda.Logging;

namespace MyAndromeda.Web.Areas.Marketing.Controllers
{
    [MyAndromedaAuthorize, ValidateInput(false)]
    public class EmailController : Controller
    {
        private readonly IMyAndromedaLogger logger;
        private readonly INotifier notifier;
        
        private readonly IMarketEmailService marketUserService;
        private readonly IMarketEmailTaskService marketEmailTaskService;
        private readonly IMailSendingService mailSendingService;
        private readonly IEmailSettingsService emailSettingsService;

        private readonly IAuthorizer authorizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailController" /> class.
        /// </summary>
        /// <param name="workContextWrapper">The work context wrapper.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="mailSendingService">The mail sending service.</param>
        /// <param name="marketUserService">The market user service.</param>
        /// <param name="marketEmailTaskService">The market email task service.</param>
        /// <param name="emailSettingsService">The email settings service.</param>
        /// <param name="notifier">The notifier.</param>
        public EmailController(
            WorkContextWrapper workContextWrapper,
            IMyAndromedaLogger logger,
            IAuthorizer authorizer,
            IMailSendingService mailSendingService,
            IMarketEmailService marketUserService,
            IMarketEmailTaskService marketEmailTaskService,
            IEmailSettingsService emailSettingsService,
            INotifier notifier) 
        {
            this.authorizer = authorizer;
            this.emailSettingsService = emailSettingsService;
            this.logger = logger;
            this.WorkContextWrapper = workContextWrapper;
            this.mailSendingService = mailSendingService;
            this.marketEmailTaskService = marketEmailTaskService;
            this.notifier = notifier;
            this.marketUserService = marketUserService;
        }

        /// <summary>
        /// Gets or sets the work context wrapper.
        /// </summary>
        /// <value>The work context wrapper.</value>
        public WorkContextWrapper WorkContextWrapper { get; private set; }

        public ActionResult Index() 
        {
            if (!this.authorizer.Authorize(MarketingFeatureEnrollment.MaketingFeature))
                return new HttpUnauthorizedResult();

            var data = marketUserService.GetMailMarketingCampaignsForSite();
            var viewModel = new ViewModels.ListEmailCampaignsViewModel() { 
                CurrentSite = this.WorkContextWrapper.Current.CurrentSite,
                Campaigns = data
            };

            return View(viewModel);    
        }

        public ActionResult Create() 
        {
            if (!this.authorizer.Authorize(UserPermissions.CreateCampaignEmails))
                return new HttpUnauthorizedResult();

            EditEmailCampaignViewModel model = new EditEmailCampaignViewModel();
            PrepairViewModel(model);

            return View(model);
        }

        [ActionName("Create"), HttpPost, ValidateAntiForgeryToken]
        public ActionResult CreatePost(EditEmailCampaignViewModel model) 
        {
            if (!this.authorizer.Authorize(UserPermissions.CreateCampaignEmails))
                return new HttpUnauthorizedResult();

            PrepairViewModel(model);
            if (!this.ModelState.IsValid) { return View(model); }

            var dataModel = new EmailCampaign();

            dataModel.UpdateFromViewModel(model);
            dataModel.ChainId = this.WorkContextWrapper.Current.CurrentChain.Chain.Id;
            dataModel.Created = DateTime.UtcNow;
            dataModel.Modified = DateTime.UtcNow;
            dataModel.SiteIds = new[] { 
                new EmailCampaignSitePart { 
                    //EmailCampaign = dataModel, 
                    SiteId = this.WorkContextWrapper.Current.CurrentSite.Site.Id 
                } 
            };

            this.marketUserService.Save(dataModel);
            this.notifier.Notify("Email campaign created: " + dataModel.Reference);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id) 
        {
            if (!this.authorizer.Authorize(UserPermissions.EditCampaignEmails))
                return new HttpUnauthorizedResult();

            var viewModel = new EditEmailCampaignViewModel();
            this.PrepairViewModel(viewModel);

            var dataModel = this.marketUserService.GetEmailCampaign(id);

            viewModel.UpdateFromDataModel(dataModel);
            
            return View(viewModel);
        }

        [ActionName("Edit"), HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditPost(EditEmailCampaignViewModel model)
        {
            if (!this.authorizer.Authorize(UserPermissions.EditCampaignEmails))
                return new HttpUnauthorizedResult();

            this.PrepairViewModel(model);
            if (!this.ModelState.IsValid) { return View(model); }

            var dataModel = this.marketUserService.GetEmailCampaign(model.Id);
            dataModel.UpdateFromViewModel(model);
            dataModel.Modified = DateTime.UtcNow;

            this.marketUserService.Save(dataModel);
            this.notifier.Notify("Email campaign updated: " + model.Reference);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Destroy(int id) 
        {
            if (!this.authorizer.Authorize(UserPermissions.EditCampaignEmails))
                return new HttpUnauthorizedResult();

            var dataModel = this.marketUserService.GetEmailCampaign(id);
            this.marketUserService.Destroy(dataModel);

            this.notifier.Notify("Email campaign removed: " + dataModel.Reference);

            return this.RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult Preview(int id) 
        {
            if (!this.authorizer.Authorize(UserPermissions.SendCampaignEmails))
                return new HttpUnauthorizedResult();

            var dataModel = this.marketUserService.GetEmailCampaign(id);
            var viewModel = new PreviewEmailCampaignViewModel() { 
                Id = id,
                Template = new EditEmailCampaignViewModel()
            };

            var innerViewModel = viewModel.Template;
            innerViewModel.UpdateFromDataModel(dataModel);

            return View(viewModel);
        }

        [ActionName("Preview"), HttpPost, ValidateAntiForgeryToken, AsyncTimeout(150)]
        public ActionResult PreviewPost(int id, string to)
        {
            if (!this.authorizer.Authorize(UserPermissions.SendCampaignEmails))
                return new HttpUnauthorizedResult();

            var model = this.marketUserService.GetEmailCampaign(id);

            try
            {
                var settings = this.emailSettingsService.GetBestSettings(
                    this.WorkContextWrapper.Available 
                        ? this.WorkContextWrapper.Current.CurrentChain.Chain.Id 
                        : 0
                );
             
                this.mailSendingService.SendPreviewEmail(model, settings, to);
                this.notifier.Notify("Email preview sent for: " + model.Reference);
            }
            catch (Exception e) 
            {
                this.logger.Error("Failed sending preview message", e);

                // I don't see the point in suffocating the user if not necessary. The error may be relevant to the user though
                if (!this.notifier.HasErrors()) { 
                    this.notifier.Error(string.Format("There was an error sending you email: {0}", e.Message));
                }

                if (e.InnerException != null) 
                {
                    this.notifier.Error(e.InnerException.Message);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Launch(int id) 
        {
            if (!this.authorizer.Authorize(UserPermissions.SendCampaignEmails))
                return new HttpUnauthorizedResult();

            var dataModel = this.marketUserService.GetEmailCampaign(id);
            var viewModel = new PreviewEmailCampaignViewModel()
            {
                Id = id,
                Template = new EditEmailCampaignViewModel()
            };
            var innerViewModel = viewModel.Template;
            innerViewModel.UpdateFromDataModel(dataModel);

            return View(viewModel);
        }

        [ActionName("Launch"), HttpPost, ValidateAntiForgeryToken]
        public ActionResult LaunchPost(int id, DateTime? sendOn)
        {
            if (!this.authorizer.Authorize(UserPermissions.SendCampaignEmails))
                return new HttpUnauthorizedResult();

            var model = this.marketUserService.GetEmailCampaign(id);

            this.marketEmailTaskService.CreateEmailCampaignTask(id, sendOn.GetValueOrDefault().ToUniversalTime());

            if (sendOn.HasValue)
            {
                this.notifier.Notify(string.Format("The campaign task has been created and will begin processing: {0} on: {1} {2}", model.Reference, sendOn.Value.ToLongDateString(), sendOn.Value.ToLongTimeString()));
            }
            else
            {
                this.notifier.Notify(string.Format("The campaign task has been created and will begin processing: {0}", model.Reference));
            }

            return RedirectToAction("Index");
        }

        private const string TokenFormat = "<span class='token'>{0}</span>";
        private void PrepairViewModel(EditEmailCampaignViewModel model) 
        {
            model.ChainId = this.WorkContextWrapper.Current.CurrentChain.Chain.Id;
            model.EmailTemplate = HttpUtility.HtmlDecode(model.EmailTemplate);
            model.Tokens = this.marketUserService.GetReplaceableFields();
        }
        
        /// <summary>
        /// Ajax request for looking up email campaigns.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public ActionResult EmailCampaign_Read([DataSourceRequest]DataSourceRequest request)
        {
            if (!this.authorizer.Authorize(UserPermissions.ViewEmailCampaigns))
                return new HttpUnauthorizedResult();

            return Json(this.marketUserService.GetMailMarketingCampaignsForSite().ToDataSourceResult(request));
        }

    }
}