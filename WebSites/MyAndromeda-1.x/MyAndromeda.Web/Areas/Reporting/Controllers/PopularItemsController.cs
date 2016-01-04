using Kendo.Mvc.UI;
using MyAndromeda.Web.Areas.Reporting.Services;
using MyAndromedaDataAccess.Domain.Reporting.Query;
using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class PopularItemsController : Controller
    {
        private readonly IOrderService orderService;

        public PopularItemsController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public JsonResult ReadPopularItems([DataSourceRequest]DataSourceRequest request, DateTime filterFrom, DateTime filterTo)
        {
            var filter = new FilterQuery()
            {
                FilterFrom = filterFrom,
                FilterTo = filterTo
            };

            var data = orderService.GetOrderPopularity(filter);
            
            var result = data.ToDataSourceResult(request);

            return Json(result);
        }
    }
}