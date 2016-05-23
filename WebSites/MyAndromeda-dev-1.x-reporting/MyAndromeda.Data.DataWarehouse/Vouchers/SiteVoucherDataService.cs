using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace MyAndromeda.Data.DataWarehouse.Vouchers
{
    public class SiteVoucherDataService : ISiteVoucherDataService
    {
        private readonly DataWarehouseDbContext dataContext;

        public SiteVoucherDataService(DataWarehouseDbContext dataContext) 
        {
            this.dataContext = dataContext;
            this.Table = this.dataContext.Set<SiteVoucher>();
            this.TableQuery = this.Table.Include(e=> e.Voucher);
        }

        public DbSet<SiteVoucher> Table { get; private set; }

        public IQueryable<SiteVoucher> TableQuery { get; private set; }

        public void ChangeIncludeScope<TPropertyModel>(Expression<Func<SiteVoucher, TPropertyModel>> predicate)
        {
            this.TableQuery = TableQuery.Include(predicate);
        }

        public SiteVoucher Get(Guid Id)
        {
            return this.dataContext.SiteVouchers.Include(e => e.Voucher).SingleOrDefault(e => e.VoucherId == Id);
        }

        public void Create(SiteVoucher siteVoucher)
        {
            if (this.TableQuery.Any(e => e.VoucherId == siteVoucher.VoucherId && e.AndromedaSiteId == siteVoucher.AndromedaSiteId))
                return;

            this.Table.Add(siteVoucher);
            this.dataContext.SaveChanges();
        }

        public void Update(SiteVoucher siteVoucher)
        {
            SiteVoucher existing = this.Table.Find(siteVoucher.VoucherId);
            ((IObjectContextAdapter)this.dataContext).ObjectContext.Detach(existing); 
            dataContext.SaveChanges();
        }

        public bool Delete(SiteVoucher siteVoucher)
        {
            SiteVoucher retObj = this.Table.Remove(siteVoucher);

            return dataContext.SaveChanges() > 0;
        }

        public SiteVoucher New()
        {
            return new SiteVoucher() { };
        }

        public SiteVoucher Get(Expression<Func<SiteVoucher, bool>> query)
        {
            var tableQuery = this.TableQuery.Where(query);
            return tableQuery.SingleOrDefault();
        }

        public IQueryable<SiteVoucher> List()
        {
            return this.TableQuery;
        }

        public IQueryable<SiteVoucher> List(Expression<Func<SiteVoucher, bool>> predicate)
        {
            return this.TableQuery.Where(predicate);
        }
    }
}
