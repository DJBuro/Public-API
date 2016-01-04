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
    // credit
    public partial class Credit
    {
        public int CreditId { get; set; } // CreditID (Primary key)
        public int? Nstoreid { get; set; } // nstoreid
        public DateTime? TDate { get; set; } // t_date
        public string StrReason { get; set; } // str_reason
        public int? AAmount { get; set; } // a_amount
        public int? NAddressid { get; set; } // n_addressid
        public DateTime? Thedate { get; set; } // thedate
    }

}
