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
    // recipegroups
    public partial class Recipegroups
    {
        public int RecipeGroupId { get; set; } // RecipeGroupID (Primary key)
        public int? NStoreId { get; set; } // nStoreID
        public int? NUid { get; set; } // nUID
        public string StrName { get; set; } // strName
        public int? NCompanyId { get; set; } // nCompanyID
    }

}
