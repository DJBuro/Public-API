// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

namespace MyAndromeda.Data.DailyReporting.Model.CodeFirst
{
    // DailySummary
    internal partial class DailySummaryConfiguration : EntityTypeConfiguration<DailySummary>
    {
        public DailySummaryConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".DailySummary");
            HasKey(x => x.RecordId);

            Property(x => x.RecordId).HasColumnName("RecordID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.TheDate).HasColumnName("TheDate").IsOptional();
            Property(x => x.NIdealCos).HasColumnName("nIdealCOS").IsOptional();
            Property(x => x.NStoreId).HasColumnName("nStoreID").IsOptional();
            Property(x => x.NColHourlyPay).HasColumnName("nCOLHourlyPay").IsOptional();
            Property(x => x.TotPay).HasColumnName("TotPay").IsOptional();
            Property(x => x.NWastage).HasColumnName("nWastage").IsOptional();
            Property(x => x.NInvDaily).HasColumnName("nInvDaily").IsOptional();
            Property(x => x.NInvWeekly).HasColumnName("nInvWeekly").IsOptional();
            Property(x => x.NInvMonthly).HasColumnName("nInvMonthly").IsOptional();
            Property(x => x.NDealsRec).HasColumnName("nDealsRec").IsOptional();
            Property(x => x.NTransIn).HasColumnName("nTransIn").IsOptional();
            Property(x => x.NTransOut).HasColumnName("nTransOut").IsOptional();
            Property(x => x.NetSales).HasColumnName("NetSales").IsOptional();
            Property(x => x.TotalOrders).HasColumnName("TotalOrders").IsOptional();
            Property(x => x.DineInNetSales).HasColumnName("DineInNetSales").IsOptional();
            Property(x => x.DelNetSales).HasColumnName("DelNetSales").IsOptional();
            Property(x => x.CarryOutNetSales).HasColumnName("CarryOutNetSales").IsOptional();
            Property(x => x.DineInTotalOrders).HasColumnName("DineInTotalOrders").IsOptional();
            Property(x => x.DelTotalOrders).HasColumnName("DelTotalOrders").IsOptional();
            Property(x => x.CarryOutTotalOrders).HasColumnName("CarryOutTotalOrders").IsOptional();
            Property(x => x.TotSalesTax).HasColumnName("TotSalesTax").IsOptional();
            Property(x => x.ManagerDiscounts).HasColumnName("ManagerDiscounts").IsOptional();
            Property(x => x.DealDiscounts).HasColumnName("DealDiscounts").IsOptional();
            Property(x => x.FullMenuPrice).HasColumnName("FullMenuPrice").IsOptional();
            Property(x => x.LabourSeconds).HasColumnName("LabourSeconds").IsOptional();
            Property(x => x.LabourCost).HasColumnName("LabourCost").IsOptional();
            Property(x => x.TotalCancels).HasColumnName("TotalCancels").IsOptional();
            Property(x => x.ValueCancels).HasColumnName("valueCancels").IsOptional();
            Property(x => x.NCloseDailyCount).HasColumnName("nCloseDailyCount").IsOptional();
            Property(x => x.NCloseWeeklyCount).HasColumnName("nCloseWeeklyCount").IsOptional();
            Property(x => x.NCloseMonthlyCount).HasColumnName("nCloseMonthlyCount").IsOptional();
            Property(x => x.NumOrdersLt30Mins).HasColumnName("NumOrdersLT30Mins").IsOptional();
            Property(x => x.NumOrdersLt45Mins).HasColumnName("NumOrdersLT45Mins").IsOptional();
            Property(x => x.AvgMake).HasColumnName("AvgMake").IsOptional();
            Property(x => x.AvgOutTheDoor).HasColumnName("AvgOutTheDoor").IsOptional();
            Property(x => x.AvgDoorTime).HasColumnName("AvgDoorTime").IsOptional();
            Property(x => x.TotCos).HasColumnName("TotCOS").IsOptional();
            Property(x => x.NReceipt).HasColumnName("nReceipt").IsOptional();
            Property(x => x.BottleDeposit).HasColumnName("BottleDeposit").IsOptional();
            Property(x => x.AltBottleDeposit).HasColumnName("AltBottleDeposit").IsOptional();
            Property(x => x.RefundsInCurr).HasColumnName("RefundsInCurr").IsOptional();
            Property(x => x.RefundsQty).HasColumnName("RefundsQty").IsOptional();
            Property(x => x.OnlineNetSales).HasColumnName("OnlineNetSales").IsOptional();
            Property(x => x.OnlineGrossSales).HasColumnName("OnlineGrossSales").IsOptional();
            Property(x => x.OnlineDelNetSales).HasColumnName("OnlineDelNetSales").IsOptional();
            Property(x => x.OnlineColNetSales).HasColumnName("OnlineColNetSales").IsOptional();
            Property(x => x.OnlineDelGrossSales).HasColumnName("OnlineDelGrossSales").IsOptional();
            Property(x => x.OnlineColGrossSales).HasColumnName("OnlineColGrossSales").IsOptional();
            Property(x => x.OnlineTrans).HasColumnName("OnlineTrans").IsOptional();
            Property(x => x.OnlineDelTrans).HasColumnName("OnlineDelTrans").IsOptional();
            Property(x => x.OnlineColTrans).HasColumnName("OnlineColTrans").IsOptional();
            Property(x => x.OnlineTotalTax).HasColumnName("OnlineTotalTax").IsOptional();
            Property(x => x.OnlineDelTax).HasColumnName("OnlineDelTax").IsOptional();
            Property(x => x.OnlineColTax).HasColumnName("OnlineColTax").IsOptional();
            Property(x => x.CreateTimeStamp).HasColumnName("Create_TimeStamp").IsOptional();
            Property(x => x.UpdateTimeStamp).HasColumnName("Update_TimeStamp").IsOptional();
            Property(x => x.NCustomerId).HasColumnName("nCustomerID").IsOptional().HasMaxLength(8);
            Property(x => x.RackTime).HasColumnName("RackTime").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
