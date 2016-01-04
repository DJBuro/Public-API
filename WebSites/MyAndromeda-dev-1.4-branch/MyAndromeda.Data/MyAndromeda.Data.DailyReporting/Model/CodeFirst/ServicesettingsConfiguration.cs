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
    // servicesettings
    internal partial class ServicesettingsConfiguration : EntityTypeConfiguration<Servicesettings>
    {
        public ServicesettingsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".servicesettings");
            HasKey(x => x.ServiceSettingId);

            Property(x => x.ServiceSettingId).HasColumnName("ServiceSettingID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Id).HasColumnName("ID").IsRequired();
            Property(x => x.DateStamp).HasColumnName("DateStamp").IsRequired();
            Property(x => x.Sleep).HasColumnName("Sleep").IsRequired();
            Property(x => x.NextRun).HasColumnName("NextRun").IsRequired();
            Property(x => x.OverrideSitelist).HasColumnName("OverrideSitelist").IsRequired();
            Property(x => x.OverrideFtp).HasColumnName("OverrideFTP").IsRequired();
            Property(x => x.IgnoreImport).HasColumnName("IgnoreImport").IsRequired();
            Property(x => x.LoggingType).HasColumnName("LoggingType").IsOptional().HasMaxLength(25);
            Property(x => x.LoggingMode).HasColumnName("LoggingMode").IsOptional();
            Property(x => x.QuickPoll).HasColumnName("QuickPoll").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
