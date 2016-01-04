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
    // metrics_vw
    internal partial class MetricsVwConfiguration : EntityTypeConfiguration<MetricsVw>
    {
        public MetricsVwConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".metrics_vw");
            HasKey(x => new { x.NStoreId, x.DriveThruCount });

            Property(x => x.NStoreId).HasColumnName("nStoreID").IsRequired();
            Property(x => x.Thehour).HasColumnName("thehour").IsOptional().HasMaxLength(30);
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional();
            Property(x => x.TotalTime).HasColumnName("Total Time").IsOptional();
            Property(x => x.OrderNet).HasColumnName("OrderNet").IsOptional();
            Property(x => x.OrderVat).HasColumnName("OrderVAT").IsOptional();
            Property(x => x.UnDiscounted).HasColumnName("UnDiscounted").IsOptional();
            Property(x => x.OrderDiscount).HasColumnName("OrderDiscount").IsOptional();
            Property(x => x.OrderGross).HasColumnName("OrderGross").IsOptional();
            Property(x => x.DeliveryCount).HasColumnName("DeliveryCount").IsOptional();
            Property(x => x.CollectionCount).HasColumnName("CollectionCount").IsOptional();
            Property(x => x.DineInCount).HasColumnName("DineInCount").IsOptional();
            Property(x => x.DriveThruCount).HasColumnName("DriveThruCount").IsRequired();
            Property(x => x.TotalOrders).HasColumnName("TotalOrders").IsOptional();
            Property(x => x.FmpOrder).HasColumnName("FMPOrder").IsOptional();
            Property(x => x.Vat).HasColumnName("VAT").IsOptional();
            Property(x => x.OrderPriceNet).HasColumnName("OrderPriceNet").IsOptional();
            Property(x => x.NAvgDoor).HasColumnName("nAvgDoor").IsOptional();
            Property(x => x.NAvgInstore).HasColumnName("nAvgInstore").IsOptional();
            Property(x => x.NAvgRackTime).HasColumnName("nAvgRackTime").IsOptional();
            Property(x => x.NAvgDrive).HasColumnName("nAvgDrive").IsOptional();
            Property(x => x.LessThen15).HasColumnName("LessThen15").IsOptional();
            Property(x => x.Over15LessThan20).HasColumnName("Over15lessThan20").IsOptional();
            Property(x => x.Over20LessThan25).HasColumnName("Over20lessThan25").IsOptional();
            Property(x => x.Over25LessThan30).HasColumnName("Over25lessThan30").IsOptional();
            Property(x => x.Over30LessThan35).HasColumnName("Over30lessThan35").IsOptional();
            Property(x => x.Over35LessThan45).HasColumnName("Over35lessThan45").IsOptional();
            Property(x => x.Over45LessThan60).HasColumnName("Over45lessThan60").IsOptional();
            Property(x => x.Over60).HasColumnName("Over60").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
