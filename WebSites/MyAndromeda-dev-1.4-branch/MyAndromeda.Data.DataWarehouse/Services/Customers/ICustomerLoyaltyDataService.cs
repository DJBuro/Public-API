using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Data.DataWarehouse.Services.Customers
{
    public interface ICustomerLoyaltyDataService : IDataProvider<CustomerLoyalty>, IDependency 
    {
    }

    public class CustomerLoyaltyDataService : ICustomerLoyaltyDataService
    {
        private readonly DataWarehouseEntities dataContext;

        public CustomerLoyaltyDataService(DataWarehouseEntities dataContext) 
        {
            this.dataContext = dataContext;
            this.Table = this.dataContext.Set<CustomerLoyalty>();
            this.TableQuery = this.Table.Include(e=> e.Customer);
        }

        public DbSet<CustomerLoyalty> Table { get; set; }

        public IQueryable<CustomerLoyalty> TableQuery { get; set; }

        public void ChangeIncludeScope<TPropertyModel>(Expression<Func<CustomerLoyalty, TPropertyModel>> predicate)
        {
            this.TableQuery = TableQuery.Include(predicate);
        }

        public CustomerLoyalty New()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public CustomerLoyalty Get(Expression<Func<CustomerLoyalty, bool>> predicate)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public IQueryable<CustomerLoyalty> List()
        {
            return this.TableQuery;
        }

        public IQueryable<CustomerLoyalty> List(Expression<Func<CustomerLoyalty, bool>> predicate)
        {
            var query = this.TableQuery.Where(predicate);

            return query;
        }
    }
}