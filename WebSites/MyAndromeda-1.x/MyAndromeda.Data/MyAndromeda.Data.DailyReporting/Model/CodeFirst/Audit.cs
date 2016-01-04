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
    // audit
    public partial class Audit
    {
        public int AuditId { get; set; } // AuditID (Primary key)
        public int? NStoreId { get; set; } // nStoreID
        public int? NRefNum { get; set; } // n_RefNum
        public int? NEventType { get; set; } // n_EventType
        public DateTime? DDate { get; set; } // d_Date
        public int? NEmployee { get; set; } // n_Employee
        public int? NOrderNum { get; set; } // n_OrderNum
        public int? NMachineId { get; set; } // n_MachineID
        public string StrField1 { get; set; } // str_field1
        public string StrField2 { get; set; } // str_field2
        public string StrField3 { get; set; } // str_field3
        public string StrField4 { get; set; } // str_field4
        public int? NField1 { get; set; } // n_field1
        public int? NField2 { get; set; } // n_field2
        public int? NField3 { get; set; } // n_field3
        public int? NField4 { get; set; } // n_field4
        public int? NField5 { get; set; } // n_field5
        public int? NField6 { get; set; } // n_field6
        public DateTime? DDate1 { get; set; } // d_Date1
        public DateTime? DDate2 { get; set; } // d_Date2
        public DateTime? TheDate { get; set; } // TheDate
    }

}
