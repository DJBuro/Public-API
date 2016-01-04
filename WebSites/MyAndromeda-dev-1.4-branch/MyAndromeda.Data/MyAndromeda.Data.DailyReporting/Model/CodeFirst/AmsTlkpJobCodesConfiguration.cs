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
    // AMS_TLKP_JobCodes
    internal partial class AmsTlkpJobCodesConfiguration : EntityTypeConfiguration<AmsTlkpJobCodes>
    {
        public AmsTlkpJobCodesConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".AMS_TLKP_JobCodes");
            HasKey(x => new { x.NStoreId, x.JobCode });

            Property(x => x.RecordId).HasColumnName("RecordID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NStoreId).HasColumnName("nStoreID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.JobCode).HasColumnName("JobCode").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.JobDescription).HasColumnName("JobDescription").IsOptional().HasMaxLength(64);
            Property(x => x.JobType).HasColumnName("JobType").IsRequired().HasMaxLength(64);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
