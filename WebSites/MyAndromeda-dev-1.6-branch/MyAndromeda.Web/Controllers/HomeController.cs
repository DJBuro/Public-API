using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MyAndromeda.Web.Services;
using MyAndromeda.Framework.Mvc;

namespace MyAndromeda.Web.Controllers
{
    public class HomeController : Controller, IRedirectController
    {
        private readonly ILoginService loginService;

        public HomeController(ILoginService loginService)
        { 
            this.loginService = loginService;
        }

        public new RedirectToRouteResult RedirectToAction(string p, string p1, object p2)
        {
            return base.RedirectToAction(p, p1, p2);
        }

        public RedirectResult Redirect(string returnUrl)
        {
            return base.Redirect(returnUrl);
        }

        [Authorize]
        public ActionResult Index(string returnUrl)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.loginService.LoggedIn(this, this.User.Identity.Name, returnUrl);
            }

            FormsAuthentication.SignOut();
            return base.RedirectToAction("Index", "Home");

            //return RedirectToAction("Index", "Sites", new { Area = "" }); 
        }

        [AllowAnonymous]
        public ActionResult BrowserSupport() 
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            return View();
        }
    }
}
