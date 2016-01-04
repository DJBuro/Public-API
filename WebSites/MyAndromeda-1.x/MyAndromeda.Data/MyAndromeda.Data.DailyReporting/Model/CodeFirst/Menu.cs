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
    // menu
    public partial class Menu
    {
        public Guid Recordid { get; set; } // recordid (Primary key)
        public int Autoid { get; set; } // autoid
        public int? Nstoreid { get; set; } // nstoreid
        public int Nuid { get; set; } // nuid
        public string Stritemname { get; set; } // stritemname
        public int? Nsubcat { get; set; } // nsubcat
        public int? Ngroupcat { get; set; } // ngroupcat
        public int? Nprintcat { get; set; } // nprintcat
        public string Strcode { get; set; } // strcode
        public int? Ntimesavailable { get; set; } // ntimesavailable
        public int? Noccasions { get; set; } // noccasions
        public int? Ntype { get; set; } // ntype
        public int? Nflags { get; set; } // nflags
        public int? Ncookingtime { get; set; } // ncookingtime
        public int? Nfreeadditions { get; set; } // nfreeadditions
        public int? Ndisplaypath { get; set; } // ndisplaypath
        public int? Nlinkid { get; set; } // nlinkid
        public int? Nmisc { get; set; } // nmisc
        public int? Ncompanyid { get; set; } // ncompanyid
        public int? Ndisabled { get; set; } // ndisabled
        public string Webname { get; set; } // webname
        public string Webdescription { get; set; } // webdescription
        public int? Webmenusectionid { get; set; } // webmenusectionid
        public int? Websequence { get; set; } // websequence
        public string Strclientname { get; set; } // strclientname
        public string Strclientgroup { get; set; } // strclientgroup

        public Menu()
        {
            Recordid = System.Guid.NewGuid();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
