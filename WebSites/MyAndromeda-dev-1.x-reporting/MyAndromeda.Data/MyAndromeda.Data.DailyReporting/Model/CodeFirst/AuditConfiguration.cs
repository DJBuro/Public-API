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
    // audit
    internal class AuditConfiguration : EntityTypeConfiguration<Audit>
    {
        public AuditConfiguration()
            : this("dbo")
        {
        }
 
        public AuditConfiguration(string schema)
        {
            ToTable(schema + ".audit");
            HasKey(x => x.AuditId);

            Property(x => x.AuditId).HasColumnName("AuditID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NStoreId).HasColumnName("nStoreID").IsOptional().HasColumnType("int");
            Property(x => x.NRefNum).HasColumnName("n_RefNum").IsOptional().HasColumnType("int");
            Property(x => x.NEventType).HasColumnName("n_EventType").IsOptional().HasColumnType("int");
            Property(x => x.DDate).HasColumnName("d_Date").IsOptional().HasColumnType("datetime");
            Property(x => x.NEmployee).HasColumnName("n_Employee").IsOptional().HasColumnType("int");
            Property(x => x.NOrderNum).HasColumnName("n_OrderNum").IsOptional().HasColumnType("int");
            Property(x => x.NMachineId).HasColumnName("n_MachineID").IsOptional().HasColumnType("int");
            Property(x => x.StrField1).HasColumnName("str_field1").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.StrField2).HasColumnName("str_field2").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.StrField3).HasColumnName("str_field3").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.StrField4).HasColumnName("str_field4").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.NField1).HasColumnName("n_field1").IsOptional().HasColumnType("int");
            Property(x => x.NField2).HasColumnName("n_field2").IsOptional().HasColumnType("int");
            Property(x => x.NField3).HasColumnName("n_field3").IsOptional().HasColumnType("int");
            Property(x => x.NField4).HasColumnName("n_field4").IsOptional().HasColumnType("int");
            Property(x => x.NField5).HasColumnName("n_field5").IsOptional().HasColumnType("int");
            Property(x => x.NField6).HasColumnName("n_field6").IsOptional().HasColumnType("int");
            Property(x => x.DDate1).HasColumnName("d_Date1").IsOptional().HasColumnType("datetime");
            Property(x => x.DDate2).HasColumnName("d_Date2").IsOptional().HasColumnType("datetime");
            Property(x => x.TheDate).HasColumnName("TheDate").IsOptional().HasColumnType("datetime");
        }
    }

}
