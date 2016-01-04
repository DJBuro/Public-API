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
    // MenuItemsMapping
    public class MenuItemsMapping
    {
        public int Id { get; set; } // ID
        public DateTime? CreateDate { get; set; } // CreateDate
        public string GroupName { get; set; } // GroupName
        public string SecondaryCat { get; set; } // secondaryCat
        public string ItemType { get; set; } // itemType
        public int? Disabled { get; set; } // disabled
        public string ItemName { get; set; } // itemName
        public int MenuId { get; set; } // menuID
        public int OccasionId { get; set; } // occasionID
        public int? MappedId { get; set; } // mappedID
        public string UserDef1 { get; set; } // UserDef1
        public string UserDef2 { get; set; } // UserDef2
        public int? Nstoreid { get; set; } // nstoreid
        
        public MenuItemsMapping()
        {
            CreateDate = System.DateTime.Now;
            Disabled = 0;
        }
    }

}
