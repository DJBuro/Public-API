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
    // ramesesaudit
    internal partial class RamesesauditConfiguration : EntityTypeConfiguration<Ramesesaudit>
    {
        public RamesesauditConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".ramesesaudit");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired();
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional();
            Property(x => x.NRefnum).HasColumnName("n_refnum").IsOptional();
            Property(x => x.NEventtype).HasColumnName("n_eventtype").IsOptional();
            Property(x => x.DDate).HasColumnName("d_date").IsOptional();
            Property(x => x.NEmployee).HasColumnName("n_employee").IsOptional();
            Property(x => x.NOrdernum).HasColumnName("n_ordernum").IsOptional();
            Property(x => x.NMachineid).HasColumnName("n_machineid").IsOptional();
            Property(x => x.StrField1).HasColumnName("str_field1").IsOptional().HasMaxLength(100);
            Property(x => x.StrField2).HasColumnName("str_field2").IsOptional().HasMaxLength(100);
            Property(x => x.StrField3).HasColumnName("str_field3").IsOptional().HasMaxLength(100);
            Property(x => x.StrField4).HasColumnName("str_field4").IsOptional().HasMaxLength(100);
            Property(x => x.NField1).HasColumnName("n_field1").IsOptional();
            Property(x => x.NField2).HasColumnName("n_field2").IsOptional();
            Property(x => x.NField3).HasColumnName("n_field3").IsOptional();
            Property(x => x.NField4).HasColumnName("n_field4").IsOptional();
            Property(x => x.NField5).HasColumnName("n_field5").IsOptional();
            Property(x => x.NField6).HasColumnName("n_field6").IsOptional();
            Property(x => x.DDate1).HasColumnName("d_date1").IsOptional();
            Property(x => x.DDate2).HasColumnName("d_date2").IsOptional();
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional();
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired();
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired();
            Property(x => x.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
            Property(x => x.Machine).HasColumnName("machine").IsRequired().HasMaxLength(100);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
