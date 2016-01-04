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
    // menu
    internal partial class MenuConfiguration : EntityTypeConfiguration<Menu>
    {
        public MenuConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".menu");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired();
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional();
            Property(x => x.Nuid).HasColumnName("nuid").IsRequired();
            Property(x => x.Stritemname).HasColumnName("stritemname").IsOptional().HasMaxLength(64);
            Property(x => x.Nsubcat).HasColumnName("nsubcat").IsOptional();
            Property(x => x.Ngroupcat).HasColumnName("ngroupcat").IsOptional();
            Property(x => x.Nprintcat).HasColumnName("nprintcat").IsOptional();
            Property(x => x.Strcode).HasColumnName("strcode").IsOptional().HasMaxLength(10);
            Property(x => x.Ntimesavailable).HasColumnName("ntimesavailable").IsOptional();
            Property(x => x.Noccasions).HasColumnName("noccasions").IsOptional();
            Property(x => x.Ntype).HasColumnName("ntype").IsOptional();
            Property(x => x.Nflags).HasColumnName("nflags").IsOptional();
            Property(x => x.Ncookingtime).HasColumnName("ncookingtime").IsOptional();
            Property(x => x.Nfreeadditions).HasColumnName("nfreeadditions").IsOptional();
            Property(x => x.Ndisplaypath).HasColumnName("ndisplaypath").IsOptional();
            Property(x => x.Nlinkid).HasColumnName("nlinkid").IsOptional();
            Property(x => x.Nmisc).HasColumnName("nmisc").IsOptional();
            Property(x => x.Ncompanyid).HasColumnName("ncompanyid").IsOptional();
            Property(x => x.Ndisabled).HasColumnName("ndisabled").IsOptional();
            Property(x => x.Webname).HasColumnName("webname").IsOptional().HasMaxLength(96);
            Property(x => x.Webdescription).HasColumnName("webdescription").IsOptional().HasMaxLength(288);
            Property(x => x.Webmenusectionid).HasColumnName("webmenusectionid").IsOptional();
            Property(x => x.Websequence).HasColumnName("websequence").IsOptional();
            Property(x => x.Strclientname).HasColumnName("strclientname").IsOptional().HasMaxLength(64);
            Property(x => x.Strclientgroup).HasColumnName("strclientgroup").IsOptional().HasMaxLength(64);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
