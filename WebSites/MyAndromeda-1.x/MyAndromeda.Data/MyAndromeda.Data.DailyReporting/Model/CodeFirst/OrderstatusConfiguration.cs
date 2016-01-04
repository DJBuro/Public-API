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
    // orderstatus
    internal partial class OrderstatusConfiguration : EntityTypeConfiguration<Orderstatus>
    {
        public OrderstatusConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".orderstatus");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired();
            Property(x => x.Id).HasColumnName("id").IsRequired();
            Property(x => x.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired();
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired();
            Property(x => x.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
            Property(x => x.Machine).HasColumnName("machine").IsRequired().HasMaxLength(100);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
