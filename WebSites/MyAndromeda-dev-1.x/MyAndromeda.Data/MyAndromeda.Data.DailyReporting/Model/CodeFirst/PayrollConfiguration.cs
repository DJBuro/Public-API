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
    // payroll
    internal class PayrollConfiguration : EntityTypeConfiguration<Payroll>
    {
        public PayrollConfiguration()
            : this("dbo")
        {
        }
 
        public PayrollConfiguration(string schema)
        {
            ToTable(schema + ".payroll");
            HasKey(x => x.Autoid);

            Property(x => x.Recordid).HasColumnName("recordid").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional().HasColumnType("int");
            Property(x => x.Nempid).HasColumnName("nempid").IsOptional().HasColumnType("int");
            Property(x => x.Npayrollid).HasColumnName("npayrollid").IsOptional().HasColumnType("int");
            Property(x => x.Strshifttype).HasColumnName("strshifttype").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Strempname).HasColumnName("strempname").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Strni).HasColumnName("strni").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.DStart).HasColumnName("d_start").IsOptional().HasColumnType("datetime");
            Property(x => x.DEnd).HasColumnName("d_end").IsOptional().HasColumnType("datetime");
            Property(x => x.NMinspaid).HasColumnName("n_minspaid").IsOptional().HasColumnType("int");
            Property(x => x.Nhourrate).HasColumnName("nhourrate").IsOptional().HasColumnType("int");
            Property(x => x.Nshiftpay).HasColumnName("nshiftpay").IsOptional().HasColumnType("int");
            Property(x => x.Ndelcomm).HasColumnName("ndelcomm").IsOptional().HasColumnType("int");
            Property(x => x.Nshifttotal).HasColumnName("nshifttotal").IsOptional().HasColumnType("int");
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional().HasColumnType("datetime");
            Property(x => x.NType).HasColumnName("n_type").IsOptional().HasColumnType("int");
        }
    }

}
