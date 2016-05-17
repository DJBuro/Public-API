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
    // script_deployment_status
    internal class sys_ScriptDeploymentStatusConfiguration : EntityTypeConfiguration<sys_ScriptDeploymentStatus>
    {
        public sys_ScriptDeploymentStatusConfiguration()
            : this("sys")
        {
        }
 
        public sys_ScriptDeploymentStatusConfiguration(string schema)
        {
            ToTable(schema + ".script_deployment_status");
            HasKey(x => new { x.DeploymentId, x.LogicalServer, x.DatabaseName, x.Status, x.NumRetries });

            Property(x => x.DeploymentId).HasColumnName("deployment_id").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.WorkerId).HasColumnName("worker_id").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.LogicalServer).HasColumnName("logical_server").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.DatabaseName).HasColumnName("database_name").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.DeploymentStart).HasColumnName("deployment_start").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.DeploymentEnd).HasColumnName("deployment_end").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.Status).HasColumnName("status").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.NumRetries).HasColumnName("num_retries").IsRequired().HasColumnType("smallint");
            Property(x => x.Messages).HasColumnName("messages").IsOptional().HasColumnType("nvarchar");
        }
    }

}
