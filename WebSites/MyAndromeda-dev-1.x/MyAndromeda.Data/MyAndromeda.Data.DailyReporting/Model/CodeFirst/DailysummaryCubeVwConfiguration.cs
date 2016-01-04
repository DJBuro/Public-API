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
    // dailysummary_cube_vw
    internal class DailysummaryCubeVwConfiguration : EntityTypeConfiguration<DailysummaryCubeVw>
    {
        public DailysummaryCubeVwConfiguration()
            : this("dbo")
        {
        }
 
        public DailysummaryCubeVwConfiguration(string schema)
        {
            ToTable(schema + ".dailysummary_cube_vw");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("bigint");
            Property(x => x.TheDate).HasColumnName("TheDate").IsOptional().HasColumnType("date");
            Property(x => x.NIdealCos).HasColumnName("nIdealCOS").IsOptional().HasColumnType("bigint");
            Property(x => x.NStoreId).HasColumnName("nStoreID").IsOptional().HasColumnType("bigint");
            Property(x => x.NColHourlyPay).HasColumnName("nCOLHourlyPay").IsOptional().HasColumnType("bigint");
            Property(x => x.TotPay).HasColumnName("TotPay").IsOptional().HasColumnType("bigint");
            Property(x => x.NWastage).HasColumnName("nWastage").IsOptional().HasColumnType("bigint");
            Property(x => x.NInvDaily).HasColumnName("nInvDaily").IsOptional().HasColumnType("bigint");
            Property(x => x.NInvWeekly).HasColumnName("nInvWeekly").IsOptional().HasColumnType("bigint");
            Property(x => x.NInvMonthly).HasColumnName("nInvMonthly").IsOptional().HasColumnType("bigint");
            Property(x => x.NDealsRec).HasColumnName("nDealsRec").IsOptional().HasColumnType("bigint");
            Property(x => x.NTransIn).HasColumnName("nTransIn").IsOptional().HasColumnType("bigint");
            Property(x => x.NTransOut).HasColumnName("nTransOut").IsOptional().HasColumnType("bigint");
            Property(x => x.NetSales).HasColumnName("NetSales").IsOptional().HasColumnType("bigint");
            Property(x => x.TotalOrders).HasColumnName("TotalOrders").IsOptional().HasColumnType("bigint");
            Property(x => x.DineInNetSales).HasColumnName("DineInNetSales").IsOptional().HasColumnType("bigint");
            Property(x => x.DelNetSales).HasColumnName("DelNetSales").IsOptional().HasColumnType("bigint");
            Property(x => x.CarryOutNetSales).HasColumnName("CarryOutNetSales").IsOptional().HasColumnType("bigint");
            Property(x => x.DineInTotalOrders).HasColumnName("DineInTotalOrders").IsOptional().HasColumnType("bigint");
            Property(x => x.DelTotalOrders).HasColumnName("DelTotalOrders").IsOptional().HasColumnType("bigint");
            Property(x => x.CarryOutTotalOrders).HasColumnName("CarryOutTotalOrders").IsOptional().HasColumnType("bigint");
            Property(x => x.TotSalesTax).HasColumnName("TotSalesTax").IsOptional().HasColumnType("bigint");
            Property(x => x.ManagerDiscounts).HasColumnName("ManagerDiscounts").IsOptional().HasColumnType("bigint");
            Property(x => x.DealDiscounts).HasColumnName("DealDiscounts").IsOptional().HasColumnType("bigint");
            Property(x => x.FullMenuPrice).HasColumnName("FullMenuPrice").IsOptional().HasColumnType("bigint");
            Property(x => x.LabourSeconds).HasColumnName("LabourSeconds").IsOptional().HasColumnType("bigint");
            Property(x => x.LabourCost).HasColumnName("LabourCost").IsOptional().HasColumnType("bigint");
            Property(x => x.TotalCancels).HasColumnName("TotalCancels").IsOptional().HasColumnType("bigint");
            Property(x => x.ValueCancels).HasColumnName("valueCancels").IsOptional().HasColumnType("bigint");
            Property(x => x.NCloseDailyCount).HasColumnName("nCloseDailyCount").IsOptional().HasColumnType("bigint");
            Property(x => x.NCloseWeeklyCount).HasColumnName("nCloseWeeklyCount").IsOptional().HasColumnType("bigint");
            Property(x => x.NCloseMonthlyCount).HasColumnName("nCloseMonthlyCount").IsOptional().HasColumnType("bigint");
            Property(x => x.NumOrdersLt30Mins).HasColumnName("NumOrdersLT30Mins").IsOptional().HasColumnType("bigint");
            Property(x => x.NumOrdersLt45Mins).HasColumnName("NumOrdersLT45Mins").IsOptional().HasColumnType("bigint");
            Property(x => x.AvgMake).HasColumnName("AvgMake").IsOptional().HasColumnType("bigint");
            Property(x => x.AvgOutTheDoor).HasColumnName("AvgOutTheDoor").IsOptional().HasColumnType("bigint");
            Property(x => x.AvgDoorTime).HasColumnName("AvgDoorTime").IsOptional().HasColumnType("bigint");
            Property(x => x.TotCos).HasColumnName("TotCOS").IsOptional().HasColumnType("bigint");
            Property(x => x.NReceipt).HasColumnName("nReceipt").IsOptional().HasColumnType("float");
            Property(x => x.BottleDeposit).HasColumnName("BottleDeposit").IsOptional().HasColumnType("bigint");
            Property(x => x.AltBottleDeposit).HasColumnName("AltBottleDeposit").IsOptional().HasColumnType("bigint");
            Property(x => x.RefundsInCurr).HasColumnName("RefundsInCurr").IsOptional().HasColumnType("bigint");
            Property(x => x.RefundsQty).HasColumnName("RefundsQty").IsOptional().HasColumnType("bigint");
            Property(x => x.OnlineNetSales).HasColumnName("OnlineNetSales").IsOptional().HasColumnType("bigint");
            Property(x => x.OnlineGrossSales).HasColumnName("OnlineGrossSales").IsOptional().HasColumnType("bigint");
            Property(x => x.OnlineDelNetSales).HasColumnName("OnlineDelNetSales").IsOptional().HasColumnType("bigint");
            Property(x => x.OnlineColNetSales).HasColumnName("OnlineColNetSales").IsOptional().HasColumnType("bigint");
            Property(x => x.OnlineDelGrossSales).HasColumnName("OnlineDelGrossSales").IsOptional().HasColumnType("bigint");
            Property(x => x.OnlineColGrossSales).HasColumnName("OnlineColGrossSales").IsOptional().HasColumnType("bigint");
            Property(x => x.OnlineTrans).HasColumnName("OnlineTrans").IsOptional().HasColumnType("bigint");
            Property(x => x.OnlineDelTrans).HasColumnName("OnlineDelTrans").IsOptional().HasColumnType("bigint");
            Property(x => x.OnlineColTrans).HasColumnName("OnlineColTrans").IsOptional().HasColumnType("bigint");
            Property(x => x.OnlineTotalTax).HasColumnName("OnlineTotalTax").IsOptional().HasColumnType("bigint");
            Property(x => x.OnlineDelTax).HasColumnName("OnlineDelTax").IsOptional().HasColumnType("bigint");
            Property(x => x.OnlineColTax).HasColumnName("OnlineColTax").IsOptional().HasColumnType("bigint");
            Property(x => x.RackTime).HasColumnName("RackTime").IsOptional().HasColumnType("bigint");
        }
    }

}
