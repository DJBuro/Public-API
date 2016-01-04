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
    // audit
    internal partial class AuditConfiguration : EntityTypeConfiguration<Audit>
    {
        public AuditConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".audit");
            HasKey(x => x.AuditId);

            Property(x => x.AuditId).HasColumnName("AuditID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NStoreId).HasColumnName("nStoreID").IsOptional();
            Property(x => x.NRefNum).HasColumnName("n_RefNum").IsOptional();
            Property(x => x.NEventType).HasColumnName("n_EventType").IsOptional();
            Property(x => x.DDate).HasColumnName("d_Date").IsOptional();
            Property(x => x.NEmployee).HasColumnName("n_Employee").IsOptional();
            Property(x => x.NOrderNum).HasColumnName("n_OrderNum").IsOptional();
            Property(x => x.NMachineId).HasColumnName("n_MachineID").IsOptional();
            Property(x => x.StrField1).HasColumnName("str_field1").IsOptional().HasMaxLength(50);
            Property(x => x.StrField2).HasColumnName("str_field2").IsOptional().HasMaxLength(50);
            Property(x => x.StrField3).HasColumnName("str_field3").IsOptional().HasMaxLength(50);
            Property(x => x.StrField4).HasColumnName("str_field4").IsOptional().HasMaxLength(50);
            Property(x => x.NField1).HasColumnName("n_field1").IsOptional();
            Property(x => x.NField2).HasColumnName("n_field2").IsOptional();
            Property(x => x.NField3).HasColumnName("n_field3").IsOptional();
            Property(x => x.NField4).HasColumnName("n_field4").IsOptional();
            Property(x => x.NField5).HasColumnName("n_field5").IsOptional();
            Property(x => x.NField6).HasColumnName("n_field6").IsOptional();
            Property(x => x.DDate1).HasColumnName("d_Date1").IsOptional();
            Property(x => x.DDate2).HasColumnName("d_Date2").IsOptional();
            Property(x => x.TheDate).HasColumnName("TheDate").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
