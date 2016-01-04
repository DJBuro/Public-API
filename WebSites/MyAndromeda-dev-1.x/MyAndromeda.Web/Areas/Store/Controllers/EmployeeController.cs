using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Stores.Employees;

namespace MyAndromeda.Web.Areas.Store.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IStoreEmployeeService storeEmployeeService;

        private readonly ICurrentSite currentSite;

        public EmployeeController(IStoreEmployeeService storeEmployeeService, ICurrentSite currentSite)
        {
            this.storeEmployeeService = storeEmployeeService;
            this.currentSite = currentSite;
        }

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult PartialIndex() 
        {
            return PartialView("Index");
        }

        public JsonResult List() 
        {
            var data = this.storeEmployeeService.GetOrUpdateEmployees(currentSite.AndromediaSiteId);

            return this.Json(new { 
                Data = data,
                Total = data.Count()
            });
        }

    }
}
