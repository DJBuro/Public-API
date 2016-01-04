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
    // Recipe
    internal partial class RecipeConfiguration : EntityTypeConfiguration<Recipe>
    {
        public RecipeConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Recipe");
            HasKey(x => x.RecipeId);

            Property(x => x.RecipeId).HasColumnName("RecipeID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NStoreId).HasColumnName("nStoreID").IsOptional();
            Property(x => x.NUid).HasColumnName("nUID").IsOptional();
            Property(x => x.StrUom).HasColumnName("str_UOM").IsOptional().HasMaxLength(50);
            Property(x => x.NUom).HasColumnName("n_UOM").IsOptional();
            Property(x => x.NPacks).HasColumnName("nPacks").IsOptional();
            Property(x => x.NPrice).HasColumnName("nPrice").IsOptional();
            Property(x => x.StrName).HasColumnName("strName").IsOptional().HasMaxLength(50);
            Property(x => x.NFlags).HasColumnName("n_Flags").IsOptional();
            Property(x => x.NCompanyId).HasColumnName("nCompanyID").IsOptional();
            Property(x => x.NRecipeGroupId).HasColumnName("nRecipeGroupID").IsOptional();
            Property(x => x.NInvType).HasColumnName("nInvType").IsOptional();
            Property(x => x.NLinkedMenuId).HasColumnName("nLinkedMenuId").IsOptional();
            Property(x => x.NMakeScreenOrder).HasColumnName("nMakeScreenOrder").IsOptional();
            Property(x => x.StrGlobalRecipeId).HasColumnName("strGlobalRecipeID").IsOptional().HasMaxLength(64);
            Property(x => x.NPrepDays).HasColumnName("nPrepDays").IsOptional();
            Property(x => x.NPrepUnit).HasColumnName("nPrepUnit").IsOptional().HasMaxLength(64);
            Property(x => x.StrCaseName).HasColumnName("strCaseName").IsOptional().HasMaxLength(64);
            Property(x => x.NPrepReport).HasColumnName("nPrepReport").IsOptional();
            Property(x => x.N3DayReport).HasColumnName("n3dayReport").IsOptional();
            Property(x => x.NHourReport).HasColumnName("nHourReport").IsOptional();
            Property(x => x.StrSupplier).HasColumnName("strSupplier").IsOptional().HasMaxLength(128);
            Property(x => x.StrSupplierCode).HasColumnName("strSupplierCode").IsOptional().HasMaxLength(64);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
