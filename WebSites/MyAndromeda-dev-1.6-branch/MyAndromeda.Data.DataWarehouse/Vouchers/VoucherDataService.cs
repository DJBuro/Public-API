using System.Linq.Expressions;
using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace MyAndromeda.Data.DataWarehouse.Vouchers
{
    public class VoucherDataService : IVoucherDataService
    {
        private readonly DataWarehouseDbContext dataContext;
        public VoucherDataService(DataWarehouseDbContext dataContext)
        {
            this.dataContext = dataContext;
            this.Table = this.dataContext.Set<Voucher>();
            this.TableQuery = this.Table.Where(e=> !e.Removed)
                .Include(e => e.SiteVouchers)
                .Include(e => e.UsedVouchers);
        }

        public DbSet<Voucher> Table { get; private set; }

        public IQueryable<Voucher> TableQuery { get; private set; }

        public void ChangeIncludeScope<TPropertyModel>(Expression<Func<Voucher, TPropertyModel>> predicate)
        {
            this.TableQuery =
                this.TableQuery.Include(predicate).Where(e => !e.Removed);
        }

        public Voucher New()
        {
            return new Voucher()
            {
                Id = Guid.NewGuid()
            };
        }

        public Models.Voucher Create(Models.Voucher voucher)
        {
            Voucher retObj = this.Table.Add(voucher);
            dataContext.SaveChanges();
            return retObj;
        }

        public void Update(Models.Voucher voucher)
        {
            Models.Voucher existing = this.Table.Find(voucher.Id);
            ((IObjectContextAdapter)this.dataContext).ObjectContext.Detach(existing);
            dataContext.Entry(voucher).State = EntityState.Modified;
            dataContext.SaveChanges();
        }

        public bool Delete(Models.Voucher voucher)
        {
            var tableQuery = this.TableQuery.Where(e => e.Id == voucher.Id);
            var result = tableQuery.SingleOrDefault();

            result.Removed = true;
            result.Active = false;

            return dataContext.SaveChanges() > 0;
        }

        public Models.Voucher Get(System.Linq.Expressions.Expression<Func<Models.Voucher, bool>> query)
        {
            var table = this.TableQuery;
            var tableQuery = table.Where(query);

            return tableQuery.SingleOrDefault();
        }

        public IQueryable<Models.Voucher> List()
        {
            return this.TableQuery;
        }

        public IQueryable<Models.Voucher> List(System.Linq.Expressions.Expression<Func<Models.Voucher, bool>> predicate)
        {
            return this.TableQuery.Where(predicate);
        }
    }
}
