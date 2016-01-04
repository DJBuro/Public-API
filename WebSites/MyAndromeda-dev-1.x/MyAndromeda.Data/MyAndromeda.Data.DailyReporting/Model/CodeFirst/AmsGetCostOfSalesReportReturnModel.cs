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
    public class AmsGetCostOfSalesReportReturnModel
    {
        public Int32 StoreID { get; set; }
        public String Storename { get; set; }
        public Double? OpenStock { get; set; }
        public Double? ReceivedStock { get; set; }
        public Double? CloseStock { get; set; }
        public Double? Usage { get; set; }
        public Double? IdealUsage { get; set; }
        public Decimal? Netsales { get; set; }
        public Double? Variance { get; set; }
        public Double? CosPercent { get; set; }
        public Double? IdealPercent { get; set; }
        public Double? VarPercent { get; set; }
        public String Trans_CostOfSalesReport { get; set; }
        public String Trans_strStoreName { get; set; }
        public String Trans_OpenStock { get; set; }
        public String Trans_ReceivedStock { get; set; }
        public String Trans_CloseStock { get; set; }
        public String Trans_Usage { get; set; }
        public String Trans_IdealUsage { get; set; }
        public String Trans_Netsales { get; set; }
        public String Trans_Variance { get; set; }
        public String Trans_CosPercent { get; set; }
        public String Trans_IdealPercent { get; set; }
        public String Trans_VarPercent { get; set; }
    }

}
