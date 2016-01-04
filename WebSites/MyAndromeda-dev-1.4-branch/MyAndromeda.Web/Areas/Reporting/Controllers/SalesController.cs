using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Web.Areas.Reporting.Services;
using MyAndromedaDataAccess.Domain.Reporting.Query;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class SalesController : Controller
    {
        private readonly WorkContextWrapper workContextWrapper;
        private readonly INotifier notifier;
        private readonly ISalesService salesService;

        public SalesController(INotifier notifier, WorkContextWrapper workContextWrapper, ISalesService salesService)
        { 
            this.salesService = salesService;
            this.workContextWrapper = workContextWrapper;
            this.notifier = notifier;
        }

        [ChildActionOnly]
        public ActionResult IndexChild(FilterQuery filter) 
        {
            var salesSummary = salesService.GetSalesSummary(filter);

            return PartialView(salesSummary);
        }

        public ActionResult Monthly(FilterQuery filter) 
        {
            var salesSummary = salesService.GetSalesSummary(filter);

            return PartialView(salesSummary);
        }

        //private readonly int[] tempValues = new[] { 5000, 1900, 1030, 10004, 7333, 5000, 1900, 1030, 10004, 7333, 5000, 1900, 1030, 10004, 7333, 5000, 1900, 1030, 10004, 7333, 1030, 10004, 7333, 5000, 1030, 10004, 7333, 5000 };
        
        //[HttpPost]
        //public ActionResult Read_Total([DataSourceRequest] DataSourceRequest request) 
        //{
        //    var data = tempValues;
            
        //    return Json(data.ToDataSourceResult(request));
        //}

        //[HttpPost]
        //public ActionResult Read_Avg([DataSourceRequest] DataSourceRequest request) 
        //{
        //    var data = tempValues;

        //    return Json(data.ToDataSourceResult(request));
        //}

        //[HttpPost]
        //public ActionResult Read_Last_Week() 
        //{
        //    return View();
        //}

        
    }
}