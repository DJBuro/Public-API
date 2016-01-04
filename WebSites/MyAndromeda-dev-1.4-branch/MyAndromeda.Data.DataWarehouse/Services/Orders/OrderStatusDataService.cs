using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public class OrderStatusDataService : IOrderStatusDataService 
    {
        private readonly DataWarehouseEntities dataContext;

        public OrderStatusDataService(DataWarehouseEntities dataContext) 
        {
            this.dataContext = dataContext;
            this.Table = this.dataContext.Set<OrderStatu>();
            this.TableQuery = this.Table;
        }

        public DbSet<OrderStatu> Table { get; set; }
        public IQueryable<OrderStatu> TableQuery { get; set; }

        public void ChangeIncludeScope<PropertyModel>(Expression<Func<OrderStatu, PropertyModel>> predicate)
        {
            this.TableQuery = TableQuery.Include(predicate);
        }

        public OrderStatu New()
        {
            return new OrderStatu() { };
        }

        public OrderStatusHistory NewHistoreRecord()
        {
            return new OrderStatusHistory() { 
                Id = Guid.NewGuid()
            };
        } 

        public void AddHistory(OrderHeader order, OrderStatu oldStatus)
        {
            var table = this.dataContext.OrderStatusHistories;

            var record = this.NewHistoreRecord();
            record.ChangedDateTime = DateTime.UtcNow;
            record.OrderStatu = order.OrderStatu;
            record.OrderHeaderId = order.ID;

            table.Add(record);
            dataContext.SaveChanges();
        }

        public OrderStatu Get(Expression<Func<OrderStatu, bool>> query)
        {
            return this.List().Where(query).SingleOrDefault();
        }

        public IQueryable<OrderStatu> List()
        {
            var table = this.TableQuery;

            return table;
        }

        public IQueryable<OrderStatu> List(Expression<Func<OrderStatu, bool>> query)
        {
            var table = this.TableQuery;

            return table.Where(query);
        }

    }
}