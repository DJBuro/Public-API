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
    // Paytypes
    public partial class Paytypes
    {
        public int Id { get; set; } // Id (Primary key)
        public int Nstoreid { get; set; } // nstoreid (Primary key)
        public int NId { get; set; } // n_id
        public string StrDesc { get; set; } // str_desc
        public int NType { get; set; } // n_type
        public int NFlags { get; set; } // nFlags
    }

}
