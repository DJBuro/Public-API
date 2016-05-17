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
    public class AmsGetSalesAndServiceReportReturnModel
    {
        public String GroupName { get; set; }
        public String CompStore { get; set; }
        public Int32 nStoreID { get; set; }
        public String strStoreName { get; set; }
        public String DayOfWeek { get; set; }
        public DateTime? SalesDate { get; set; }
        public Double? SalesThisYear { get; set; }
        public Double? SalesLastWeek { get; set; }
        public Double? SalesLastYear { get; set; }
        public Double? DiffInCurrency { get; set; }
        public Double? DiffInPercentage { get; set; }
        public Double? CollectionSales { get; set; }
        public Double? DeliverySales { get; set; }
        public Int32? DeliveryTickets { get; set; }
        public Int32? Tickets { get; set; }
        public Double? InternetSales { get; set; }
        public Int32? InternetTickets { get; set; }
        public Double? AvgSpend { get; set; }
        public Double? AvgInternetSpend { get; set; }
        public Double? AvgInStoreTime { get; set; }
        public Double? AvgDriveTime { get; set; }
        public Double? AvgTimeToDoor { get; set; }
        public Int32? LessThan30Min { get; set; }
        public Double? Less30PercOfDel { get; set; }
        public Int32? MoreThan45Min { get; set; }
        public Double? More45PercOfDel { get; set; }
        public Double? PercTicketsAsDelivery { get; set; }
        public Double? TotalFullMenuPrice { get; set; }
        public Double? TotalVat { get; set; }
        public Double? TotalOrderPriceNet { get; set; }
        public String Trans_SalesAndServiceReport { get; set; }
        public String Trans_ClickToviewDetails { get; set; }
        public String Trans_groupname { get; set; }
        public String Trans_strStoreName { get; set; }
        public String Trans_theDate { get; set; }
        public String Trans_dayofweek { get; set; }
        public String Trans_SalesThisYear { get; set; }
        public String Trans_SalesLastWeek { get; set; }
        public String Trans_SalesLastYear { get; set; }
        public String Trans_DiffInCurrency { get; set; }
        public String Trans_DiffInPercentage { get; set; }
        public String Trans_CollectionSales { get; set; }
        public String Trans_DeliverySales { get; set; }
        public String Trans_DeliveryTickets { get; set; }
        public String Trans_Tickets { get; set; }
        public String Trans_InternetSales { get; set; }
        public String Trans_AvgSpend { get; set; }
        public String Trans_AvgInternetSpend { get; set; }
        public String Trans_AvgInStoreTime { get; set; }
        public String Trans_AvgDriveTime { get; set; }
        public String Trans_AvgTimeToDoor { get; set; }
        public String Trans_LessThan30Min { get; set; }
        public String Trans_Less30PercOfDel { get; set; }
        public String Trans_MoreThan45Min { get; set; }
        public String Trans_PercTicketsAsDelivery { get; set; }
        public String Trans_TotalFullMenuPrice { get; set; }
        public String Trans_TotalVat { get; set; }
        public String Trans_TotalOrderPriceNet { get; set; }
        public String Trans_InternetTickets { get; set; }
        public String Trans_MoreThan45PercOfDel { get; set; }
    }

}
