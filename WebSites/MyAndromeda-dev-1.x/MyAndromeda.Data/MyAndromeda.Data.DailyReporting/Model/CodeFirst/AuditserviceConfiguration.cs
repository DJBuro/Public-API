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
    // auditservice
    internal class AuditserviceConfiguration : EntityTypeConfiguration<Auditservice>
    {
        public AuditserviceConfiguration()
            : this("dbo")
        {
        }
 
        public AuditserviceConfiguration(string schema)
        {
            ToTable(schema + ".auditservice");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Recordid).HasColumnName("recordid").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsRequired().HasColumnType("int");
            Property(x => x.NLess30).HasColumnName("nLess30").IsOptional().HasColumnType("int");
            Property(x => x.Navginstore).HasColumnName("navginstore").IsOptional().HasColumnType("int");
            Property(x => x.Navgdoor).HasColumnName("navgdoor").IsOptional().HasColumnType("int");
            Property(x => x.Navgmake).HasColumnName("navgmake").IsOptional().HasColumnType("int");
            Property(x => x.Navgdrive).HasColumnName("navgdrive").IsOptional().HasColumnType("int");
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional().HasColumnType("datetime");
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired().HasColumnType("datetime");
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired().HasColumnType("datetime");
            Property(x => x.Username).HasColumnName("username").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Machine).HasColumnName("machine").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Changetype).HasColumnName("changetype").IsRequired().IsFixedLength().HasColumnType("nchar").HasMaxLength(1);
            Property(x => x.Creationdate).HasColumnName("creationdate").IsRequired().HasColumnType("datetime");
        }
    }

}
