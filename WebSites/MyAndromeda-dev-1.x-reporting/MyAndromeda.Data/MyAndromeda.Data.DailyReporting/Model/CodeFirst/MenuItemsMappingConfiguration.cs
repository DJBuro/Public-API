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
    // MenuItemsMapping
    internal class MenuItemsMappingConfiguration : EntityTypeConfiguration<MenuItemsMapping>
    {
        public MenuItemsMappingConfiguration()
            : this("dbo")
        {
        }
 
        public MenuItemsMappingConfiguration(string schema)
        {
            ToTable(schema + ".MenuItemsMapping");
            HasKey(x => new { x.Id, x.MenuId, x.OccasionId });

            Property(x => x.Id).HasColumnName("ID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional().HasColumnType("datetime");
            Property(x => x.GroupName).HasColumnName("GroupName").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.SecondaryCat).HasColumnName("secondaryCat").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.ItemType).HasColumnName("itemType").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Disabled).HasColumnName("disabled").IsOptional().HasColumnType("int");
            Property(x => x.ItemName).HasColumnName("itemName").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.MenuId).HasColumnName("menuID").IsRequired().HasColumnType("int");
            Property(x => x.OccasionId).HasColumnName("occasionID").IsRequired().HasColumnType("int");
            Property(x => x.MappedId).HasColumnName("mappedID").IsOptional().HasColumnType("int");
            Property(x => x.UserDef1).HasColumnName("UserDef1").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.UserDef2).HasColumnName("UserDef2").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional().HasColumnType("int");
        }
    }

}
