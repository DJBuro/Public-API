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
    // OrderDetails3_vw
    internal class OrderDetails3VwConfiguration : EntityTypeConfiguration<OrderDetails3Vw>
    {
        public OrderDetails3VwConfiguration()
            : this("dbo")
        {
        }
 
        public OrderDetails3VwConfiguration(string schema)
        {
            ToTable(schema + ".OrderDetails3_vw");
            HasKey(x => new { x.Nstoreid, x.MenuuId, x.ADeal, x.Topping, x.InADeal, x.ToppingRemoved, x.Voided, x.OrderItemType, x.NOrdernum, x.Occasion, x.OrderType, x.DealPrice, x.DealTax });

            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsRequired().HasColumnType("int");
            Property(x => x.DisplayBarName).HasColumnName("DisplayBarName").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.PrimaryCatName).HasColumnName("PrimaryCatName").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.SecondaryCatName).HasColumnName("SecondaryCatName").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.MenuuId).HasColumnName("MenuuID").IsRequired().HasColumnType("int");
            Property(x => x.MenuName).HasColumnName("MenuName").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.NAmount).HasColumnName("n_amount").IsOptional().HasColumnType("int");
            Property(x => x.NMenuPrice).HasColumnName("n_MenuPrice").IsOptional().HasColumnType("int");
            Property(x => x.NTaxCharged).HasColumnName("n_TaxCharged").IsOptional().HasColumnType("int");
            Property(x => x.Itempct).HasColumnName("itempct").IsOptional().HasColumnType("int");
            Property(x => x.NOrderdiscount).HasColumnName("n_orderdiscount").IsOptional().HasColumnType("int");
            Property(x => x.NOrderdiscounttype).HasColumnName("n_orderdiscounttype").IsOptional().HasColumnType("int");
            Property(x => x.ADeal).HasColumnName("ADeal").IsRequired().HasColumnType("int");
            Property(x => x.Topping).HasColumnName("Topping").IsRequired().HasColumnType("int");
            Property(x => x.InADeal).HasColumnName("InADeal").IsRequired().HasColumnType("int");
            Property(x => x.ToppingRemoved).HasColumnName("ToppingRemoved").IsRequired().HasColumnType("int");
            Property(x => x.Voided).HasColumnName("Voided").IsRequired().HasColumnType("int");
            Property(x => x.OrderItemType).HasColumnName("OrderItemType").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.NType).HasColumnName("n_type").IsOptional().HasColumnType("int");
            Property(x => x.NOrdernum).HasColumnName("n_ordernum").IsRequired().HasColumnType("int");
            Property(x => x.NRelnum).HasColumnName("n_relnum").IsOptional().HasColumnType("int");
            Property(x => x.NOrderpricegross).HasColumnName("n_orderpricegross").IsOptional().HasColumnType("int");
            Property(x => x.NOrderpricenet).HasColumnName("n_orderpricenet").IsOptional().HasColumnType("int");
            Property(x => x.NVatcost).HasColumnName("n_vatcost").IsOptional().HasColumnType("int");
            Property(x => x.NLine).HasColumnName("n_line").IsOptional().HasColumnType("int");
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional().HasColumnType("datetime");
            Property(x => x.Strstorename).HasColumnName("strstorename").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Occasion).HasColumnName("Occasion").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(10);
            Property(x => x.OrderType).HasColumnName("OrderType").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(8);
            Property(x => x.NChargeprice).HasColumnName("n_Chargeprice").IsOptional().HasColumnType("int");
            Property(x => x.PriceAfterDiscount).HasColumnName("PriceAfterDiscount").IsOptional().HasColumnType("int");
            Property(x => x.NAddressid).HasColumnName("n_addressid").IsOptional().HasColumnType("int");
            Property(x => x.NContactid).HasColumnName("n_contactid").IsOptional().HasColumnType("int");
            Property(x => x.NNameid).HasColumnName("n_nameid").IsOptional().HasColumnType("int");
            Property(x => x.DealPrice).HasColumnName("dealPrice").IsRequired().HasColumnType("int");
            Property(x => x.DealTax).HasColumnName("dealTax").IsRequired().HasColumnType("int");
            Property(x => x.ItemDealPct).HasColumnName("ItemDealPct").IsOptional().HasColumnType("int");
            Property(x => x.ItemDiscountedAmount).HasColumnName("ItemDiscountedAmount").IsOptional().HasColumnType("numeric").HasPrecision(30,6);
            Property(x => x.ItemDiscountedTax).HasColumnName("ItemDiscountedTax").IsOptional().HasColumnType("numeric").HasPrecision(38,6);
            Property(x => x.ItemPriceMinusTax).HasColumnName("ItemPriceMinusTax").IsOptional().HasColumnType("float");
            Property(x => x.Hr).HasColumnName("hr").IsOptional().HasColumnType("int");
            Property(x => x.Ngroupcat).HasColumnName("ngroupcat").IsOptional().HasColumnType("int");
            Property(x => x.Family).HasColumnName("Family").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.OrderDiscountPct).HasColumnName("OrderDiscountPCT").IsOptional().HasColumnType("float");
            Property(x => x.OrderDiscountAmount).HasColumnName("OrderDiscountAmount").IsOptional().HasColumnType("float");
            Property(x => x.DeliveryCharge).HasColumnName("DeliveryCharge").IsOptional().HasColumnType("int");
            Property(x => x.DeliveryMinusTax).HasColumnName("DeliveryMinusTax").IsOptional().HasColumnType("float");
            Property(x => x.DeliveryChargeTax).HasColumnName("DeliveryChargeTax").IsOptional().HasColumnType("float");
            Property(x => x.SbaDisabled).HasColumnName("SBA_Disabled").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.MappingDisabled).HasColumnName("MappingDisabled").IsOptional().HasColumnType("int");
        }
    }

}
