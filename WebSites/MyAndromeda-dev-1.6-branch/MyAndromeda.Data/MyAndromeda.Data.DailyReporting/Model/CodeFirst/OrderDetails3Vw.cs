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
    public class OrderDetails3Vw
    {
        public int Nstoreid { get; set; } // nstoreid
        public string DisplayBarName { get; set; } // DisplayBarName
        public string PrimaryCatName { get; set; } // PrimaryCatName
        public string SecondaryCatName { get; set; } // SecondaryCatName
        public int MenuuId { get; set; } // MenuuID
        public string MenuName { get; set; } // MenuName
        public int? NAmount { get; set; } // n_amount
        public int? NMenuPrice { get; set; } // n_MenuPrice
        public int? NTaxCharged { get; set; } // n_TaxCharged
        public int? Itempct { get; set; } // itempct
        public int? NOrderdiscount { get; set; } // n_orderdiscount
        public int? NOrderdiscounttype { get; set; } // n_orderdiscounttype
        public int ADeal { get; set; } // ADeal
        public int Topping { get; set; } // Topping
        public int InADeal { get; set; } // InADeal
        public int ToppingRemoved { get; set; } // ToppingRemoved
        public int Voided { get; set; } // Voided
        public string OrderItemType { get; set; } // OrderItemType
        public int? NType { get; set; } // n_type
        public int NOrdernum { get; set; } // n_ordernum
        public int? NRelnum { get; set; } // n_relnum
        public int? NOrderpricegross { get; set; } // n_orderpricegross
        public int? NOrderpricenet { get; set; } // n_orderpricenet
        public int? NVatcost { get; set; } // n_vatcost
        public int? NLine { get; set; } // n_line
        public DateTime? Thedate { get; set; } // thedate
        public string Strstorename { get; set; } // strstorename
        public string Occasion { get; set; } // Occasion
        public string OrderType { get; set; } // OrderType
        public int? NChargeprice { get; set; } // n_Chargeprice
        public int? PriceAfterDiscount { get; set; } // PriceAfterDiscount
        public int? NAddressid { get; set; } // n_addressid
        public int? NContactid { get; set; } // n_contactid
        public int? NNameid { get; set; } // n_nameid
        public int DealPrice { get; set; } // dealPrice
        public int DealTax { get; set; } // dealTax
        public int? ItemDealPct { get; set; } // ItemDealPct
        public decimal? ItemDiscountedAmount { get; set; } // ItemDiscountedAmount
        public decimal? ItemDiscountedTax { get; set; } // ItemDiscountedTax
        public double? ItemPriceMinusTax { get; set; } // ItemPriceMinusTax
        public int? Hr { get; set; } // hr
        public int? Ngroupcat { get; set; } // ngroupcat
        public string Family { get; set; } // Family
        public double? OrderDiscountPct { get; set; } // OrderDiscountPCT
        public double? OrderDiscountAmount { get; set; } // OrderDiscountAmount
        public int? DeliveryCharge { get; set; } // DeliveryCharge
        public double? DeliveryMinusTax { get; set; } // DeliveryMinusTax
        public double? DeliveryChargeTax { get; set; } // DeliveryChargeTax
        public string SbaDisabled { get; set; } // SBA_Disabled
        public int? MappingDisabled { get; set; } // MappingDisabled
    }

}
