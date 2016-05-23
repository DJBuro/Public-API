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
    public class DailyReportingCodeFirstDbContext : DbContext, IDailyReportingCodeFirstDbContext
    {
        public DbSet<AddressesOld> AddressesOlds { get; set; } // addresses_old
        public DbSet<AmsMenuBasedRegion> AmsMenuBasedRegions { get; set; } // AMS_MenuBasedRegions
        public DbSet<AmsTlkpJobCode> AmsTlkpJobCodes { get; set; } // AMS_TLKP_JobCodes
        public DbSet<AmsTlkpOrderLineType> AmsTlkpOrderLineTypes { get; set; } // AMS_TLKP_OrderLineTypes
        public DbSet<AmsTranslation> AmsTranslations { get; set; } // AMS_Translation
        public DbSet<AmsUploadStatu> AmsUploadStatus { get; set; } // AMSUploadStatus
        public DbSet<Audit> Audits { get; set; } // audit
        public DbSet<Auditservice> Auditservices { get; set; } // auditservice
        public DbSet<Basesize> Basesizes { get; set; } // basesize
        public DbSet<Cashrec> Cashrecs { get; set; } // cashrec
        public DbSet<Chain> Chains { get; set; } // Chains
        public DbSet<Country> Countries { get; set; } // Country
        public DbSet<Createorder> Createorders { get; set; } // createorder
        public DbSet<Credit> Credits { get; set; } // credit
        public DbSet<CustomLookup> CustomLookups { get; set; } // CustomLookup
        public DbSet<DailySummary> DailySummaries { get; set; } // DailySummary
        public DbSet<DailysummaryCubeVw> DailysummaryCubeVws { get; set; } // dailysummary_cube_vw
        public DbSet<DailysummaryVw> DailysummaryVws { get; set; } // dailysummary_vw
        public DbSet<DeleteId> DeleteIds { get; set; } // Delete_IDs
        public DbSet<DeleteTheDate> DeleteTheDates { get; set; } // Delete_TheDate
        public DbSet<Displaybar> Displaybars { get; set; } // displaybar
        public DbSet<Employee> Employees { get; set; } // employees
        public DbSet<Group> Groups { get; set; } // Groups
        public DbSet<GroupsExt> GroupsExts { get; set; } // Groups_Ext
        public DbSet<HourlyServiceMetric> HourlyServiceMetrics { get; set; } // HourlyServiceMetrics
        public DbSet<KfcCreateorder> KfcCreateorders { get; set; } // kfc_createorder
        public DbSet<MenucontentsOld> MenucontentsOlds { get; set; } // menucontents_old
        public DbSet<MenuItemsMapping> MenuItemsMappings { get; set; } // MenuItemsMapping
        public DbSet<MenuOld> MenuOlds { get; set; } // menu_old
        public DbSet<MetricsVw> MetricsVws { get; set; } // metrics_vw
        public DbSet<OrderDetails3Vw> OrderDetails3Vw { get; set; } // OrderDetails3_vw
        public DbSet<Orderstatu> Orderstatus { get; set; } // orderstatus
        public DbSet<Ordertype> Ordertypes { get; set; } // ordertype
        public DbSet<OtdVw> OtdVws { get; set; } // OTD_vw
        public DbSet<PafSector> PafSectors { get; set; } // PAFSectors
        public DbSet<Payroll> Payrolls { get; set; } // payroll
        public DbSet<Paytype> Paytypes { get; set; } // Paytypes
        public DbSet<PollingMonitorTranslation> PollingMonitorTranslations { get; set; } // PollingMonitor_Translation
        public DbSet<Primarycat> Primarycats { get; set; } // primarycat
        public DbSet<Ramesesaudit> Ramesesaudits { get; set; } // ramesesaudit
        public DbSet<Recipe> Recipes { get; set; } // Recipe
        public DbSet<Recipegroup> Recipegroups { get; set; } // recipegroups
        public DbSet<Scheduling> Schedulings { get; set; } // scheduling
        public DbSet<Secondarycat> Secondarycats { get; set; } // secondarycat
        public DbSet<Servicesetting> Servicesettings { get; set; } // servicesettings
        public DbSet<Store> Stores { get; set; } // stores
        public DbSet<Storecatagory> Storecatagories { get; set; } // storecatagory
        public DbSet<Storecatagorytogroup> Storecatagorytogroups { get; set; } // storecatagorytogroup
        public DbSet<Storegroup> Storegroups { get; set; } // storegroup
        public DbSet<Storegroup1> Storegroup1 { get; set; } // storegroups
        public DbSet<sys_DatabaseFirewallRule> sys_DatabaseFirewallRules { get; set; } // database_firewall_rules
        public DbSet<sys_ScriptDeployment> sys_ScriptDeployments { get; set; } // script_deployments
        public DbSet<sys_ScriptDeploymentStatus> sys_ScriptDeploymentStatus { get; set; } // script_deployment_status
        public DbSet<Testorder> Testorders { get; set; } // testorder
        
        static DailyReportingCodeFirstDbContext()
        {
            System.Data.Entity.Database.SetInitializer<DailyReportingCodeFirstDbContext>(null);
        }

        public DailyReportingCodeFirstDbContext()
            : base("Name=DailyReportingCodeFirstDbContext")
        {
        }

        public DailyReportingCodeFirstDbContext(string connectionString) : base(connectionString)
        {
        }

        public DailyReportingCodeFirstDbContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new AddressesOldConfiguration());
            modelBuilder.Configurations.Add(new AmsMenuBasedRegionConfiguration());
            modelBuilder.Configurations.Add(new AmsTlkpJobCodeConfiguration());
            modelBuilder.Configurations.Add(new AmsTlkpOrderLineTypeConfiguration());
            modelBuilder.Configurations.Add(new AmsTranslationConfiguration());
            modelBuilder.Configurations.Add(new AmsUploadStatuConfiguration());
            modelBuilder.Configurations.Add(new AuditConfiguration());
            modelBuilder.Configurations.Add(new AuditserviceConfiguration());
            modelBuilder.Configurations.Add(new BasesizeConfiguration());
            modelBuilder.Configurations.Add(new CashrecConfiguration());
            modelBuilder.Configurations.Add(new ChainConfiguration());
            modelBuilder.Configurations.Add(new CountryConfiguration());
            modelBuilder.Configurations.Add(new CreateorderConfiguration());
            modelBuilder.Configurations.Add(new CreditConfiguration());
            modelBuilder.Configurations.Add(new CustomLookupConfiguration());
            modelBuilder.Configurations.Add(new DailySummaryConfiguration());
            modelBuilder.Configurations.Add(new DailysummaryCubeVwConfiguration());
            modelBuilder.Configurations.Add(new DailysummaryVwConfiguration());
            modelBuilder.Configurations.Add(new DeleteIdConfiguration());
            modelBuilder.Configurations.Add(new DeleteTheDateConfiguration());
            modelBuilder.Configurations.Add(new DisplaybarConfiguration());
            modelBuilder.Configurations.Add(new EmployeeConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new GroupsExtConfiguration());
            modelBuilder.Configurations.Add(new HourlyServiceMetricConfiguration());
            modelBuilder.Configurations.Add(new KfcCreateorderConfiguration());
            modelBuilder.Configurations.Add(new MenucontentsOldConfiguration());
            modelBuilder.Configurations.Add(new MenuItemsMappingConfiguration());
            modelBuilder.Configurations.Add(new MenuOldConfiguration());
            modelBuilder.Configurations.Add(new MetricsVwConfiguration());
            modelBuilder.Configurations.Add(new OrderDetails3VwConfiguration());
            modelBuilder.Configurations.Add(new OrderstatuConfiguration());
            modelBuilder.Configurations.Add(new OrdertypeConfiguration());
            modelBuilder.Configurations.Add(new OtdVwConfiguration());
            modelBuilder.Configurations.Add(new PafSectorConfiguration());
            modelBuilder.Configurations.Add(new PayrollConfiguration());
            modelBuilder.Configurations.Add(new PaytypeConfiguration());
            modelBuilder.Configurations.Add(new PollingMonitorTranslationConfiguration());
            modelBuilder.Configurations.Add(new PrimarycatConfiguration());
            modelBuilder.Configurations.Add(new RamesesauditConfiguration());
            modelBuilder.Configurations.Add(new RecipeConfiguration());
            modelBuilder.Configurations.Add(new RecipegroupConfiguration());
            modelBuilder.Configurations.Add(new SchedulingConfiguration());
            modelBuilder.Configurations.Add(new SecondarycatConfiguration());
            modelBuilder.Configurations.Add(new ServicesettingConfiguration());
            modelBuilder.Configurations.Add(new StoreConfiguration());
            modelBuilder.Configurations.Add(new StorecatagoryConfiguration());
            modelBuilder.Configurations.Add(new StorecatagorytogroupConfiguration());
            modelBuilder.Configurations.Add(new StoregroupConfiguration());
            modelBuilder.Configurations.Add(new Storegroup1Configuration());
            modelBuilder.Configurations.Add(new sys_DatabaseFirewallRuleConfiguration());
            modelBuilder.Configurations.Add(new sys_ScriptDeploymentConfiguration());
            modelBuilder.Configurations.Add(new sys_ScriptDeploymentStatusConfiguration());
            modelBuilder.Configurations.Add(new TestorderConfiguration());
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new AddressesOldConfiguration(schema));
            modelBuilder.Configurations.Add(new AmsMenuBasedRegionConfiguration(schema));
            modelBuilder.Configurations.Add(new AmsTlkpJobCodeConfiguration(schema));
            modelBuilder.Configurations.Add(new AmsTlkpOrderLineTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new AmsTranslationConfiguration(schema));
            modelBuilder.Configurations.Add(new AmsUploadStatuConfiguration(schema));
            modelBuilder.Configurations.Add(new AuditConfiguration(schema));
            modelBuilder.Configurations.Add(new AuditserviceConfiguration(schema));
            modelBuilder.Configurations.Add(new BasesizeConfiguration(schema));
            modelBuilder.Configurations.Add(new CashrecConfiguration(schema));
            modelBuilder.Configurations.Add(new ChainConfiguration(schema));
            modelBuilder.Configurations.Add(new CountryConfiguration(schema));
            modelBuilder.Configurations.Add(new CreateorderConfiguration(schema));
            modelBuilder.Configurations.Add(new CreditConfiguration(schema));
            modelBuilder.Configurations.Add(new CustomLookupConfiguration(schema));
            modelBuilder.Configurations.Add(new DailySummaryConfiguration(schema));
            modelBuilder.Configurations.Add(new DailysummaryCubeVwConfiguration(schema));
            modelBuilder.Configurations.Add(new DailysummaryVwConfiguration(schema));
            modelBuilder.Configurations.Add(new DeleteIdConfiguration(schema));
            modelBuilder.Configurations.Add(new DeleteTheDateConfiguration(schema));
            modelBuilder.Configurations.Add(new DisplaybarConfiguration(schema));
            modelBuilder.Configurations.Add(new EmployeeConfiguration(schema));
            modelBuilder.Configurations.Add(new GroupConfiguration(schema));
            modelBuilder.Configurations.Add(new GroupsExtConfiguration(schema));
            modelBuilder.Configurations.Add(new HourlyServiceMetricConfiguration(schema));
            modelBuilder.Configurations.Add(new KfcCreateorderConfiguration(schema));
            modelBuilder.Configurations.Add(new MenucontentsOldConfiguration(schema));
            modelBuilder.Configurations.Add(new MenuItemsMappingConfiguration(schema));
            modelBuilder.Configurations.Add(new MenuOldConfiguration(schema));
            modelBuilder.Configurations.Add(new MetricsVwConfiguration(schema));
            modelBuilder.Configurations.Add(new OrderDetails3VwConfiguration(schema));
            modelBuilder.Configurations.Add(new OrderstatuConfiguration(schema));
            modelBuilder.Configurations.Add(new OrdertypeConfiguration(schema));
            modelBuilder.Configurations.Add(new OtdVwConfiguration(schema));
            modelBuilder.Configurations.Add(new PafSectorConfiguration(schema));
            modelBuilder.Configurations.Add(new PayrollConfiguration(schema));
            modelBuilder.Configurations.Add(new PaytypeConfiguration(schema));
            modelBuilder.Configurations.Add(new PollingMonitorTranslationConfiguration(schema));
            modelBuilder.Configurations.Add(new PrimarycatConfiguration(schema));
            modelBuilder.Configurations.Add(new RamesesauditConfiguration(schema));
            modelBuilder.Configurations.Add(new RecipeConfiguration(schema));
            modelBuilder.Configurations.Add(new RecipegroupConfiguration(schema));
            modelBuilder.Configurations.Add(new SchedulingConfiguration(schema));
            modelBuilder.Configurations.Add(new SecondarycatConfiguration(schema));
            modelBuilder.Configurations.Add(new ServicesettingConfiguration(schema));
            modelBuilder.Configurations.Add(new StoreConfiguration(schema));
            modelBuilder.Configurations.Add(new StorecatagoryConfiguration(schema));
            modelBuilder.Configurations.Add(new StorecatagorytogroupConfiguration(schema));
            modelBuilder.Configurations.Add(new StoregroupConfiguration(schema));
            modelBuilder.Configurations.Add(new Storegroup1Configuration(schema));
            modelBuilder.Configurations.Add(new sys_DatabaseFirewallRuleConfiguration(schema));
            modelBuilder.Configurations.Add(new sys_ScriptDeploymentConfiguration(schema));
            modelBuilder.Configurations.Add(new sys_ScriptDeploymentStatusConfiguration(schema));
            modelBuilder.Configurations.Add(new TestorderConfiguration(schema));
            return modelBuilder;
        }
        
        // Stored Procedures
        public int AmsCreateMissingMenuMappings()
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_CreateMissingMenuMappings] ", procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetAllCustomersDetailsReport(string stores)
        {
            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = stores, Size = 128 };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetAllCustomersDetailsReport] @stores", storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsGetCostOfSalesReportReturnModel> AmsGetCostOfSalesReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, int? period)
        {
            int procResult;
            return AmsGetCostOfSalesReport(quickDateRange, fromdate, todate, stores, period, out procResult);
        }

        public List<AmsGetCostOfSalesReportReturnModel> AmsGetCostOfSalesReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, int? period, out int procResult)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var periodParam = new SqlParameter { ParameterName = "@period", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = period.GetValueOrDefault() };
            if (!period.HasValue)
                periodParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetCostOfSalesReportReturnModel>("EXEC @procResult = [dbo].[AMS_GetCostOfSalesReport] @QuickDateRange, @fromdate, @todate, @Stores, @period", quickDateRangeParam, fromdateParam, todateParam, storesParam, periodParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsGetCustomerDeliveryReport(DateTime? fromDate, DateTime? toDate, string stores)
        {
            var fromDateParam = new SqlParameter { ParameterName = "@FromDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromDate.GetValueOrDefault() };
            if (!fromDate.HasValue)
                fromDateParam.Value = DBNull.Value;

            var toDateParam = new SqlParameter { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = toDate.GetValueOrDefault() };
            if (!toDate.HasValue)
                toDateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetCustomerDeliveryReport] @FromDate, @ToDate, @Stores", fromDateParam, toDateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetCustomersDetailsReport(string quickDateRange, DateTime? fromDate, DateTime? toDate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromDateParam = new SqlParameter { ParameterName = "@FromDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromDate.GetValueOrDefault() };
            if (!fromDate.HasValue)
                fromDateParam.Value = DBNull.Value;

            var toDateParam = new SqlParameter { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = toDate.GetValueOrDefault() };
            if (!toDate.HasValue)
                toDateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetCustomersDetailsReport] @QuickDateRange, @FromDate, @ToDate, @Stores", quickDateRangeParam, fromDateParam, toDateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetCustomersOrderPatternReport(DateTime? fromdate, DateTime? todate, string stores)
        {
            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetCustomersOrderPatternReport] @fromdate, @todate, @Stores", fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetCyoReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetCYOReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsGetDailyTotalsReturnModel> AmsGetDailyTotals(DateTime? startdate, DateTime? toDate, string storeId)
        {
            int procResult;
            return AmsGetDailyTotals(startdate, toDate, storeId, out procResult);
        }

        public List<AmsGetDailyTotalsReturnModel> AmsGetDailyTotals(DateTime? startdate, DateTime? toDate, string storeId, out int procResult)
        {
            var startdateParam = new SqlParameter { ParameterName = "@Startdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = startdate.GetValueOrDefault() };
            if (!startdate.HasValue)
                startdateParam.Value = DBNull.Value;

            var toDateParam = new SqlParameter { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = toDate.GetValueOrDefault() };
            if (!toDate.HasValue)
                toDateParam.Value = DBNull.Value;

            var storeIdParam = new SqlParameter { ParameterName = "@StoreID", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = storeId, Size = 512 };
            if (storeIdParam.Value == null)
                storeIdParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetDailyTotalsReturnModel>("EXEC @procResult = [dbo].[AMS_GetDailyTotals] @Startdate, @ToDate, @StoreID", startdateParam, toDateParam, storeIdParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsGetDailyTotalsPart2(DateTime? startdate, DateTime? toDate, string storeId)
        {
            var startdateParam = new SqlParameter { ParameterName = "@Startdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = startdate.GetValueOrDefault() };
            if (!startdate.HasValue)
                startdateParam.Value = DBNull.Value;

            var toDateParam = new SqlParameter { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = toDate.GetValueOrDefault() };
            if (!toDate.HasValue)
                toDateParam.Value = DBNull.Value;

            var storeIdParam = new SqlParameter { ParameterName = "@StoreID", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = storeId, Size = 512 };
            if (storeIdParam.Value == null)
                storeIdParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetDailyTotals_part2] @Startdate, @ToDate, @StoreID", startdateParam, toDateParam, storeIdParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetDealsReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = stores, Size = 500 };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetDealsReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetDeliverycommissionReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetDeliverycommissionReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsGetDiscountCancellationsDetailsReortReturnModel> AmsGetDiscountCancellationsDetailsReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetDiscountCancellationsDetailsReort(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetDiscountCancellationsDetailsReortReturnModel> AmsGetDiscountCancellationsDetailsReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetDiscountCancellationsDetailsReortReturnModel>("EXEC @procResult = [dbo].[AMS_GetDiscountCancellationsDetailsReort] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsGetDiscountCancellationsReortReturnModel> AmsGetDiscountCancellationsReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetDiscountCancellationsReort(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetDiscountCancellationsReortReturnModel> AmsGetDiscountCancellationsReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetDiscountCancellationsReortReturnModel>("EXEC @procResult = [dbo].[AMS_GetDiscountCancellationsReort] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsGetDriverDeliveries(DateTime? fromdate, DateTime? todate, string stores)
        {
            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetDriverDeliveries] @fromdate, @todate, @stores", fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetDriversPerfomanceReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetDriversPerfomanceReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsGetHcasReportReturnModel> AmsGetHcasReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetHcasReport(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetHcasReportReturnModel> AmsGetHcasReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetHcasReportReturnModel>("EXEC @procResult = [dbo].[AMS_GetHCASReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsGetIncompleteOrdersReortReturnModel> AmsGetIncompleteOrdersReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetIncompleteOrdersReort(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetIncompleteOrdersReortReturnModel> AmsGetIncompleteOrdersReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetIncompleteOrdersReortReturnModel>("EXEC @procResult = [dbo].[AMS_GetIncompleteOrdersReort] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsGetInv03Report(string quickDateRange, DateTime? fromdate, DateTime? todate, int? period, string stores, string recipeItems)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var periodParam = new SqlParameter { ParameterName = "@Period", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = period.GetValueOrDefault() };
            if (!period.HasValue)
                periodParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var recipeItemsParam = new SqlParameter { ParameterName = "@RecipeItems", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = recipeItems };
            if (recipeItemsParam.Value == null)
                recipeItemsParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetINV_03report] @QuickDateRange, @fromdate, @todate, @Period, @Stores, @RecipeItems", quickDateRangeParam, fromdateParam, todateParam, periodParam, storesParam, recipeItemsParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetInv03Summary(string quickDateRange, DateTime? fromdate, DateTime? todate, int? period, string stores, string recipeItems)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var periodParam = new SqlParameter { ParameterName = "@period", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = period.GetValueOrDefault() };
            if (!period.HasValue)
                periodParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var recipeItemsParam = new SqlParameter { ParameterName = "@RecipeItems", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = recipeItems };
            if (recipeItemsParam.Value == null)
                recipeItemsParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetINV_03Summary] @QuickDateRange, @fromdate, @todate, @period, @Stores, @RecipeItems", quickDateRangeParam, fromdateParam, todateParam, periodParam, storesParam, recipeItemsParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetinv04Report(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, string recipeItems)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var recipeItemsParam = new SqlParameter { ParameterName = "@RecipeItems", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = recipeItems };
            if (recipeItemsParam.Value == null)
                recipeItemsParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GETINV_04Report] @QuickDateRange, @fromdate, @todate, @Stores, @RecipeItems", quickDateRangeParam, fromdateParam, todateParam, storesParam, recipeItemsParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetInv04Summary(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, string recipeItems)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var recipeItemsParam = new SqlParameter { ParameterName = "@RecipeItems", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = recipeItems };
            if (recipeItemsParam.Value == null)
                recipeItemsParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetINV_04Summary] @QuickDateRange, @fromdate, @todate, @Stores, @RecipeItems", quickDateRangeParam, fromdateParam, todateParam, storesParam, recipeItemsParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetinv02Report(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, string recipeItems)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var recipeItemsParam = new SqlParameter { ParameterName = "@RecipeItems", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = recipeItems };
            if (recipeItemsParam.Value == null)
                recipeItemsParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GETINV02Report] @QuickDateRange, @fromdate, @todate, @Stores, @RecipeItems", quickDateRangeParam, fromdateParam, todateParam, storesParam, recipeItemsParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsGetInvRecipeGroupsReturnModel> AmsGetInvRecipeGroups(string stores)
        {
            int procResult;
            return AmsGetInvRecipeGroups(stores, out procResult);
        }

        public List<AmsGetInvRecipeGroupsReturnModel> AmsGetInvRecipeGroups(string stores, out int procResult)
        {
            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores, Size = 100 };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetInvRecipeGroupsReturnModel>("EXEC @procResult = [dbo].[AMS_GetInvRecipeGroups] @stores", storesParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsGetMarketingReport(DateTime? fromdate, DateTime? todate, string stores)
        {
            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetMarketingReport] @fromdate, @todate, @stores", fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetNewCustomersReport(string quickDateRange, DateTime? fromDate, DateTime? toDate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromDateParam = new SqlParameter { ParameterName = "@FromDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromDate.GetValueOrDefault() };
            if (!fromDate.HasValue)
                fromDateParam.Value = DBNull.Value;

            var toDateParam = new SqlParameter { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = toDate.GetValueOrDefault() };
            if (!toDate.HasValue)
                toDateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetNewCustomersReport] @QuickDateRange, @FromDate, @ToDate, @Stores", quickDateRangeParam, fromDateParam, toDateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetOrderDeliveryStatistics(DateTime? fromdate, DateTime? todate, string stores, string cityparam)
        {
            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var cityparamParam = new SqlParameter { ParameterName = "@cityparam", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = cityparam, Size = 64 };
            if (cityparamParam.Value == null)
                cityparamParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetOrderDeliveryStatistics] @fromdate, @todate, @stores, @cityparam", fromdateParam, todateParam, storesParam, cityparamParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetOrderDeliveryTimesReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetOrderDeliveryTimesReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetOrderDetailReport(DateTime? seldate, string storeid, string orderNum)
        {
            var seldateParam = new SqlParameter { ParameterName = "@seldate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = seldate.GetValueOrDefault() };
            if (!seldate.HasValue)
                seldateParam.Value = DBNull.Value;

            var storeidParam = new SqlParameter { ParameterName = "@storeid", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = storeid };
            if (storeidParam.Value == null)
                storeidParam.Value = DBNull.Value;

            var orderNumParam = new SqlParameter { ParameterName = "@orderNum", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = orderNum };
            if (orderNumParam.Value == null)
                orderNumParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_getOrderDetailReport] @seldate, @storeid, @orderNum", seldateParam, storeidParam, orderNumParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetOrderedStockreport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, string recipeItems)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var recipeItemsParam = new SqlParameter { ParameterName = "@RecipeItems", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = recipeItems };
            if (recipeItemsParam.Value == null)
                recipeItemsParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetOrderedStockreport] @QuickDateRange, @fromdate, @todate, @Stores, @RecipeItems", quickDateRangeParam, fromdateParam, todateParam, storesParam, recipeItemsParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetOrderItemsReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = stores, Size = 100 };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetOrderItemsReport] @QuickDateRange, @fromdate, @todate, @Stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetOrderLateDeliveryReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, int? timeInMinutes)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var timeInMinutesParam = new SqlParameter { ParameterName = "@timeInMinutes", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = timeInMinutes.GetValueOrDefault() };
            if (!timeInMinutes.HasValue)
                timeInMinutesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetOrderLateDeliveryReport] @QuickDateRange, @fromdate, @todate, @stores, @timeInMinutes", quickDateRangeParam, fromdateParam, todateParam, storesParam, timeInMinutesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetPayrollReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetPayrollReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetPayrollSummaryReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetPayrollSummaryReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetPizzabysizeReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetPizzabysizeReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetProductMixDetailsReport(string quickDateRange, DateTime? fromDate, DateTime? toDate, string stores, string menuItems)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromDateParam = new SqlParameter { ParameterName = "@FromDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromDate.GetValueOrDefault() };
            if (!fromDate.HasValue)
                fromDateParam.Value = DBNull.Value;

            var toDateParam = new SqlParameter { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = toDate.GetValueOrDefault() };
            if (!toDate.HasValue)
                toDateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var menuItemsParam = new SqlParameter { ParameterName = "@MenuItems", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = menuItems };
            if (menuItemsParam.Value == null)
                menuItemsParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetProductMixDetailsReport] @QuickDateRange, @FromDate, @ToDate, @Stores, @MenuItems", quickDateRangeParam, fromDateParam, toDateParam, storesParam, menuItemsParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetProductMixQueryReport(string quickDateRange, DateTime? fromDate, DateTime? toDate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromDateParam = new SqlParameter { ParameterName = "@FromDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromDate.GetValueOrDefault() };
            if (!fromDate.HasValue)
                fromDateParam.Value = DBNull.Value;

            var toDateParam = new SqlParameter { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = toDate.GetValueOrDefault() };
            if (!toDate.HasValue)
                toDateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetProductMixQueryReport] @QuickDateRange, @FromDate, @ToDate, @stores", quickDateRangeParam, fromDateParam, toDateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsGetQuickDatesReturnModel> AmsGetQuickDates()
        {
            int procResult;
            return AmsGetQuickDates(out procResult);
        }

        public List<AmsGetQuickDatesReturnModel> AmsGetQuickDates( out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetQuickDatesReturnModel>("EXEC @procResult = [dbo].[AMS_GetQuickDates] ", procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsGetRecipeGroupsByStoreReturnModel> AmsGetRecipeGroupsByStore(string stores, string enabledisable)
        {
            int procResult;
            return AmsGetRecipeGroupsByStore(stores, enabledisable, out procResult);
        }

        public List<AmsGetRecipeGroupsByStoreReturnModel> AmsGetRecipeGroupsByStore(string stores, string enabledisable, out int procResult)
        {
            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = stores, Size = 512 };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var enabledisableParam = new SqlParameter { ParameterName = "@enabledisable", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = enabledisable, Size = 16 };
            if (enabledisableParam.Value == null)
                enabledisableParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetRecipeGroupsByStoreReturnModel>("EXEC @procResult = [dbo].[AMS_GetRecipeGroupsByStore] @stores, @enabledisable", storesParam, enabledisableParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsGetRecipeItemsByStoreReturnModel> AmsGetRecipeItemsByStore(string stores, string enabledItems, string groupId)
        {
            int procResult;
            return AmsGetRecipeItemsByStore(stores, enabledItems, groupId, out procResult);
        }

        public List<AmsGetRecipeItemsByStoreReturnModel> AmsGetRecipeItemsByStore(string stores, string enabledItems, string groupId, out int procResult)
        {
            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = stores, Size = 512 };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var enabledItemsParam = new SqlParameter { ParameterName = "@EnabledItems", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = enabledItems, Size = 32 };
            if (enabledItemsParam.Value == null)
                enabledItemsParam.Value = DBNull.Value;

            var groupIdParam = new SqlParameter { ParameterName = "@GroupID", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = groupId, Size = 128 };
            if (groupIdParam.Value == null)
                groupIdParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetRecipeItemsByStoreReturnModel>("EXEC @procResult = [dbo].[AMS_GetRecipeItemsByStore] @Stores, @EnabledItems, @GroupID", storesParam, enabledItemsParam, groupIdParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsGetSalesAndServiceReportReturnModel> AmsGetSalesAndServiceReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetSalesAndServiceReport(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetSalesAndServiceReportReturnModel> AmsGetSalesAndServiceReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetSalesAndServiceReportReturnModel>("EXEC @procResult = [dbo].[AMS_GetSalesAndServiceReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsGetSalesByArticle2(DateTime? startDate, DateTime? endDate, string stores)
        {
            var startDateParam = new SqlParameter { ParameterName = "@startDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = startDate.GetValueOrDefault() };
            if (!startDate.HasValue)
                startDateParam.Value = DBNull.Value;

            var endDateParam = new SqlParameter { ParameterName = "@endDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = endDate.GetValueOrDefault() };
            if (!endDate.HasValue)
                endDateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = stores, Size = 128 };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetSalesByArticle2] @startDate, @endDate, @stores", startDateParam, endDateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetSalesByFamily2(DateTime? fromDate, DateTime? toDate, string stores)
        {
            var fromDateParam = new SqlParameter { ParameterName = "@FromDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromDate.GetValueOrDefault() };
            if (!fromDate.HasValue)
                fromDateParam.Value = DBNull.Value;

            var toDateParam = new SqlParameter { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = toDate.GetValueOrDefault() };
            if (!toDate.HasValue)
                toDateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetSalesByFamily2] @FromDate, @ToDate, @Stores", fromDateParam, toDateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetSalesbyPostCodeSectorreport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_getSalesbyPostCodeSectorreport] @QuickDateRange, @fromdate, @todate, @Stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetSalesbysourcedetailsReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = stores, Size = 500 };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetSalesbysourcedetailsReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsGetSalesByStoreReportReturnModel> AmsGetSalesByStoreReport(string quickDateRange, DateTime? fromdate, DateTime? todate)
        {
            int procResult;
            return AmsGetSalesByStoreReport(quickDateRange, fromdate, todate, out procResult);
        }

        public List<AmsGetSalesByStoreReportReturnModel> AmsGetSalesByStoreReport(string quickDateRange, DateTime? fromdate, DateTime? todate, out int procResult)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetSalesByStoreReportReturnModel>("EXEC @procResult = [dbo].[AMS_GetSalesByStoreReport] @QuickDateRange, @fromdate, @todate", quickDateRangeParam, fromdateParam, todateParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsGetSalesByTaxDetailsReport(DateTime? seldate, string store)
        {
            var seldateParam = new SqlParameter { ParameterName = "@seldate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = seldate.GetValueOrDefault() };
            if (!seldate.HasValue)
                seldateParam.Value = DBNull.Value;

            var storeParam = new SqlParameter { ParameterName = "@store", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = store };
            if (storeParam.Value == null)
                storeParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_getSalesByTaxDetailsReport] @seldate, @store", seldateParam, storeParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsGetSalesByTaxPercentReportReturnModel> AmsGetSalesByTaxPercentReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetSalesByTaxPercentReport(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetSalesByTaxPercentReportReturnModel> AmsGetSalesByTaxPercentReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetSalesByTaxPercentReportReturnModel>("EXEC @procResult = [dbo].[AMS_GetSalesByTaxPercentReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsGetSalesByWeekReportReturnModel> AmsGetSalesByWeekReport(DateTime? fromDate, DateTime? toDate)
        {
            int procResult;
            return AmsGetSalesByWeekReport(fromDate, toDate, out procResult);
        }

        public List<AmsGetSalesByWeekReportReturnModel> AmsGetSalesByWeekReport(DateTime? fromDate, DateTime? toDate, out int procResult)
        {
            var fromDateParam = new SqlParameter { ParameterName = "@FromDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromDate.GetValueOrDefault() };
            if (!fromDate.HasValue)
                fromDateParam.Value = DBNull.Value;

            var toDateParam = new SqlParameter { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = toDate.GetValueOrDefault() };
            if (!toDate.HasValue)
                toDateParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetSalesByWeekReportReturnModel>("EXEC @procResult = [dbo].[AMS_GetSalesByWeekReport] @FromDate, @ToDate", fromDateParam, toDateParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsGetSalesSummary(DateTime? startDate, DateTime? endDate, string stores)
        {
            var startDateParam = new SqlParameter { ParameterName = "@startDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = startDate.GetValueOrDefault() };
            if (!startDate.HasValue)
                startDateParam.Value = DBNull.Value;

            var endDateParam = new SqlParameter { ParameterName = "@endDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = endDate.GetValueOrDefault() };
            if (!endDate.HasValue)
                endDateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = stores, Size = 128 };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetSalesSummary] @startDate, @endDate, @stores", startDateParam, endDateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsGetServiceAnalysisDetailsReportReturnModel> AmsGetServiceAnalysisDetailsReport(DateTime? seldate, string store)
        {
            int procResult;
            return AmsGetServiceAnalysisDetailsReport(seldate, store, out procResult);
        }

        public List<AmsGetServiceAnalysisDetailsReportReturnModel> AmsGetServiceAnalysisDetailsReport(DateTime? seldate, string store, out int procResult)
        {
            var seldateParam = new SqlParameter { ParameterName = "@seldate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = seldate.GetValueOrDefault() };
            if (!seldate.HasValue)
                seldateParam.Value = DBNull.Value;

            var storeParam = new SqlParameter { ParameterName = "@store", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = store };
            if (storeParam.Value == null)
                storeParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetServiceAnalysisDetailsReportReturnModel>("EXEC @procResult = [dbo].[AMS_getServiceAnalysisDetailsReport] @seldate, @store", seldateParam, storeParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsGetStaffAverageSpendReportReturnModel> AmsGetStaffAverageSpendReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetStaffAverageSpendReport(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetStaffAverageSpendReportReturnModel> AmsGetStaffAverageSpendReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetStaffAverageSpendReportReturnModel>("EXEC @procResult = [dbo].[AMS_GetStaffAverageSpendReport] @QuickDateRange, @fromdate, @todate, @stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsGetStoreGroupsReturnModel> AmsGetStoreGroups()
        {
            int procResult;
            return AmsGetStoreGroups(out procResult);
        }

        public List<AmsGetStoreGroupsReturnModel> AmsGetStoreGroups( out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetStoreGroupsReturnModel>("EXEC @procResult = [dbo].[AMS_GetStoreGroups] ", procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsGetStoresByGroupsReturnModel> AmsGetStoresByGroups(string masterId)
        {
            int procResult;
            return AmsGetStoresByGroups(masterId, out procResult);
        }

        public List<AmsGetStoresByGroupsReturnModel> AmsGetStoresByGroups(string masterId, out int procResult)
        {
            var masterIdParam = new SqlParameter { ParameterName = "@MasterID", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = masterId };
            if (masterIdParam.Value == null)
                masterIdParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetStoresByGroupsReturnModel>("EXEC @procResult = [dbo].[AMS_GetStoresBYGroups] @MasterID", masterIdParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsGetTotalsByHourReport2(DateTime? fromdate, DateTime? todate, string stores)
        {
            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetTotalsByHourReport2] @fromdate, @todate, @stores", fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsGetTranslation(string translationKey, out string newTranslation)
        {
            var translationKeyParam = new SqlParameter { ParameterName = "@TranslationKey", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = translationKey };
            if (translationKeyParam.Value == null)
                translationKeyParam.Value = DBNull.Value;

            var newTranslationParam = new SqlParameter { ParameterName = "@NewTranslation", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_GetTranslation] @TranslationKey, @NewTranslation OUTPUT", translationKeyParam, newTranslationParam, procResultParam);
            if (((INullable)newTranslationParam.SqlValue).IsNull)
                newTranslation = default(string);
            else
            newTranslation = (string) newTranslationParam.Value;
 
            return (int) procResultParam.Value;
        }

        public List<AmsGetWeeklyPayrollReportReturnModel> AmsGetWeeklyPayrollReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetWeeklyPayrollReport(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetWeeklyPayrollReportReturnModel> AmsGetWeeklyPayrollReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
            var quickDateRangeParam = new SqlParameter { ParameterName = "@QuickDateRange", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = quickDateRange };
            if (quickDateRangeParam.Value == null)
                quickDateRangeParam.Value = DBNull.Value;

            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@Stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsGetWeeklyPayrollReportReturnModel>("EXEC @procResult = [dbo].[AMS_GetWeeklyPayrollReport] @QuickDateRange, @fromdate, @todate, @Stores", quickDateRangeParam, fromdateParam, todateParam, storesParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsHelperCanxOrdersReturnModel> AmsHelperCanxOrders(int? storeId, DateTime? businessDate, int? orderNum)
        {
            int procResult;
            return AmsHelperCanxOrders(storeId, businessDate, orderNum, out procResult);
        }

        public List<AmsHelperCanxOrdersReturnModel> AmsHelperCanxOrders(int? storeId, DateTime? businessDate, int? orderNum, out int procResult)
        {
            var storeIdParam = new SqlParameter { ParameterName = "@StoreID", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = storeId.GetValueOrDefault() };
            if (!storeId.HasValue)
                storeIdParam.Value = DBNull.Value;

            var businessDateParam = new SqlParameter { ParameterName = "@BusinessDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = businessDate.GetValueOrDefault() };
            if (!businessDate.HasValue)
                businessDateParam.Value = DBNull.Value;

            var orderNumParam = new SqlParameter { ParameterName = "@OrderNum", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = orderNum.GetValueOrDefault() };
            if (!orderNum.HasValue)
                orderNumParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsHelperCanxOrdersReturnModel>("EXEC @procResult = [dbo].[AMS_Helper_Canx_Orders] @StoreID, @BusinessDate, @OrderNum", storeIdParam, businessDateParam, orderNumParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsHomePageGetCompStoresReturnModel> AmsHomePageGetCompStores()
        {
            int procResult;
            return AmsHomePageGetCompStores(out procResult);
        }

        public List<AmsHomePageGetCompStoresReturnModel> AmsHomePageGetCompStores( out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsHomePageGetCompStoresReturnModel>("EXEC @procResult = [dbo].[AMS_HomePage_GetCompStores] ", procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsHomePageServiceSummaryReturnModel> AmsHomePageServiceSummary()
        {
            int procResult;
            return AmsHomePageServiceSummary(out procResult);
        }

        public List<AmsHomePageServiceSummaryReturnModel> AmsHomePageServiceSummary( out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsHomePageServiceSummaryReturnModel>("EXEC @procResult = [dbo].[AMS_HomePage_ServiceSummary] ", procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsSubGetCustomersOrderPatternReport(DateTime? fromdate, DateTime? todate, string typeRequired, int? storeid, out double? value)
        {
            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var typeRequiredParam = new SqlParameter { ParameterName = "@typeRequired", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = typeRequired };
            if (typeRequiredParam.Value == null)
                typeRequiredParam.Value = DBNull.Value;

            var storeidParam = new SqlParameter { ParameterName = "@storeid", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = storeid.GetValueOrDefault() };
            if (!storeid.HasValue)
                storeidParam.Value = DBNull.Value;

            var valueParam = new SqlParameter { ParameterName = "@value", SqlDbType = SqlDbType.Float, Direction = ParameterDirection.Output };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_SUB_GetCustomersOrderPatternReport] @fromdate, @todate, @typeRequired, @storeid, @value OUTPUT", fromdateParam, todateParam, typeRequiredParam, storeidParam, valueParam, procResultParam);
            if (((INullable)valueParam.SqlValue).IsNull)
                value = null;
            else
            value = (double) valueParam.Value;
 
            return (int) procResultParam.Value;
        }

        public int AmsUpdateProcess(int? nStoreId, DateTime? processDate)
        {
            var nStoreIdParam = new SqlParameter { ParameterName = "@nStoreID", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = nStoreId.GetValueOrDefault() };
            if (!nStoreId.HasValue)
                nStoreIdParam.Value = DBNull.Value;

            var processDateParam = new SqlParameter { ParameterName = "@ProcessDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = processDate.GetValueOrDefault() };
            if (!processDate.HasValue)
                processDateParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_UpdateProcess] @nStoreID, @ProcessDate", nStoreIdParam, processDateParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsUpdateStores(int? androSiteId, string custSiteId, string name, string storeStatus, int? countryKey)
        {
            var androSiteIdParam = new SqlParameter { ParameterName = "@AndroSiteId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = androSiteId.GetValueOrDefault() };
            if (!androSiteId.HasValue)
                androSiteIdParam.Value = DBNull.Value;

            var custSiteIdParam = new SqlParameter { ParameterName = "@CustSiteID", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = custSiteId, Size = 100 };
            if (custSiteIdParam.Value == null)
                custSiteIdParam.Value = DBNull.Value;

            var nameParam = new SqlParameter { ParameterName = "@Name", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = name, Size = 100 };
            if (nameParam.Value == null)
                nameParam.Value = DBNull.Value;

            var storeStatusParam = new SqlParameter { ParameterName = "@StoreStatus", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = storeStatus, Size = 32 };
            if (storeStatusParam.Value == null)
                storeStatusParam.Value = DBNull.Value;

            var countryKeyParam = new SqlParameter { ParameterName = "@CountryKey", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = countryKey.GetValueOrDefault() };
            if (!countryKey.HasValue)
                countryKeyParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[AMS_UpdateStores] @AndroSiteId, @CustSiteID, @Name, @StoreStatus, @CountryKey", androSiteIdParam, custSiteIdParam, nameParam, storeStatusParam, countryKeyParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsspCompstorespollingserviceReturnModel> AmsspCompstorespollingservice()
        {
            int procResult;
            return AmsspCompstorespollingservice(out procResult);
        }

        public List<AmsspCompstorespollingserviceReturnModel> AmsspCompstorespollingservice( out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsspCompstorespollingserviceReturnModel>("EXEC @procResult = [dbo].[amssp_compstorespollingservice] ", procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsspDellocaldeals(int? storeid)
        {
            var storeidParam = new SqlParameter { ParameterName = "@storeid", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = storeid.GetValueOrDefault() };
            if (!storeid.HasValue)
                storeidParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[amssp_dellocaldeals] @storeid", storeidParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsspDelmenu(int? storeid)
        {
            var storeidParam = new SqlParameter { ParameterName = "@storeid", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = storeid.GetValueOrDefault() };
            if (!storeid.HasValue)
                storeidParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[amssp_delmenu] @storeid", storeidParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsspGetallmenusiteidsReturnModel> AmsspGetallmenusiteids(int? storeid)
        {
            int procResult;
            return AmsspGetallmenusiteids(storeid, out procResult);
        }

        public List<AmsspGetallmenusiteidsReturnModel> AmsspGetallmenusiteids(int? storeid, out int procResult)
        {
            var storeidParam = new SqlParameter { ParameterName = "@storeid", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = storeid.GetValueOrDefault() };
            if (!storeid.HasValue)
                storeidParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsspGetallmenusiteidsReturnModel>("EXEC @procResult = [dbo].[amssp_getallmenusiteids] @storeid", storeidParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsspGetamsidReturnModel> AmsspGetamsid()
        {
            int procResult;
            return AmsspGetamsid(out procResult);
        }

        public List<AmsspGetamsidReturnModel> AmsspGetamsid( out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsspGetamsidReturnModel>("EXEC @procResult = [dbo].[amssp_getamsid] ", procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsspGetmastermenuidReturnModel> AmsspGetmastermenuid()
        {
            int procResult;
            return AmsspGetmastermenuid(out procResult);
        }

        public List<AmsspGetmastermenuidReturnModel> AmsspGetmastermenuid( out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsspGetmastermenuidReturnModel>("EXEC @procResult = [dbo].[amssp_getmastermenuid] ", procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsspGetmissingstores(int? days)
        {
            var daysParam = new SqlParameter { ParameterName = "@days", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = days.GetValueOrDefault() };
            if (!days.HasValue)
                daysParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[amssp_getmissingstores] @days", daysParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsspGetservicesettingsReturnModel> AmsspGetservicesettings()
        {
            int procResult;
            return AmsspGetservicesettings(out procResult);
        }

        public List<AmsspGetservicesettingsReturnModel> AmsspGetservicesettings( out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsspGetservicesettingsReturnModel>("EXEC @procResult = [dbo].[amssp_getservicesettings] ", procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<AmsspGetstoresReturnModel> AmsspGetstores()
        {
            int procResult;
            return AmsspGetstores(out procResult);
        }

        public List<AmsspGetstoresReturnModel> AmsspGetstores( out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsspGetstoresReturnModel>("EXEC @procResult = [dbo].[amssp_getstores] ", procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsspSelpollingstatus()
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[amssp_selpollingstatus] ", procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AmsspUpdateservicesettingsReturnModel> AmsspUpdateservicesettings(DateTime? nextrun, int? overridesitelist, DateTime? datestamp)
        {
            int procResult;
            return AmsspUpdateservicesettings(nextrun, overridesitelist, datestamp, out procResult);
        }

        public List<AmsspUpdateservicesettingsReturnModel> AmsspUpdateservicesettings(DateTime? nextrun, int? overridesitelist, DateTime? datestamp, out int procResult)
        {
            var nextrunParam = new SqlParameter { ParameterName = "@nextrun", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = nextrun.GetValueOrDefault() };
            if (!nextrun.HasValue)
                nextrunParam.Value = DBNull.Value;

            var overridesitelistParam = new SqlParameter { ParameterName = "@overridesitelist", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = overridesitelist.GetValueOrDefault() };
            if (!overridesitelist.HasValue)
                overridesitelistParam.Value = DBNull.Value;

            var datestampParam = new SqlParameter { ParameterName = "@datestamp", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = datestamp.GetValueOrDefault() };
            if (!datestamp.HasValue)
                datestampParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AmsspUpdateservicesettingsReturnModel>("EXEC @procResult = [dbo].[amssp_updateservicesettings] @nextrun, @overridesitelist, @datestamp", nextrunParam, overridesitelistParam, datestampParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AmsspUpdatestorelastupdate(int? storeid)
        {
            var storeidParam = new SqlParameter { ParameterName = "@storeid", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = storeid.GetValueOrDefault() };
            if (!storeid.HasValue)
                storeidParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[amssp_updatestorelastupdate] @storeid", storeidParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AmsspUpdmastermenu(int? storeid, int? version)
        {
            var storeidParam = new SqlParameter { ParameterName = "@storeid", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = storeid.GetValueOrDefault() };
            if (!storeid.HasValue)
                storeidParam.Value = DBNull.Value;

            var versionParam = new SqlParameter { ParameterName = "@version", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = version.GetValueOrDefault() };
            if (!version.HasValue)
                versionParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[amssp_updmastermenu] @storeid, @version", storeidParam, versionParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AndroAmsSpDeliverycommission(DateTime? fromdate, DateTime? todate, string stores)
        {
            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[ANDRO_AMS_SP_Deliverycommission] @fromdate, @todate, @stores", fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<AndroAmsSpDiscountCancellationsReortReturnModel> AndroAmsSpDiscountCancellationsReort(DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AndroAmsSpDiscountCancellationsReort(fromdate, todate, stores, out procResult);
        }

        public List<AndroAmsSpDiscountCancellationsReortReturnModel> AndroAmsSpDiscountCancellationsReort(DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<AndroAmsSpDiscountCancellationsReortReturnModel>("EXEC @procResult = [dbo].[Andro_AMS_SP_DiscountCancellationsReort] @fromdate, @todate, @stores", fromdateParam, todateParam, storesParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int AndroAmsSpDrv02(DateTime? fromdate, DateTime? todate, string stores)
        {
            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[ANDRO_AMS_SP_DRV_02] @fromdate, @todate, @stores", fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AndroAmsSpOrderDeliveryTimes(DateTime? fromdate, DateTime? todate, string stores)
        {
            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[ANDRO_AMS_SP_OrderDeliveryTimes] @fromdate, @todate, @stores", fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int AndroAmsSpStaffAverageSpendReport(DateTime? fromdate, DateTime? todate, string stores)
        {
            var fromdateParam = new SqlParameter { ParameterName = "@fromdate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromdate.GetValueOrDefault() };
            if (!fromdate.HasValue)
                fromdateParam.Value = DBNull.Value;

            var todateParam = new SqlParameter { ParameterName = "@todate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = todate.GetValueOrDefault() };
            if (!todate.HasValue)
                todateParam.Value = DBNull.Value;

            var storesParam = new SqlParameter { ParameterName = "@stores", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = stores };
            if (storesParam.Value == null)
                storesParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[Andro_AMS_SP_StaffAverageSpendReport] @fromdate, @todate, @stores", fromdateParam, todateParam, storesParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int FixNulls()
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[FixNulls] ", procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<GetIndexFragmentationReturnModel> GetIndexFragmentation(string tableName)
        {
            int procResult;
            return GetIndexFragmentation(tableName, out procResult);
        }

        public List<GetIndexFragmentationReturnModel> GetIndexFragmentation(string tableName, out int procResult)
        {
            var tableNameParam = new SqlParameter { ParameterName = "@tableName", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = tableName };
            if (tableNameParam.Value == null)
                tableNameParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<GetIndexFragmentationReturnModel>("EXEC @procResult = [dbo].[GetIndexFragmentation] @tableName", tableNameParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

    }
}
