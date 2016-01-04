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
    // createorder
    internal partial class CreateorderConfiguration : EntityTypeConfiguration<Createorder>
    {
        public CreateorderConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".createorder");
            HasKey(x => new { x.Nstoreid, x.NOrdernum });

            Property(x => x.Recordid).HasColumnName("recordid").IsOptional();
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.NOrdernum).HasColumnName("n_ordernum").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.StrOrderaddressnotes).HasColumnName("str_orderaddressnotes").IsOptional().HasMaxLength(192);
            Property(x => x.NTakerid).HasColumnName("n_takerid").IsOptional();
            Property(x => x.NCashierid).HasColumnName("n_cashierid").IsOptional();
            Property(x => x.NMgrid).HasColumnName("n_mgrid").IsOptional();
            Property(x => x.NServerid).HasColumnName("n_serverid").IsOptional();
            Property(x => x.NOrderpricegross).HasColumnName("n_orderpricegross").IsOptional();
            Property(x => x.NOrderpricenet).HasColumnName("n_orderpricenet").IsOptional();
            Property(x => x.NOrderplacedtime).HasColumnName("n_orderplacedtime").IsOptional();
            Property(x => x.NOrdermadetime).HasColumnName("n_ordermadetime").IsOptional();
            Property(x => x.NOrdercuttime).HasColumnName("n_ordercuttime").IsOptional();
            Property(x => x.NOrderdispatchedtime).HasColumnName("n_orderdispatchedtime").IsOptional();
            Property(x => x.NOrdercashedtime).HasColumnName("n_ordercashedtime").IsOptional();
            Property(x => x.NStatusflags).HasColumnName("n_statusflags").IsOptional();
            Property(x => x.NOccasion).HasColumnName("n_occasion").IsOptional();
            Property(x => x.NOrderdiscounttype).HasColumnName("n_orderdiscounttype").IsOptional();
            Property(x => x.NOrderdiscount).HasColumnName("n_orderdiscount").IsOptional();
            Property(x => x.NVatcost).HasColumnName("n_vatcost").IsOptional();
            Property(x => x.NTotaltime).HasColumnName("n_totaltime").IsOptional();
            Property(x => x.NPrintstart).HasColumnName("n_printstart").IsOptional();
            Property(x => x.NCategories).HasColumnName("n_categories").IsOptional();
            Property(x => x.NDrivetime).HasColumnName("n_drivetime").IsOptional();
            Property(x => x.NPaytype).HasColumnName("n_paytype").IsOptional();
            Property(x => x.NEstimatedTotalTime).HasColumnName("n_EstimatedTotalTime").IsOptional();
            Property(x => x.NLoyaltyDiscount).HasColumnName("n_LoyaltyDiscount").IsOptional();
            Property(x => x.NPointsRedeemed).HasColumnName("n_PointsRedeemed").IsOptional();
            Property(x => x.StrLoyaltyCard).HasColumnName("str_LoyaltyCard").IsOptional().HasMaxLength(64);
            Property(x => x.NPriceA).HasColumnName("n_PriceA").IsOptional();
            Property(x => x.NPriceB).HasColumnName("n_PriceB").IsOptional();
            Property(x => x.NPriceC).HasColumnName("n_PriceC").IsOptional();
            Property(x => x.NTaxA).HasColumnName("n_TaxA").IsOptional();
            Property(x => x.NTaxB).HasColumnName("n_TaxB").IsOptional();
            Property(x => x.NTaxC).HasColumnName("n_TaxC").IsOptional();
            Property(x => x.NCardid).HasColumnName("n_cardid").IsOptional();
            Property(x => x.StrChefnotes).HasColumnName("str_chefnotes").IsOptional().HasMaxLength(256);
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional();
            Property(x => x.NRelnum).HasColumnName("n_relnum").IsOptional();
            Property(x => x.NRectime).HasColumnName("n_rectime").IsOptional();
            Property(x => x.NTimetaken).HasColumnName("n_timetaken").IsOptional();
            Property(x => x.NRemotenum).HasColumnName("n_remotenum").IsOptional();
            Property(x => x.NRemoteflags).HasColumnName("n_remoteflags").IsOptional();
            Property(x => x.NStorenum).HasColumnName("n_storenum").IsOptional();
            Property(x => x.NCardstage).HasColumnName("n_cardstage").IsOptional();
            Property(x => x.StrAuthcode).HasColumnName("str_authcode").IsOptional().HasMaxLength(100);
            Property(x => x.NContactid).HasColumnName("n_contactid").IsOptional();
            Property(x => x.NAddressid).HasColumnName("n_addressid").IsOptional();
            Property(x => x.NNameid).HasColumnName("n_nameid").IsOptional();
            Property(x => x.Tstamp).HasColumnName("tstamp").IsOptional();
            Property(x => x.NDelcharge).HasColumnName("n_delcharge").IsOptional();
            Property(x => x.NFlags).HasColumnName("n_flags").IsOptional();
            Property(x => x.NCardcharge).HasColumnName("n_cardcharge").IsOptional();
            Property(x => x.NOrdercost).HasColumnName("n_ordercost").IsOptional();
            Property(x => x.NOrderpoints).HasColumnName("n_orderpoints").IsOptional();
            Property(x => x.NDelcomm).HasColumnName("n_delcomm").IsOptional();
            Property(x => x.NDistance).HasColumnName("n_distance").IsOptional();
            Property(x => x.NPriceadjust).HasColumnName("n_priceadjust").IsOptional();
            Property(x => x.NWebstatus).HasColumnName("n_webstatus").IsOptional();
            Property(x => x.StrReason).HasColumnName("str_reason").IsOptional().HasMaxLength(255);
            Property(x => x.NOrderoriginallyplacedtime).HasColumnName("n_orderoriginallyplacedtime").IsOptional();
            Property(x => x.StrDatacashid).HasColumnName("str_datacashid").IsOptional().HasMaxLength(255);
            Property(x => x.NTillid).HasColumnName("n_tillid").IsOptional();
            Property(x => x.NCovers).HasColumnName("n_covers").IsOptional();
            Property(x => x.NCcflags).HasColumnName("n_ccflags").IsOptional();
            Property(x => x.NOpd).HasColumnName("n_opd").IsOptional();
            Property(x => x.StrContact).HasColumnName("str_contact").IsOptional().HasMaxLength(64);
            Property(x => x.StrName).HasColumnName("str_name").IsOptional().HasMaxLength(64);
            Property(x => x.Entrydate).HasColumnName("entrydate").IsOptional();
            Property(x => x.Editdate).HasColumnName("editdate").IsOptional();
            Property(x => x.Username).HasColumnName("username").IsOptional().HasMaxLength(32);
            Property(x => x.Machine).HasColumnName("machine").IsOptional().HasMaxLength(32);
            Property(x => x.NTotalmenuprice).HasColumnName("n_totalmenuprice").IsOptional();
            Property(x => x.Delchargetax).HasColumnName("delchargetax").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
