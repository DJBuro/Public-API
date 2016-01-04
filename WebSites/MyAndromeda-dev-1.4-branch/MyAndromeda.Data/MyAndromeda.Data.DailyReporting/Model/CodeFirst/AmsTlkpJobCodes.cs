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
    // AMS_TLKP_JobCodes
    public partial class AmsTlkpJobCodes
    {
        public int RecordId { get; set; } // RecordID
        public int NStoreId { get; set; } // nStoreID (Primary key)
        public int JobCode { get; set; } // JobCode (Primary key)
        public string JobDescription { get; set; } // JobDescription
        public string JobType { get; set; } // JobType
    }

}
