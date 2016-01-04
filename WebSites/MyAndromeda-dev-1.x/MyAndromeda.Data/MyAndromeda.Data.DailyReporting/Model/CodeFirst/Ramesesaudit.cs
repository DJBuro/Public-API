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
    // ramesesaudit
    public class Ramesesaudit
    {
        public Guid Recordid { get; set; } // recordid (Primary key)
        public int Autoid { get; set; } // autoid
        public int? Nstoreid { get; set; } // nstoreid
        public int? NRefnum { get; set; } // n_refnum
        public int? NEventtype { get; set; } // n_eventtype
        public DateTime? DDate { get; set; } // d_date
        public int? NEmployee { get; set; } // n_employee
        public int? NOrdernum { get; set; } // n_ordernum
        public int? NMachineid { get; set; } // n_machineid
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
        public DateTime? DDate1 { get; set; } // d_date1
        public DateTime? DDate2 { get; set; } // d_date2
        public DateTime? Thedate { get; set; } // thedate
        public DateTime Entrydate { get; set; } // entrydate
        public DateTime Editdate { get; set; } // editdate
        public string Username { get; set; } // username
        public string Machine { get; set; } // machine
    }

}
