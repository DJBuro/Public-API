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
    // Recipe
    internal class RecipeConfiguration : EntityTypeConfiguration<Recipe>
    {
        public RecipeConfiguration()
            : this("dbo")
        {
        }
 
        public RecipeConfiguration(string schema)
        {
            ToTable(schema + ".Recipe");
            HasKey(x => x.RecipeId);

            Property(x => x.RecipeId).HasColumnName("RecipeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NStoreId).HasColumnName("nStoreID").IsOptional().HasColumnType("int");
            Property(x => x.NUid).HasColumnName("nUID").IsOptional().HasColumnType("int");
            Property(x => x.StrUom).HasColumnName("str_UOM").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.NUom).HasColumnName("n_UOM").IsOptional().HasColumnType("int");
            Property(x => x.NPacks).HasColumnName("nPacks").IsOptional().HasColumnType("int");
            Property(x => x.NPrice).HasColumnName("nPrice").IsOptional().HasColumnType("int");
            Property(x => x.StrName).HasColumnName("strName").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.NFlags).HasColumnName("n_Flags").IsOptional().HasColumnType("int");
            Property(x => x.NCompanyId).HasColumnName("nCompanyID").IsOptional().HasColumnType("int");
            Property(x => x.NRecipeGroupId).HasColumnName("nRecipeGroupID").IsOptional().HasColumnType("int");
            Property(x => x.NInvType).HasColumnName("nInvType").IsOptional().HasColumnType("int");
            Property(x => x.NLinkedMenuId).HasColumnName("nLinkedMenuId").IsOptional().HasColumnType("int");
            Property(x => x.NMakeScreenOrder).HasColumnName("nMakeScreenOrder").IsOptional().HasColumnType("int");
            Property(x => x.StrGlobalRecipeId).HasColumnName("strGlobalRecipeID").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.NPrepDays).HasColumnName("nPrepDays").IsOptional().HasColumnType("int");
            Property(x => x.NPrepUnit).HasColumnName("nPrepUnit").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.StrCaseName).HasColumnName("strCaseName").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.NPrepReport).HasColumnName("nPrepReport").IsOptional().HasColumnType("int");
            Property(x => x.N3DayReport).HasColumnName("n3dayReport").IsOptional().HasColumnType("int");
            Property(x => x.NHourReport).HasColumnName("nHourReport").IsOptional().HasColumnType("int");
            Property(x => x.StrSupplier).HasColumnName("strSupplier").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.StrSupplierCode).HasColumnName("strSupplierCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
        }
    }

}
