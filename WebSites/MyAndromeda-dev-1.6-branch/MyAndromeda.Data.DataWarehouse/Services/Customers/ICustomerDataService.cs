using System;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using System.Linq;
using MyAndromeda.Data.DataWarehouse.Models;
using System.Data.Entity;

namespace MyAndromeda.Data.DataWarehouse.Services.Customers
{
    public interface ICustomerDataService : IDataProvider<Customer>, IDependency
    {
        IDbSet<Customer> Customers { get; }

        Contact GetContact(Expression<Func<Contact, bool>> query);

        IQueryable<Contact> ListContacts(Expression<Func<Contact, bool>> query);
    }
}