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
    // employees
    public class Employee
    {
        public Guid Recordid { get; set; } // recordid
        public int? Nstoreid { get; set; } // nstoreid
        public int? NEmployeecode { get; set; } // n_employeecode
        public int? NPayrollNo { get; set; } // n_PayrollNo
        public string StrEmployeeName { get; set; } // str_EmployeeName
        public int? NSecurityFlags { get; set; } // n_SecurityFlags
        public int? NJobType { get; set; } // n_JobType
        public string NCrossTraining { get; set; } // n_CrossTraining
        public int? NRate1 { get; set; } // n_Rate1
        public int? NRate2 { get; set; } // n_Rate2
        public int? NRate3 { get; set; } // n_Rate3
        public int? NRate4 { get; set; } // n_Rate4
        public string StrNiNum { get; set; } // str_NINum
        public string StrPassWord { get; set; } // str_PassWord
        public DateTime? NLastChange { get; set; } // n_LastChange
        public int? NTargetHours { get; set; } // n_TargetHours
        public int? NWorkTimeOptOut { get; set; } // n_WorkTimeOptOut
        public string StrLicenceNumber { get; set; } // str_LicenceNumber
        public int? NFourWeeklyPaid { get; set; } // n_FourWeeklyPaid
        public int? NCrossTrain { get; set; } // n_CrossTrain
        public string StrScheduleNotes { get; set; } // strScheduleNotes
        public int? NSwipeId { get; set; } // n_SwipeID
        public string StrEmployeeShortName { get; set; } // str_EmployeeShortName
        public string StrDallas { get; set; } // str_Dallas
        
        public Employee()
        {
            Recordid = System.Guid.NewGuid();
        }
    }

}
