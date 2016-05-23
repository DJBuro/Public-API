using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Vouchers;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Services.Vouchers.Services
{
    public class SiteVoucherService : ISiteVoucherService
    {
        private readonly ISiteVoucherDataService siteVoucherDataService;
        private readonly IVoucherDataService voucherDataService;
        private readonly ICurrentSite currentSite;

        public SiteVoucherService(ISiteVoucherDataService siteVoucherDataService, 
            IVoucherDataService voucherDataService,
            ICurrentSite currentSite)
        {
            this.siteVoucherDataService = siteVoucherDataService;
            this.voucherDataService = voucherDataService;
            this.currentSite = currentSite;
        }


        public Voucher Get(Guid Id)
        {
            return this.voucherDataService
                .Get(e => e.Id == Id && e.SiteVouchers.Any(siteVoucher => siteVoucher.AndromedaSiteId == this.currentSite.AndromediaSiteId));
        }

        public Voucher GetByExpression(System.Linq.Expressions.Expression<Func<Voucher, bool>> query)
        {
            int andromedaSiteId = this.currentSite.AndromediaSiteId;
            var queryDataService = this.voucherDataService
                .List(query)
                .Where(e => e.SiteVouchers.Any(siteVoucher => siteVoucher.AndromedaSiteId == andromedaSiteId));

            var result = queryDataService.SingleOrDefault();

            return result;
        }

        

        public Voucher New()
        {
            return new Voucher() { Id = Guid.NewGuid() };
        }

        public void Create(Voucher voucher)
        {
            if (!this.currentSite.Available)
            {
                throw new ArgumentNullException("Site is not available");
            }
            
            this.voucherDataService.Create(voucher);

            if (!voucher.SiteVouchers.Any()) 
            {
                this.siteVoucherDataService.Create(new SiteVoucher() { AndromedaSiteId = this.currentSite.AndromediaSiteId, VoucherId = voucher.Id });
            }
          
        }

        public void Update(Voucher voucher)
        {
            this.voucherDataService.Update(voucher);
        }

        public bool Delete(Voucher voucher)
        {
            return this.voucherDataService.Delete(voucher);
        }

        public Voucher Get(Expression<Func<Voucher, bool>> predicate)
        {
            var voucher = this.List().Where(predicate).SingleOrDefault();

            return voucher;
        }

        public IQueryable<Voucher> List()
        {
            if (!this.currentSite.Available)
            {
                return Enumerable.Empty<Voucher>().AsQueryable();
            }
            
            return this.voucherDataService.List().Where(e => e.SiteVouchers.Any(x => x.AndromedaSiteId == this.currentSite.AndromediaSiteId));
        }

        public IQueryable<Voucher> List(Expression<Func<Voucher, bool>> predicate)
        {
            if (!this.currentSite.Available)
            {
                return Enumerable.Empty<Voucher>().AsQueryable();
            }

            var query = this.voucherDataService.List()
                .Where(e => e.SiteVouchers.Any(x => x.AndromedaSiteId == this.currentSite.AndromediaSiteId))
                .Where(predicate);

            return query;
        }
    }
}
