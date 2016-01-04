using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Data.AcsServices.Context;

namespace MyAndromeda.Web.Areas.Menu.Controllers
{
    public class MediaServiceAdminController : Controller
    {
        private readonly IActiveMenuContext menuContext;

        public MediaServiceAdminController(IActiveMenuContext menuContext) 
        { 
            this.menuContext = menuContext;
        }

        public ActionResult Index() 
        {
            var menuServer = this.menuContext.MediaServer;

            return View();
        }

        public ActionResult Edit(int mediaServerId) 
        {
            return View();
        }

        [ActionName("Edit")]
        public ActionResult EditPost() 
        {
            return View();  
        }
    }
}