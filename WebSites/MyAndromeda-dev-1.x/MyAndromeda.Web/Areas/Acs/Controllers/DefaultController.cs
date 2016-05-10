using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using MyAndromeda.Data.DataAccess;

namespace MyAndromeda.Web.Areas.Acs.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IAcsApplicationDataService acsApplications;

        public DefaultController(IAcsApplicationDataService acsApplications)
        {
            this.acsApplications = acsApplications;
        }

        // GET: Acs/Default
        public ActionResult Index()
        {
            var a = "";

            return View();
        }

        public JsonResult List([DataSourceRequest]DataSourceRequest request) 
        {
            var data = acsApplications.Query();

            return Json(data.ToDataSourceResult(request, e => new { 
                e.Id,
                e.Name,
                Partner = new {
                    Name = e.Partner.Name,
                    Id = e.Partner.Id
                }
            }));
        }
    }
}