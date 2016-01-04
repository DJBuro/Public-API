using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Web.Areas.Reporting.Context;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class StoreController : Controller
    {
        private readonly IReportingContext reportingContext;
        private readonly ICurrentUser currentUser;

        public StoreController(IReportingContext reportingContext, ICurrentUser currentUser) 
        {
            this.currentUser = currentUser;
            this.reportingContext = reportingContext;
        }

        public ActionResult Index() 
        {
            return View(this.reportingContext);
        }

        public ActionResult TopStores() 
        {
            return View(this.reportingContext);
        }

    }
}