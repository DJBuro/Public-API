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
    // auditservice
    public partial class Auditservice
    {
        public int Id { get; set; } // id (Primary key)
        public string Recordid { get; set; } // recordid
        public int Nstoreid { get; set; } // nstoreid
        public int? NLess30 { get; set; } // nLess30
        public int? Navginstore { get; set; } // navginstore
        public int? Navgdoor { get; set; } // navgdoor
        public int? Navgmake { get; set; } // navgmake
        public int? Navgdrive { get; set; } // navgdrive
        public DateTime? Thedate { get; set; } // thedate
        public DateTime Entrydate { get; set; } // entrydate
        public DateTime Editdate { get; set; } // editdate
        public string Username { get; set; } // username
        public string Machine { get; set; } // machine
        public string Changetype { get; set; } // changetype
        public DateTime Creationdate { get; set; } // creationdate
    }

}
