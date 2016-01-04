using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.Helpers;

namespace AndroAdmin.Controllers
{
    [Authorize]
    public class MyAndromedaController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
