using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MyAndromeda.Data.DataWarehouse.Services.Customers;
using MyAndromeda.Framework.Contexts;
using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Web.Areas.Reporting.ViewModels;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Framework.Dates;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerDataService customerDataService;
        private readonly IWorkContext workContext;
        private readonly IAuthorizer authorizer;
        private readonly INotifier notifier;
        private readonly ITranslator translator;
        private readonly IDateServices dateService;
        //private readonly ISpreadsheetDocumentExport spreadsheetExport;

        public CustomerController(ICustomerDataService customerDataService,
            IWorkContext workContext,
            IAuthorizer authorizer,
            INotifier notifier,
            ITranslator translator,
            IDateServices dateService)
        {
            this.dateService = dateService;
            this.customerDataService = customerDataService;
            this.workContext = workContext;
            this.authorizer = authorizer;
            this.notifier = notifier;
            this.translator = translator;
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult List([DataSourceRequest]DataSourceRequest request)
        {
            var applications = this.workContext.CurrentSite.AcsApplicationIds.ToArray();
            var data = this.customerDataService.List().Where(e => applications.Contains(e.ACSAplicationId));

            return Json(data.ToDataSourceResult(request, e => e.ToViewModel()));
        }

        public ActionResult ListMarketingOptions([DataSourceRequest]DataSourceRequest request, Guid id)
        {
            var maketing = this.customerDataService.ListContacts(e => e.CustomerId == id);

            return Json(maketing.ToDataSourceResult(request, e => e.ToViewModel()));
        }

        public ActionResult ListValue([DataSourceRequest]DataSourceRequest request)
        {
            var applications = this.workContext.CurrentSite.AcsApplicationIds.ToArray();
            var data = this.customerDataService
                .List()
                .Where(e => applications.Contains(e.ACSAplicationId))
                .Select(e => new
                {
                    Id = e.ID,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    RegisteredDateTime = e.RegisteredDateTime,
                    Email = e.Contacts.FirstOrDefault(contact => contact.ContactTypeId == 0).Value,
                    Phone = e.Contacts.FirstOrDefault(contact => contact.ContactTypeId == 1).Value,
                    //Address = e.Address.ToViewModel(),
                    ACSAplicationId = e.ACSAplicationId,
                    Title = e.Title,
                    TotalOrders = e.OrderHeaders.Count(),
                    TotalValue = e.OrderHeaders.Sum(order => (decimal?)order.FinalPrice) ?? 0,
                    AvgOrderValue = e.OrderHeaders.Average(order => (decimal?)order.FinalPrice) ?? 0,
                    LastOrderTime = e.OrderHeaders.Max(order => (DateTime?)order.TimeStamp)
                }).ToArray();

            var updatedData = data.Select(e => new CustomerValueViewModel
            {
                AvgOrderValue = e.AvgOrderValue,
                FirstName = e.FirstName,
                Id = e.Id,
                LastName = e.LastName,
                LastOrderTime = this.dateService.ConvertToLocalFromUtc(e.LastOrderTime),
                RegisteredDateTime = this.dateService.ConvertToLocalFromUtc(e.RegisteredDateTime),
                Title = e.Title,
                TotalOrders = e.TotalOrders,
                TotalValue = e.TotalValue,
                Email = e.Email,
                Phone = e.Phone
            });


            return Json(updatedData.ToDataSourceResult(request));
        }

        public ActionResult CustomerDetail(Guid id)
        {
            var customer = this.customerDataService.Get(e => e.ID == id);

            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            return View(customer);
        }

    }
}
