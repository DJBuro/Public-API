using System;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using System.Linq;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Data.DataWarehouse.Services.Customers
{
    public interface ICustomerDataService : IDataProvider<Customer>, IDependency
    {
        Contact GetContact(Expression<Func<Contact, bool>> query);

        IQueryable<Contact> ListContacts(Expression<Func<Contact, bool>> query);
    }
}