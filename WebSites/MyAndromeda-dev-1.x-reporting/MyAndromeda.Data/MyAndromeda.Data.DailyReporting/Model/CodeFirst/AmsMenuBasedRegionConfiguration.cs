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
    // AMS_MenuBasedRegions
    internal class AmsMenuBasedRegionConfiguration : EntityTypeConfiguration<AmsMenuBasedRegion>
    {
        public AmsMenuBasedRegionConfiguration()
            : this("dbo")
        {
        }
 
        public AmsMenuBasedRegionConfiguration(string schema)
        {
            ToTable(schema + ".AMS_MenuBasedRegions");
            HasKey(x => x.MasterMenuId);

            Property(x => x.MasterMenuId).HasColumnName("MasterMenuID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.RegionName).HasColumnName("RegionName").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.AppearanceOrder).HasColumnName("AppearanceOrder").IsOptional().HasColumnType("int");
            Property(x => x.Userdef1).HasColumnName("userdef1").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
        }
    }

}
