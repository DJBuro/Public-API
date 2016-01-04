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
    // AMS_TLKP_JobCodes
    internal class AmsTlkpJobCodeConfiguration : EntityTypeConfiguration<AmsTlkpJobCode>
    {
        public AmsTlkpJobCodeConfiguration()
            : this("dbo")
        {
        }
 
        public AmsTlkpJobCodeConfiguration(string schema)
        {
            ToTable(schema + ".AMS_TLKP_JobCodes");
            HasKey(x => new { x.NStoreId, x.JobCode });

            Property(x => x.RecordId).HasColumnName("RecordID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NStoreId).HasColumnName("nStoreID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.JobCode).HasColumnName("JobCode").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.JobDescription).HasColumnName("JobDescription").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.JobType).HasColumnName("JobType").IsRequired().HasColumnType("nvarchar").HasMaxLength(64);
        }
    }

}
