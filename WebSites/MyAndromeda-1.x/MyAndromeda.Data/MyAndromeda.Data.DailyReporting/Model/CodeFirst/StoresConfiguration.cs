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
    // stores
    internal partial class StoresConfiguration : EntityTypeConfiguration<Stores>
    {
        public StoresConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".stores");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired();
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsRequired();
            Property(x => x.Strstorename).HasColumnName("strstorename").IsOptional().HasMaxLength(100);
            Property(x => x.Strstaticip).HasColumnName("strstaticip").IsOptional().HasMaxLength(100);
            Property(x => x.Strserverip).HasColumnName("strserverip").IsOptional().HasMaxLength(100);
            Property(x => x.Groupid).HasColumnName("groupid").IsOptional();
            Property(x => x.Lastupdate).HasColumnName("lastupdate").IsOptional().HasMaxLength(100);
            Property(x => x.Ftpid).HasColumnName("ftpid").IsOptional().HasMaxLength(100);
            Property(x => x.Ftpuser).HasColumnName("ftpuser").IsOptional().HasMaxLength(100);
            Property(x => x.Ftppass).HasColumnName("ftppass").IsOptional().HasMaxLength(100);
            Property(x => x.Storetype).HasColumnName("storetype").IsRequired().HasMaxLength(1);
            Property(x => x.Compstore).HasColumnName("compstore").IsOptional().HasMaxLength(100);
            Property(x => x.Menuid).HasColumnName("menuid").IsOptional();
            Property(x => x.Group1).HasColumnName("group1").IsOptional();
            Property(x => x.Group2).HasColumnName("group2").IsOptional();
            Property(x => x.Group3).HasColumnName("group3").IsOptional();
            Property(x => x.Group4).HasColumnName("group4").IsOptional();
            Property(x => x.Group5).HasColumnName("group5").IsOptional();
            Property(x => x.Group6).HasColumnName("group6").IsOptional();
            Property(x => x.Group7).HasColumnName("group7").IsOptional();
            Property(x => x.Companyid).HasColumnName("companyid").IsOptional().HasMaxLength(100);
            Property(x => x.Vtid).HasColumnName("vtid").IsOptional().HasMaxLength(100);
            Property(x => x.Accno).HasColumnName("accno").IsOptional().HasMaxLength(100);
            Property(x => x.Sortcode).HasColumnName("sortcode").IsOptional().HasMaxLength(100);
            Property(x => x.Accname).HasColumnName("accname").IsOptional().HasMaxLength(100);
            Property(x => x.Franchisee).HasColumnName("franchisee").IsOptional().HasMaxLength(100);
            Property(x => x.Menuversion).HasColumnName("menuversion").IsOptional();
            Property(x => x.Menuupdate).HasColumnName("menuupdate").IsOptional();
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired();
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired();
            Property(x => x.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
            Property(x => x.Machine).HasColumnName("machine").IsRequired().HasMaxLength(100);
            Property(x => x.Contactnumber).HasColumnName("contactnumber").IsOptional().HasMaxLength(50);
            Property(x => x.Countrykey).HasColumnName("countrykey").IsOptional();
            Property(x => x.Userdef1).HasColumnName("userdef1").IsOptional().HasMaxLength(100);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
