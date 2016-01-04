using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAndromeda.Web.Controllers
{
    public class PublishStatusController : Controller
    {
        public PublishStatusController() { }

        //
        // GET: /PublishStatus/
        public ActionResult Index()
        {
            return View();
        }
	}
}