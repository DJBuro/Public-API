// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

namespace MyAndromeda.Data.DailyReporting.Model.CodeFirst
{
    // servicesettings
    public partial class Servicesettings
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
