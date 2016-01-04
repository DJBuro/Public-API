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
    // credit
    internal partial class CreditConfiguration : EntityTypeConfiguration<Credit>
    {
        public CreditConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".credit");
            HasKey(x => x.CreditId);

            Property(x => x.CreditId).HasColumnName("CreditID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional();
            Property(x => x.TDate).HasColumnName("t_date").IsOptional();
            Property(x => x.StrReason).HasColumnName("str_reason").IsOptional().HasMaxLength(50);
            Property(x => x.AAmount).HasColumnName("a_amount").IsOptional();
            Property(x => x.NAddressid).HasColumnName("n_addressid").IsOptional();
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
