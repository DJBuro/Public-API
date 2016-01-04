using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MyAndromeda.Data.DataWarehouse.Services.Customers;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Framework.Dates;
using MyAndromeda.Web.Areas.Reporting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace MyAndromeda.Web.Controllers.Api
{
    public class CustomerController : ApiController
    {
        private readonly ICustomerOrdersDataService orderHeaderDataService;
        private readonly ICustomerDataService customerDataService;
        private readonly IDateServices dateService;

        public CustomerController(ICustomerOrdersDataService orderHeaderDataService, ICustomerDataService customerDataService, IDateServices dateService)
        {
            this.dateService = dateService;
            this.customerDataService = customerDataService;
            this.orderHeaderDataService = orderHeaderDataService;
        }


        [Route("api/marketing/customers", Name="AllCustomerData")]
        public DataSourceResult List([ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request) 
        {


            var data = this.customerDataService.List().Select(e => new {
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
            });

            var result = data.ToDataSourceResult(request, e => new CustomerValueViewModel
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

            return result;
        }

    }
}