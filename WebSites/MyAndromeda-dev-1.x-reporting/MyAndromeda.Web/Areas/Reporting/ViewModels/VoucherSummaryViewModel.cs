using System;
using System.ComponentModel;
using System.Collections.Generic;
using MyAndromeda.Data.DataWarehouse.Domain.Reporting;

namespace MyAndromeda.Web.Areas.Reporting.ViewModels
{
    public class VoucherSummaryViewModel
    {
        [DisplayName("Voucher")]
        public Guid VoucherId { get; set; }

        public VoucherSummary VoucherSummary { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public VoucherSummaryViewModel()
        { 
            VoucherSummary = new VoucherSummary(new List<VoucherSummaryByCode>());
        }
    }
}