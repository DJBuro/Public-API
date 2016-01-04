using Kendo.Mvc.Extensions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MyAndromeda.Data.DataWarehouse.Services.Customers;
using MyAndromeda.Framework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAndromeda.Web.Areas.Reporting.ViewModels;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Framework.Messaging;
using MyAndromeda.Framework;
using MyAndromeda.Export;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerDataService customerDataService;
        private readonly IWorkContext workContext;
        private readonly IAuthorizer authorizer;
        private readonly INotifier notifier;
        private readonly ITranslator translator;
        private readonly ISpreadsheetDocumentExport spreadsheetExport;

        public CustomerController(ICustomerDataService customerDataService, 
            IWorkContext workContext, 
            IAuthorizer authorizer, 
            INotifier notifier,
            ITranslator translator,
            ISpreadsheetDocumentExport spreadsheetExport
            )
        {
            this.customerDataService = customerDataService;
            this.workContext = workContext;
            this.authorizer = authorizer;
            this.notifier = notifier;
            this.translator = translator;
            this.spreadsheetExport = spreadsheetExport;
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult List([DataSourceRequest]DataSourceRequest request) 
        {
            var applications = this.workContext.CurrentSite.AcsApplicationIds.ToArray();
            var data = this.customerDataService.List().Where(e => applications.Contains(e.ACSAplicationId));

            return Json(data.ToDataSourceResult(request, e=> e.ToViewModel()));
        }

        public ActionResult ListMarketingOptions([DataSourceRequest]DataSourceRequest request, Guid id) 
        {
            var maketing = this.customerDataService.ListContacts(e => e.CustomerId == id);

            return Json(maketing.ToDataSourceResult(request, e=> e.ToViewModel()));
        }

        public ActionResult ListValue([DataSourceRequest]DataSourceRequest request) 
        {
            var applications = this.workContext.CurrentSite.AcsApplicationIds.ToArray();
            var data = this.customerDataService
                .List()
                .Where(e => applications.Contains(e.ACSAplicationId))
                .Select(e=> new {
                    Id = e.ID,
                    e.FirstName,
                    e.LastName,
                    e.RegisteredDateTime,
                    //Address = e.Address.ToViewModel(),
                    e.ACSAplicationId,
                    e.Title,
                    TotalOrders = e.OrderHeaders.Count(),
                    TotalValue = e.OrderHeaders.Sum(order => (decimal?) order.FinalPrice) ?? 0,
                    AvgOrderValue = e.OrderHeaders.Average(order => (decimal?) order.FinalPrice) ?? 0
                }).ToArray();

            return Json(data.ToDataSourceResult(request));
        }

        //public ActionResult ListCustomerAddresses([DataSourceRequest]DataSourceRequest request, Guid id) 
        //{
        //    var customer = this.customerDataService.Get(e => e.ID == id);
        //    var customerAddresses = customer.CustomerAddresses;
        //}

        public ActionResult CustomerDetail (Guid id)
        {
            var customer = this.customerDataService.Get(e => e.ID == id);

            if (customer == null) 
            {
                throw new ArgumentNullException("customer");
            }

            return View(customer);
        }

        public ActionResult ReadExcel(string title, string model, string data)
        {
            if (!authorizer.Authorize(Permissions.ViewReports))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));

                return new HttpUnauthorizedResult();
            }

            var streamResult = this.spreadsheetExport.Export(model, data, title);

            return new FileContentResult(streamResult, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = string.Format("{0}.xlsx", title) };
        }
    }
}
