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
    // scheduling
    public class Scheduling
    {
        public Guid Recordid { get; set; } // recordid
        public int Autoid { get; set; } // autoid
        public int Nstoreid { get; set; } // nstoreid
        public string StrPayrollno { get; set; } // str_payrollno
        public string StrEmployeename { get; set; } // str_employeename
        public DateTime? DStart { get; set; } // d_start
        public DateTime? DEnd { get; set; } // d_end
        public short? NType { get; set; } // n_type
        public DateTime Thedate { get; set; } // thedate
        public short? NShifttype { get; set; } // n_shifttype
        public int? NPayrate { get; set; } // n_payrate
        public int? NMinspaid { get; set; } // n_minspaid
        public int? NDelcom { get; set; } // n_delcom
        public string StrNotes { get; set; } // str_notes
        public DateTime Entrydate { get; set; } // entrydate
        
        public Scheduling()
        {
            Recordid = System.Guid.NewGuid();
            Entrydate = System.DateTime.Now;
        }
    }

}
