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
    internal class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
            : this("dbo")
        {
        }
 
        public EmployeeConfiguration(string schema)
        {
            ToTable(schema + ".employees");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional().HasColumnType("int");
            Property(x => x.NEmployeecode).HasColumnName("n_employeecode").IsOptional().HasColumnType("int");
            Property(x => x.NPayrollNo).HasColumnName("n_PayrollNo").IsOptional().HasColumnType("int");
            Property(x => x.StrEmployeeName).HasColumnName("str_EmployeeName").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.NSecurityFlags).HasColumnName("n_SecurityFlags").IsOptional().HasColumnType("int");
            Property(x => x.NJobType).HasColumnName("n_JobType").IsOptional().HasColumnType("int");
            Property(x => x.NCrossTraining).HasColumnName("n_CrossTraining").IsOptional().HasColumnType("ntext").IsMaxLength();
            Property(x => x.NRate1).HasColumnName("n_Rate1").IsOptional().HasColumnType("int");
            Property(x => x.NRate2).HasColumnName("n_Rate2").IsOptional().HasColumnType("int");
            Property(x => x.NRate3).HasColumnName("n_Rate3").IsOptional().HasColumnType("int");
            Property(x => x.NRate4).HasColumnName("n_Rate4").IsOptional().HasColumnType("int");
            Property(x => x.StrNiNum).HasColumnName("str_NINum").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.StrPassWord).HasColumnName("str_PassWord").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.NLastChange).HasColumnName("n_LastChange").IsOptional().HasColumnType("datetime");
            Property(x => x.NTargetHours).HasColumnName("n_TargetHours").IsOptional().HasColumnType("int");
            Property(x => x.NWorkTimeOptOut).HasColumnName("n_WorkTimeOptOut").IsOptional().HasColumnType("int");
            Property(x => x.StrLicenceNumber).HasColumnName("str_LicenceNumber").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.NFourWeeklyPaid).HasColumnName("n_FourWeeklyPaid").IsOptional().HasColumnType("int");
            Property(x => x.NCrossTrain).HasColumnName("n_CrossTrain").IsOptional().HasColumnType("int");
            Property(x => x.StrScheduleNotes).HasColumnName("strScheduleNotes").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.NSwipeId).HasColumnName("n_SwipeID").IsOptional().HasColumnType("int");
            Property(x => x.StrEmployeeShortName).HasColumnName("str_EmployeeShortName").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.StrDallas).HasColumnName("str_Dallas").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
        }
    }

}
