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
    // primarycat
    public class Primarycat
    {
        public Guid? Recordid { get; set; } // recordid
        public int Autoid { get; set; } // autoid
        public int? Nstoreid { get; set; } // nstoreid
        public int? Nuid { get; set; } // nuid
        public int? Nmenubarid { get; set; } // nmenubarid
        public string Strname { get; set; } // strname
        public int? Ntimesavailable { get; set; } // ntimesavailable
        public int? Noccasions { get; set; } // noccasions
        public int? Nflags { get; set; } // nflags
        public int? Nicon { get; set; } // nicon
        public int? Ncompanyid { get; set; } // ncompanyid
        
        public Primarycat()
        {
            Recordid = System.Guid.NewGuid();
        }
    }

}
