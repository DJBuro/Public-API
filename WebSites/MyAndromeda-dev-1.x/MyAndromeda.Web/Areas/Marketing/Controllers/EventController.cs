using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Web.Areas.Marketing.Controllers
{
    public class EventController : Controller
    {
        private readonly IAuthorizer authorizer;

        public EventController(IAuthorizer authorizer)
        {
            this.authorizer = authorizer;
        }

        // GET: Marketing/Event
        public ActionResult Index()
        {
            if (!this.authorizer.AuthorizeAll(MarketingFeatureEnrollment.MaketingFeature,
                UserPermissions.ChangeEventMarketing)) 
            { 
                return new HttpUnauthorizedResult();
            }

            return View();
        }
    }

}