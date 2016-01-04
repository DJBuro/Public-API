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
    // storecatagorytogroup
    internal class StorecatagorytogroupConfiguration : EntityTypeConfiguration<Storecatagorytogroup>
    {
        public StorecatagorytogroupConfiguration()
            : this("dbo")
        {
        }
 
        public StorecatagorytogroupConfiguration(string schema)
        {
            ToTable(schema + ".storecatagorytogroup");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Mapid).HasColumnName("mapid").IsRequired().HasColumnType("int");
            Property(x => x.Catagoryid).HasColumnName("catagoryid").IsRequired().HasColumnType("int");
            Property(x => x.Groupid).HasColumnName("groupid").IsRequired().HasColumnType("int");
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired().HasColumnType("datetime");
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired().HasColumnType("datetime");
            Property(x => x.Username).HasColumnName("username").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Machine).HasColumnName("machine").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
        }
    }

}
