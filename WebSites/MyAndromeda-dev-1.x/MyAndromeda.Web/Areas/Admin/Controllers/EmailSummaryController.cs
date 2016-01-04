using System.Threading.Tasks;
using MyAndromeda.SendGridService.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace MyAndromeda.Web.Areas.Admin.Controllers
{
    public class EmailSummaryController : Controller
    {
        private readonly IEmailSummaryService emailSummaryService;

        public EmailSummaryController(IEmailSummaryService emailSummaryService)
        { 
            this.emailSummaryService = emailSummaryService;
        }

        [OutputCache(Duration = 300)]
        public ActionResult Index() 
        {
            return View();
        }

        [OutputCache(Duration=300)]
        public async Task<JsonResult> Data([DataSourceRequest]DataSourceRequest request) 
        {
            var data = await emailSummaryService.GetSummaryResultsAync();

            return Json(data.ToDataSourceResult(request));
        }
    }
}