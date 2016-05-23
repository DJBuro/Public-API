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
    // AMSUploadStatus
    internal class AmsUploadStatuConfiguration : EntityTypeConfiguration<AmsUploadStatu>
    {
        public AmsUploadStatuConfiguration()
            : this("dbo")
        {
        }
 
        public AmsUploadStatuConfiguration(string schema)
        {
            ToTable(schema + ".AMSUploadStatus");
            HasKey(x => new { x.StoreId, x.LastUpdate });

            Property(x => x.StoreId).HasColumnName("StoreID").IsRequired().HasColumnType("int");
            Property(x => x.LastUpdate).HasColumnName("LastUpdate").IsRequired().HasColumnType("datetime");
        }
    }

}
