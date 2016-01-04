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
    // storegroups
    public partial class Storegroups
    {
        public Guid Recordid { get; set; } // recordid (Primary key)
        public int Autoid { get; set; } // autoid
        public int Groupid { get; set; } // groupid
        public int? Groupposition { get; set; } // groupposition
        public string Groupcategory { get; set; } // groupcategory
        public string Groupname { get; set; } // groupname
        public DateTime Entrydate { get; set; } // entrydate
        public DateTime Editdate { get; set; } // editdate
        public string Username { get; set; } // username
        public string Machine { get; set; } // machine
        public string Userdef1 { get; set; } // userdef1
        public string Userdef2 { get; set; } // userdef2
    }

}
