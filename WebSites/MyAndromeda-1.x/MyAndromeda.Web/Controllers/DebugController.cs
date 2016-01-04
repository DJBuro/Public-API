using MyAndromeda.Authorization.Services;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAndromeda.Web.Controllers
{
    public class DebugController : Controller
    {
        private readonly IAuthorizer authorizer;
        private readonly ICurrentSite currentSite;
        private readonly ICurrentChain currentChain;
        private readonly ICurrentUser currentUser;

        private readonly ICurrentPermissionService currentPermissions;

        private const string UnavailableMessage = "Content is unavailable"; 

        public DebugController(
            ICurrentChain currentChain,
            ICurrentSite currentSite,
            ICurrentUser currentUser,
            ICurrentPermissionService currentPermissions,
            IAuthorizer authorizer)
        {
            this.authorizer = authorizer;
            this.currentSite = currentSite;
            this.currentChain = currentChain;
            this.currentUser = currentUser;
            this.currentPermissions = currentPermissions;
        }

        public ActionResult Announce() 
        {
            return View();
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
    }
}