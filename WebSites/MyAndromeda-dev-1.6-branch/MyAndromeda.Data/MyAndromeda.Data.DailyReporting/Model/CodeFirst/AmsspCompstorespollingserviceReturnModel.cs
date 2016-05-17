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
    public class AmsspCompstorespollingserviceReturnModel
    {
        public String groupname { get; set; }
        public String compstore { get; set; }
        public DateTime? SalesDate { get; set; }
        public String DayOfWeek { get; set; }
        public Double? CompSalesTY { get; set; }
        public Double? CompSalesLY { get; set; }
        public Decimal? PercentDiff { get; set; }
        public Int32? CompTransactionsTY { get; set; }
        public Int32? CompTransactionsLY { get; set; }
        public Decimal? PercentTransDiff { get; set; }
    }

}
