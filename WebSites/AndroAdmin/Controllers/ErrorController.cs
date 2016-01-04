using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.Helpers;

namespace AndroAdmin.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Http403()
        {
            return View();
        }

        public ActionResult Http404()
        {
            return View();
        }

        public ActionResult NotAuthorized()
        {
            return View();
        }
    }
}