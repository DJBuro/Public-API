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
    // servicesettings
    internal class ServicesettingConfiguration : EntityTypeConfiguration<Servicesetting>
    {
        public ServicesettingConfiguration()
            : this("dbo")
        {
        }
 
        public ServicesettingConfiguration(string schema)
        {
            ToTable(schema + ".servicesettings");
            HasKey(x => x.ServiceSettingId);

            Property(x => x.ServiceSettingId).HasColumnName("ServiceSettingID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Id).HasColumnName("ID").IsRequired().HasColumnType("int");
            Property(x => x.DateStamp).HasColumnName("DateStamp").IsRequired().HasColumnType("smalldatetime");
            Property(x => x.Sleep).HasColumnName("Sleep").IsRequired().HasColumnType("int");
            Property(x => x.NextRun).HasColumnName("NextRun").IsRequired().HasColumnType("smalldatetime");
            Property(x => x.OverrideSitelist).HasColumnName("OverrideSitelist").IsRequired().HasColumnType("int");
            Property(x => x.OverrideFtp).HasColumnName("OverrideFTP").IsRequired().HasColumnType("int");
            Property(x => x.IgnoreImport).HasColumnName("IgnoreImport").IsRequired().HasColumnType("int");
            Property(x => x.LoggingType).HasColumnName("LoggingType").IsOptional().HasColumnType("nvarchar").HasMaxLength(25);
            Property(x => x.LoggingMode).HasColumnName("LoggingMode").IsOptional().HasColumnType("int");
            Property(x => x.QuickPoll).HasColumnName("QuickPoll").IsOptional().HasColumnType("smalldatetime");
        }
    }

}
