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
    // PollingMonitor_Translation
    internal class PollingMonitorTranslationConfiguration : EntityTypeConfiguration<PollingMonitorTranslation>
    {
        public PollingMonitorTranslationConfiguration()
            : this("dbo")
        {
        }
 
        public PollingMonitorTranslationConfiguration(string schema)
        {
            ToTable(schema + ".PollingMonitor_Translation");
            HasKey(x => x.LangId);

            Property(x => x.LangId).HasColumnName("LangID").IsRequired().HasColumnType("int");
            Property(x => x.TranslationKey).HasColumnName("TranslationKey").IsOptional().HasColumnType("nvarchar");
            Property(x => x.TranslationValue).HasColumnName("TranslationValue").IsOptional().HasColumnType("nvarchar");
        }
    }

}
