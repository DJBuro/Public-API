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
    public class AmsGetDailyTotalsReturnModel
    {
        public String strstorename { get; set; }
        public Int32 nstoreid { get; set; }
        public DateTime? thedate { get; set; }
        public String Occasion { get; set; }
        public Int32? PriceA { get; set; }
        public Int32? PriceB { get; set; }
        public Int32? PriceC { get; set; }
        public Int32? TaxA { get; set; }
        public Int32? TaxB { get; set; }
        public Int32? TaxC { get; set; }
        public Int32? Delivery { get; set; }
        public Double? DeliveryMinusTax { get; set; }
        public Double? DeliveryTax { get; set; }
        public Int32? DayTotal { get; set; }
        public Int32? VATTotal { get; set; }
        public Int32? SortOrder { get; set; }
    }

}
