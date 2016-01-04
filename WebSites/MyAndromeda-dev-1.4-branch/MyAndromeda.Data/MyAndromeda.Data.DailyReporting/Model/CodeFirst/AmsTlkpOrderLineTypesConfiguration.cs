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
    // AMS_TLKP_OrderLineTypes
    internal partial class AmsTlkpOrderLineTypesConfiguration : EntityTypeConfiguration<AmsTlkpOrderLineTypes>
    {
        public AmsTlkpOrderLineTypesConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".AMS_TLKP_OrderLineTypes");
            HasKey(x => x.OrderLineTypeId);

            Property(x => x.RecordId).HasColumnName("RecordID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.OrderLineTypeId).HasColumnName("OrderLineTypeID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ProductType).HasColumnName("ProductType").IsRequired().HasMaxLength(100);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
