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
    public interface IDailyReportingCodeFirstDbContext : IDisposable
    {
        DbSet<AddressesOld> AddressesOlds { get; set; } // addresses_old
        DbSet<AmsMenuBasedRegion> AmsMenuBasedRegions { get; set; } // AMS_MenuBasedRegions
        DbSet<AmsTlkpJobCode> AmsTlkpJobCodes { get; set; } // AMS_TLKP_JobCodes
        DbSet<AmsTlkpOrderLineType> AmsTlkpOrderLineTypes { get; set; } // AMS_TLKP_OrderLineTypes
        DbSet<AmsTranslation> AmsTranslations { get; set; } // AMS_Translation
        DbSet<AmsUploadStatu> AmsUploadStatus { get; set; } // AMSUploadStatus
        DbSet<Audit> Audits { get; set; } // audit
        DbSet<Auditservice> Auditservices { get; set; } // auditservice
        DbSet<Basesize> Basesizes { get; set; } // basesize
        DbSet<Cashrec> Cashrecs { get; set; } // cashrec
        DbSet<Chain> Chains { get; set; } // Chains
        DbSet<Country> Countries { get; set; } // Country
        DbSet<Createorder> Createorders { get; set; } // createorder
        DbSet<Credit> Credits { get; set; } // credit
        DbSet<CustomLookup> CustomLookups { get; set; } // CustomLookup
        DbSet<DailySummary> DailySummaries { get; set; } // DailySummary
        DbSet<DailysummaryCubeVw> DailysummaryCubeVws { get; set; } // dailysummary_cube_vw
        DbSet<DailysummaryVw> DailysummaryVws { get; set; } // dailysummary_vw
        DbSet<DeleteId> DeleteIds { get; set; } // Delete_IDs
        DbSet<DeleteTheDate> DeleteTheDates { get; set; } // Delete_TheDate
        DbSet<Displaybar> Displaybars { get; set; } // displaybar
        DbSet<Employee> Employees { get; set; } // employees
        DbSet<Group> Groups { get; set; } // Groups
        DbSet<GroupsExt> GroupsExts { get; set; } // Groups_Ext
        DbSet<HourlyServiceMetric> HourlyServiceMetrics { get; set; } // HourlyServiceMetrics
        DbSet<KfcCreateorder> KfcCreateorders { get; set; } // kfc_createorder
        DbSet<MenucontentsOld> MenucontentsOlds { get; set; } // menucontents_old
        DbSet<MenuItemsMapping> MenuItemsMappings { get; set; } // MenuItemsMapping
        DbSet<MenuOld> MenuOlds { get; set; } // menu_old
        DbSet<MetricsVw> MetricsVws { get; set; } // metrics_vw
        DbSet<OrderDetails3Vw> OrderDetails3Vw { get; set; } // OrderDetails3_vw
        DbSet<Orderstatu> Orderstatus { get; set; } // orderstatus
        DbSet<Ordertype> Ordertypes { get; set; } // ordertype
        DbSet<OtdVw> OtdVws { get; set; } // OTD_vw
        DbSet<PafSector> PafSectors { get; set; } // PAFSectors
        DbSet<Payroll> Payrolls { get; set; } // payroll
        DbSet<Paytype> Paytypes { get; set; } // Paytypes
        DbSet<PollingMonitorTranslation> PollingMonitorTranslations { get; set; } // PollingMonitor_Translation
        DbSet<Primarycat> Primarycats { get; set; } // primarycat
        DbSet<Ramesesaudit> Ramesesaudits { get; set; } // ramesesaudit
        DbSet<Recipe> Recipes { get; set; } // Recipe
        DbSet<Recipegroup> Recipegroups { get; set; } // recipegroups
        DbSet<Scheduling> Schedulings { get; set; } // scheduling
        DbSet<Secondarycat> Secondarycats { get; set; } // secondarycat
        DbSet<Servicesetting> Servicesettings { get; set; } // servicesettings
        DbSet<Store> Stores { get; set; } // stores
        DbSet<Storecatagory> Storecatagories { get; set; } // storecatagory
        DbSet<Storecatagorytogroup> Storecatagorytogroups { get; set; } // storecatagorytogroup
        DbSet<Storegroup> Storegroups { get; set; } // storegroup
        DbSet<Storegroup1> Storegroup1 { get; set; } // storegroups
        DbSet<sys_DatabaseFirewallRule> sys_DatabaseFirewallRules { get; set; } // database_firewall_rules
        DbSet<sys_ScriptDeployment> sys_ScriptDeployments { get; set; } // script_deployments
        DbSet<sys_ScriptDeploymentStatus> sys_ScriptDeploymentStatus { get; set; } // script_deployment_status
        DbSet<Testorder> Testorders { get; set; } // testorder

        int SaveChanges();
        System.Threading.Tasks.Task<int> SaveChangesAsync();
        System.Threading.Tasks.Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
        // Stored Procedures
        int AmsCreateMissingMenuMappings();
        int AmsGetAllCustomersDetailsReport(string stores);
        List<AmsGetCostOfSalesReportReturnModel> AmsGetCostOfSalesReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, int? period, out int procResult);
        int AmsGetCustomerDeliveryReport(DateTime? fromDate, DateTime? toDate, string stores);
        int AmsGetCustomersDetailsReport(string quickDateRange, DateTime? fromDate, DateTime? toDate, string stores);
        int AmsGetCustomersOrderPatternReport(DateTime? fromdate, DateTime? todate, string stores);
        int AmsGetCyoReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores);
        List<AmsGetDailyTotalsReturnModel> AmsGetDailyTotals(DateTime? startdate, DateTime? toDate, string storeId, out int procResult);
        int AmsGetDailyTotalsPart2(DateTime? startdate, DateTime? toDate, string storeId);
        int AmsGetDealsReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores);
        int AmsGetDeliverycommissionReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores);
        List<AmsGetDiscountCancellationsDetailsReortReturnModel> AmsGetDiscountCancellationsDetailsReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult);
        List<AmsGetDiscountCancellationsReortReturnModel> AmsGetDiscountCancellationsReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult);
        int AmsGetDriverDeliveries(DateTime? fromdate, DateTime? todate, string stores);
        int AmsGetDriversPerfomanceReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores);
        List<AmsGetHcasReportReturnModel> AmsGetHcasReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult);
        List<AmsGetIncompleteOrdersReortReturnModel> AmsGetIncompleteOrdersReort(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult);
        int AmsGetInv03Report(string quickDateRange, DateTime? fromdate, DateTime? todate, int? period, string stores, string recipeItems);
        int AmsGetInv03Summary(string quickDateRange, DateTime? fromdate, DateTime? todate, int? period, string stores, string recipeItems);
        int AmsGetinv04Report(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, string recipeItems);
        int AmsGetInv04Summary(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, string recipeItems);
        int AmsGetinv02Report(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, string recipeItems);
        List<AmsGetInvRecipeGroupsReturnModel> AmsGetInvRecipeGroups(string stores, out int procResult);
        int AmsGetMarketingReport(DateTime? fromdate, DateTime? todate, string stores);
        int AmsGetNewCustomersReport(string quickDateRange, DateTime? fromDate, DateTime? toDate, string stores);
        int AmsGetOrderDeliveryStatistics(DateTime? fromdate, DateTime? todate, string stores, string cityparam);
        int AmsGetOrderDeliveryTimesReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores);
        int AmsGetOrderDetailReport(DateTime? seldate, string storeid, string orderNum);
        int AmsGetOrderedStockreport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, string recipeItems);
        int AmsGetOrderItemsReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores);
        int AmsGetOrderLateDeliveryReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, int? timeInMinutes);
        int AmsGetPayrollReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores);
        int AmsGetPayrollSummaryReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores);
        int AmsGetPizzabysizeReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores);
        int AmsGetProductMixDetailsReport(string quickDateRange, DateTime? fromDate, DateTime? toDate, string stores, string menuItems);
        int AmsGetProductMixQueryReport(string quickDateRange, DateTime? fromDate, DateTime? toDate, string stores);
        List<AmsGetQuickDatesReturnModel> AmsGetQuickDates( out int procResult);
        List<AmsGetRecipeGroupsByStoreReturnModel> AmsGetRecipeGroupsByStore(string stores, string enabledisable, out int procResult);
        List<AmsGetRecipeItemsByStoreReturnModel> AmsGetRecipeItemsByStore(string stores, string enabledItems, string groupId, out int procResult);
        List<AmsGetSalesAndServiceReportReturnModel> AmsGetSalesAndServiceReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult);
        int AmsGetSalesByArticle2(DateTime? startDate, DateTime? endDate, string stores);
        int AmsGetSalesByFamily2(DateTime? fromDate, DateTime? toDate, string stores);
        int AmsGetSalesbyPostCodeSectorreport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores);
        int AmsGetSalesbysourcedetailsReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores);
        List<AmsGetSalesByStoreReportReturnModel> AmsGetSalesByStoreReport(string quickDateRange, DateTime? fromdate, DateTime? todate, out int procResult);
        int AmsGetSalesByTaxDetailsReport(DateTime? seldate, string store);
        List<AmsGetSalesByTaxPercentReportReturnModel> AmsGetSalesByTaxPercentReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult);
        List<AmsGetSalesByWeekReportReturnModel> AmsGetSalesByWeekReport(DateTime? fromDate, DateTime? toDate, out int procResult);
        int AmsGetSalesSummary(DateTime? startDate, DateTime? endDate, string stores);
        List<AmsGetServiceAnalysisDetailsReportReturnModel> AmsGetServiceAnalysisDetailsReport(DateTime? seldate, string store, out int procResult);
        List<AmsGetStaffAverageSpendReportReturnModel> AmsGetStaffAverageSpendReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult);
        List<AmsGetStoreGroupsReturnModel> AmsGetStoreGroups( out int procResult);
        List<AmsGetStoresByGroupsReturnModel> AmsGetStoresByGroups(string masterId, out int procResult);
        int AmsGetTotalsByHourReport2(DateTime? fromdate, DateTime? todate, string stores);
        int AmsGetTranslation(string translationKey, out string newTranslation);
        List<AmsGetWeeklyPayrollReportReturnModel> AmsGetWeeklyPayrollReport(string quickDateRange, DateTime? fromdate, DateTime? todate, string stores, out int procResult);
        List<AmsHelperCanxOrdersReturnModel> AmsHelperCanxOrders(int? storeId, DateTime? businessDate, int? orderNum, out int procResult);
        List<AmsHomePageGetCompStoresReturnModel> AmsHomePageGetCompStores( out int procResult);
        List<AmsHomePageServiceSummaryReturnModel> AmsHomePageServiceSummary( out int procResult);
        int AmsSubGetCustomersOrderPatternReport(DateTime? fromdate, DateTime? todate, string typeRequired, int? storeid, out double? value);
        int AmsUpdateProcess(int? nStoreId, DateTime? processDate);
        int AmsUpdateStores(int? androSiteId, string custSiteId, string name, string storeStatus, int? countryKey);
        List<AmsspCompstorespollingserviceReturnModel> AmsspCompstorespollingservice( out int procResult);
        int AmsspDellocaldeals(int? storeid);
        int AmsspDelmenu(int? storeid);
        List<AmsspGetallmenusiteidsReturnModel> AmsspGetallmenusiteids(int? storeid, out int procResult);
        List<AmsspGetamsidReturnModel> AmsspGetamsid( out int procResult);
        List<AmsspGetmastermenuidReturnModel> AmsspGetmastermenuid( out int procResult);
        int AmsspGetmissingstores(int? days);
        List<AmsspGetservicesettingsReturnModel> AmsspGetservicesettings( out int procResult);
        List<AmsspGetstoresReturnModel> AmsspGetstores( out int procResult);
        int AmsspSelpollingstatus();
        List<AmsspUpdateservicesettingsReturnModel> AmsspUpdateservicesettings(DateTime? nextrun, int? overridesitelist, DateTime? datestamp, out int procResult);
        int AmsspUpdatestorelastupdate(int? storeid);
        int AmsspUpdmastermenu(int? storeid, int? version);
        int AndroAmsSpDeliverycommission(DateTime? fromdate, DateTime? todate, string stores);
        List<AndroAmsSpDiscountCancellationsReortReturnModel> AndroAmsSpDiscountCancellationsReort(DateTime? fromdate, DateTime? todate, string stores, out int procResult);
        int AndroAmsSpDrv02(DateTime? fromdate, DateTime? todate, string stores);
        int AndroAmsSpOrderDeliveryTimes(DateTime? fromdate, DateTime? todate, string stores);
        int AndroAmsSpStaffAverageSpendReport(DateTime? fromdate, DateTime? todate, string stores);
        int FixNulls();
        List<GetIndexFragmentationReturnModel> GetIndexFragmentation(string tableName, out int procResult);
    }

}
