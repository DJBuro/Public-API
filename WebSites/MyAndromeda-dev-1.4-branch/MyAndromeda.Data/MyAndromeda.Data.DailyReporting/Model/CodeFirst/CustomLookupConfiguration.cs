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
    // CustomLookup
    internal partial class CustomLookupConfiguration : EntityTypeConfiguration<CustomLookup>
    {
        public CustomLookupConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".CustomLookup");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("ID").IsRequired();
            Property(x => x.LookupType).HasColumnName("LookupType").IsOptional().HasMaxLength(32);
            Property(x => x.LookupId).HasColumnName("LookupID").IsRequired();
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(64);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
