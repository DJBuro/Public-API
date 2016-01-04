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
    // auditservice
    public class Auditservice
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
