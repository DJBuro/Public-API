using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public class CustomerOrderDataService : 
        ICustomerOrdersDataService, 
        IOrderHeaderDataService
    {
        private readonly DataWarehouseEntities dataContext;

        public CustomerOrderDataService(DataWarehouseEntities dataContext) 
        {
            this.dataContext = dataContext;
            this.Table = this.dataContext.Set<OrderHeader>();
            this.TableQuery = this.Table
                .Include(e=> e.OrderStatu);
        }

        public void ChangeIncludeScope<PropertyModel>(Expression<Func<OrderHeader, PropertyModel>> predicate)
        {
            this.TableQuery = TableQuery.Include(predicate);
        }

        public DbSet<OrderHeader> Table
        {
            get;
            private set;
        }

        public IQueryable<OrderHeader> TableQuery { get; private set; }

        public OrderHeader GetByOrderId(Guid acsOrderId)
        {
            return TableQuery.SingleOrDefault(e => e.ACSOrderId == acsOrderId);
        }

        public OrderHeader Get(Guid acsOrderId)
        {
            return TableQuery.SingleOrDefault(e => e.ACSOrderId == acsOrderId);
        }

        public void Update(OrderHeader orderHeader)
        {
            dataContext.SaveChanges();
        }
        

        public void Update(IEnumerable<OrderHeader> orderHeaders)
        {
            dataContext.SaveChanges();
        }

        public OrderHeader New()
        {
            return new OrderHeader();
        }

        public OrderHeader Get(Expression<Func<OrderHeader, bool>> query)
        {
            var tableQuery = TableQuery.Where(query);

            return tableQuery.SingleOrDefault();
        }

        public IQueryable<OrderHeader> List()
        {
            return this.TableQuery;
        }

        public IQueryable<OrderHeader> List(Expression<Func<OrderHeader, bool>> query)
        {
            return this.TableQuery.Where(query);
        }
    }
}