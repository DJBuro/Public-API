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
    // ramesesaudit
    internal class RamesesauditConfiguration : EntityTypeConfiguration<Ramesesaudit>
    {
        public RamesesauditConfiguration()
            : this("dbo")
        {
        }
 
        public RamesesauditConfiguration(string schema)
        {
            ToTable(schema + ".ramesesaudit");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional().HasColumnType("int");
            Property(x => x.NRefnum).HasColumnName("n_refnum").IsOptional().HasColumnType("int");
            Property(x => x.NEventtype).HasColumnName("n_eventtype").IsOptional().HasColumnType("int");
            Property(x => x.DDate).HasColumnName("d_date").IsOptional().HasColumnType("datetime");
            Property(x => x.NEmployee).HasColumnName("n_employee").IsOptional().HasColumnType("int");
            Property(x => x.NOrdernum).HasColumnName("n_ordernum").IsOptional().HasColumnType("int");
            Property(x => x.NMachineid).HasColumnName("n_machineid").IsOptional().HasColumnType("int");
            Property(x => x.StrField1).HasColumnName("str_field1").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.StrField2).HasColumnName("str_field2").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.StrField3).HasColumnName("str_field3").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.StrField4).HasColumnName("str_field4").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.NField1).HasColumnName("n_field1").IsOptional().HasColumnType("int");
            Property(x => x.NField2).HasColumnName("n_field2").IsOptional().HasColumnType("int");
            Property(x => x.NField3).HasColumnName("n_field3").IsOptional().HasColumnType("int");
            Property(x => x.NField4).HasColumnName("n_field4").IsOptional().HasColumnType("int");
            Property(x => x.NField5).HasColumnName("n_field5").IsOptional().HasColumnType("int");
            Property(x => x.NField6).HasColumnName("n_field6").IsOptional().HasColumnType("int");
            Property(x => x.DDate1).HasColumnName("d_date1").IsOptional().HasColumnType("datetime");
            Property(x => x.DDate2).HasColumnName("d_date2").IsOptional().HasColumnType("datetime");
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional().HasColumnType("datetime");
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired().HasColumnType("datetime");
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired().HasColumnType("datetime");
            Property(x => x.Username).HasColumnName("username").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Machine).HasColumnName("machine").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
        }
    }

}
