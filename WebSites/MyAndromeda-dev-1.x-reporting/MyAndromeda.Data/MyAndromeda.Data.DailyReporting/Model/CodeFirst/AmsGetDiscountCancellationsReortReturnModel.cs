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
    public class AmsGetDiscountCancellationsReortReturnModel
    {
        public Int32 nstoreid { get; set; }
        public String strstorename { get; set; }
        public DateTime? Date { get; set; }
        public Double? GrossSales { get; set; }
        public Double? TotalDiscounts { get; set; }
        public Double? Cancellations { get; set; }
        public String Trans_DiscountCancellationsReort { get; set; }
        public String Trans_strstoreName { get; set; }
        public String Trans_Date { get; set; }
        public String Trans_GrossSales { get; set; }
        public String Trans_TotalDiscounts { get; set; }
        public String Trans_Cancellations { get; set; }
    }

}
