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
    // secondarycat
    public class Secondarycat
    {
        public Guid? Recordid { get; set; } // recordid
        public int Autoid { get; set; } // autoid
        public int? Nstoreid { get; set; } // nstoreid
        public int Nuid { get; set; } // nuid
        public int? Nprimarycatid { get; set; } // nprimarycatid
        public string Strname { get; set; } // strname
        public int? Ntimesavailable { get; set; } // ntimesavailable
        public int? Noccasions { get; set; } // noccasions
        public int? Nflags { get; set; } // nflags
        public int? Nicon { get; set; } // nicon
        public int? Ncompanyid { get; set; } // ncompanyid
        public int? Nwebsection { get; set; } // nwebsection
        public int? NOrder { get; set; } // n_order
        public string Strclientgroup { get; set; } // strclientgroup
        public int? NMasterLinkId { get; set; } // nMasterLinkID
        public int? NDealPricePremium { get; set; } // nDealPricePremium
        public int? NHideBaseSize { get; set; } // nHideBaseSize
        public int? NNoPrintBaseSize { get; set; } // nNoPrintBaseSize
        
        public Secondarycat()
        {
            Recordid = System.Guid.NewGuid();
        }
    }

}
