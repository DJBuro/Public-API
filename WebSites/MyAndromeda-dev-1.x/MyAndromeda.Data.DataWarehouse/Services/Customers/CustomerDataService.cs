using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Framework.Attributes;

namespace MyAndromeda.Data.DataWarehouse.Services.Customers
{
    public class CustomerDataService : ICustomerDataService 
    {
        private readonly DataWarehouseDbContext dataContext;

        public CustomerDataService(
            //[ReadOnlyData]
            DataWarehouseDbContext dataContext) 
        {
            this.dataContext = dataContext;
            this.Table = this.dataContext.Set<Customer>();
            
            this.TableQuery = this.Table
                .Include(e => e.Address)
                .Include(e => e.Address.Country)
                .Include(e => e.CustomerAccount)
                .Include(e => e.CustomerAccount.AccountType);
        }

        public DbSet<Customer> Table { get; private set; }
        public IQueryable<Customer> TableQuery { get; private set; }

        public void ChangeIncludeScope<PropertyModel>(Expression<Func<Customer, PropertyModel>> predicate)
        {
            this.TableQuery = TableQuery.Include(predicate);
        }

        public Customer New()
        {
            return new Customer();
        }

        public Customer Get(Expression<Func<Customer, bool>> query)
        {
            return this.TableQuery
                .Where(query)
                .SingleOrDefault();
        }

        public IQueryable<Customer> List()
        {
            return this.TableQuery;
        }

        public IQueryable<Customer> List(Expression<Func<Customer, bool>> query)
        {
            var tableQuery = TableQuery.Where(query);

            return tableQuery;
        }

        public IDbSet<Customer> Customers
        {
            get { return this.Table; }
        }

        public Contact GetContact(Expression<Func<Contact, bool>> query)
        {
            Contact result = null;

            var table = this.dataContext.Contacts;
            var tableQuery = table.Where(query);

            result = tableQuery.FirstOrDefault();

            return result;
        }

        public IQueryable<Contact> ListContacts(Expression<Func<Contact, bool>> query)
        {
            var table = this.dataContext.Contacts
                .Include(e=> e.MarketingLevel)
                .Include(e=> e.ContactType);

            var tableQuery = table.Where(query);

            return tableQuery;
        }
    }
}
