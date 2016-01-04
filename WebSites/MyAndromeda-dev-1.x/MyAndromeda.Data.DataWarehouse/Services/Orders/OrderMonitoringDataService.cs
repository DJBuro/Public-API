using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public class OrderMonitoringDataService : IOrderMonitoringDataService
    {
        private readonly DataWarehouseDbContext dataContext;
        public OrderMonitoringDataService(DataWarehouseDbContext dataContext)
        {
            this.dataContext = dataContext;
            this.Table = this.dataContext.Set<OrderHeader>();
            this.TableQuery = this.Table;
        }
        public DbSet<OrderHeader> Table { get; private set; }

        public IQueryable<OrderHeader> TableQuery { get; private set; }

        public List<OrderHeader> GetOrders(double minutes, int status)
        {
            DateTime dtTemp = DateTime.UtcNow.AddMinutes(minutes);
            List<OrderHeader> data
                = TableQuery
                .Include(e => e.OrderLines)
                .Include(e => e.OrderLines.Select(x => x.modifiers))
                .Include(e => e.Customer)
                .Include(e => e.CustomerAddress)
                .Include(e => e.ACSErrorCode1)
                .Include(e => e.UsedVouchers)
                .Include(e => e.UsedVouchers.Select(x => x.Voucher))
                .Include(e => e.OrderStatu)
                .Where(x => ((status < 0 || x.Status == status) && x.OrderWantedTime < dtTemp)).Select(x => x).ToList();
            return data;
        }

        public Models.OrderHeader GetOrderById(Guid id)
        {
            OrderHeader data
                = TableQuery
                .Include(e => e.OrderLines)
                .Include(e => e.OrderLines.Select(x => x.modifiers))
                .Include(e => e.Customer.Contacts)
                .Include(e => e.CustomerAddress)
                .Include(e => e.ACSErrorCode1)
                .Include(e => e.UsedVouchers)
                .Include(e => e.UsedVouchers.Select(x => x.Voucher))
                .Include(e => e.OrderStatu)
                .Where(x => x.ID == id).Select(x => x).SingleOrDefault();
            return data;
        }
        public void ChangeIncludeScope<TPropertyModel>(System.Linq.Expressions.Expression<Func<Models.OrderHeader, TPropertyModel>> predicate)
        {
            this.TableQuery =
                this.TableQuery.Include(predicate);
        }

        public Models.OrderHeader New()
        {
            throw new NotImplementedException();
        }

        public Models.OrderHeader Get(System.Linq.Expressions.Expression<Func<Models.OrderHeader, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Models.OrderHeader> List()
        {
            return this.TableQuery.Include(e => e.OrderLines)
                .Include(e => e.Customer)
                .Include(e => e.CustomerAddress)
                .Include(e => e.ACSErrorCode1)
                .Include(e => e.UsedVouchers)
                .Include(e => e.UsedVouchers.Select(x => x.Voucher));
        }

        public IQueryable<Models.OrderHeader> List(System.Linq.Expressions.Expression<Func<Models.OrderHeader, bool>> predicate)
        {
            return this.TableQuery
                .Include(e => e.OrderLines)
                .Include(e => e.OrderLines.Select(x => x.modifiers))
                .Include(e => e.Customer)
                .Include(e => e.CustomerAddress)
                .Include(e => e.ACSErrorCode1)
                .Include(e => e.UsedVouchers)
                .Include(e => e.UsedVouchers.Select(x => x.Voucher))
                .Include(e => e.OrderStatu).Where(predicate);
        }

        public void Update(Models.OrderHeader model)
        {
            Models.OrderHeader existing = this.Table.Find(model.ID);
            ((IObjectContextAdapter)this.dataContext).ObjectContext.Detach(existing);
            dataContext.Entry(model).State = EntityState.Modified;
            dataContext.SaveChanges();
        }
    }
}
