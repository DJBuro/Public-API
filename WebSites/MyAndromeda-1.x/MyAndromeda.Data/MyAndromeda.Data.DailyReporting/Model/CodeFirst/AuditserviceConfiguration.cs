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
    // auditservice
    internal partial class AuditserviceConfiguration : EntityTypeConfiguration<Auditservice>
    {
        public AuditserviceConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".auditservice");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Recordid).HasColumnName("recordid").IsRequired().HasMaxLength(100);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsRequired();
            Property(x => x.NLess30).HasColumnName("nLess30").IsOptional();
            Property(x => x.Navginstore).HasColumnName("navginstore").IsOptional();
            Property(x => x.Navgdoor).HasColumnName("navgdoor").IsOptional();
            Property(x => x.Navgmake).HasColumnName("navgmake").IsOptional();
            Property(x => x.Navgdrive).HasColumnName("navgdrive").IsOptional();
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional();
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired();
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired();
            Property(x => x.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
            Property(x => x.Machine).HasColumnName("machine").IsRequired().HasMaxLength(100);
            Property(x => x.Changetype).HasColumnName("changetype").IsRequired().HasMaxLength(1);
            Property(x => x.Creationdate).HasColumnName("creationdate").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
