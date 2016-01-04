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
    // storecatagorytogroup
    public partial class Storecatagorytogroup
    {
        public Guid Recordid { get; set; } // recordid (Primary key)
        public int Autoid { get; set; } // autoid
        public int Mapid { get; set; } // mapid
        public int Catagoryid { get; set; } // catagoryid
        public int Groupid { get; set; } // groupid
        public DateTime Entrydate { get; set; } // entrydate
        public DateTime Editdate { get; set; } // editdate
        public string Username { get; set; } // username
        public string Machine { get; set; } // machine
    }

}
