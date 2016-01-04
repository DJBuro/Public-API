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
    // servicesettings
    public class Servicesetting
    {
        public int ServiceSettingId { get; set; } // ServiceSettingID (Primary key)
        public int Id { get; set; } // ID
        public DateTime DateStamp { get; set; } // DateStamp
        public int Sleep { get; set; } // Sleep
        public DateTime NextRun { get; set; } // NextRun
        public int OverrideSitelist { get; set; } // OverrideSitelist
        public int OverrideFtp { get; set; } // OverrideFTP
        public int IgnoreImport { get; set; } // IgnoreImport
        public string LoggingType { get; set; } // LoggingType
        public int? LoggingMode { get; set; } // LoggingMode
        public DateTime? QuickPoll { get; set; } // QuickPoll
    }

}
