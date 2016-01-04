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
    // OrderDetails3_vw
    internal partial class OrderDetails3VwConfiguration : EntityTypeConfiguration<OrderDetails3Vw>
    {
        public OrderDetails3VwConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".OrderDetails3_vw");
            HasKey(x => new { x.Nstoreid, x.MenuuId, x.ADeal, x.Topping, x.InADeal, x.ToppingRemoved, x.Voided, x.OrderItemType, x.NOrdernum, x.Occasion, x.OrderType, x.DealPrice, x.DealTax });

            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsRequired();
            Property(x => x.DisplayBarName).HasColumnName("DisplayBarName").IsOptional().HasMaxLength(100);
            Property(x => x.PrimaryCatName).HasColumnName("PrimaryCatName").IsOptional().HasMaxLength(100);
            Property(x => x.SecondaryCatName).HasColumnName("SecondaryCatName").IsOptional().HasMaxLength(100);
            Property(x => x.MenuuId).HasColumnName("MenuuID").IsRequired();
            Property(x => x.MenuName).HasColumnName("MenuName").IsOptional().HasMaxLength(64);
            Property(x => x.NAmount).HasColumnName("n_amount").IsOptional();
            Property(x => x.NMenuPrice).HasColumnName("n_MenuPrice").IsOptional();
            Property(x => x.NTaxCharged).HasColumnName("n_TaxCharged").IsOptional();
            Property(x => x.Itempct).HasColumnName("itempct").IsOptional();
            Property(x => x.NOrderdiscount).HasColumnName("n_orderdiscount").IsOptional();
            Property(x => x.NOrderdiscounttype).HasColumnName("n_orderdiscounttype").IsOptional();
            Property(x => x.ADeal).HasColumnName("ADeal").IsRequired();
            Property(x => x.Topping).HasColumnName("Topping").IsRequired();
            Property(x => x.InADeal).HasColumnName("InADeal").IsRequired();
            Property(x => x.ToppingRemoved).HasColumnName("ToppingRemoved").IsRequired();
            Property(x => x.Voided).HasColumnName("Voided").IsRequired();
            Property(x => x.OrderItemType).HasColumnName("OrderItemType").IsRequired().HasMaxLength(20);
            Property(x => x.NType).HasColumnName("n_type").IsOptional();
            Property(x => x.NOrdernum).HasColumnName("n_ordernum").IsRequired();
            Property(x => x.NRelnum).HasColumnName("n_relnum").IsOptional();
            Property(x => x.NOrderpricegross).HasColumnName("n_orderpricegross").IsOptional();
            Property(x => x.NOrderpricenet).HasColumnName("n_orderpricenet").IsOptional();
            Property(x => x.NVatcost).HasColumnName("n_vatcost").IsOptional();
            Property(x => x.NLine).HasColumnName("n_line").IsOptional();
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional();
            Property(x => x.Strstorename).HasColumnName("strstorename").IsOptional().HasMaxLength(100);
            Property(x => x.Occasion).HasColumnName("Occasion").IsRequired().HasMaxLength(10);
            Property(x => x.OrderType).HasColumnName("OrderType").IsRequired().HasMaxLength(8);
            Property(x => x.NChargeprice).HasColumnName("n_Chargeprice").IsOptional();
            Property(x => x.PriceAfterDiscount).HasColumnName("PriceAfterDiscount").IsOptional();
            Property(x => x.NAddressid).HasColumnName("n_addressid").IsOptional();
            Property(x => x.NContactid).HasColumnName("n_contactid").IsOptional();
            Property(x => x.NNameid).HasColumnName("n_nameid").IsOptional();
            Property(x => x.DealPrice).HasColumnName("dealPrice").IsRequired();
            Property(x => x.DealTax).HasColumnName("dealTax").IsRequired();
            Property(x => x.ItemDealPct).HasColumnName("ItemDealPct").IsOptional();
            Property(x => x.ItemDiscountedAmount).HasColumnName("ItemDiscountedAmount").IsOptional().HasPrecision(30,6);
            Property(x => x.ItemDiscountedTax).HasColumnName("ItemDiscountedTax").IsOptional().HasPrecision(38,6);
            Property(x => x.ItemPriceMinusTax).HasColumnName("ItemPriceMinusTax").IsOptional();
            Property(x => x.Hr).HasColumnName("hr").IsOptional();
            Property(x => x.Ngroupcat).HasColumnName("ngroupcat").IsOptional();
            Property(x => x.Family).HasColumnName("Family").IsOptional().HasMaxLength(64);
            Property(x => x.OrderDiscountPct).HasColumnName("OrderDiscountPCT").IsOptional();
            Property(x => x.OrderDiscountAmount).HasColumnName("OrderDiscountAmount").IsOptional();
            Property(x => x.DeliveryCharge).HasColumnName("DeliveryCharge").IsOptional();
            Property(x => x.DeliveryMinusTax).HasColumnName("DeliveryMinusTax").IsOptional();
            Property(x => x.DeliveryChargeTax).HasColumnName("DeliveryChargeTax").IsOptional();
            Property(x => x.SbaDisabled).HasColumnName("SBA_Disabled").IsOptional().HasMaxLength(64);
            Property(x => x.MappingDisabled).HasColumnName("MappingDisabled").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
