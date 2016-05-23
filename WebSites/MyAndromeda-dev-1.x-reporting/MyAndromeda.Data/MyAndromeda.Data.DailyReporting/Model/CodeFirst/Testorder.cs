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
    // testorder
    public class Testorder
    {
        public Guid? Recordid { get; set; } // recordid
        public int Nstoreid { get; set; } // nstoreid
        public int NOrdernum { get; set; } // n_ordernum
        public string StrOrderaddressnotes { get; set; } // str_orderaddressnotes
        public int? NTakerid { get; set; } // n_takerid
        public int? NCashierid { get; set; } // n_cashierid
        public int? NMgrid { get; set; } // n_mgrid
        public int? NServerid { get; set; } // n_serverid
        public int? NOrderpricegross { get; set; } // n_orderpricegross
        public int? NOrderpricenet { get; set; } // n_orderpricenet
        public DateTime? NOrderplacedtime { get; set; } // n_orderplacedtime
        public DateTime? NOrdermadetime { get; set; } // n_ordermadetime
        public DateTime? NOrdercuttime { get; set; } // n_ordercuttime
        public DateTime? NOrderdispatchedtime { get; set; } // n_orderdispatchedtime
        public DateTime? NOrdercashedtime { get; set; } // n_ordercashedtime
        public int? NStatusflags { get; set; } // n_statusflags
        public int? NOccasion { get; set; } // n_occasion
        public int? NOrderdiscounttype { get; set; } // n_orderdiscounttype
        public int? NOrderdiscount { get; set; } // n_orderdiscount
        public int? NVatcost { get; set; } // n_vatcost
        public int? NTotaltime { get; set; } // n_totaltime
        public int? NPrintstart { get; set; } // n_printstart
        public int? NCategories { get; set; } // n_categories
        public int? NDrivetime { get; set; } // n_drivetime
        public int? NPaytype { get; set; } // n_paytype
        public int? NEstimatedtotaltime { get; set; } // n_estimatedtotaltime
        public int? NLoyaltydiscount { get; set; } // n_loyaltydiscount
        public int? NPointsredeemed { get; set; } // n_pointsredeemed
        public string StrLoyaltyCard { get; set; } // str_LoyaltyCard
        public int? NPricea { get; set; } // n_pricea
        public int? NPriceb { get; set; } // n_priceb
        public int? NPricec { get; set; } // n_pricec
        public int? NTaxa { get; set; } // n_taxa
        public int? NTaxb { get; set; } // n_taxb
        public int? NTaxc { get; set; } // n_taxc
        public int? NCardid { get; set; } // n_cardid
        public string StrChefnotes { get; set; } // str_chefnotes
        public DateTime? Thedate { get; set; } // thedate
        public int? NRelnum { get; set; } // n_relnum
        public int? NRectime { get; set; } // n_rectime
        public int? NTimetaken { get; set; } // n_timetaken
        public int? NRemotenum { get; set; } // n_remotenum
        public int? NRemoteflags { get; set; } // n_remoteflags
        public int? NStorenum { get; set; } // n_storenum
        public int? NCardstage { get; set; } // n_cardstage
        public string StrAuthcode { get; set; } // str_authcode
        public int? NContactid { get; set; } // n_contactid
        public int? NAddressid { get; set; } // n_addressid
        public int? NNameid { get; set; } // n_nameid
        public DateTime? Tstamp { get; set; } // tstamp
        public int? NDelcharge { get; set; } // n_delcharge
        public int? NFlags { get; set; } // n_flags
        public int? NCardcharge { get; set; } // n_cardcharge
        public int? NOrdercost { get; set; } // n_ordercost
        public int? NOrderpoints { get; set; } // n_orderpoints
        public int? NDelcomm { get; set; } // n_delcomm
        public int? NDistance { get; set; } // n_distance
        public int? NPriceadjust { get; set; } // n_priceadjust
        public int? NWebstatus { get; set; } // n_webstatus
        public string StrReason { get; set; } // str_reason
        public DateTime? NOrderoriginallyplacedtime { get; set; } // n_orderoriginallyplacedtime
        public string StrDatacashid { get; set; } // str_datacashid
        public int? NTillid { get; set; } // n_tillid
        public int? NCovers { get; set; } // n_covers
        public int? NCcflags { get; set; } // n_ccflags
        public int? NOpd { get; set; } // n_opd
        public string StrContact { get; set; } // str_contact
        public string StrName { get; set; } // str_name
        public int? NTotalmenuprice { get; set; } // n_totalmenuprice
        public double? Delchargetax { get; set; } // delchargetax
    }

}
