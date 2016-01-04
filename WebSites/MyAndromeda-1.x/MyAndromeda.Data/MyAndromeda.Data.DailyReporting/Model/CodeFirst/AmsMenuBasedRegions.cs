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
    // AMS_MenuBasedRegions
    public partial class AmsMenuBasedRegions
    {
        public int MasterMenuId { get; set; } // MasterMenuID (Primary key)
        public string RegionName { get; set; } // RegionName
        public int? AppearanceOrder { get; set; } // AppearanceOrder
        public string Userdef1 { get; set; } // userdef1
    }

}
