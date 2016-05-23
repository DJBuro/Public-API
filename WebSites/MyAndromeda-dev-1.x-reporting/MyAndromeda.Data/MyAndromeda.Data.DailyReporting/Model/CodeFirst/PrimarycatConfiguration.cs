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
    // primarycat
    internal class PrimarycatConfiguration : EntityTypeConfiguration<Primarycat>
    {
        public PrimarycatConfiguration()
            : this("dbo")
        {
        }
 
        public PrimarycatConfiguration(string schema)
        {
            ToTable(schema + ".primarycat");
            HasKey(x => x.Autoid);

            Property(x => x.Recordid).HasColumnName("recordid").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional().HasColumnType("int");
            Property(x => x.Nuid).HasColumnName("nuid").IsOptional().HasColumnType("int");
            Property(x => x.Nmenubarid).HasColumnName("nmenubarid").IsOptional().HasColumnType("int");
            Property(x => x.Strname).HasColumnName("strname").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Ntimesavailable).HasColumnName("ntimesavailable").IsOptional().HasColumnType("int");
            Property(x => x.Noccasions).HasColumnName("noccasions").IsOptional().HasColumnType("int");
            Property(x => x.Nflags).HasColumnName("nflags").IsOptional().HasColumnType("int");
            Property(x => x.Nicon).HasColumnName("nicon").IsOptional().HasColumnType("int");
            Property(x => x.Ncompanyid).HasColumnName("ncompanyid").IsOptional().HasColumnType("int");
        }
    }

}
