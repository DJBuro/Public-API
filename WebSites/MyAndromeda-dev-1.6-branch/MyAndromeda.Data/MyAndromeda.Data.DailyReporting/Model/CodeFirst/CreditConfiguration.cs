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
    // credit
    internal class CreditConfiguration : EntityTypeConfiguration<Credit>
    {
        public CreditConfiguration()
            : this("dbo")
        {
        }
 
        public CreditConfiguration(string schema)
        {
            ToTable(schema + ".credit");
            HasKey(x => x.CreditId);

            Property(x => x.CreditId).HasColumnName("CreditID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional().HasColumnType("int");
            Property(x => x.TDate).HasColumnName("t_date").IsOptional().HasColumnType("smalldatetime");
            Property(x => x.StrReason).HasColumnName("str_reason").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.AAmount).HasColumnName("a_amount").IsOptional().HasColumnType("int");
            Property(x => x.NAddressid).HasColumnName("n_addressid").IsOptional().HasColumnType("int");
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional().HasColumnType("smalldatetime");
        }
    }

}
