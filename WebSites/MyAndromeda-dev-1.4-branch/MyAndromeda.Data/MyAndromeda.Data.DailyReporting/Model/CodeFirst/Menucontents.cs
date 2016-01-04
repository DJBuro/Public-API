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
    // menucontents
    public partial class Menucontents
    {
        public int MenuContentsId { get; set; } // MenuContentsID (Primary key)
        public int? NStoreId { get; set; } // nStoreID
        public int? NMenuId { get; set; } // nMenuID
        public int? NRecipeId { get; set; } // nRecipeID
        public int? NAmount { get; set; } // nAmount
    }

}
