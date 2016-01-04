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
    public class AmsspGetstoresReturnModel
    {
        public Guid recordid { get; set; }
        public Int32 autoid { get; set; }
        public Int32 nstoreid { get; set; }
        public String strstorename { get; set; }
        public String strstaticip { get; set; }
        public String strserverip { get; set; }
        public Int32? groupid { get; set; }
        public String lastupdate { get; set; }
        public String ftpid { get; set; }
        public String ftpuser { get; set; }
        public String ftppass { get; set; }
        public String storetype { get; set; }
        public String compstore { get; set; }
        public Int32? menuid { get; set; }
        public Int32? group1 { get; set; }
        public Int32? group2 { get; set; }
        public Int32? group3 { get; set; }
        public Int32? group4 { get; set; }
        public Int32? group5 { get; set; }
        public Int32? group6 { get; set; }
        public Int32? group7 { get; set; }
        public String companyid { get; set; }
        public String vtid { get; set; }
        public String accno { get; set; }
        public String sortcode { get; set; }
        public String accname { get; set; }
        public String franchisee { get; set; }
        public Int32? menuversion { get; set; }
        public DateTime? menuupdate { get; set; }
        public DateTime entrydate { get; set; }
        public DateTime editdate { get; set; }
        public String username { get; set; }
        public String machine { get; set; }
        public String contactnumber { get; set; }
        public Int32? countrykey { get; set; }
        public String userdef1 { get; set; }
    }

}
