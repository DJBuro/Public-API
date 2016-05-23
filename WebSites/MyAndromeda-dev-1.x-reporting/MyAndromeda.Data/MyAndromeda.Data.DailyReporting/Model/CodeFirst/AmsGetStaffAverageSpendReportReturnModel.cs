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
    public class AmsGetStaffAverageSpendReportReturnModel
    {
        public Int32 StoreID { get; set; }
        public String strstoreName { get; set; }
        public String EmployeeName { get; set; }
        public Decimal? OrderNet { get; set; }
        public Decimal? OrderVAT { get; set; }
        public Decimal? UnDiscounted { get; set; }
        public Decimal? OrderDiscount { get; set; }
        public Decimal? OrderGross { get; set; }
        public DateTime? TheDate { get; set; }
        public Int32? TotalOrders { get; set; }
        public Decimal? AvgSpendExcVAT { get; set; }
        public Decimal? AvgSpendIncVAT { get; set; }
        public String Trans_StaffAverageSpendReportas { get; set; }
        public String Trans_strstoreName { get; set; }
        public String Trans_EmployeeName { get; set; }
        public String Trans_OrderNet { get; set; }
        public String Trans_OrderVAT { get; set; }
        public String Trans_UnDiscounted { get; set; }
        public String Trans_OrderDiscount { get; set; }
        public String Trans_OrderGross { get; set; }
        public String Trans_TheDate { get; set; }
        public String Trans_TotalOrders { get; set; }
        public String Trans_AvgSpendExcVAT { get; set; }
        public String Trans_AvgSpendIncVAT { get; set; }
        public String Trnas_Total { get; set; }
    }

}
