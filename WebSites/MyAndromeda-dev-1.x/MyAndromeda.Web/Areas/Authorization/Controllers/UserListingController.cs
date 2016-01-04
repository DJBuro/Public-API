using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Authorization.Controllers
{
    public class UserListingController : Controller
    {
        //
        // GET: /Authorization/UserListing/

        public ActionResult Index()
        {
            return View();
        }

    }
}
