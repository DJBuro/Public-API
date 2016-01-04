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
    // HourlyServiceMetrics
    public partial class HourlyServiceMetrics
    {
        public int NStoreId { get; set; } // nStoreID (Primary key)
        public int Thehour { get; set; } // thehour (Primary key)
        public DateTime Thedate { get; set; } // thedate (Primary key)
        public int? TotalTime { get; set; } // TotalTime
        public double? OrderNet { get; set; } // OrderNet
        public double? OrderVat { get; set; } // OrderVAT
        public double? UnDiscounted { get; set; } // UnDiscounted
        public double? OrderDiscount { get; set; } // OrderDiscount
        public double? OrderGross { get; set; } // OrderGross
        public int? DeliveryCount { get; set; } // DeliveryCount
        public int? CollectionCount { get; set; } // CollectionCount
        public int? DineInCount { get; set; } // DineInCount
        public int? DriveThruCount { get; set; } // DriveThruCount
        public int? TotalOrders { get; set; } // TotalOrders
        public double? FmpOrder { get; set; } // FMPOrder
        public double? Vat { get; set; } // VAT
        public double? OrderPriceNet { get; set; } // OrderPriceNet
        public double? NAvgDoor { get; set; } // nAvgDoor
        public double? NAvgInstore { get; set; } // nAvgInstore
        public double? NAvgRackTime { get; set; } // nAvgRackTime
        public double? NAvgDrive { get; set; } // nAvgDrive
        public int? LessThen15 { get; set; } // LessThen15
        public int? Over15LessThan20 { get; set; } // Over15lessThan20
        public int? Over20LessThan25 { get; set; } // Over20lessThan25
        public int? Over25LessThan30 { get; set; } // Over25lessThan30
        public int? Over30LessThan35 { get; set; } // Over30lessThan35
        public int? Over35LessThan45 { get; set; } // Over35lessThan45
        public int? Over45LessThan60 { get; set; } // Over45lessThan60
        public int? Over60 { get; set; } // Over60
    }

}
