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
    // dailysummary_cube_vw
    public partial class DailysummaryCubeVw
    {
        public long Id { get; set; } // Id
        public DateTime? TheDate { get; set; } // TheDate
        public long? NIdealCos { get; set; } // nIdealCOS
        public long? NStoreId { get; set; } // nStoreID
        public long? NColHourlyPay { get; set; } // nCOLHourlyPay
        public long? TotPay { get; set; } // TotPay
        public long? NWastage { get; set; } // nWastage
        public long? NInvDaily { get; set; } // nInvDaily
        public long? NInvWeekly { get; set; } // nInvWeekly
        public long? NInvMonthly { get; set; } // nInvMonthly
        public long? NDealsRec { get; set; } // nDealsRec
        public long? NTransIn { get; set; } // nTransIn
        public long? NTransOut { get; set; } // nTransOut
        public long? NetSales { get; set; } // NetSales
        public long? TotalOrders { get; set; } // TotalOrders
        public long? DineInNetSales { get; set; } // DineInNetSales
        public long? DelNetSales { get; set; } // DelNetSales
        public long? CarryOutNetSales { get; set; } // CarryOutNetSales
        public long? DineInTotalOrders { get; set; } // DineInTotalOrders
        public long? DelTotalOrders { get; set; } // DelTotalOrders
        public long? CarryOutTotalOrders { get; set; } // CarryOutTotalOrders
        public long? TotSalesTax { get; set; } // TotSalesTax
        public long? ManagerDiscounts { get; set; } // ManagerDiscounts
        public long? DealDiscounts { get; set; } // DealDiscounts
        public long? FullMenuPrice { get; set; } // FullMenuPrice
        public long? LabourSeconds { get; set; } // LabourSeconds
        public long? LabourCost { get; set; } // LabourCost
        public long? TotalCancels { get; set; } // TotalCancels
        public long? ValueCancels { get; set; } // valueCancels
        public long? NCloseDailyCount { get; set; } // nCloseDailyCount
        public long? NCloseWeeklyCount { get; set; } // nCloseWeeklyCount
        public long? NCloseMonthlyCount { get; set; } // nCloseMonthlyCount
        public long? NumOrdersLt30Mins { get; set; } // NumOrdersLT30Mins
        public long? NumOrdersLt45Mins { get; set; } // NumOrdersLT45Mins
        public long? AvgMake { get; set; } // AvgMake
        public long? AvgOutTheDoor { get; set; } // AvgOutTheDoor
        public long? AvgDoorTime { get; set; } // AvgDoorTime
        public long? TotCos { get; set; } // TotCOS
        public double? NReceipt { get; set; } // nReceipt
        public long? BottleDeposit { get; set; } // BottleDeposit
        public long? AltBottleDeposit { get; set; } // AltBottleDeposit
        public long? RefundsInCurr { get; set; } // RefundsInCurr
        public long? RefundsQty { get; set; } // RefundsQty
        public long? OnlineNetSales { get; set; } // OnlineNetSales
        public long? OnlineGrossSales { get; set; } // OnlineGrossSales
        public long? OnlineDelNetSales { get; set; } // OnlineDelNetSales
        public long? OnlineColNetSales { get; set; } // OnlineColNetSales
        public long? OnlineDelGrossSales { get; set; } // OnlineDelGrossSales
        public long? OnlineColGrossSales { get; set; } // OnlineColGrossSales
        public long? OnlineTrans { get; set; } // OnlineTrans
        public long? OnlineDelTrans { get; set; } // OnlineDelTrans
        public long? OnlineColTrans { get; set; } // OnlineColTrans
        public long? OnlineTotalTax { get; set; } // OnlineTotalTax
        public long? OnlineDelTax { get; set; } // OnlineDelTax
        public long? OnlineColTax { get; set; } // OnlineColTax
        public long? RackTime { get; set; } // RackTime
    }

}
