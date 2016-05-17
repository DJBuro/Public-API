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
    // addresses_old
    internal class AddressesOldConfiguration : EntityTypeConfiguration<AddressesOld>
    {
        public AddressesOldConfiguration()
            : this("dbo")
        {
        }
 
        public AddressesOldConfiguration(string schema)
        {
            ToTable(schema + ".addresses_old");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional().HasColumnType("int");
            Property(x => x.NId).HasColumnName("n_id").IsOptional().HasColumnType("int");
            Property(x => x.Org1).HasColumnName("org1").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Org2).HasColumnName("org2").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Org3).HasColumnName("org3").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Prem1).HasColumnName("prem1").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Prem2).HasColumnName("prem2").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Prem3).HasColumnName("prem3").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Roadnum).HasColumnName("roadnum").IsOptional().HasColumnType("nvarchar").HasMaxLength(96);
            Property(x => x.Roadname).HasColumnName("roadname").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Userlocality).HasColumnName("userlocality").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Locality).HasColumnName("locality").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Town).HasColumnName("town").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.County).HasColumnName("county").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Postcode).HasColumnName("postcode").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.Country).HasColumnName("country").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Grid).HasColumnName("grid").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Refno).HasColumnName("refno").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.StrDirections).HasColumnName("str_directions").IsOptional().HasColumnType("nvarchar").HasMaxLength(320);
            Property(x => x.StrStaffnotes).HasColumnName("str_staffnotes").IsOptional().HasColumnType("nvarchar").HasMaxLength(320);
            Property(x => x.StrDps).HasColumnName("str_dps").IsOptional().HasColumnType("nvarchar").HasMaxLength(4);
            Property(x => x.StrType).HasColumnName("str_type").IsOptional().HasColumnType("nvarchar").HasMaxLength(2);
            Property(x => x.NFlags).HasColumnName("n_flags").IsOptional().HasColumnType("int");
            Property(x => x.NTimesordered).HasColumnName("n_timesordered").IsOptional().HasColumnType("int");
            Property(x => x.Tstamp).HasColumnName("tstamp").IsOptional().HasColumnType("datetime");
            Property(x => x.Dtfirstorder).HasColumnName("dtfirstorder").IsOptional().HasColumnType("datetime");
            Property(x => x.Dtlastorder).HasColumnName("dtlastorder").IsOptional().HasColumnType("datetime");
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired().HasColumnType("datetime");
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired().HasColumnType("datetime");
            Property(x => x.Username).HasColumnName("username").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.Machine).HasColumnName("machine").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.NDistance).HasColumnName("nDistance").IsOptional().HasColumnType("int");
            Property(x => x.Prem4).HasColumnName("Prem4").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Prem5).HasColumnName("Prem5").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Prem6).HasColumnName("Prem6").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.SubLocality).HasColumnName("SubLocality").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.City).HasColumnName("City").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.State).HasColumnName("State").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.SubRoad).HasColumnName("SubRoad").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Userdef1).HasColumnName("userdef1").IsOptional().HasColumnType("bit");
        }
    }

}
