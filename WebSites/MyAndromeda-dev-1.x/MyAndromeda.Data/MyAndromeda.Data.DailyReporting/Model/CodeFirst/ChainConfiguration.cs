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
    // Chains
    internal class ChainConfiguration : EntityTypeConfiguration<Chain>
    {
        public ChainConfiguration()
            : this("dbo")
        {
        }
 
        public ChainConfiguration(string schema)
        {
            ToTable(schema + ".Chains");
            HasKey(x => x.ChainId);

            Property(x => x.Name).HasColumnName("Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ParentId).HasColumnName("ParentID").IsOptional().HasColumnType("int");
            Property(x => x.ChainId).HasColumnName("ChainID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Foreign keys
            HasOptional(a => a.Chain_ParentId).WithMany(b => b.Chains).HasForeignKey(c => c.ParentId); // FK_Chains_Chains
        }
    }

}
