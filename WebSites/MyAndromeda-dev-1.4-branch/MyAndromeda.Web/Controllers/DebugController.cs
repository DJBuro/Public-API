using MyAndromeda.Authorization.Management;
using MyAndromeda.Authorization.Services;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyAndromeda.SendGridService.MarketingEmails;
using MyAndromeda.Web.Controllers.Api.GprsGateway;

namespace MyAndromeda.Web.Controllers
{
    public class DebugController : Controller
    {
        private readonly IEmailCampaign emailCampaign;

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
            IEmailCampaign emailCampaign)
        {
            this.emailCampaign = emailCampaign;
            this.authorizer = authorizer;
            this.currentSite = currentSite;
            this.currentChain = currentChain;
            this.currentUser = currentUser;
            this.currentPermissions = currentPermissions;
            this.permissionManager = permissionManager;
        }

        public ActionResult Customers() 
        {

            if (!authorizer.Authorize(DebugPermissions.ExportCustomers)) 
            {
                return this.HttpNotFound();
            }

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
        public ActionResult TestCampaingEmail() 
        {
            return View();
        }

        [HttpPost, ActionName("TestCampaingEmail")]
        public async Task<ActionResult> TestCampaingEmailPost()
        {
            await emailCampaign.Send();

            return RedirectToAction("TestCampaingEmail");
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