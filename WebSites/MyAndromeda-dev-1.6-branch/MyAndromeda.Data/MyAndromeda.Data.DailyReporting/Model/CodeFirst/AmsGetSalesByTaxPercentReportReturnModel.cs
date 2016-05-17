// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// TargetFrameworkVersion = 4.5
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Entity.ModelConfiguration;
using System.Threading;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace MyAndromeda.Data.DailyReporting.Model.CodeFirst
{
    public class AmsGetSalesByTaxPercentReportReturnModel
    {
        public String strStoreName { get; set; }
        public Int32 storeID { get; set; }
        public DateTime? theDate { get; set; }
        public Decimal? NetA { get; set; }
        public Decimal? NetB { get; set; }
        public Decimal? NetC { get; set; }
        public Decimal? NettotalSales { get; set; }
        public Decimal? TaxA { get; set; }
        public Decimal? TaxB { get; set; }
        public Decimal? TaxC { get; set; }
        public Decimal? TaxTotal { get; set; }
        public Decimal? PriceA { get; set; }
        public Decimal? PriceB { get; set; }
        public Decimal? PriceC { get; set; }
        public Decimal? GrossTotal { get; set; }
        public Decimal? DelCharge { get; set; }
        public Decimal? TotalNetSales { get; set; }
        public String Trans_SalesByTaxPercentReport { get; set; }
        public String Trans_strStoreName { get; set; }
        public String Trans_theDate { get; set; }
        public String Trans_NetSalesA { get; set; }
        public String Trans_NetSalesB { get; set; }
        public String Trans_NetSalesC { get; set; }
        public String Trans_NetSales { get; set; }
        public String Trans_VATA { get; set; }
        public String Trans_VATB { get; set; }
        public String Trans_VATC { get; set; }
        public String Trans_VAT { get; set; }
        public String Trans_GrossSalesA { get; set; }
        public String Trans_GrossSalesB { get; set; }
        public String Trans_GrossSalesC { get; set; }
        public String Trans_GrossSales { get; set; }
        public String Trans_GrandTotal { get; set; }
        public String Trans_DelCharge { get; set; }
        public String Trans_TotalNetSales { get; set; }
    }

}
