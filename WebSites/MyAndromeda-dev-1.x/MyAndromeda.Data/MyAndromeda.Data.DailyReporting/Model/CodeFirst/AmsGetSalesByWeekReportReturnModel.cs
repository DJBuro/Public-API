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
    public class AmsGetSalesByWeekReportReturnModel
    {
        public Int32 nStoreID { get; set; }
        public String TotalName { get; set; }
        public String strStoreName { get; set; }
        public String CompanyID { get; set; }
        public String CompStore { get; set; }
        public Decimal? Gross { get; set; }
        public Decimal? PostDiscountSales { get; set; }
        public Decimal? Discounts { get; set; }
        public Decimal? VAT { get; set; }
        public Decimal? MenuPriceGross { get; set; }
        public String Trans_Week { get; set; }
        public String Trans_SalesByWeek { get; set; }
        public String Trans_nStoreID { get; set; }
        public String Trans_TotalName { get; set; }
        public String Trans_strStoreName { get; set; }
        public String Trans_CompanyID { get; set; }
        public String Trans_CompStore { get; set; }
        public String Trans_Gross { get; set; }
        public String Trans_PostDiscountSales { get; set; }
        public String Trans_Discounts { get; set; }
        public String Trans_VAT { get; set; }
        public String Trans_MenuPriceGross { get; set; }
    }

}
