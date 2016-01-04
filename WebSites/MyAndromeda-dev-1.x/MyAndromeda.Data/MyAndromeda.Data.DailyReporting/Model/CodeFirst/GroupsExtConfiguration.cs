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
    // Groups_Ext
    internal class GroupsExtConfiguration : EntityTypeConfiguration<GroupsExt>
    {
        public GroupsExtConfiguration()
            : this("dbo")
        {
        }
 
        public GroupsExtConfiguration(string schema)
        {
            ToTable(schema + ".Groups_Ext");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.UserDef1).HasColumnName("UserDef1").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.UserDef2).HasColumnName("UserDef2").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.UserDef3).HasColumnName("UserDef3").IsOptional().HasColumnType("nvarchar").HasMaxLength(64);
        }
    }

}
