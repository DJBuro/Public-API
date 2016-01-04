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
    // addresses
    public partial class Addresses
    {
        public Guid Recordid { get; set; } // recordid (Primary key)
        public int Autoid { get; set; } // autoid
        public int? Nstoreid { get; set; } // nstoreid
        public int? NId { get; set; } // n_id
        public string Org1 { get; set; } // org1
        public string Org2 { get; set; } // org2
        public string Org3 { get; set; } // org3
        public string Prem1 { get; set; } // prem1
        public string Prem2 { get; set; } // prem2
        public string Prem3 { get; set; } // prem3
        public string Roadnum { get; set; } // roadnum
        public string Roadname { get; set; } // roadname
        public string Userlocality { get; set; } // userlocality
        public string Locality { get; set; } // locality
        public string Town { get; set; } // town
        public string County { get; set; } // county
        public string Postcode { get; set; } // postcode
        public string Country { get; set; } // country
        public string Grid { get; set; } // grid
        public string Refno { get; set; } // refno
        public string StrDirections { get; set; } // str_directions
        public string StrStaffnotes { get; set; } // str_staffnotes
        public string StrDps { get; set; } // str_dps
        public string StrType { get; set; } // str_type
        public int? NFlags { get; set; } // n_flags
        public int? NTimesordered { get; set; } // n_timesordered
        public DateTime? Tstamp { get; set; } // tstamp
        public DateTime? Dtfirstorder { get; set; } // dtfirstorder
        public DateTime? Dtlastorder { get; set; } // dtlastorder
        public DateTime Entrydate { get; set; } // entrydate
        public DateTime Editdate { get; set; } // editdate
        public string Username { get; set; } // username
        public string Machine { get; set; } // machine
        public int? NDistance { get; set; } // nDistance
        public string Prem4 { get; set; } // Prem4
        public string Prem5 { get; set; } // Prem5
        public string Prem6 { get; set; } // Prem6
        public string SubLocality { get; set; } // SubLocality
        public string City { get; set; } // City
        public string State { get; set; } // State
        public string SubRoad { get; set; } // SubRoad
        public bool? Userdef1 { get; set; } // userdef1

        public Addresses()
        {
            Recordid = System.Guid.NewGuid();
            Entrydate = System.DateTime.Now;
            Editdate = System.DateTime.Now;
            Username = "user_name()";
            Machine = "host_name()";
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
