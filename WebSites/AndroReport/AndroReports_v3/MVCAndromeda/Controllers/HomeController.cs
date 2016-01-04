using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCAndromeda.Models;

namespace MVCAndromeda.Controllers
{
    public class HomeController : Controller
    {
        //[OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Andromeda Store Performance Reporting!";
           if (!Request.IsAuthenticated)
            return RedirectToAction("Login", "Account");
           
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Andromeda cloud reporting";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
