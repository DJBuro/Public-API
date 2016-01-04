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
    // Groups
    internal class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
            : this("dbo")
        {
        }
 
        public GroupConfiguration(string schema)
        {
            ToTable(schema + ".Groups");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.NUid).HasColumnName("nUID").IsOptional().HasColumnType("int");
            Property(x => x.StrName).HasColumnName("strName").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.NType).HasColumnName("nType").IsOptional().HasColumnType("int");
            Property(x => x.NDisplayFlags).HasColumnName("nDisplayFlags").IsOptional().HasColumnType("int");
            Property(x => x.NCompanyId).HasColumnName("nCompanyId").IsOptional().HasColumnType("int");
            Property(x => x.NHalfHalf).HasColumnName("nHalfHalf").IsOptional().HasColumnType("int");
            Property(x => x.WMenusectionId).HasColumnName("w_menusectionId").IsOptional().HasColumnType("int");
            Property(x => x.StrWebTitle).HasColumnName("strWebTitle").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.NToppingGroup1).HasColumnName("nToppingGroup1").IsOptional().HasColumnType("int");
            Property(x => x.NToppingGroup2).HasColumnName("nToppingGroup2").IsOptional().HasColumnType("int");
            Property(x => x.NToppingGroup3).HasColumnName("nToppingGroup3").IsOptional().HasColumnType("int");
            Property(x => x.NToppingGroup4).HasColumnName("nToppingGroup4").IsOptional().HasColumnType("int");
            Property(x => x.NToppingGroup5).HasColumnName("nToppingGroup5").IsOptional().HasColumnType("int");
            Property(x => x.NForeColor).HasColumnName("nForeColor").IsOptional().HasColumnType("int");
            Property(x => x.NBackColor).HasColumnName("nBackColor").IsOptional().HasColumnType("int");
            Property(x => x.NParentId).HasColumnName("nParentId").IsOptional().HasColumnType("int");
            Property(x => x.NRecipeGroup).HasColumnName("nRecipeGroup").IsOptional().HasColumnType("int");
            Property(x => x.NFlags).HasColumnName("nFlags").IsOptional().HasColumnType("int");
        }
    }

}
