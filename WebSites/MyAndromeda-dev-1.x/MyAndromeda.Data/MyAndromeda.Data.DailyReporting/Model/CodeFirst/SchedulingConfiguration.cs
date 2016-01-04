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
    // scheduling
    internal class SchedulingConfiguration : EntityTypeConfiguration<Scheduling>
    {
        public SchedulingConfiguration()
            : this("dbo")
        {
        }
 
        public SchedulingConfiguration(string schema)
        {
            ToTable(schema + ".scheduling");
            HasKey(x => new { x.Recordid, x.Autoid, x.Nstoreid, x.StrPayrollno, x.Thedate, x.Entrydate });

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsRequired().HasColumnType("int");
            Property(x => x.StrPayrollno).HasColumnName("str_payrollno").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.StrEmployeename).HasColumnName("str_employeename").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.DStart).HasColumnName("d_start").IsOptional().HasColumnType("datetime");
            Property(x => x.DEnd).HasColumnName("d_end").IsOptional().HasColumnType("datetime");
            Property(x => x.NType).HasColumnName("n_type").IsOptional().HasColumnType("smallint");
            Property(x => x.Thedate).HasColumnName("thedate").IsRequired().HasColumnType("smalldatetime");
            Property(x => x.NShifttype).HasColumnName("n_shifttype").IsOptional().HasColumnType("smallint");
            Property(x => x.NPayrate).HasColumnName("n_payrate").IsOptional().HasColumnType("int");
            Property(x => x.NMinspaid).HasColumnName("n_minspaid").IsOptional().HasColumnType("int");
            Property(x => x.NDelcom).HasColumnName("n_delcom").IsOptional().HasColumnType("int");
            Property(x => x.StrNotes).HasColumnName("str_notes").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired().HasColumnType("datetime");
        }
    }

}
