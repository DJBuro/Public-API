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
    // Paytypes
    internal class PaytypeConfiguration : EntityTypeConfiguration<Paytype>
    {
        public PaytypeConfiguration()
            : this("dbo")
        {
        }
 
        public PaytypeConfiguration(string schema)
        {
            ToTable(schema + ".Paytypes");
            HasKey(x => new { x.Nstoreid, x.Id });

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.NId).HasColumnName("n_id").IsRequired().HasColumnType("int");
            Property(x => x.StrDesc).HasColumnName("str_desc").IsRequired().HasColumnType("nvarchar").HasMaxLength(32);
            Property(x => x.NType).HasColumnName("n_type").IsRequired().HasColumnType("int");
            Property(x => x.NFlags).HasColumnName("nFlags").IsRequired().HasColumnType("int");
        }
    }

}
