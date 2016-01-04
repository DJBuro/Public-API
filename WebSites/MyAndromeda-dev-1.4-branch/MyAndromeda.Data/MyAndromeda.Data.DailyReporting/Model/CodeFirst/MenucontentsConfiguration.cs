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
    // menucontents
    internal partial class MenucontentsConfiguration : EntityTypeConfiguration<Menucontents>
    {
        public MenucontentsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".menucontents");
            HasKey(x => x.MenuContentsId);

            Property(x => x.MenuContentsId).HasColumnName("MenuContentsID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NStoreId).HasColumnName("nStoreID").IsOptional();
            Property(x => x.NMenuId).HasColumnName("nMenuID").IsOptional();
            Property(x => x.NRecipeId).HasColumnName("nRecipeID").IsOptional();
            Property(x => x.NAmount).HasColumnName("nAmount").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
