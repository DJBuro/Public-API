using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromedaDataAccess.Domain.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DataWarehouse.Vouchers
{
    public class VoucherReportDataService : IVoucherReportingDataService
    {
        internal IQueryable<Voucher> VoucherTable { get; set; }
        internal IQueryable<UsedVoucher> UsedVoucherTable { get; set; }
        internal IQueryable<OrderHeader> OrderHeaderTable { get; set; }

        public VoucherReportDataService(DataWarehouseEntities dbcontext)
        {
            var dbContext = dbcontext;

            this.VoucherTable = dbContext.Set<Voucher>();
            this.UsedVoucherTable = dbContext.Set<UsedVoucher>();
            this.OrderHeaderTable = dbContext.Set<OrderHeader>();
        }

        public VoucherSummary GetTotalOrdersByCode(Guid voucherId)
        {
            var summaryData = from voucher in VoucherTable
                              join uVoucher in UsedVoucherTable on voucher.Id equals uVoucher.VoucherId
                              join oh in OrderHeaderTable on uVoucher.OrderId equals oh.ID
                              where voucher.Id.Equals(voucherId) && oh.Status != 6 // omit cancelled orders
                              select new
                              {
                                  Id = voucher.Id,
                                  Code = voucher.VoucherCode,
                                  OrderTime = oh.TimeStamp,
                                  FinalPrice = oh.FinalPrice,
                                  TotalTax = oh.TotalTax,
                                  DelivaryCharge = oh.DeliveryCharge,
                                  IncludeTax = oh.PriceIncludeTax,
                              };

            var groupedData = summaryData.GroupBy(e =>
                new
                {
                    e.OrderTime.Day,
                    e.OrderTime.Month,
                    e.OrderTime.Year,
                    e.Id,
                }).Select(e => new
                {
                    Day = e.Key.Day,
                    Month = e.Key.Month,
                    Year = e.Key.Year,
                    NumberOfUses = e.Count(),
                    Total = e.Sum(item => item.FinalPrice),
                    MinPrice = e.Min(item => item.FinalPrice),
                    MaxPrice = e.Max(item => item.FinalPrice),
                    AvgPrice = e.Average(item => item.FinalPrice),
                }).ToArray();

            var results = groupedData.Select(e => new VoucherSummaryByCode()
            {
                NumberOfUses = e.NumberOfUses,
                Total = e.Total,
                Average = e.AvgPrice,
                Day = new DateTime(e.Year,e.Month,e.Day)
            });

            VoucherSummary summary = new VoucherSummary(results);
            return summary;
        }
    }
}
