using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public class CustomerOrderDataService : 
        ICustomerOrdersDataService, 
        IOrderHeaderDataService
    {
        private readonly DataWarehouseDbContext dbContext;

        public CustomerOrderDataService(DataWarehouseDbContext dbContext) 
        {
            this.dbContext = dbContext;
            this.Table = this.dbContext.Set<OrderHeader>();
            this.OrderStatusHistory = this.dbContext.OrderStatusHistories;
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

        public IDbSet<OrderHeader> OrderHeaders
        {
            get
            {
                return this.Table;
            }
        }

        public IDbSet<OrderStatusHistory> OrderStatusHistory { get; private set; }

        public OrderHeader GetByOrderId(Guid acsOrderId)
        {
            return TableQuery.SingleOrDefault(e => e.ACSOrderId == acsOrderId);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.dbContext.SaveChangesAsync();
        }

        public OrderHeader Get(Guid acsOrderId)
        {
            return TableQuery.SingleOrDefault(e => e.ACSOrderId == acsOrderId);
        }

        public void Update(OrderHeader orderHeader)
        {
            dbContext.SaveChanges();
        }
        

        public void Update(IEnumerable<OrderHeader> orderHeaders)
        {
            dbContext.SaveChanges();
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