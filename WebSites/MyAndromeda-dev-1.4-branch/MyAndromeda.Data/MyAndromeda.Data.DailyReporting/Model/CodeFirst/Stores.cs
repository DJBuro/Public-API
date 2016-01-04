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
    // stores
    public partial class Stores
    {
        public Guid Recordid { get; set; } // recordid (Primary key)
        public int Autoid { get; set; } // autoid
        public int Nstoreid { get; set; } // nstoreid
        public string Strstorename { get; set; } // strstorename
        public string Strstaticip { get; set; } // strstaticip
        public string Strserverip { get; set; } // strserverip
        public int? Groupid { get; set; } // groupid
        public string Lastupdate { get; set; } // lastupdate
        public string Ftpid { get; set; } // ftpid
        public string Ftpuser { get; set; } // ftpuser
        public string Ftppass { get; set; } // ftppass
        public string Storetype { get; set; } // storetype
        public string Compstore { get; set; } // compstore
        public int? Menuid { get; set; } // menuid
        public int? Group1 { get; set; } // group1
        public int? Group2 { get; set; } // group2
        public int? Group3 { get; set; } // group3
        public int? Group4 { get; set; } // group4
        public int? Group5 { get; set; } // group5
        public int? Group6 { get; set; } // group6
        public int? Group7 { get; set; } // group7
        public string Companyid { get; set; } // companyid
        public string Vtid { get; set; } // vtid
        public string Accno { get; set; } // accno
        public string Sortcode { get; set; } // sortcode
        public string Accname { get; set; } // accname
        public string Franchisee { get; set; } // franchisee
        public int? Menuversion { get; set; } // menuversion
        public DateTime? Menuupdate { get; set; } // menuupdate
        public DateTime Entrydate { get; set; } // entrydate
        public DateTime Editdate { get; set; } // editdate
        public string Username { get; set; } // username
        public string Machine { get; set; } // machine
        public string Contactnumber { get; set; } // contactnumber
        public int? Countrykey { get; set; } // countrykey
        public string Userdef1 { get; set; } // userdef1
    }

}
