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
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class FakeDailyReportingCodeFirstDbContext : IDailyReportingCodeFirstDbContext
    {
        public DbSet<AddressesOld> AddressesOlds { get; set; }
        public DbSet<AmsMenuBasedRegion> AmsMenuBasedRegions { get; set; }
        public DbSet<AmsTlkpJobCode> AmsTlkpJobCodes { get; set; }
        public DbSet<AmsTlkpOrderLineType> AmsTlkpOrderLineTypes { get; set; }
        public DbSet<AmsTranslation> AmsTranslations { get; set; }
        public DbSet<AmsUploadStatu> AmsUploadStatus { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Auditservice> Auditservices { get; set; }
        public DbSet<Basesize> Basesizes { get; set; }
        public DbSet<Cashrec> Cashrecs { get; set; }
        public DbSet<Chain> Chains { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Createorder> Createorders { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<CustomLookup> CustomLookups { get; set; }
        public DbSet<DailySummary> DailySummaries { get; set; }
        public DbSet<DailysummaryCubeVw> DailysummaryCubeVws { get; set; }
        public DbSet<DailysummaryVw> DailysummaryVws { get; set; }
        public DbSet<DeleteId> DeleteIds { get; set; }
        public DbSet<DeleteTheDate> DeleteTheDates { get; set; }
        public DbSet<Displaybar> Displaybars { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupsExt> GroupsExts { get; set; }
        public DbSet<HourlyServiceMetric> HourlyServiceMetrics { get; set; }
        public DbSet<KfcCreateorder> KfcCreateorders { get; set; }
        public DbSet<MenucontentsOld> MenucontentsOlds { get; set; }
        public DbSet<MenuItemsMapping> MenuItemsMappings { get; set; }
        public DbSet<MenuOld> MenuOlds { get; set; }
        public DbSet<MetricsVw> MetricsVws { get; set; }
        public DbSet<OrderDetails3Vw> OrderDetails3Vw { get; set; }
        public DbSet<Orderstatu> Orderstatus { get; set; }
        public DbSet<Ordertype> Ordertypes { get; set; }
        public DbSet<OtdVw> OtdVws { get; set; }
        public DbSet<PafSector> PafSectors { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Paytype> Paytypes { get; set; }
        public DbSet<PollingMonitorTranslation> PollingMonitorTranslations { get; set; }
        public DbSet<Primarycat> Primarycats { get; set; }
        public DbSet<Ramesesaudit> Ramesesaudits { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Recipegroup> Recipegroups { get; set; }
        public DbSet<Scheduling> Schedulings { get; set; }
        public DbSet<Secondarycat> Secondarycats { get; set; }
        public DbSet<Servicesetting> Servicesettings { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Storecatagory> Storecatagories { get; set; }
        public DbSet<Storecatagorytogroup> Storecatagorytogroups { get; set; }
        public DbSet<Storegroup> Storegroups { get; set; }
        public DbSet<Storegroup1> Storegroup1 { get; set; }
        public DbSet<sys_DatabaseFirewallRule> sys_DatabaseFirewallRules { get; set; }
        public DbSet<sys_ScriptDeployment> sys_ScriptDeployments { get; set; }
        public DbSet<sys_ScriptDeploymentStatus> sys_ScriptDeploymentStatus { get; set; }
        public DbSet<Testorder> Testorders { get; set; }

        public FakeDailyReportingCodeFirstDbContext()
        {
            AddressesOlds = new FakeDbSet<AddressesOld>();
            AmsMenuBasedRegions = new FakeDbSet<AmsMenuBasedRegion>();
            AmsTlkpJobCodes = new FakeDbSet<AmsTlkpJobCode>();
            AmsTlkpOrderLineTypes = new FakeDbSet<AmsTlkpOrderLineType>();
            AmsTranslations = new FakeDbSet<AmsTranslation>();
            AmsUploadStatus = new FakeDbSet<AmsUploadStatu>();
            Audits = new FakeDbSet<Audit>();
            Auditservices = new FakeDbSet<Auditservice>();
            Basesizes = new FakeDbSet<Basesize>();
            Cashrecs = new FakeDbSet<Cashrec>();
            Chains = new FakeDbSet<Chain>();
            Countries = new FakeDbSet<Country>();
            Createorders = new FakeDbSet<Createorder>();
            Credits = new FakeDbSet<Credit>();
            CustomLookups = new FakeDbSet<CustomLookup>();
            DailySummaries = new FakeDbSet<DailySummary>();
            DailysummaryCubeVws = new FakeDbSet<DailysummaryCubeVw>();
            DailysummaryVws = new FakeDbSet<DailysummaryVw>();
            DeleteIds = new FakeDbSet<DeleteId>();
            DeleteTheDates = new FakeDbSet<DeleteTheDate>();
            Displaybars = new FakeDbSet<Displaybar>();
            Employees = new FakeDbSet<Employee>();
            Groups = new FakeDbSet<Group>();
            GroupsExts = new FakeDbSet<GroupsExt>();
            HourlyServiceMetrics = new FakeDbSet<HourlyServiceMetric>();
            KfcCreateorders = new FakeDbSet<KfcCreateorder>();
            MenucontentsOlds = new FakeDbSet<MenucontentsOld>();
            MenuItemsMappings = new FakeDbSet<MenuItemsMapping>();
            MenuOlds = new FakeDbSet<MenuOld>();
            MetricsVws = new FakeDbSet<MetricsVw>();
            OrderDetails3Vw = new FakeDbSet<OrderDetails3Vw>();
            Orderstatus = new FakeDbSet<Orderstatu>();
            Ordertypes = new FakeDbSet<Ordertype>();
            OtdVws = new FakeDbSet<OtdVw>();
            PafSectors = new FakeDbSet<PafSector>();
            Payrolls = new FakeDbSet<Payroll>();
            Paytypes = new FakeDbSet<Paytype>();
            PollingMonitorTranslations = new FakeDbSet<PollingMonitorTranslation>();
            Primarycats = new FakeDbSet<Primarycat>();
            Ramesesaudits = new FakeDbSet<Ramesesaudit>();
            Recipes = new FakeDbSet<Recipe>();
            Recipegroups = new FakeDbSet<Recipegroup>();
            Schedulings = new FakeDbSet<Scheduling>();
            Secondarycats = new FakeDbSet<Secondarycat>();
            Servicesettings = new FakeDbSet<Servicesetting>();
            Stores = new FakeDbSet<Store>();
            Storecatagories = new FakeDbSet<Storecatagory>();
            Storecatagorytogroups = new FakeDbSet<Storecatagorytogroup>();
            Storegroups = new FakeDbSet<Storegroup>();
            Storegroup1 = new FakeDbSet<Storegroup1>();
            sys_DatabaseFirewallRules = new FakeDbSet<sys_DatabaseFirewallRule>();
            sys_ScriptDeployments = new FakeDbSet<sys_ScriptDeployment>();
            sys_ScriptDeploymentStatus = new FakeDbSet<sys_ScriptDeploymentStatus>();
            Testorders = new FakeDbSet<Testorder>();
        }
        
        public int SaveChangesCount { get; private set; } 
        public int SaveChanges()
        {
            ++SaveChangesCount;
            return 1;
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        
        // Stored Procedures
        public int AmsCreateMissingMenuMappings()
        {
 
            return 0;
        }

        public int AmsGetAllCustomersDetailsReport(string stores)
        {
 
            return 0;
        }

        public List<AmsGetCostOfSalesReportReturnModel> AmsGetCostOfSalesReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, int? period)
        {
            int procResult;
            return AmsGetCostOfSalesReport(quickDateRange, fromdate, todate, stores, period, out procResult);
        }

        public List<AmsGetCostOfSalesReportReturnModel> AmsGetCostOfSalesReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, int? period, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetCostOfSalesReportReturnModel>();
        }

        public int AmsGetCustomerDeliveryReport(DateTime? fromDate, DateTime? toDate, string stores)
        {
 
            return 0;
        }

        public int AmsGetCustomersDetailsReport(string quickDateRange, DateTime? fromDate, DateTime? toDate, string stores)
        {
 
            return 0;
        }

        public int AmsGetCustomersOrderPatternReport(DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AmsGetCyoReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public List<AmsGetDailyTotalsReturnModel> AmsGetDailyTotals(DateTime? startdate, DateTime? toDate, string storeId)
        {
            int procResult;
            return AmsGetDailyTotals(startdate, toDate, storeId, out procResult);
        }

        public List<AmsGetDailyTotalsReturnModel> AmsGetDailyTotals(DateTime? startdate, DateTime? toDate, string storeId, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetDailyTotalsReturnModel>();
        }

        public int AmsGetDailyTotalsPart2(DateTime? startdate, DateTime? toDate, string storeId)
        {
 
            return 0;
        }

        public int AmsGetDealsReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AmsGetDeliverycommissionReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public List<AmsGetDiscountCancellationsDetailsReortReturnModel> AmsGetDiscountCancellationsDetailsReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetDiscountCancellationsDetailsReort(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetDiscountCancellationsDetailsReortReturnModel> AmsGetDiscountCancellationsDetailsReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetDiscountCancellationsDetailsReortReturnModel>();
        }

        public List<AmsGetDiscountCancellationsReortReturnModel> AmsGetDiscountCancellationsReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetDiscountCancellationsReort(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetDiscountCancellationsReortReturnModel> AmsGetDiscountCancellationsReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetDiscountCancellationsReortReturnModel>();
        }

        public int AmsGetDriverDeliveries(DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AmsGetDriversPerfomanceReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public List<AmsGetHcasReportReturnModel> AmsGetHcasReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetHcasReport(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetHcasReportReturnModel> AmsGetHcasReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetHcasReportReturnModel>();
        }

        public List<AmsGetIncompleteOrdersReortReturnModel> AmsGetIncompleteOrdersReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetIncompleteOrdersReort(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetIncompleteOrdersReortReturnModel> AmsGetIncompleteOrdersReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetIncompleteOrdersReortReturnModel>();
        }

        public int AmsGetInv03Report(string quickDateRange, DateTime? fromdate, DateTime? todate, int? period, string stores, string recipeItems)
        {
 
            return 0;
        }

        public int AmsGetInv03Summary(string quickDateRange, DateTime? fromdate, DateTime? todate, int? period, string stores, string recipeItems)
        {
 
            return 0;
        }

        public int AmsGetinv04Report(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, string recipeItems)
        {
 
            return 0;
        }

        public int AmsGetInv04Summary(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, string recipeItems)
        {
 
            return 0;
        }

        public int AmsGetinv02Report(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, string recipeItems)
        {
 
            return 0;
        }

        public List<AmsGetInvRecipeGroupsReturnModel> AmsGetInvRecipeGroups(string stores)
        {
            int procResult;
            return AmsGetInvRecipeGroups(stores, out procResult);
        }

        public List<AmsGetInvRecipeGroupsReturnModel> AmsGetInvRecipeGroups(string stores, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetInvRecipeGroupsReturnModel>();
        }

        public int AmsGetMarketingReport(DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AmsGetNewCustomersReport(string quickDateRange, DateTime? fromDate, DateTime? toDate, string stores)
        {
 
            return 0;
        }

        public int AmsGetOrderDeliveryStatistics(DateTime? fromdate, DateTime? todate, string stores, string cityparam)
        {
 
            return 0;
        }

        public int AmsGetOrderDeliveryTimesReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AmsGetOrderDetailReport(DateTime? seldate, string storeid, string orderNum)
        {
 
            return 0;
        }

        public int AmsGetOrderedStockreport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, string recipeItems)
        {
 
            return 0;
        }

        public int AmsGetOrderItemsReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AmsGetOrderLateDeliveryReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, int? timeInMinutes)
        {
 
            return 0;
        }

        public int AmsGetPayrollReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AmsGetPayrollSummaryReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AmsGetPizzabysizeReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AmsGetProductMixDetailsReport(string quickDateRange, DateTime? fromDate, DateTime? toDate, string stores, string menuItems)
        {
 
            return 0;
        }

        public int AmsGetProductMixQueryReport(string quickDateRange, DateTime? fromDate, DateTime? toDate, string stores)
        {
 
            return 0;
        }

        public List<AmsGetQuickDatesReturnModel> AmsGetQuickDates()
        {
            int procResult;
            return AmsGetQuickDates(out procResult);
        }

        public List<AmsGetQuickDatesReturnModel> AmsGetQuickDates( out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetQuickDatesReturnModel>();
        }

        public List<AmsGetRecipeGroupsByStoreReturnModel> AmsGetRecipeGroupsByStore(string stores, string enabledisable)
        {
            int procResult;
            return AmsGetRecipeGroupsByStore(stores, enabledisable, out procResult);
        }

        public List<AmsGetRecipeGroupsByStoreReturnModel> AmsGetRecipeGroupsByStore(string stores, string enabledisable, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetRecipeGroupsByStoreReturnModel>();
        }

        public List<AmsGetRecipeItemsByStoreReturnModel> AmsGetRecipeItemsByStore(string stores, string enabledItems, string groupId)
        {
            int procResult;
            return AmsGetRecipeItemsByStore(stores, enabledItems, groupId, out procResult);
        }

        public List<AmsGetRecipeItemsByStoreReturnModel> AmsGetRecipeItemsByStore(string stores, string enabledItems, string groupId, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetRecipeItemsByStoreReturnModel>();
        }

        public List<AmsGetSalesAndServiceReportReturnModel> AmsGetSalesAndServiceReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetSalesAndServiceReport(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetSalesAndServiceReportReturnModel> AmsGetSalesAndServiceReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetSalesAndServiceReportReturnModel>();
        }

        public int AmsGetSalesByArticle2(DateTime? startDate, DateTime? endDate, string stores)
        {
 
            return 0;
        }

        public int AmsGetSalesByFamily2(DateTime? fromDate, DateTime? toDate, string stores)
        {
 
            return 0;
        }

        public int AmsGetSalesbyPostCodeSectorreport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AmsGetSalesbysourcedetailsReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public List<AmsGetSalesByStoreReportReturnModel> AmsGetSalesByStoreReport(string quickDateRange, DateTime? fromdate, DateTime? todate)
        {
            int procResult;
            return AmsGetSalesByStoreReport(quickDateRange, fromdate, todate, out procResult);
        }

        public List<AmsGetSalesByStoreReportReturnModel> AmsGetSalesByStoreReport(string quickDateRange, DateTime? fromdate, DateTime? todate, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetSalesByStoreReportReturnModel>();
        }

        public int AmsGetSalesByTaxDetailsReport(DateTime? seldate, string store)
        {
 
            return 0;
        }

        public List<AmsGetSalesByTaxPercentReportReturnModel> AmsGetSalesByTaxPercentReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetSalesByTaxPercentReport(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetSalesByTaxPercentReportReturnModel> AmsGetSalesByTaxPercentReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetSalesByTaxPercentReportReturnModel>();
        }

        public List<AmsGetSalesByWeekReportReturnModel> AmsGetSalesByWeekReport(DateTime? fromDate, DateTime? toDate)
        {
            int procResult;
            return AmsGetSalesByWeekReport(fromDate, toDate, out procResult);
        }

        public List<AmsGetSalesByWeekReportReturnModel> AmsGetSalesByWeekReport(DateTime? fromDate, DateTime? toDate, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetSalesByWeekReportReturnModel>();
        }

        public int AmsGetSalesSummary(DateTime? startDate, DateTime? endDate, string stores)
        {
 
            return 0;
        }

        public List<AmsGetServiceAnalysisDetailsReportReturnModel> AmsGetServiceAnalysisDetailsReport(DateTime? seldate, string store)
        {
            int procResult;
            return AmsGetServiceAnalysisDetailsReport(seldate, store, out procResult);
        }

        public List<AmsGetServiceAnalysisDetailsReportReturnModel> AmsGetServiceAnalysisDetailsReport(DateTime? seldate, string store, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetServiceAnalysisDetailsReportReturnModel>();
        }

        public List<AmsGetStaffAverageSpendReportReturnModel> AmsGetStaffAverageSpendReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetStaffAverageSpendReport(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetStaffAverageSpendReportReturnModel> AmsGetStaffAverageSpendReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetStaffAverageSpendReportReturnModel>();
        }

        public List<AmsGetStoreGroupsReturnModel> AmsGetStoreGroups()
        {
            int procResult;
            return AmsGetStoreGroups(out procResult);
        }

        public List<AmsGetStoreGroupsReturnModel> AmsGetStoreGroups( out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetStoreGroupsReturnModel>();
        }

        public List<AmsGetStoresByGroupsReturnModel> AmsGetStoresByGroups(string masterId)
        {
            int procResult;
            return AmsGetStoresByGroups(masterId, out procResult);
        }

        public List<AmsGetStoresByGroupsReturnModel> AmsGetStoresByGroups(string masterId, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetStoresByGroupsReturnModel>();
        }

        public int AmsGetTotalsByHourReport2(DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AmsGetTranslation(string translationKey, out string newTranslation)
        {
            newTranslation = default(string);
 
            return 0;
        }

        public List<AmsGetWeeklyPayrollReportReturnModel> AmsGetWeeklyPayrollReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AmsGetWeeklyPayrollReport(quickDateRange, fromdate, todate, stores, out procResult);
        }

        public List<AmsGetWeeklyPayrollReportReturnModel> AmsGetWeeklyPayrollReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsGetWeeklyPayrollReportReturnModel>();
        }

        public List<AmsHelperCanxOrdersReturnModel> AmsHelperCanxOrders(int? storeId, DateTime? businessDate, int? orderNum)
        {
            int procResult;
            return AmsHelperCanxOrders(storeId, businessDate, orderNum, out procResult);
        }

        public List<AmsHelperCanxOrdersReturnModel> AmsHelperCanxOrders(int? storeId, DateTime? businessDate, int? orderNum, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsHelperCanxOrdersReturnModel>();
        }

        public List<AmsHomePageGetCompStoresReturnModel> AmsHomePageGetCompStores()
        {
            int procResult;
            return AmsHomePageGetCompStores(out procResult);
        }

        public List<AmsHomePageGetCompStoresReturnModel> AmsHomePageGetCompStores( out int procResult)
        {
 
            procResult = 0;
            return new List<AmsHomePageGetCompStoresReturnModel>();
        }

        public List<AmsHomePageServiceSummaryReturnModel> AmsHomePageServiceSummary()
        {
            int procResult;
            return AmsHomePageServiceSummary(out procResult);
        }

        public List<AmsHomePageServiceSummaryReturnModel> AmsHomePageServiceSummary( out int procResult)
        {
 
            procResult = 0;
            return new List<AmsHomePageServiceSummaryReturnModel>();
        }

        public int AmsSubGetCustomersOrderPatternReport(DateTime? fromdate, DateTime? todate, string typeRequired, int? storeid, out double? value)
        {
            value = default(double);
 
            return 0;
        }

        public int AmsUpdateProcess(int? nStoreId, DateTime? processDate)
        {
 
            return 0;
        }

        public int AmsUpdateStores(int? androSiteId, string custSiteId, string name, string storeStatus, int? countryKey)
        {
 
            return 0;
        }

        public List<AmsspCompstorespollingserviceReturnModel> AmsspCompstorespollingservice()
        {
            int procResult;
            return AmsspCompstorespollingservice(out procResult);
        }

        public List<AmsspCompstorespollingserviceReturnModel> AmsspCompstorespollingservice( out int procResult)
        {
 
            procResult = 0;
            return new List<AmsspCompstorespollingserviceReturnModel>();
        }

        public int AmsspDellocaldeals(int? storeid)
        {
 
            return 0;
        }

        public int AmsspDelmenu(int? storeid)
        {
 
            return 0;
        }

        public List<AmsspGetallmenusiteidsReturnModel> AmsspGetallmenusiteids(int? storeid)
        {
            int procResult;
            return AmsspGetallmenusiteids(storeid, out procResult);
        }

        public List<AmsspGetallmenusiteidsReturnModel> AmsspGetallmenusiteids(int? storeid, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsspGetallmenusiteidsReturnModel>();
        }

        public List<AmsspGetamsidReturnModel> AmsspGetamsid()
        {
            int procResult;
            return AmsspGetamsid(out procResult);
        }

        public List<AmsspGetamsidReturnModel> AmsspGetamsid( out int procResult)
        {
 
            procResult = 0;
            return new List<AmsspGetamsidReturnModel>();
        }

        public List<AmsspGetmastermenuidReturnModel> AmsspGetmastermenuid()
        {
            int procResult;
            return AmsspGetmastermenuid(out procResult);
        }

        public List<AmsspGetmastermenuidReturnModel> AmsspGetmastermenuid( out int procResult)
        {
 
            procResult = 0;
            return new List<AmsspGetmastermenuidReturnModel>();
        }

        public int AmsspGetmissingstores(int? days)
        {
 
            return 0;
        }

        public List<AmsspGetservicesettingsReturnModel> AmsspGetservicesettings()
        {
            int procResult;
            return AmsspGetservicesettings(out procResult);
        }

        public List<AmsspGetservicesettingsReturnModel> AmsspGetservicesettings( out int procResult)
        {
 
            procResult = 0;
            return new List<AmsspGetservicesettingsReturnModel>();
        }

        public List<AmsspGetstoresReturnModel> AmsspGetstores()
        {
            int procResult;
            return AmsspGetstores(out procResult);
        }

        public List<AmsspGetstoresReturnModel> AmsspGetstores( out int procResult)
        {
 
            procResult = 0;
            return new List<AmsspGetstoresReturnModel>();
        }

        public int AmsspSelpollingstatus()
        {
 
            return 0;
        }

        public List<AmsspUpdateservicesettingsReturnModel> AmsspUpdateservicesettings(DateTime? nextrun, int? overridesitelist, DateTime? datestamp)
        {
            int procResult;
            return AmsspUpdateservicesettings(nextrun, overridesitelist, datestamp, out procResult);
        }

        public List<AmsspUpdateservicesettingsReturnModel> AmsspUpdateservicesettings(DateTime? nextrun, int? overridesitelist, DateTime? datestamp, out int procResult)
        {
 
            procResult = 0;
            return new List<AmsspUpdateservicesettingsReturnModel>();
        }

        public int AmsspUpdatestorelastupdate(int? storeid)
        {
 
            return 0;
        }

        public int AmsspUpdmastermenu(int? storeid, int? version)
        {
 
            return 0;
        }

        public int AndroAmsSpDeliverycommission(DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public List<AndroAmsSpDiscountCancellationsReortReturnModel> AndroAmsSpDiscountCancellationsReort(DateTime? fromdate, DateTime? todate, string stores)
        {
            int procResult;
            return AndroAmsSpDiscountCancellationsReort(fromdate, todate, stores, out procResult);
        }

        public List<AndroAmsSpDiscountCancellationsReortReturnModel> AndroAmsSpDiscountCancellationsReort(DateTime? fromdate, DateTime? todate, string stores, out int procResult)
        {
 
            procResult = 0;
            return new List<AndroAmsSpDiscountCancellationsReortReturnModel>();
        }

        public int AndroAmsSpDrv02(DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AndroAmsSpOrderDeliveryTimes(DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int AndroAmsSpStaffAverageSpendReport(DateTime? fromdate, DateTime? todate, string stores)
        {
 
            return 0;
        }

        public int FixNulls()
        {
 
            return 0;
        }

        public List<GetIndexFragmentationReturnModel> GetIndexFragmentation(string tableName)
        {
            int procResult;
            return GetIndexFragmentation(tableName, out procResult);
        }

        public List<GetIndexFragmentationReturnModel> GetIndexFragmentation(string tableName, out int procResult)
        {
 
            procResult = 0;
            return new List<GetIndexFragmentationReturnModel>();
        }

    }
}
