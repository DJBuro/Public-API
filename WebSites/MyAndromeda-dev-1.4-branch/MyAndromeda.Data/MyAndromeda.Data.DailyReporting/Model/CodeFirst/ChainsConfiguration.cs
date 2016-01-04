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
    // Chains
    internal partial class ChainsConfiguration : EntityTypeConfiguration<Chains>
    {
        public ChainsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Chains");
            HasKey(x => x.ChainId);

            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.ParentId).HasColumnName("ParentID").IsOptional();
            Property(x => x.ChainId).HasColumnName("ChainID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Foreign keys
            HasOptional(a => a.Chains1).WithMany(b => b.Chains2).HasForeignKey(c => c.ParentId); // FK_Chains_Chains
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
