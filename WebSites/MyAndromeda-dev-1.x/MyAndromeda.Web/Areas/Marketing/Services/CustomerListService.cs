using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromeda.Core;
using MyAndromeda.Framework;
using MyAndromeda.Framework.Contexts;
using MyAndromedaDataAccess;
using MyAndromedaDataAccess.Domain.Marketing;

namespace MyAndromeda.Web.Areas.Marketing.Services
{
    public interface ICustomerListService : IDependency 
    {
        IEnumerable<Customer> GetAllCustomersBySiteId();
    }

    public class CustomerListService : ICustomerListService
    {
        private readonly IDataAccessFactory dataAccessFactory;
        private readonly WorkContextWrapper workContextWrapper;

        public CustomerListService(IDataAccessFactory dataAccessFactory, WorkContextWrapper workContextWrapper)
        {
            this.workContextWrapper = workContextWrapper;
            this.dataAccessFactory = dataAccessFactory;
        }

        public IEnumerable<Customer> GetAllCustomersBySiteId()
        {
            var siteId = workContextWrapper.Current.CurrentSite.SiteId;
            var data = dataAccessFactory.CustomerDataAccess.ListBySite(siteId);

            return data;
        }
    }
}