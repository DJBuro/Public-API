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
    // recipegroups
    internal class RecipegroupConfiguration : EntityTypeConfiguration<Recipegroup>
    {
        public RecipegroupConfiguration()
            : this("dbo")
        {
        }
 
        public RecipegroupConfiguration(string schema)
        {
            ToTable(schema + ".recipegroups");
            HasKey(x => x.RecipeGroupId);

            Property(x => x.RecipeGroupId).HasColumnName("RecipeGroupID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NStoreId).HasColumnName("nStoreID").IsOptional().HasColumnType("int");
            Property(x => x.NUid).HasColumnName("nUID").IsOptional().HasColumnType("int");
            Property(x => x.StrName).HasColumnName("strName").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.NCompanyId).HasColumnName("nCompanyID").IsOptional().HasColumnType("int");
        }
    }

}
