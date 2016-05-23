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
    public class AmsspUpdateservicesettingsReturnModel
    {
        public Int32 ServiceSettingID { get; set; }
        public Int32 ID { get; set; }
        public DateTime DateStamp { get; set; }
        public Int32 Sleep { get; set; }
        public DateTime NextRun { get; set; }
        public Int32 OverrideSitelist { get; set; }
        public Int32 OverrideFTP { get; set; }
        public Int32 IgnoreImport { get; set; }
        public String LoggingType { get; set; }
        public Int32? LoggingMode { get; set; }
        public DateTime? QuickPoll { get; set; }
    }

}
