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
    // Paytypes
    internal partial class PaytypesConfiguration : EntityTypeConfiguration<Paytypes>
    {
        public PaytypesConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Paytypes");
            HasKey(x => new { x.Id, x.Nstoreid });

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.NId).HasColumnName("n_id").IsRequired();
            Property(x => x.StrDesc).HasColumnName("str_desc").IsRequired().HasMaxLength(32);
            Property(x => x.NType).HasColumnName("n_type").IsRequired();
            Property(x => x.NFlags).HasColumnName("nFlags").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
