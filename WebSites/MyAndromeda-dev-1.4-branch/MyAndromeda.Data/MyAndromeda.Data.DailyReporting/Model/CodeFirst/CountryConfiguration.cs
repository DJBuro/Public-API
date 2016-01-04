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
    // Country
    internal partial class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        public CountryConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Country");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CountryName).HasColumnName("CountryName").IsRequired().HasMaxLength(64);
            Property(x => x.Iso31661Alpha2).HasColumnName("ISO3166_1_alpha_2").IsRequired().HasMaxLength(2);
            Property(x => x.Iso31661Numeric).HasColumnName("ISO3166_1_numeric").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
