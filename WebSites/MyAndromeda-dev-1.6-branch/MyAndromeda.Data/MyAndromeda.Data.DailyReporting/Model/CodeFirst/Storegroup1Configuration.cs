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
    // storegroups
    internal class Storegroup1Configuration : EntityTypeConfiguration<Storegroup1>
    {
        public Storegroup1Configuration()
            : this("dbo")
        {
        }
 
        public Storegroup1Configuration(string schema)
        {
            ToTable(schema + ".storegroups");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Groupid).HasColumnName("groupid").IsRequired().HasColumnType("int");
            Property(x => x.Groupposition).HasColumnName("groupposition").IsOptional().HasColumnType("int");
            Property(x => x.Groupcategory).HasColumnName("groupcategory").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Groupname).HasColumnName("groupname").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired().HasColumnType("datetime");
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired().HasColumnType("datetime");
            Property(x => x.Username).HasColumnName("username").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Machine).HasColumnName("machine").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Userdef1).HasColumnName("userdef1").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Userdef2).HasColumnName("userdef2").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
        }
    }

}
