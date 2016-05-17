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
    public class sys_ScriptDeployment
    {
        public Guid DeploymentId { get; set; } // deployment_id
        public Guid CoordinatorId { get; set; } // coordinator_id
        public string DeploymentName { get; set; } // deployment_name
        public DateTimeOffset DeploymentSubmitted { get; set; } // deployment_submitted
        public DateTimeOffset? DeploymentStart { get; set; } // deployment_start
        public DateTimeOffset? DeploymentEnd { get; set; } // deployment_end
        public string Status { get; set; } // status
        public string ResultsTable { get; set; } // results_table
        public string RetryPolicy { get; set; } // retry_policy
        public string Script { get; set; } // script
        public string Messages { get; set; } // messages
    }

}
