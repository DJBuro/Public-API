using MyAndromeda.Data.DataWarehouse.Domain.Reporting;
using MyAndromeda.Web.Areas.Voucher.Models;
using MyAndromedaDataAccess.Domain.Reporting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Reporting.ViewModels
{
    public class VoucherSummaryViewModel
    {
        public Guid VoucherId { get; set; }
        public VoucherSummary VoucherSummary { get; set; }

        public VoucherSummaryViewModel()
        { 
            VoucherSummary = new VoucherSummary(new List<VoucherSummaryByCode>());
        }
    }
}