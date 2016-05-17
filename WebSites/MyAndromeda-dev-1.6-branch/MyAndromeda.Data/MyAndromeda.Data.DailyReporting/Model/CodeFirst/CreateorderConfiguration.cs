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
    // createorder
    internal class CreateorderConfiguration : EntityTypeConfiguration<Createorder>
    {
        public CreateorderConfiguration()
            : this("dbo")
        {
        }
 
        public CreateorderConfiguration(string schema)
        {
            ToTable(schema + ".createorder");
            HasKey(x => new { x.Recordid, x.Nstoreid, x.NOrdernum });

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsRequired().HasColumnType("int");
            Property(x => x.NOrdernum).HasColumnName("n_ordernum").IsRequired().HasColumnType("int");
            Property(x => x.StrOrderaddressnotes).HasColumnName("str_orderaddressnotes").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.NTakerid).HasColumnName("n_takerid").IsOptional().HasColumnType("int");
            Property(x => x.NCashierid).HasColumnName("n_cashierid").IsOptional().HasColumnType("int");
            Property(x => x.NMgrid).HasColumnName("n_mgrid").IsOptional().HasColumnType("int");
            Property(x => x.NServerid).HasColumnName("n_serverid").IsOptional().HasColumnType("int");
            Property(x => x.NOrderpricegross).HasColumnName("n_orderpricegross").IsOptional().HasColumnType("int");
            Property(x => x.NOrderpricenet).HasColumnName("n_orderpricenet").IsOptional().HasColumnType("int");
            Property(x => x.NOrderplacedtime).HasColumnName("n_orderplacedtime").IsOptional().HasColumnType("datetime");
            Property(x => x.NOrdermadetime).HasColumnName("n_ordermadetime").IsOptional().HasColumnType("datetime");
            Property(x => x.NOrdercuttime).HasColumnName("n_ordercuttime").IsOptional().HasColumnType("datetime");
            Property(x => x.NOrderdispatchedtime).HasColumnName("n_orderdispatchedtime").IsOptional().HasColumnType("datetime");
            Property(x => x.NOrdercashedtime).HasColumnName("n_ordercashedtime").IsOptional().HasColumnType("datetime");
            Property(x => x.NStatusflags).HasColumnName("n_statusflags").IsOptional().HasColumnType("int");
            Property(x => x.NOccasion).HasColumnName("n_occasion").IsOptional().HasColumnType("int");
            Property(x => x.NOrderdiscounttype).HasColumnName("n_orderdiscounttype").IsOptional().HasColumnType("int");
            Property(x => x.NOrderdiscount).HasColumnName("n_orderdiscount").IsOptional().HasColumnType("int");
            Property(x => x.NVatcost).HasColumnName("n_vatcost").IsOptional().HasColumnType("int");
            Property(x => x.NTotaltime).HasColumnName("n_totaltime").IsOptional().HasColumnType("int");
            Property(x => x.NPrintstart).HasColumnName("n_printstart").IsOptional().HasColumnType("int");
            Property(x => x.NCategories).HasColumnName("n_categories").IsOptional().HasColumnType("int");
            Property(x => x.NDrivetime).HasColumnName("n_drivetime").IsOptional().HasColumnType("int");
            Property(x => x.NPaytype).HasColumnName("n_paytype").IsOptional().HasColumnType("int");
            Property(x => x.NEstimatedTotalTime).HasColumnName("n_EstimatedTotalTime").IsOptional().HasColumnType("int");
            Property(x => x.NLoyaltyDiscount).HasColumnName("n_LoyaltyDiscount").IsOptional().HasColumnType("int");
            Property(x => x.NPointsRedeemed).HasColumnName("n_PointsRedeemed").IsOptional().HasColumnType("int");
            Property(x => x.StrLoyaltyCard).HasColumnName("str_LoyaltyCard").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.NPriceA).HasColumnName("n_PriceA").IsOptional().HasColumnType("int");
            Property(x => x.NPriceB).HasColumnName("n_PriceB").IsOptional().HasColumnType("int");
            Property(x => x.NPriceC).HasColumnName("n_PriceC").IsOptional().HasColumnType("int");
            Property(x => x.NTaxA).HasColumnName("n_TaxA").IsOptional().HasColumnType("int");
            Property(x => x.NTaxB).HasColumnName("n_TaxB").IsOptional().HasColumnType("int");
            Property(x => x.NTaxC).HasColumnName("n_TaxC").IsOptional().HasColumnType("int");
            Property(x => x.NCardid).HasColumnName("n_cardid").IsOptional().HasColumnType("int");
            Property(x => x.StrChefnotes).HasColumnName("str_chefnotes").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional().HasColumnType("datetime");
            Property(x => x.NRelnum).HasColumnName("n_relnum").IsOptional().HasColumnType("int");
            Property(x => x.NRectime).HasColumnName("n_rectime").IsOptional().HasColumnType("int");
            Property(x => x.NTimetaken).HasColumnName("n_timetaken").IsOptional().HasColumnType("int");
            Property(x => x.NRemotenum).HasColumnName("n_remotenum").IsOptional().HasColumnType("int");
            Property(x => x.NRemoteflags).HasColumnName("n_remoteflags").IsOptional().HasColumnType("int");
            Property(x => x.NStorenum).HasColumnName("n_storenum").IsOptional().HasColumnType("int");
            Property(x => x.NCardstage).HasColumnName("n_cardstage").IsOptional().HasColumnType("int");
            Property(x => x.StrAuthcode).HasColumnName("str_authcode").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.NContactid).HasColumnName("n_contactid").IsOptional().HasColumnType("int");
            Property(x => x.NAddressid).HasColumnName("n_addressid").IsOptional().HasColumnType("int");
            Property(x => x.NNameid).HasColumnName("n_nameid").IsOptional().HasColumnType("int");
            Property(x => x.Tstamp).HasColumnName("tstamp").IsOptional().HasColumnType("datetime");
            Property(x => x.NDelcharge).HasColumnName("n_delcharge").IsOptional().HasColumnType("int");
            Property(x => x.NFlags).HasColumnName("n_flags").IsOptional().HasColumnType("int");
            Property(x => x.NCardcharge).HasColumnName("n_cardcharge").IsOptional().HasColumnType("int");
            Property(x => x.NOrdercost).HasColumnName("n_ordercost").IsOptional().HasColumnType("int");
            Property(x => x.NOrderpoints).HasColumnName("n_orderpoints").IsOptional().HasColumnType("int");
            Property(x => x.NDelcomm).HasColumnName("n_delcomm").IsOptional().HasColumnType("int");
            Property(x => x.NDistance).HasColumnName("n_distance").IsOptional().HasColumnType("int");
            Property(x => x.NPriceadjust).HasColumnName("n_priceadjust").IsOptional().HasColumnType("int");
            Property(x => x.NWebstatus).HasColumnName("n_webstatus").IsOptional().HasColumnType("int");
            Property(x => x.StrReason).HasColumnName("str_reason").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.NOrderoriginallyplacedtime).HasColumnName("n_orderoriginallyplacedtime").IsOptional().HasColumnType("datetime");
            Property(x => x.StrDatacashid).HasColumnName("str_datacashid").IsOptional().HasColumnType("nvarchar").HasMaxLength(1000);
            Property(x => x.NTillid).HasColumnName("n_tillid").IsOptional().HasColumnType("int");
            Property(x => x.NCovers).HasColumnName("n_covers").IsOptional().HasColumnType("int");
            Property(x => x.NCcflags).HasColumnName("n_ccflags").IsOptional().HasColumnType("int");
            Property(x => x.NOpd).HasColumnName("n_opd").IsOptional().HasColumnType("int");
            Property(x => x.StrContact).HasColumnName("str_contact").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.StrName).HasColumnName("str_name").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.NTotalmenuprice).HasColumnName("n_totalmenuprice").IsOptional().HasColumnType("int");
            Property(x => x.Delchargetax).HasColumnName("delchargetax").IsOptional().HasColumnType("float");
        }
    }

}
