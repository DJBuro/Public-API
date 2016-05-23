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
    // Country
    internal class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        public CountryConfiguration()
            : this("dbo")
        {
        }
 
        public CountryConfiguration(string schema)
        {
            ToTable(schema + ".Country");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CountryName).HasColumnName("CountryName").IsRequired().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.Iso31661Alpha2).HasColumnName("ISO3166_1_alpha_2").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(2);
            Property(x => x.Iso31661Numeric).HasColumnName("ISO3166_1_numeric").IsRequired().HasColumnType("int");
        }
    }

}
