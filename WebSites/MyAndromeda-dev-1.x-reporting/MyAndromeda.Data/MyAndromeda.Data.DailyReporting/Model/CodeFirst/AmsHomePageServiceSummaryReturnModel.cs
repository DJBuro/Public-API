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
    public class AmsHomePageServiceSummaryReturnModel
    {
        public Int32? CompanyID { get; set; }
        public String DayOfWeek { get; set; }
        public DateTime? TheDate { get; set; }
        public Double? TotalSalesTY { get; set; }
        public Int32? TotalTickets { get; set; }
        public Double? AvgMake { get; set; }
        public Double? AvgOven { get; set; }
        public Double? AvgInstore { get; set; }
        public Double? AvgDrive { get; set; }
        public Double? AvgDoor { get; set; }
        public String Trans_ServicePerfomance { get; set; }
        public String Trans_DayOfWeek { get; set; }
        public String Trans_TotalTickets { get; set; }
        public String Trans_AvgMake { get; set; }
        public String Trans_AvgOven { get; set; }
        public String Trans_AvgInstore { get; set; }
        public String Trans_AvgDrive { get; set; }
        public String Trans_AvgDoor { get; set; }
        public String Trans_TotalSalesTY { get; set; }
    }

}
