//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAndromeda.Data.DailyReporting.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class payroll
    {
        public Nullable<System.Guid> recordid { get; set; }
        public int autoid { get; set; }
        public Nullable<int> nstoreid { get; set; }
        public Nullable<int> nempid { get; set; }
        public Nullable<int> npayrollid { get; set; }
        public string strshifttype { get; set; }
        public string strempname { get; set; }
        public string strni { get; set; }
        public Nullable<System.DateTime> d_start { get; set; }
        public Nullable<System.DateTime> d_end { get; set; }
        public Nullable<int> n_minspaid { get; set; }
        public Nullable<int> nhourrate { get; set; }
        public Nullable<int> nshiftpay { get; set; }
        public Nullable<int> ndelcomm { get; set; }
        public Nullable<int> nshifttotal { get; set; }
        public Nullable<System.DateTime> thedate { get; set; }
        public Nullable<int> n_type { get; set; }
    }
}
