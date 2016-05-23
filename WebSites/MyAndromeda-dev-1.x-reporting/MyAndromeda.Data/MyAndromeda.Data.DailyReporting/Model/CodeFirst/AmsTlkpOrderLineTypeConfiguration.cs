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
    // AMS_TLKP_OrderLineTypes
    internal class AmsTlkpOrderLineTypeConfiguration : EntityTypeConfiguration<AmsTlkpOrderLineType>
    {
        public AmsTlkpOrderLineTypeConfiguration()
            : this("dbo")
        {
        }
 
        public AmsTlkpOrderLineTypeConfiguration(string schema)
        {
            ToTable(schema + ".AMS_TLKP_OrderLineTypes");
            HasKey(x => x.OrderLineTypeId);

            Property(x => x.RecordId).HasColumnName("RecordID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.OrderLineTypeId).HasColumnName("OrderLineTypeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ProductType).HasColumnName("ProductType").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
        }
    }

}
