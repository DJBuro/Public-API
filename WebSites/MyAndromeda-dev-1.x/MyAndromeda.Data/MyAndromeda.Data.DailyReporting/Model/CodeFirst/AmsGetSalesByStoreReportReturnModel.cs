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
    public class AmsGetSalesByStoreReportReturnModel
    {
        public String GroupName { get; set; }
        public Int32 nStoreID { get; set; }
        public String strStoreName { get; set; }
        public String CompanyID { get; set; }
        public DateTime? theDate { get; set; }
        public String DayOfWeek { get; set; }
        public Decimal? OrderNet { get; set; }
        public Decimal? OrderVAT { get; set; }
        public Decimal? UnDiscounted { get; set; }
        public Decimal? OrderDiscount { get; set; }
        public Decimal? OrderGross { get; set; }
        public Decimal? MenuPriceGross { get; set; }
        public String Trans_SalesByStoreReport { get; set; }
        public String Trans_ClickToviewDetails { get; set; }
        public String Trans_groupname { get; set; }
        public String Trans_strStoreName { get; set; }
        public String Trans_CompanyID { get; set; }
        public String Trans_theDate { get; set; }
        public String Trans_dayofweek { get; set; }
        public String Trans_OrderNet { get; set; }
        public String Trans_OrderVAT { get; set; }
        public String Trans_UnDiscounted { get; set; }
        public String Trans_OrderDiscount { get; set; }
        public String Trans_OrderGross { get; set; }
        public String Trans_MenuPriceGross { get; set; }
    }

}
