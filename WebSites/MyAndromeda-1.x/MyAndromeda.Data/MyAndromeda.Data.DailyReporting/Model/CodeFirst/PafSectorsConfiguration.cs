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
    // PAFSectors
    internal partial class PafSectorsConfiguration : EntityTypeConfiguration<PafSectors>
    {
        public PafSectorsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".PAFSectors");
            HasKey(x => x.NId);

            Property(x => x.NId).HasColumnName("nID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NStoreId).HasColumnName("nStoreID").IsRequired();
            Property(x => x.StrSector).HasColumnName("strSector").IsRequired().HasMaxLength(255);
            Property(x => x.NAddresses).HasColumnName("nAddresses").IsRequired();
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired();
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired();
            Property(x => x.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
            Property(x => x.Machine).HasColumnName("machine").IsRequired().HasMaxLength(100);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
