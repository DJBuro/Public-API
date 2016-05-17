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
    public class AmsGetWeeklyPayrollReportReturnModel
    {
        public Int32 StoreID { get; set; }
        public String StoreName { get; set; }
        public Int32? EmployeeNumber { get; set; }
        public String EmployeeName { get; set; }
        public String JobRole { get; set; }
        public Double? PayRate { get; set; }
        public Double? TimePaid { get; set; }
        public Double? DeliveryCommission { get; set; }
        public Double? TotalPay { get; set; }
        public String Trans_WeeklyPayrollReport { get; set; }
        public String Trans_strStoreName { get; set; }
        public String Trans_JobRole { get; set; }
        public String Trans_EmployeeNumber { get; set; }
        public String Trans_EmployeeName { get; set; }
        public String Trans_PayRate { get; set; }
        public String Trans_TimePaid { get; set; }
        public String Trans_DeliveryCommission { get; set; }
        public String Trans_TotalPay { get; set; }
    }

}
