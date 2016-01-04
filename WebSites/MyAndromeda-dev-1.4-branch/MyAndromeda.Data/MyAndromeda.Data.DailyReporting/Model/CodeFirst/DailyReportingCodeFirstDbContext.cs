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
    public partial class DailyReportingCodeFirstDbContext : DbContext, IDailyReportingCodeFirstDbContext
    {
        public IDbSet<Addresses> Addresses { get; set; } // addresses
        public IDbSet<AmsMenuBasedRegions> AmsMenuBasedRegions { get; set; } // AMS_MenuBasedRegions
        public IDbSet<AmsTlkpJobCodes> AmsTlkpJobCodes { get; set; } // AMS_TLKP_JobCodes
        public IDbSet<AmsTlkpOrderLineTypes> AmsTlkpOrderLineTypes { get; set; } // AMS_TLKP_OrderLineTypes
        public IDbSet<Audit> Audit { get; set; } // audit
        public IDbSet<Auditservice> Auditservice { get; set; } // auditservice
        public IDbSet<Basesize> Basesize { get; set; } // basesize
        public IDbSet<Chains> Chains { get; set; } // Chains
        public IDbSet<Country> Country { get; set; } // Country
        public IDbSet<Createorder> Createorder { get; set; } // createorder
        public IDbSet<Credit> Credit { get; set; } // credit
        public IDbSet<CustomLookup> CustomLookup { get; set; } // CustomLookup
        public IDbSet<DailySummary> DailySummary { get; set; } // DailySummary
        public IDbSet<DailysummaryCubeVw> DailysummaryCubeVw { get; set; } // dailysummary_cube_vw
        public IDbSet<DailysummaryVw> DailysummaryVw { get; set; } // dailysummary_vw
        public IDbSet<Employees> Employees { get; set; } // employees
        public IDbSet<Groups> Groups { get; set; } // Groups
        public IDbSet<GroupsExt> GroupsExt { get; set; } // Groups_Ext
        public IDbSet<HourlyServiceMetrics> HourlyServiceMetrics { get; set; } // HourlyServiceMetrics
        public IDbSet<KfcCreateorder> KfcCreateorder { get; set; } // kfc_createorder
        public IDbSet<Menu> Menu { get; set; } // menu
        public IDbSet<Menucontents> Menucontents { get; set; } // menucontents
        public IDbSet<MetricsVw> MetricsVw { get; set; } // metrics_vw
        public IDbSet<OrderDetails3Vw> OrderDetails3Vw { get; set; } // OrderDetails3_vw
        public IDbSet<Orderstatus> Orderstatus { get; set; } // orderstatus
        public IDbSet<Ordertype> Ordertype { get; set; } // ordertype
        public IDbSet<OtdVw> OtdVw { get; set; } // OTD_vw
        public IDbSet<PafSectors> PafSectors { get; set; } // PAFSectors
        public IDbSet<Paytypes> Paytypes { get; set; } // Paytypes
        public IDbSet<Ramesesaudit> Ramesesaudit { get; set; } // ramesesaudit
        public IDbSet<Recipe> Recipe { get; set; } // Recipe
        public IDbSet<Recipegroups> Recipegroups { get; set; } // recipegroups
        public IDbSet<Servicesettings> Servicesettings { get; set; } // servicesettings
        public IDbSet<Storecatagory> Storecatagory { get; set; } // storecatagory
        public IDbSet<Storecatagorytogroup> Storecatagorytogroup { get; set; } // storecatagorytogroup
        public IDbSet<Storegroup> Storegroup { get; set; } // storegroup
        public IDbSet<Storegroups> Storegroups { get; set; } // storegroups
        public IDbSet<Stores> Stores { get; set; } // stores

        static DailyReportingCodeFirstDbContext()
        {
            Database.SetInitializer<DailyReportingCodeFirstDbContext>(null);
        }

        public DailyReportingCodeFirstDbContext()
            : base("Name=DailyReportingCodeFirstDbContext")
        {
        InitializePartial();
        }

        public DailyReportingCodeFirstDbContext(string connectionString) : base(connectionString)
        {
        InitializePartial();
        }

        public DailyReportingCodeFirstDbContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
        {
        InitializePartial();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new AddressesConfiguration());
            modelBuilder.Configurations.Add(new AmsMenuBasedRegionsConfiguration());
            modelBuilder.Configurations.Add(new AmsTlkpJobCodesConfiguration());
            modelBuilder.Configurations.Add(new AmsTlkpOrderLineTypesConfiguration());
            modelBuilder.Configurations.Add(new AuditConfiguration());
            modelBuilder.Configurations.Add(new AuditserviceConfiguration());
            modelBuilder.Configurations.Add(new BasesizeConfiguration());
            modelBuilder.Configurations.Add(new ChainsConfiguration());
            modelBuilder.Configurations.Add(new CountryConfiguration());
            modelBuilder.Configurations.Add(new CreateorderConfiguration());
            modelBuilder.Configurations.Add(new CreditConfiguration());
            modelBuilder.Configurations.Add(new CustomLookupConfiguration());
            modelBuilder.Configurations.Add(new DailySummaryConfiguration());
            modelBuilder.Configurations.Add(new DailysummaryCubeVwConfiguration());
            modelBuilder.Configurations.Add(new DailysummaryVwConfiguration());
            modelBuilder.Configurations.Add(new EmployeesConfiguration());
            modelBuilder.Configurations.Add(new GroupsConfiguration());
            modelBuilder.Configurations.Add(new GroupsExtConfiguration());
            modelBuilder.Configurations.Add(new HourlyServiceMetricsConfiguration());
            modelBuilder.Configurations.Add(new KfcCreateorderConfiguration());
            modelBuilder.Configurations.Add(new MenuConfiguration());
            modelBuilder.Configurations.Add(new MenucontentsConfiguration());
            modelBuilder.Configurations.Add(new MetricsVwConfiguration());
            modelBuilder.Configurations.Add(new OrderDetails3VwConfiguration());
            modelBuilder.Configurations.Add(new OrderstatusConfiguration());
            modelBuilder.Configurations.Add(new OrdertypeConfiguration());
            modelBuilder.Configurations.Add(new OtdVwConfiguration());
            modelBuilder.Configurations.Add(new PafSectorsConfiguration());
            modelBuilder.Configurations.Add(new PaytypesConfiguration());
            modelBuilder.Configurations.Add(new RamesesauditConfiguration());
            modelBuilder.Configurations.Add(new RecipeConfiguration());
            modelBuilder.Configurations.Add(new RecipegroupsConfiguration());
            modelBuilder.Configurations.Add(new ServicesettingsConfiguration());
            modelBuilder.Configurations.Add(new StorecatagoryConfiguration());
            modelBuilder.Configurations.Add(new StorecatagorytogroupConfiguration());
            modelBuilder.Configurations.Add(new StoregroupConfiguration());
            modelBuilder.Configurations.Add(new StoregroupsConfiguration());
            modelBuilder.Configurations.Add(new StoresConfiguration());
        OnModelCreatingPartial(modelBuilder);
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new AddressesConfiguration(schema));
            modelBuilder.Configurations.Add(new AmsMenuBasedRegionsConfiguration(schema));
            modelBuilder.Configurations.Add(new AmsTlkpJobCodesConfiguration(schema));
            modelBuilder.Configurations.Add(new AmsTlkpOrderLineTypesConfiguration(schema));
            modelBuilder.Configurations.Add(new AuditConfiguration(schema));
            modelBuilder.Configurations.Add(new AuditserviceConfiguration(schema));
            modelBuilder.Configurations.Add(new BasesizeConfiguration(schema));
            modelBuilder.Configurations.Add(new ChainsConfiguration(schema));
            modelBuilder.Configurations.Add(new CountryConfiguration(schema));
            modelBuilder.Configurations.Add(new CreateorderConfiguration(schema));
            modelBuilder.Configurations.Add(new CreditConfiguration(schema));
            modelBuilder.Configurations.Add(new CustomLookupConfiguration(schema));
            modelBuilder.Configurations.Add(new DailySummaryConfiguration(schema));
            modelBuilder.Configurations.Add(new DailysummaryCubeVwConfiguration(schema));
            modelBuilder.Configurations.Add(new DailysummaryVwConfiguration(schema));
            modelBuilder.Configurations.Add(new EmployeesConfiguration(schema));
            modelBuilder.Configurations.Add(new GroupsConfiguration(schema));
            modelBuilder.Configurations.Add(new GroupsExtConfiguration(schema));
            modelBuilder.Configurations.Add(new HourlyServiceMetricsConfiguration(schema));
            modelBuilder.Configurations.Add(new KfcCreateorderConfiguration(schema));
            modelBuilder.Configurations.Add(new MenuConfiguration(schema));
            modelBuilder.Configurations.Add(new MenucontentsConfiguration(schema));
            modelBuilder.Configurations.Add(new MetricsVwConfiguration(schema));
            modelBuilder.Configurations.Add(new OrderDetails3VwConfiguration(schema));
            modelBuilder.Configurations.Add(new OrderstatusConfiguration(schema));
            modelBuilder.Configurations.Add(new OrdertypeConfiguration(schema));
            modelBuilder.Configurations.Add(new OtdVwConfiguration(schema));
            modelBuilder.Configurations.Add(new PafSectorsConfiguration(schema));
            modelBuilder.Configurations.Add(new PaytypesConfiguration(schema));
            modelBuilder.Configurations.Add(new RamesesauditConfiguration(schema));
            modelBuilder.Configurations.Add(new RecipeConfiguration(schema));
            modelBuilder.Configurations.Add(new RecipegroupsConfiguration(schema));
            modelBuilder.Configurations.Add(new ServicesettingsConfiguration(schema));
            modelBuilder.Configurations.Add(new StorecatagoryConfiguration(schema));
            modelBuilder.Configurations.Add(new StorecatagorytogroupConfiguration(schema));
            modelBuilder.Configurations.Add(new StoregroupConfiguration(schema));
            modelBuilder.Configurations.Add(new StoregroupsConfiguration(schema));
            modelBuilder.Configurations.Add(new StoresConfiguration(schema));
            return modelBuilder;
        }

        partial void InitializePartial();
        partial void OnModelCreatingPartial(DbModelBuilder modelBuilder);
    }
}
