using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromeda.Core;
using MyAndromeda.Framework;
using MyAndromeda.Framework.Contexts;
using MyAndromedaDataAccess;

namespace MyAndromeda.Web.Services.Customer
{
    public interface ICustomerService : IDependency 
    {
        /// <summary>
        /// Gets the chains customers.
        /// </summary>
        /// <returns></returns>
        IEnumerable<MyAndromedaDataAccess.Domain.Marketing.Customer> GetChainsCustomers();

        /// <summary>
        /// Gets the sites customers.
        /// </summary>
        /// <returns></returns>
        IEnumerable<MyAndromedaDataAccess.Domain.Marketing.Customer> GetSitesCustomers();
    }

    public class CustomerService : ICustomerService 
    {
        private readonly IDataAccessFactory dataAccessFactory;

        private readonly WorkContextWrapper workContextWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerService" /> class.
        /// </summary>
        /// <param name="workContext">The work context.</param>
        /// <param name="dataAccessFactory">The data access factory.</param>
        public CustomerService(WorkContextWrapper workContextWrapper, IDataAccessFactory dataAccessFactory)
        { 
            this.workContextWrapper = workContextWrapper;
            this.dataAccessFactory = dataAccessFactory;
        }

        /// <summary>
        /// Gets the chains customers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MyAndromedaDataAccess.Domain.Marketing.Customer> GetChainsCustomers() 
        {
            IEnumerable<MyAndromedaDataAccess.Domain.Marketing.Customer> customers = 
                dataAccessFactory.CustomerDataAccess.ListByChain(this.workContextWrapper.Current.CurrentChain.Chain.Id);

            return customers;
        }

        /// <summary>
        /// Gets the sites customers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MyAndromedaDataAccess.Domain.Marketing.Customer> GetSitesCustomers() 
        {
            IEnumerable<MyAndromedaDataAccess.Domain.Marketing.Customer> customers =
                dataAccessFactory.CustomerDataAccess.ListBySite(this.workContextWrapper.Current.CurrentSite.Site.Id);

            return customers;
        }
    }
}