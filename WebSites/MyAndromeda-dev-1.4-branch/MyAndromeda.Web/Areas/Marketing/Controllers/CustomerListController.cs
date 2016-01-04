using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyAndromeda.Framework.Notification;
using Kendo.Mvc.UI;
using MyAndromeda.ImportExport.Customers;
using MyAndromeda.Web.Areas.Marketing.Services;
using Kendo.Mvc.Extensions;
using MyAndromedaDataAccess.Domain.Marketing;

namespace MyAndromeda.Web.Areas.Marketing.Controllers
{
    public class CustomerListController : Controller
    {
        private readonly INotifier notifier;
        private readonly ICustomerListService customerListService;
        private readonly IExportCustomerList exportCustomerList;

        public CustomerListController(INotifier notifier, ICustomerListService customerListService, IExportCustomerList exportCustomerList) 
        {
            this.exportCustomerList = exportCustomerList;
            this.customerListService = customerListService;
            this.notifier = notifier;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Csv([DataSourceRequest] DataSourceRequest request) 
        {
            var data = customerListService.GetAllCustomersBySiteId();
            var result = data.ToDataSourceResult(request).Data;

            var memoryStream = exportCustomerList.GetCsv(result as IEnumerable<Customer>);

            return File(memoryStream, "text/comma-separated-values", "Customers.csv");
        }

        [HttpPost]
        public JsonResult Read([DataSourceRequest] DataSourceRequest request) 
        {
            var data = customerListService.GetAllCustomersBySiteId();

            return Json(data.ToDataSourceResult(request, this.ModelState));
        }
    }
}
