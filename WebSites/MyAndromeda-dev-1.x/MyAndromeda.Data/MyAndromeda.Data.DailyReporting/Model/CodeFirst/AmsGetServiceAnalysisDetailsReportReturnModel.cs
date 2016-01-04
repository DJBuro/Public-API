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
    public class AmsGetServiceAnalysisDetailsReportReturnModel
    {
        public Int32 nstoreid { get; set; }
        public String strstorename { get; set; }
        public DateTime? thedate { get; set; }
        public String doyofweek { get; set; }
        public String CustomerName { get; set; }
        public String PhoneNumber { get; set; }
        public String OrderTaker { get; set; }
        public String Cashier { get; set; }
        public String Driver { get; set; }
        public Int32? OrderNumber { get; set; }
        public Int32 OrderIDNumber { get; set; }
        public String Occasion { get; set; }
        public String OrderType { get; set; }
        public DateTime? OrderPlacedTime { get; set; }
        public Double? Column1 { get; set; }
        public Double? Column2 { get; set; }
        public Double? Column3 { get; set; }
        public Double? Column4 { get; set; }
        public Double? Column5 { get; set; }
        public Double? CallTime { get; set; }
        public Double? MakeTime { get; set; }
        public Double? OvenTime { get; set; }
        public Double? InstoreTime { get; set; }
        public Double? DriveTime { get; set; }
        public Double? DoorTime { get; set; }
        public Int32? MinutesAhead { get; set; }
        public Int32? RemoteOrderReference { get; set; }
        public String Trans_ServiceAnalysisDetailsReport { get; set; }
        public String Trans_ClickToviewDetails { get; set; }
        public String Trans_strStoreName { get; set; }
        public String Trans_theDate { get; set; }
        public String Trans_dayofweek { get; set; }
        public String Trans_CustomerName { get; set; }
        public String Trans_PhoneNumber { get; set; }
        public String Trans_OrderTaker { get; set; }
        public String Trans_Cashier { get; set; }
        public String Trans_Driver { get; set; }
        public String Trans_OrderNumber { get; set; }
        public String Trans_Occasion { get; set; }
        public String Trans_OrderType { get; set; }
        public String Trans_OrderPlacedTime { get; set; }
        public String Trans_OrderNet { get; set; }
        public String Trans_OrderVAT { get; set; }
        public String Trans_UnDiscounted { get; set; }
        public String Trans_OrderDiscount { get; set; }
        public String Trans_OrderGross { get; set; }
        public String Trans_CallTime { get; set; }
        public String Trans_MakeTime { get; set; }
        public String Trans_OvenTime { get; set; }
        public String Trans_InstoreTime { get; set; }
        public String Trans_DriveTime { get; set; }
        public String Trans_DoorTime { get; set; }
        public String Trans_MinutesAhead { get; set; }
        public String Trans_RemoteOrderReference { get; set; }
    }

}
