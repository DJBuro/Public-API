using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core.Data;
using MyAndromeda.Core;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Data.DataWarehouse.Services.Customers;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Services.Customers
{
    public interface IApplicationCustomers : IDependency
    {
        IQueryable<Customer> List();
    }

    public class SiteCustomers : IApplicationCustomers
    {
        private readonly ICurrentSite currentSite;

        private readonly ICustomerDataService customerDataService;

        public SiteCustomers(ICurrentSite currentSite, ICustomerDataService customerDataService) 
        {
            this.currentSite = currentSite;
            this.customerDataService = customerDataService;
        }

        public IQueryable<Customer> List()
        {
            var acsApplications = currentSite.AcsApplicationIds.ToArray();
            var siteCustomers = customerDataService.List()
                .Where(e=> acsApplications.Contains(e.ACSAplicationId));

            return siteCustomers;
        }
    }
}
