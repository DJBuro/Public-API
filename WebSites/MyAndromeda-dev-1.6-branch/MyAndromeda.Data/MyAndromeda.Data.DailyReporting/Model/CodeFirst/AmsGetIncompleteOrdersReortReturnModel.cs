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
    public class AmsGetIncompleteOrdersReortReturnModel
    {
        public Int32 nstoreid { get; set; }
        public Int32 ordernum { get; set; }
        public String strstorename { get; set; }
        public DateTime? Date { get; set; }
        public Int32? OrderNumber { get; set; }
        public Double? Amount { get; set; }
        public DateTime? PlacedTime { get; set; }
        public String Trans_IncompleteOrdersReort { get; set; }
        public String Trans_strstoreName { get; set; }
        public String Trans_Date { get; set; }
        public String Trans_OrderNumber { get; set; }
        public String Trans_Amount { get; set; }
        public String Trans_OrderPlacedTime { get; set; }
    }

}
