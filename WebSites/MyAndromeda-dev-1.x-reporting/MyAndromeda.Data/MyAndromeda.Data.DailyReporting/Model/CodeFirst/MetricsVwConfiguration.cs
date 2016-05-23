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
    // metrics_vw
    internal class MetricsVwConfiguration : EntityTypeConfiguration<MetricsVw>
    {
        public MetricsVwConfiguration()
            : this("dbo")
        {
        }
 
        public MetricsVwConfiguration(string schema)
        {
            ToTable(schema + ".metrics_vw");
            HasKey(x => new { x.NStoreId, x.DriveThruCount });

            Property(x => x.NStoreId).HasColumnName("nStoreID").IsRequired().HasColumnType("int");
            Property(x => x.Thehour).HasColumnName("thehour").IsOptional().HasColumnType("nvarchar").HasMaxLength(30);
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional().HasColumnType("date");
            Property(x => x.TotalTime).HasColumnName("Total Time").IsOptional().HasColumnType("int");
            Property(x => x.OrderNet).HasColumnName("OrderNet").IsOptional().HasColumnType("float");
            Property(x => x.OrderVat).HasColumnName("OrderVAT").IsOptional().HasColumnType("float");
            Property(x => x.UnDiscounted).HasColumnName("UnDiscounted").IsOptional().HasColumnType("float");
            Property(x => x.OrderDiscount).HasColumnName("OrderDiscount").IsOptional().HasColumnType("float");
            Property(x => x.OrderGross).HasColumnName("OrderGross").IsOptional().HasColumnType("float");
            Property(x => x.DeliveryCount).HasColumnName("DeliveryCount").IsOptional().HasColumnType("int");
            Property(x => x.CollectionCount).HasColumnName("CollectionCount").IsOptional().HasColumnType("int");
            Property(x => x.DineInCount).HasColumnName("DineInCount").IsOptional().HasColumnType("int");
            Property(x => x.DriveThruCount).HasColumnName("DriveThruCount").IsRequired().HasColumnType("int");
            Property(x => x.TotalOrders).HasColumnName("TotalOrders").IsOptional().HasColumnType("int");
            Property(x => x.FmpOrder).HasColumnName("FMPOrder").IsOptional().HasColumnType("float");
            Property(x => x.Vat).HasColumnName("VAT").IsOptional().HasColumnType("float");
            Property(x => x.OrderPriceNet).HasColumnName("OrderPriceNet").IsOptional().HasColumnType("float");
            Property(x => x.NAvgDoor).HasColumnName("nAvgDoor").IsOptional().HasColumnType("float");
            Property(x => x.NAvgInstore).HasColumnName("nAvgInstore").IsOptional().HasColumnType("float");
            Property(x => x.NAvgRackTime).HasColumnName("nAvgRackTime").IsOptional().HasColumnType("float");
            Property(x => x.NAvgDrive).HasColumnName("nAvgDrive").IsOptional().HasColumnType("float");
            Property(x => x.LessThen15).HasColumnName("LessThen15").IsOptional().HasColumnType("int");
            Property(x => x.Over15LessThan20).HasColumnName("Over15lessThan20").IsOptional().HasColumnType("int");
            Property(x => x.Over20LessThan25).HasColumnName("Over20lessThan25").IsOptional().HasColumnType("int");
            Property(x => x.Over25LessThan30).HasColumnName("Over25lessThan30").IsOptional().HasColumnType("int");
            Property(x => x.Over30LessThan35).HasColumnName("Over30lessThan35").IsOptional().HasColumnType("int");
            Property(x => x.Over35LessThan45).HasColumnName("Over35lessThan45").IsOptional().HasColumnType("int");
            Property(x => x.Over45LessThan60).HasColumnName("Over45lessThan60").IsOptional().HasColumnType("int");
            Property(x => x.Over60).HasColumnName("Over60").IsOptional().HasColumnType("int");
        }
    }

}
