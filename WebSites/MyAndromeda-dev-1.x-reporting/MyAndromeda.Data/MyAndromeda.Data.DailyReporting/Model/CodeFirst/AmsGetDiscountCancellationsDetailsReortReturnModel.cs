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
    public class AmsGetDiscountCancellationsDetailsReortReturnModel
    {
        public Int32 nStoreId { get; set; }
        public Int32 ordernum { get; set; }
        public String strstorename { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? PlacedTime { get; set; }
        public Int32? OrderNumber { get; set; }
        public String Type { get; set; }
        public Double? OrderValue { get; set; }
        public Double? Undiscounted { get; set; }
        public String Reason { get; set; }
        public String Trans_DiscountCancellationsDetailsReort { get; set; }
        public String Trans_strstoreName { get; set; }
        public String Trans_Date { get; set; }
        public String Trans_Type { get; set; }
        public String Trans_OrderNumber { get; set; }
        public String Trans_PlacedTime { get; set; }
        public String Trans_OrderValue { get; set; }
        public String Trans_Undiscounted { get; set; }
        public String Trans_Reason { get; set; }
    }

}
