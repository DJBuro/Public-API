using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Web.Areas.Menu.Controllers
{
    public class MenuOverviewController : Controller
    {
        private readonly ICurrentChain currentChain;

        public MenuOverviewController(ICurrentChain currentChain)
        { 
            this.currentChain = currentChain;
        }

        //
        // GET: /Menu/MenuOverview/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReadStores() 
        {
            return View();
        }

    }
}
