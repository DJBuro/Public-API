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
    // employees
    internal partial class EmployeesConfiguration : EntityTypeConfiguration<Employees>
    {
        public EmployeesConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".employees");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired();
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsOptional();
            Property(x => x.NEmployeecode).HasColumnName("n_employeecode").IsOptional();
            Property(x => x.NPayrollNo).HasColumnName("n_PayrollNo").IsOptional();
            Property(x => x.StrEmployeeName).HasColumnName("str_EmployeeName").IsOptional().HasMaxLength(50);
            Property(x => x.NSecurityFlags).HasColumnName("n_SecurityFlags").IsOptional();
            Property(x => x.NJobType).HasColumnName("n_JobType").IsOptional();
            Property(x => x.NCrossTraining).HasColumnName("n_CrossTraining").IsOptional().HasMaxLength(1073741823);
            Property(x => x.NRate1).HasColumnName("n_Rate1").IsOptional();
            Property(x => x.NRate2).HasColumnName("n_Rate2").IsOptional();
            Property(x => x.NRate3).HasColumnName("n_Rate3").IsOptional();
            Property(x => x.NRate4).HasColumnName("n_Rate4").IsOptional();
            Property(x => x.StrNiNum).HasColumnName("str_NINum").IsOptional().HasMaxLength(50);
            Property(x => x.StrPassWord).HasColumnName("str_PassWord").IsOptional().HasMaxLength(50);
            Property(x => x.NLastChange).HasColumnName("n_LastChange").IsOptional();
            Property(x => x.NTargetHours).HasColumnName("n_TargetHours").IsOptional();
            Property(x => x.NWorkTimeOptOut).HasColumnName("n_WorkTimeOptOut").IsOptional();
            Property(x => x.StrLicenceNumber).HasColumnName("str_LicenceNumber").IsOptional().HasMaxLength(50);
            Property(x => x.NFourWeeklyPaid).HasColumnName("n_FourWeeklyPaid").IsOptional();
            Property(x => x.NCrossTrain).HasColumnName("n_CrossTrain").IsOptional();
            Property(x => x.StrScheduleNotes).HasColumnName("strScheduleNotes").IsOptional().HasMaxLength(255);
            Property(x => x.NSwipeId).HasColumnName("n_SwipeID").IsOptional();
            Property(x => x.StrEmployeeShortName).HasColumnName("str_EmployeeShortName").IsOptional().HasMaxLength(50);
            Property(x => x.StrDallas).HasColumnName("str_Dallas").IsOptional().HasMaxLength(255);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
