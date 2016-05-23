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
    // menu_old
    internal class MenuOldConfiguration : EntityTypeConfiguration<MenuOld>
    {
        public MenuOldConfiguration()
            : this("dbo")
        {
        }
 
        public MenuOldConfiguration(string schema)
        {
            ToTable(schema + ".menu_old");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional().HasColumnType("int");
            Property(x => x.Nuid).HasColumnName("nuid").IsRequired().HasColumnType("int");
            Property(x => x.Stritemname).HasColumnName("stritemname").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.Nsubcat).HasColumnName("nsubcat").IsOptional().HasColumnType("int");
            Property(x => x.Ngroupcat).HasColumnName("ngroupcat").IsOptional().HasColumnType("int");
            Property(x => x.Nprintcat).HasColumnName("nprintcat").IsOptional().HasColumnType("int");
            Property(x => x.Strcode).HasColumnName("strcode").IsOptional().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.Ntimesavailable).HasColumnName("ntimesavailable").IsOptional().HasColumnType("int");
            Property(x => x.Noccasions).HasColumnName("noccasions").IsOptional().HasColumnType("int");
            Property(x => x.Ntype).HasColumnName("ntype").IsOptional().HasColumnType("int");
            Property(x => x.Nflags).HasColumnName("nflags").IsOptional().HasColumnType("int");
            Property(x => x.Ncookingtime).HasColumnName("ncookingtime").IsOptional().HasColumnType("int");
            Property(x => x.Nfreeadditions).HasColumnName("nfreeadditions").IsOptional().HasColumnType("int");
            Property(x => x.Ndisplaypath).HasColumnName("ndisplaypath").IsOptional().HasColumnType("int");
            Property(x => x.Nlinkid).HasColumnName("nlinkid").IsOptional().HasColumnType("int");
            Property(x => x.Nmisc).HasColumnName("nmisc").IsOptional().HasColumnType("int");
            Property(x => x.Ncompanyid).HasColumnName("ncompanyid").IsOptional().HasColumnType("int");
            Property(x => x.Ndisabled).HasColumnName("ndisabled").IsOptional().HasColumnType("int");
            Property(x => x.Webname).HasColumnName("webname").IsOptional().HasColumnType("nvarchar").HasMaxLength(96);
            Property(x => x.Webdescription).HasColumnName("webdescription").IsOptional().HasColumnType("nvarchar").HasMaxLength(288);
            Property(x => x.Webmenusectionid).HasColumnName("webmenusectionid").IsOptional().HasColumnType("int");
            Property(x => x.Websequence).HasColumnName("websequence").IsOptional().HasColumnType("int");
            Property(x => x.Strclientname).HasColumnName("strclientname").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.Strclientgroup).HasColumnName("strclientgroup").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
        }
    }

}
