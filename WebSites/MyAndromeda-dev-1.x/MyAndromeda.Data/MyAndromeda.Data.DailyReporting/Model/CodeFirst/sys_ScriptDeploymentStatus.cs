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
    public class sys_ScriptDeploymentStatus
    {
        public Guid DeploymentId { get; set; } // deployment_id
        public Guid? WorkerId { get; set; } // worker_id
        public string LogicalServer { get; set; } // logical_server
        public string DatabaseName { get; set; } // database_name
        public DateTimeOffset? DeploymentStart { get; set; } // deployment_start
        public DateTimeOffset? DeploymentEnd { get; set; } // deployment_end
        public string Status { get; set; } // status
        public short NumRetries { get; set; } // num_retries
        public string Messages { get; set; } // messages
    }

}
