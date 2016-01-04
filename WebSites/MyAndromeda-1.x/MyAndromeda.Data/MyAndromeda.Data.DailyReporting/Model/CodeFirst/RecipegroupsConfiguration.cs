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
    // recipegroups
    internal partial class RecipegroupsConfiguration : EntityTypeConfiguration<Recipegroups>
    {
        public RecipegroupsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".recipegroups");
            HasKey(x => x.RecipeGroupId);

            Property(x => x.RecipeGroupId).HasColumnName("RecipeGroupID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NStoreId).HasColumnName("nStoreID").IsOptional();
            Property(x => x.NUid).HasColumnName("nUID").IsOptional();
            Property(x => x.StrName).HasColumnName("strName").IsOptional().HasMaxLength(50);
            Property(x => x.NCompanyId).HasColumnName("nCompanyID").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
