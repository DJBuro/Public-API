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
    public interface IDailyReportingCodeFirstDbContext : IDisposable
    {
        IDbSet<Addresses> Addresses { get; set; } // addresses
        IDbSet<AmsMenuBasedRegions> AmsMenuBasedRegions { get; set; } // AMS_MenuBasedRegions
        IDbSet<AmsTlkpJobCodes> AmsTlkpJobCodes { get; set; } // AMS_TLKP_JobCodes
        IDbSet<AmsTlkpOrderLineTypes> AmsTlkpOrderLineTypes { get; set; } // AMS_TLKP_OrderLineTypes
        IDbSet<Audit> Audit { get; set; } // audit
        IDbSet<Auditservice> Auditservice { get; set; } // auditservice
        IDbSet<Basesize> Basesize { get; set; } // basesize
        IDbSet<Chains> Chains { get; set; } // Chains
        IDbSet<Country> Country { get; set; } // Country
        IDbSet<Createorder> Createorder { get; set; } // createorder
        IDbSet<Credit> Credit { get; set; } // credit
        IDbSet<CustomLookup> CustomLookup { get; set; } // CustomLookup
        IDbSet<DailySummary> DailySummary { get; set; } // DailySummary
        IDbSet<DailysummaryCubeVw> DailysummaryCubeVw { get; set; } // dailysummary_cube_vw
        IDbSet<DailysummaryVw> DailysummaryVw { get; set; } // dailysummary_vw
        IDbSet<Employees> Employees { get; set; } // employees
        IDbSet<Groups> Groups { get; set; } // Groups
        IDbSet<GroupsExt> GroupsExt { get; set; } // Groups_Ext
        IDbSet<HourlyServiceMetrics> HourlyServiceMetrics { get; set; } // HourlyServiceMetrics
        IDbSet<KfcCreateorder> KfcCreateorder { get; set; } // kfc_createorder
        IDbSet<Menu> Menu { get; set; } // menu
        IDbSet<Menucontents> Menucontents { get; set; } // menucontents
        IDbSet<MetricsVw> MetricsVw { get; set; } // metrics_vw
        IDbSet<OrderDetails3Vw> OrderDetails3Vw { get; set; } // OrderDetails3_vw
        IDbSet<Orderstatus> Orderstatus { get; set; } // orderstatus
        IDbSet<Ordertype> Ordertype { get; set; } // ordertype
        IDbSet<OtdVw> OtdVw { get; set; } // OTD_vw
        IDbSet<PafSectors> PafSectors { get; set; } // PAFSectors
        IDbSet<Paytypes> Paytypes { get; set; } // Paytypes
        IDbSet<Ramesesaudit> Ramesesaudit { get; set; } // ramesesaudit
        IDbSet<Recipe> Recipe { get; set; } // Recipe
        IDbSet<Recipegroups> Recipegroups { get; set; } // recipegroups
        IDbSet<Servicesettings> Servicesettings { get; set; } // servicesettings
        IDbSet<Storecatagory> Storecatagory { get; set; } // storecatagory
        IDbSet<Storecatagorytogroup> Storecatagorytogroup { get; set; } // storecatagorytogroup
        IDbSet<Storegroup> Storegroup { get; set; } // storegroup
        IDbSet<Storegroups> Storegroups { get; set; } // storegroups
        IDbSet<Stores> Stores { get; set; } // stores

        int SaveChanges();
    }

}
