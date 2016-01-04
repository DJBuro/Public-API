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
    // addresses
    internal partial class AddressesConfiguration : EntityTypeConfiguration<Addresses>
    {
        public AddressesConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".addresses");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired();
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional();
            Property(x => x.NId).HasColumnName("n_id").IsOptional();
            Property(x => x.Org1).HasColumnName("org1").IsOptional().HasMaxLength(100);
            Property(x => x.Org2).HasColumnName("org2").IsOptional().HasMaxLength(100);
            Property(x => x.Org3).HasColumnName("org3").IsOptional().HasMaxLength(100);
            Property(x => x.Prem1).HasColumnName("prem1").IsOptional().HasMaxLength(100);
            Property(x => x.Prem2).HasColumnName("prem2").IsOptional().HasMaxLength(100);
            Property(x => x.Prem3).HasColumnName("prem3").IsOptional().HasMaxLength(100);
            Property(x => x.Roadnum).HasColumnName("roadnum").IsOptional().HasMaxLength(96);
            Property(x => x.Roadname).HasColumnName("roadname").IsOptional().HasMaxLength(100);
            Property(x => x.Userlocality).HasColumnName("userlocality").IsOptional().HasMaxLength(100);
            Property(x => x.Locality).HasColumnName("locality").IsOptional().HasMaxLength(100);
            Property(x => x.Town).HasColumnName("town").IsOptional().HasMaxLength(100);
            Property(x => x.County).HasColumnName("county").IsOptional().HasMaxLength(100);
            Property(x => x.Postcode).HasColumnName("postcode").IsOptional().HasMaxLength(64);
            Property(x => x.Country).HasColumnName("country").IsOptional().HasMaxLength(100);
            Property(x => x.Grid).HasColumnName("grid").IsOptional().HasMaxLength(100);
            Property(x => x.Refno).HasColumnName("refno").IsOptional().HasMaxLength(100);
            Property(x => x.StrDirections).HasColumnName("str_directions").IsOptional().HasMaxLength(320);
            Property(x => x.StrStaffnotes).HasColumnName("str_staffnotes").IsOptional().HasMaxLength(320);
            Property(x => x.StrDps).HasColumnName("str_dps").IsOptional().HasMaxLength(4);
            Property(x => x.StrType).HasColumnName("str_type").IsOptional().HasMaxLength(2);
            Property(x => x.NFlags).HasColumnName("n_flags").IsOptional();
            Property(x => x.NTimesordered).HasColumnName("n_timesordered").IsOptional();
            Property(x => x.Tstamp).HasColumnName("tstamp").IsOptional();
            Property(x => x.Dtfirstorder).HasColumnName("dtfirstorder").IsOptional();
            Property(x => x.Dtlastorder).HasColumnName("dtlastorder").IsOptional();
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired();
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired();
            Property(x => x.Username).HasColumnName("username").IsOptional().HasMaxLength(64);
            Property(x => x.Machine).HasColumnName("machine").IsOptional().HasMaxLength(64);
            Property(x => x.NDistance).HasColumnName("nDistance").IsOptional();
            Property(x => x.Prem4).HasColumnName("Prem4").IsOptional().HasMaxLength(128);
            Property(x => x.Prem5).HasColumnName("Prem5").IsOptional().HasMaxLength(128);
            Property(x => x.Prem6).HasColumnName("Prem6").IsOptional().HasMaxLength(128);
            Property(x => x.SubLocality).HasColumnName("SubLocality").IsOptional().HasMaxLength(128);
            Property(x => x.City).HasColumnName("City").IsOptional().HasMaxLength(128);
            Property(x => x.State).HasColumnName("State").IsOptional().HasMaxLength(128);
            Property(x => x.SubRoad).HasColumnName("SubRoad").IsOptional().HasMaxLength(128);
            Property(x => x.Userdef1).HasColumnName("userdef1").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
