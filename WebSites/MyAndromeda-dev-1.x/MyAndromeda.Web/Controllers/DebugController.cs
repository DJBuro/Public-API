using MyAndromeda.Authorization.Management;
using MyAndromeda.Authorization.Services;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyAndromeda.SendGridService.MarketingEmails;
using MyAndromeda.SendGridService.Models;
using MyAndromeda.Web.Controllers.Api.GprsGateway;
using Postal;

namespace MyAndromeda.Web.Controllers
{
    public class DebugController : Controller
    {
        private readonly IPreviewEmailCampaign emailCampaign;

        private readonly IAuthorizer authorizer;
        private readonly ICurrentSite currentSite;
        private readonly ICurrentChain currentChain;
        private readonly ICurrentUser currentUser;

        private readonly ICurrentPermissionService currentPermissions;

        private readonly IPermissionManager permissionManager;

        private const string UnavailableMessage = "Content is unavailable"; 

        public DebugController(
            ICurrentChain currentChain,
            ICurrentSite currentSite,
            ICurrentUser currentUser,
            ICurrentPermissionService currentPermissions,
            IPermissionManager permissionManager,
            IAuthorizer authorizer,
            IPreviewEmailCampaign emailCampaign)
        {
            this.emailCampaign = emailCampaign;
            this.authorizer = authorizer;
            this.currentSite = currentSite;
            this.currentChain = currentChain;
            this.currentUser = currentUser;
            this.currentPermissions = currentPermissions;
            this.permissionManager = permissionManager;
        }

        public ActionResult Orders(int andromedaSiteId) 
        {
            return View();
        }

        public ActionResult Customers() 
        {

            if (!authorizer.Authorize(DebugPermissions.ExportCustomers)) 
            {
                return this.HttpNotFound();
            }

            return View();
            
        }

        public ActionResult WebHooks() 
        {
            return View();
        }


        public ActionResult Announce() 
        {
            return View();
        }

        public ActionResult TestLoyalty()
        {
            return View();
        }

        public ActionResult TestPermissions() 
        {
            var permissions = this.permissionManager.GetInternalPermissions();

            return View(permissions);
        }

        [HttpGet]
        public ActionResult TestCampaingEmail(int andromedaSiteId) 
        {
            return View();
        }

        [HttpPost, ActionName("TestCampaingEmail")]
        public async Task<ActionResult> TestCampaingEmailPost(int andromedaSiteId)
        {
            var message = new MarketingEmailMessage() { 
                Store = this.currentSite.Store,
                WebsiteStuff = this.currentSite.AndroWebOrderingSites.First()
            };

            var postalEmailService = new EmailService();
            var output = postalEmailService.CreateMailMessage(message);

            var to = new[] { "matthew.green@androtech.com" };
            await emailCampaign.SendAsync(output.Body, to);

            return RedirectToAction("TestCampaingEmail", new { AndromedaSiteId = andromedaSiteId });
        }


        [ChildActionOnly]
        public ActionResult Index()
        {
            return PartialView(this.authorizer);
        }

        [ChildActionOnly]
        public ActionResult Site() 
        {
            if (!this.currentSite.Available)
                return Content(UnavailableMessage);
            return PartialView(this.currentSite);
        }

        [ChildActionOnly]
        public ActionResult Chain() 
        {
            if (!this.currentChain.Available)
                return Content(UnavailableMessage);
            return PartialView(this.currentChain);
        }

        [ChildActionOnly]
        public ActionResult User() 
        {
            if (!this.currentUser.Available)
                return Content(UnavailableMessage);

            return PartialView(this.currentUser);
        }

        [ChildActionOnly]
        public ActionResult Permissions() 
        {
            return PartialView(currentPermissions);
        }

        [ChildActionOnly]
        public ActionResult SyncHubStatus() 
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult MenuDetail() 
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Logging() 
        {
            return PartialView();
        }

        public ActionResult PrinterStatus() 
        {
            return View();
        }


        [HttpGet]
        public ActionResult TestOrder()
        {
            //ListingController.TestOrder = "#13*1*10003*1;Chicken;3.00;*0*0;29.10;4;Tom;Address of the Customer;15:47 03-08-10;113;7;cod:;008612345678;*Comment#";

            return View();
        }

        [HttpPost]
        [ActionName("TestOrder")]
        public ActionResult TestOrderPost(string order)
        {
            ListingController.TestOrder = order;

            return RedirectToAction("TestOrder");
        }
    }
}