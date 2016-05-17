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
    // script_deployments
    internal class sys_ScriptDeploymentConfiguration : EntityTypeConfiguration<sys_ScriptDeployment>
    {
        public sys_ScriptDeploymentConfiguration()
            : this("sys")
        {
        }
 
        public sys_ScriptDeploymentConfiguration(string schema)
        {
            ToTable(schema + ".script_deployments");
            HasKey(x => new { x.DeploymentId, x.CoordinatorId, x.DeploymentName, x.DeploymentSubmitted, x.Status, x.RetryPolicy, x.Script });

            Property(x => x.DeploymentId).HasColumnName("deployment_id").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.CoordinatorId).HasColumnName("coordinator_id").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.DeploymentName).HasColumnName("deployment_name").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.DeploymentSubmitted).HasColumnName("deployment_submitted").IsRequired().HasColumnType("datetimeoffset");
            Property(x => x.DeploymentStart).HasColumnName("deployment_start").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.DeploymentEnd).HasColumnName("deployment_end").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.Status).HasColumnName("status").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ResultsTable).HasColumnName("results_table").IsOptional().HasColumnType("nvarchar").HasMaxLength(261);
            Property(x => x.RetryPolicy).HasColumnName("retry_policy").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Script).HasColumnName("script").IsRequired().HasColumnType("nvarchar");
            Property(x => x.Messages).HasColumnName("messages").IsOptional().HasColumnType("nvarchar");
        }
    }

}
