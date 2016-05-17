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
    // Groups
    public class Group
    {
        public Guid Id { get; set; } // id (Primary key)
        public int? NUid { get; set; } // nUID
        public string StrName { get; set; } // strName
        public int? NType { get; set; } // nType
        public int? NDisplayFlags { get; set; } // nDisplayFlags
        public int? NCompanyId { get; set; } // nCompanyId
        public int? NHalfHalf { get; set; } // nHalfHalf
        public int? WMenusectionId { get; set; } // w_menusectionId
        public string StrWebTitle { get; set; } // strWebTitle
        public int? NToppingGroup1 { get; set; } // nToppingGroup1
        public int? NToppingGroup2 { get; set; } // nToppingGroup2
        public int? NToppingGroup3 { get; set; } // nToppingGroup3
        public int? NToppingGroup4 { get; set; } // nToppingGroup4
        public int? NToppingGroup5 { get; set; } // nToppingGroup5
        public int? NForeColor { get; set; } // nForeColor
        public int? NBackColor { get; set; } // nBackColor
        public int? NParentId { get; set; } // nParentId
        public int? NRecipeGroup { get; set; } // nRecipeGroup
        public int? NFlags { get; set; } // nFlags
    }

}
