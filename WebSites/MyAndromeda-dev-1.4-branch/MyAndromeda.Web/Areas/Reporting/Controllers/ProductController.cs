using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Web.Areas.Reporting.Context;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class ProductController : Controller
    {
        private readonly IReportingContext reportingContext;
        private readonly ICurrentUser currentUser;

        public ProductController(IReportingContext reportingContext, ICurrentUser currentUser)
        {
            this.currentUser = currentUser;
            this.reportingContext = reportingContext;
        }

        public ActionResult Index()
        {
            return View(this.reportingContext);
        }
    }
}