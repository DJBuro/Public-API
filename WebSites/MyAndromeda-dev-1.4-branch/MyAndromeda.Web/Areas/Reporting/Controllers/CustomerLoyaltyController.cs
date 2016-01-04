using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAndromeda.Data.DataWarehouse.Services.Customers;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class CustomerLoyaltyController : Controller
    {
        private readonly ICustomerLoyaltyDataService customerLoylatyDataService;
        private readonly ICurrentSite currentSite;

        public CustomerLoyaltyController(ICustomerLoyaltyDataService customerLoylatyDataService, ICurrentSite currentSite) 
        {
            this.currentSite = currentSite;
            this.customerLoylatyDataService = customerLoylatyDataService;
        }

        // GET: Reporting/CustomerLoyalty
        public ActionResult Index()
        {
            var data = customerLoylatyDataService.List(e => this.currentSite.AcsApplicationIds.Contains(e.Customer.ACSAplicationId))
                .Select(e => new { 
                    e.Customer.FirstName,
                    e.Customer.LastName,
                    e.Points,
                    PendingPoints = e.Customer.OrderHeaders.SelectMany(order => order.OrderLoyalties)
                        .Where(loyalty => !loyalty.Applied)
                        .Sum(loyalty=> loyalty.awardedPoints),
                    TotalAwardedPoints = e.Customer.OrderHeaders.SelectMany(order => order.OrderLoyalties)
                        .Where(loyalty => loyalty.Applied)
                        .Sum(loyalty => loyalty.awardedPoints)
                }).ToArray();

            var result = data.Select(e => new ViewModels.CustomerLoyaltyViewModel() { 
                FirstName = e.FirstName,
                LastName = e.LastName,
                Points = e.Points,
                PendingPoints = e.PendingPoints.GetValueOrDefault(),
                EarnedPoints = e.TotalAwardedPoints.GetValueOrDefault()
            });

            return View(result);
        }
    }
}