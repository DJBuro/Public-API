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
    // Recipe
    public partial class Recipe
    {
        public int RecipeId { get; set; } // RecipeID (Primary key)
        public int? NStoreId { get; set; } // nStoreID
        public int? NUid { get; set; } // nUID
        public string StrUom { get; set; } // str_UOM
        public int? NUom { get; set; } // n_UOM
        public int? NPacks { get; set; } // nPacks
        public int? NPrice { get; set; } // nPrice
        public string StrName { get; set; } // strName
        public int? NFlags { get; set; } // n_Flags
        public int? NCompanyId { get; set; } // nCompanyID
        public int? NRecipeGroupId { get; set; } // nRecipeGroupID
        public int? NInvType { get; set; } // nInvType
        public int? NLinkedMenuId { get; set; } // nLinkedMenuId
        public int? NMakeScreenOrder { get; set; } // nMakeScreenOrder
        public string StrGlobalRecipeId { get; set; } // strGlobalRecipeID
        public int? NPrepDays { get; set; } // nPrepDays
        public string NPrepUnit { get; set; } // nPrepUnit
        public string StrCaseName { get; set; } // strCaseName
        public int? NPrepReport { get; set; } // nPrepReport
        public int? N3DayReport { get; set; } // n3dayReport
        public int? NHourReport { get; set; } // nHourReport
        public string StrSupplier { get; set; } // strSupplier
        public string StrSupplierCode { get; set; } // strSupplierCode
    }

}
