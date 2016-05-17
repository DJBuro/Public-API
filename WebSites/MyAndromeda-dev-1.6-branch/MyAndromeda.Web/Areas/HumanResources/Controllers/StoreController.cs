using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Web.Areas.HumanResources.Controllers
{
    public class StoreController : Controller
    {
        private readonly IWorkContext workContext;
        private readonly IAuthorizer authorizer;

        public StoreController(IWorkContext context, IAuthorizer authorizer)
        { 
            this.authorizer = authorizer;
            this.workContext = context;
        }

        public ActionResult Index()
        {
            if (!this.authorizer.Authorize(UserPermissions.ViewHrSection)) 
            {
                throw new UnauthorizedAccessException();
            }

            return View();
        }
    }
}