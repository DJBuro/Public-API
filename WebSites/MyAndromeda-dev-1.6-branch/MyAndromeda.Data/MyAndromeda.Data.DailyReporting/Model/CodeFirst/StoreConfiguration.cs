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
    // stores
    internal class StoreConfiguration : EntityTypeConfiguration<Store>
    {
        public StoreConfiguration()
            : this("dbo")
        {
        }
 
        public StoreConfiguration(string schema)
        {
            ToTable(schema + ".stores");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsRequired().HasColumnType("int");
            Property(x => x.Strstorename).HasColumnName("strstorename").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Strstaticip).HasColumnName("strstaticip").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Strserverip).HasColumnName("strserverip").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Groupid).HasColumnName("groupid").IsOptional().HasColumnType("int");
            Property(x => x.Lastupdate).HasColumnName("lastupdate").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Ftpid).HasColumnName("ftpid").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Ftpuser).HasColumnName("ftpuser").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Ftppass).HasColumnName("ftppass").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Storetype).HasColumnName("storetype").IsRequired().HasColumnType("nvarchar").HasMaxLength(1);
            Property(x => x.Compstore).HasColumnName("compstore").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Menuid).HasColumnName("menuid").IsOptional().HasColumnType("int");
            Property(x => x.Group1).HasColumnName("group1").IsOptional().HasColumnType("int");
            Property(x => x.Group2).HasColumnName("group2").IsOptional().HasColumnType("int");
            Property(x => x.Group3).HasColumnName("group3").IsOptional().HasColumnType("int");
            Property(x => x.Group4).HasColumnName("group4").IsOptional().HasColumnType("int");
            Property(x => x.Group5).HasColumnName("group5").IsOptional().HasColumnType("int");
            Property(x => x.Group6).HasColumnName("group6").IsOptional().HasColumnType("int");
            Property(x => x.Group7).HasColumnName("group7").IsOptional().HasColumnType("int");
            Property(x => x.Companyid).HasColumnName("companyid").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(100);
            Property(x => x.Vtid).HasColumnName("vtid").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(100);
            Property(x => x.Accno).HasColumnName("accno").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(100);
            Property(x => x.Sortcode).HasColumnName("sortcode").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(100);
            Property(x => x.Accname).HasColumnName("accname").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(100);
            Property(x => x.Franchisee).HasColumnName("franchisee").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(100);
            Property(x => x.Menuversion).HasColumnName("menuversion").IsOptional().HasColumnType("int");
            Property(x => x.Menuupdate).HasColumnName("menuupdate").IsOptional().HasColumnType("datetime");
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired().HasColumnType("datetime");
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired().HasColumnType("datetime");
            Property(x => x.Username).HasColumnName("username").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Machine).HasColumnName("machine").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Contactnumber).HasColumnName("contactnumber").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Countrykey).HasColumnName("countrykey").IsOptional().HasColumnType("int");
            Property(x => x.Userdef1).HasColumnName("userdef1").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
        }
    }

}
