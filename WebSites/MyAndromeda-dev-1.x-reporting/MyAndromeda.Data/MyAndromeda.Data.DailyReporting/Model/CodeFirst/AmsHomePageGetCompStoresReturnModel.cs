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
    public class AmsHomePageGetCompStoresReturnModel
    {
        public String DayOfWeek { get; set; }
        public Double? SalesTY { get; set; }
        public Double? SalesLY { get; set; }
        public Double? PercentDiff { get; set; }
        public Double? TotalFMP { get; set; }
        public Double? TotalDiscDeal { get; set; }
        public Double? TotalVAT { get; set; }
        public Double? TotalDiff { get; set; }
        public String groupName { get; set; }
        public String Trans_HomePageTitle { get; set; }
        public String Trans_SalesPerfomance { get; set; }
        public String Trans_DayOfWeek { get; set; }
        public String Trans_SalesTY { get; set; }
        public String Trans_SalesLY { get; set; }
        public String Trans_PercentDiff { get; set; }
        public String Trans_TotalFMP { get; set; }
        public String Trans_TotalDiscDeal { get; set; }
        public String Trans_TotalVAT { get; set; }
        public String Trans_TotalDiff { get; set; }
        public String Trans_groupName { get; set; }
        public String Trans_Total { get; set; }
        public String Trans_TYTranslation { get; set; }
        public String Trans_LYTranslation { get; set; }
    }

}
