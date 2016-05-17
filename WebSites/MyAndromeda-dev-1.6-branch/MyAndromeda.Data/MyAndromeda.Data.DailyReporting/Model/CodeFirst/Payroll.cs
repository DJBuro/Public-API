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
    // payroll
    public class Payroll
    {
        public Guid? Recordid { get; set; } // recordid
        public int Autoid { get; set; } // autoid
        public int? Nstoreid { get; set; } // nstoreid
        public int? Nempid { get; set; } // nempid
        public int? Npayrollid { get; set; } // npayrollid
        public string Strshifttype { get; set; } // strshifttype
        public string Strempname { get; set; } // strempname
        public string Strni { get; set; } // strni
        public DateTime? DStart { get; set; } // d_start
        public DateTime? DEnd { get; set; } // d_end
        public int? NMinspaid { get; set; } // n_minspaid
        public int? Nhourrate { get; set; } // nhourrate
        public int? Nshiftpay { get; set; } // nshiftpay
        public int? Ndelcomm { get; set; } // ndelcomm
        public int? Nshifttotal { get; set; } // nshifttotal
        public DateTime? Thedate { get; set; } // thedate
        public int? NType { get; set; } // n_type
        
        public Payroll()
        {
            Recordid = System.Guid.NewGuid();
        }
    }

}
