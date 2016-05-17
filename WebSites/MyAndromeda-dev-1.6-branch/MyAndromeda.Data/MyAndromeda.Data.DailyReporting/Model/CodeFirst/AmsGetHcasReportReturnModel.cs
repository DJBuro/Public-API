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
    public class AmsGetHcasReportReturnModel
    {
        public Double? TotalAmount { get; set; }
        public DateTime? Date { get; set; }
        public String strStoreName { get; set; }
        public String CompanyID { get; set; }
        public String Catagory { get; set; }
        public String TheDetails { get; set; }
        public Decimal? TotalWebCard { get; set; }
        public Decimal? PaidOut { get; set; }
        public Decimal? Receipts { get; set; }
        public Decimal? Banked { get; set; }
        public String Trans_HCasReport { get; set; }
        public String Trans_TotalAmount { get; set; }
        public String Trans_Date { get; set; }
        public String Trans_StoreName { get; set; }
        public String Trans_CompanyID { get; set; }
        public String Trans_Catagory { get; set; }
        public String Trans_TheDetails { get; set; }
        public String Trans_TotalWebCard { get; set; }
        public String Trans_PaidOut { get; set; }
        public String Trans_Receipts { get; set; }
        public String Trans_Banked { get; set; }
    }

}
