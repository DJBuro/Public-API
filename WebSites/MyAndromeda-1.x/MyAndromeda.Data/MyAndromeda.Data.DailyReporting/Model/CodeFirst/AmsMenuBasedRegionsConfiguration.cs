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
    // AMS_MenuBasedRegions
    internal partial class AmsMenuBasedRegionsConfiguration : EntityTypeConfiguration<AmsMenuBasedRegions>
    {
        public AmsMenuBasedRegionsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".AMS_MenuBasedRegions");
            HasKey(x => x.MasterMenuId);

            Property(x => x.MasterMenuId).HasColumnName("MasterMenuID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.RegionName).HasColumnName("RegionName").IsOptional().HasMaxLength(50);
            Property(x => x.AppearanceOrder).HasColumnName("AppearanceOrder").IsOptional();
            Property(x => x.Userdef1).HasColumnName("userdef1").IsOptional().HasMaxLength(100);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
