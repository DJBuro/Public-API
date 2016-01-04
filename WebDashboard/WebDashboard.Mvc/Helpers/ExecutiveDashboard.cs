using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebDashboard.Dao.Domain;
using WebDashboard.Dao.Domain.Helpers;
using System.Diagnostics;
using WebDashboard.Dao;

namespace WebDashboard.Mvc.Helpers
{
    public static class ExecutiveDashboard
    {
        public static ExecutiveGroupDashboard GetGroupData(
            HeadOffice headOffice, 
            IList<HeadOffice> group, 
            IList<GroupExchangeRate> groupExchangeRates,
            DateTime? forDateTime,
            IHistoricalDataDao historicalDataDao)
        {
            ExecutiveGroupDashboard executiveGroupDashboard = new ExecutiveGroupDashboard();

            double salesTodayStoreCount = 0;
            double salesLastWeekStoreCount = 0;
            double salesLYValueStoreCount = 0;
            double ordersTodayCountStoreCount = 0;
            double driversWorkCountStoreCount = 0;
            double driversRoadCountStoreCount = 0;
            double compSalesTodayValueStoreCount = 0;
            double compOrdersTodayCountStoreCount = 0;
            double oprCountStoreCount = 0;
            int storeCountStoreCount = 0;
            
            int totalOrdersLastWeek = 0;
            int totalOrdersLastYear = 0;
            
            List<Site> groupSites = new List<Site>();
            int allSitesReportingTodayCount = 0;
            
            // Get data for each company in the group
            foreach (HeadOffice company in group)
            {
                // Is the company part of the group?
                if (company.GroupId.HasValue &&
                    company.GroupId.Value == headOffice.GroupId.Value)
                {
                    // Set the exchange rate for the company
                    Calculations.SetCompanyExchangeRate(company, groupExchangeRates);

                    // Get historic data for this head office and trading day
                    IDictionary<int, HistoricalData> historicData = null;
                    if (forDateTime.HasValue)
                    {
                        historicData = historicalDataDao.FindByHeadOfficeIdAndTradingDay(company.Id.Value, forDateTime.Value);
                    }

                    // Add all the sites for this region
                    List<Site> sites = new List<Site>();
                    foreach (Site site in company.Sites)
                    {
                        ExecutiveDashboard.AddSite(forDateTime, site, historicData, sites);
                    }

                    // Zero values for sites that have not reported today 
                    // Are we displaying live data?
                    if (forDateTime == null)
                    {
                        // We are displaying live data so we should zero data for stores
                        // that haven't reported today
                        Calculations.CleanupData(sites, true);
                    }
                    
                    int sitesReportingTodayCount = 0;
                    
                    // Add all sites in this company to the list of sites in the group of companies                       
                    foreach (Site site in sites) //company.Sites)
                    {
                        // Only use sites that are reporting today
                        if (site.IsReportingToday)
                        {
                            // Set the sites exchange rate to be the same as the company that's it's a part of
                            site.ExchangeRate = company.ExchangeRate;
                            
                            // Keep a count of the reporting stores
                            sitesReportingTodayCount++;

                            groupSites.Add(site);
                        }
                    }

                    // Keep a count of all stores reporting in the entire group of companies
                    allSitesReportingTodayCount += sitesReportingTodayCount;

                    // Do calculations for the company
                    ExecutiveCompanyDashboard executiveCompanyDashboard = ExecutiveDashboard.GetCompanyData(company, (IList<Site>)sites); //company.Sites);
                  
                    // Do total and average calculations and pre-calculations
                    // >>> Sales <<<
                    
                    // Today £ (Total)
                    if (executiveCompanyDashboard.Company.Sales > 0)
                    {
                        salesTodayStoreCount += sitesReportingTodayCount;
                        executiveGroupDashboard.SalesTodayValueTotal += executiveCompanyDashboard.Company.Sales;
                    }
                    
                    // LW £ (Total)
                    if (executiveCompanyDashboard.Company.SalesLastWeek > 0) 
                    {
                        salesLastWeekStoreCount += sitesReportingTodayCount;
                        executiveGroupDashboard.SalesLWValueTotal += executiveCompanyDashboard.Company.SalesLastWeek;
                    }
                    
                    // LY £ (Total)
                    if (executiveCompanyDashboard.Company.SalesLastYear > 0)
                    {
                        salesLYValueStoreCount += sitesReportingTodayCount;
                        executiveGroupDashboard.SalesLYValueTotal += executiveCompanyDashboard.Company.SalesLastYear;
                    }                    

                    // >>> Orders <<<
                    
                    // Today (Total)
                    if (executiveCompanyDashboard.Company.Orders > 0)
                    {
                        ordersTodayCountStoreCount += sitesReportingTodayCount;
                        executiveGroupDashboard.OrdersTodayCountTotal += executiveCompanyDashboard.Company.Orders;
                    }

                    // LW % (Total)
                    totalOrdersLastWeek += executiveCompanyDashboard.Company.OrdersLastWeek;

                    // LY % (Total)
                    totalOrdersLastYear += executiveCompanyDashboard.Company.OrdersLastYear;
                    
                    // >>> Drivers <<<
                    
                    // Work (Total)
                    if (executiveCompanyDashboard.Company.Drivers > 0)
                    {
                        driversWorkCountStoreCount += sitesReportingTodayCount;
                        executiveGroupDashboard.DriversWorkCountTotal += executiveCompanyDashboard.Company.Drivers;
                    }

                    // Road (Total)
                    if (executiveCompanyDashboard.Company.DriversDelivering > 0)
                    {
                        driversRoadCountStoreCount += sitesReportingTodayCount;
                        executiveGroupDashboard.DriversRoadCountTotal += executiveCompanyDashboard.Company.DriversDelivering;
                    }

                    // >>> Comp Sales <<<

                    // Today (Total), LY £ (Total)
                    foreach (Site site in sites) //company.Sites)
                    {
                        if (site.Comp)
                        {
                            compSalesTodayValueStoreCount++;                           
                        }
                    }
                    executiveGroupDashboard.CompSalesTodayValueTotal += executiveCompanyDashboard.CompCompany.Sales;
                    executiveGroupDashboard.CompSalesLYValueTotal += executiveCompanyDashboard.CompCompany.SalesLastYear;

                    // >>> Comp Orders <<<

                    // Today (Total), LY # (Total)
                    foreach (Site site in sites) //company.Sites)
                    {
                        if (site.Comp)
                        {
                            compOrdersTodayCountStoreCount++;
                        }
                    }
                    executiveGroupDashboard.CompOrdersTodayCountTotal += executiveCompanyDashboard.CompCompany.Orders;
                    executiveGroupDashboard.CompOrdersLYCountTotal += executiveCompanyDashboard.CompCompany.OrdersLastYear;

                    // OPR
                    if (executiveCompanyDashboard.PerStoreAverage.OPR > 0)
                    {
                        oprCountStoreCount++;
                        executiveGroupDashboard.OPRCountTotal += executiveCompanyDashboard.PerStoreAverage.OPR;
                    }
                    
                    executiveGroupDashboard.AvgSpendValueTotal += executiveCompanyDashboard.PerStoreAverage.AverageSpend;

                    // Store Count
                    if (executiveCompanyDashboard.StoreCount > 0) 
                    {
                        storeCountStoreCount++;
                        executiveGroupDashboard.StoreCountTotal += executiveCompanyDashboard.StoreCount;
                    }
                    
                    executiveGroupDashboard.CompanyDashboards.Add(executiveCompanyDashboard);
                }
            }

            // >>> Sales <<<
            
            // Today (Average)
            if (executiveGroupDashboard.SalesTodayValueTotal > 0 && allSitesReportingTodayCount > 0)
            {
                executiveGroupDashboard.SalesTodayValueAverage = executiveGroupDashboard.SalesTodayValueTotal / allSitesReportingTodayCount;
            }

            // LW £ (Average)
            if (executiveGroupDashboard.SalesLWValueTotal > 0 && allSitesReportingTodayCount > 0)
            {
                executiveGroupDashboard.SalesLWValueAverage = executiveGroupDashboard.SalesLWValueTotal / allSitesReportingTodayCount;
            }

            // LW % (Total)
            executiveGroupDashboard.SalesLWPercentageTotal = Calculations.GetAverageTotalForColumn(groupSites, (int)ColumnsEnum.NetSalesLastWeek, true);
            executiveGroupDashboard.SalesLWPercentageTotal = Calculations.GetComparableValues(executiveGroupDashboard.SalesLWValueTotal, executiveGroupDashboard.SalesTodayValueTotal);

            // LY £ (Average)
            if (executiveGroupDashboard.SalesLYValueTotal > 0 && allSitesReportingTodayCount > 0)
            {
                executiveGroupDashboard.SalesLYValueAverage = executiveGroupDashboard.SalesLYValueTotal / allSitesReportingTodayCount;
            }

            // LY % (Total)
            executiveGroupDashboard.SalesLYPercentageTotal = Calculations.GetComparableValues(executiveGroupDashboard.SalesLYValueTotal, executiveGroupDashboard.SalesTodayValueTotal);
            
            // >>> Orders <<<
            
            // Today (Average)
            if (executiveGroupDashboard.OrdersTodayCountTotal > 0 && allSitesReportingTodayCount > 0)
            {
                executiveGroupDashboard.OrdersTodayCountAverage = executiveGroupDashboard.OrdersTodayCountTotal / allSitesReportingTodayCount;
            }

            // LW % (Total)
            executiveGroupDashboard.OrdersLWPercentageTotal = Calculations.GetComparableValues(totalOrdersLastWeek, executiveGroupDashboard.OrdersTodayCountTotal);
           
            // LY % (Total)
            executiveGroupDashboard.OrdersLYPercentageTotal = Calculations.GetComparableValues(totalOrdersLastYear, executiveGroupDashboard.OrdersTodayCountTotal);

            // >>> Drivers <<<
            
            // Work (Average)
            if (executiveGroupDashboard.DriversWorkCountTotal > 0 && driversWorkCountStoreCount > 0)
            {
                executiveGroupDashboard.DriversWorkCountAverage = executiveGroupDashboard.DriversWorkCountTotal / driversWorkCountStoreCount;
            }

            // Road (Average)
            if (executiveGroupDashboard.DriversRoadCountTotal > 0 && driversRoadCountStoreCount > 0)
            {
                executiveGroupDashboard.DriversRoadcountAverage = executiveGroupDashboard.DriversRoadCountTotal / driversRoadCountStoreCount;
            }

            // >>> Comp Sales <<<
            
            // Today (Average)
            if (executiveGroupDashboard.CompSalesTodayValueTotal > 0 && compSalesTodayValueStoreCount > 0)
            {
                executiveGroupDashboard.CompSalesTodayValueAverage = executiveGroupDashboard.CompSalesTodayValueTotal / compSalesTodayValueStoreCount;
            }

            // LY £ (Average)            
            if (executiveGroupDashboard.CompSalesLYValueTotal > 0 && compSalesTodayValueStoreCount > 0)
            {
                executiveGroupDashboard.CompSalesLYValueAverage = executiveGroupDashboard.CompSalesLYValueTotal / compSalesTodayValueStoreCount;
            }

            // LY % (Total)
            executiveGroupDashboard.CompSalesLYPercentageTotal = Calculations.GetComparableValues(executiveGroupDashboard.CompSalesLYValueTotal, executiveGroupDashboard.CompSalesTodayValueTotal);
            
            // >>> Comp Orders <<<
            
            // Today (Average)
            if (executiveGroupDashboard.CompOrdersTodayCountTotal > 0 && compOrdersTodayCountStoreCount > 0)
            {
                executiveGroupDashboard.CompOrdersTodayCountAverage = executiveGroupDashboard.CompOrdersTodayCountTotal / compOrdersTodayCountStoreCount;
            }

            // LY # (Average)
            if (executiveGroupDashboard.CompOrdersLYCountTotal > 0 && compOrdersTodayCountStoreCount > 0)
            {
                executiveGroupDashboard.CompOrdersLYCountAverage = executiveGroupDashboard.CompOrdersLYCountTotal / compOrdersTodayCountStoreCount;
            }

            // LY % (Total)
            executiveGroupDashboard.CompOrdersLYPercentageTotal = Calculations.GetComparableValues(executiveGroupDashboard.CompOrdersLYCountTotal, executiveGroupDashboard.CompOrdersTodayCountTotal);

            // >>> Times <<<

            // <30 (Total)
            executiveGroupDashboard.DelTimeLT30MinAverage = ExecutiveDashboard.CalculateLessThan30Min(groupSites);

            // <45 (Total)
            executiveGroupDashboard.DelTimeLT45MinAverage = ExecutiveDashboard.CalculateLessThan45Min(groupSites);
            
            // Make (Total)
            executiveGroupDashboard.MakeMinsAverage = ExecutiveDashboard.CalculateMake(groupSites);

            // OTD (Total)
            executiveGroupDashboard.OTDMinsAverage = ExecutiveDashboard.CalculateOutTheDoor(groupSites);
            
            // TTD (Total)
            executiveGroupDashboard.TTDMinsAverage = ExecutiveDashboard.CalculateToTheDoor(groupSites);

            // Drive (Total)
            executiveGroupDashboard.DriveMinsAverage = ExecutiveDashboard.CalculateAverageDriverTime(groupSites);

            // OPR
            executiveGroupDashboard.OPRCountAverage = ExecutiveDashboard.CalculateOPR(groupSites);
            
            // Avg Spend
            executiveGroupDashboard.AvgSpendValueAverage = ExecutiveDashboard.CalculateAverageSpend(groupSites);
            
            // Store Count
            executiveGroupDashboard.UploadingStoreCountTotal = allSitesReportingTodayCount;

            if (executiveGroupDashboard.UploadingStoreCountTotal > 0 && storeCountStoreCount > 0)
            {
                executiveGroupDashboard.UploadingStoreCountAverage = executiveGroupDashboard.UploadingStoreCountTotal / storeCountStoreCount;
            }

            if (executiveGroupDashboard.StoreCountTotal > 0 && storeCountStoreCount > 0)
            {
                executiveGroupDashboard.StoreCountAverage = executiveGroupDashboard.StoreCountTotal / storeCountStoreCount;
            }

            executiveGroupDashboard.CompanyDashboards.Sort();
            
            return executiveGroupDashboard;
        }

        public static ExecutiveCompanyDashboard GetCompanyData(HeadOffice headOffice, IList<Site> allSites)
        {
            var exDashboard = new ExecutiveCompanyDashboard();

            exDashboard.Company = ExecutiveDashboard.GetExecutiveData(exDashboard.Company, allSites);
            exDashboard.CompCompany = ExecutiveDashboard.GetCompExecutiveData(exDashboard.CompCompany, allSites);
            exDashboard.PerStoreAverage = ExecutiveDashboard.GetExecutiveDataCompanyAverages(exDashboard.PerStoreAverage, allSites);
            exDashboard.StoreCount = allSites.Count;

            exDashboard.Company.CompanyId = headOffice.Id.Value;

            // Get the number of stores that have uploaded recently
            int compWithSalesCount = 0;
            int compCount = 0;

            foreach (Site site in allSites)
            {
                if (site.LastUpdated != null)
                {
                    TimeSpan timeSpan = DateTime.Now - site.LastUpdated.Value;

                    // Has the store uploaded in the last 10 mins?
                    if (site.IsReportingToday) // timeSpan.TotalMinutes <= 10)
                    {
                        exDashboard.UploadingStoreCount ++;
                    }
                }

                if (site.Comp)
                {
                    if (site.Column18 > 0)
                    {
                        compWithSalesCount++;
                    }

                    compCount++;
                }
            }

            exDashboard.Company.Name = headOffice.Name;
            exDashboard.CompWithSalesCount = compWithSalesCount;
            exDashboard.CompCount = compCount;
            
            return exDashboard;
        }

        public static ExecutiveRegionDashboard GetRegionalData(HeadOffice headOffice, IList<Region> allRegions)
        {
            var exDashboard = new ExecutiveRegionDashboard();
            exDashboard.RegionalList = new List<ExecutiveRegionDashboard>();

            foreach (var region in allRegions)
            {
                var regionDashboard = new ExecutiveRegionDashboard();

                var allRegionalSites = region.RegionalSites;

                regionDashboard.Region.Name = region.Name;
                regionDashboard.RegionId = region.Id.Value;
                regionDashboard.RegionStoreCount = allRegionalSites.Count;

                regionDashboard.Region = GetExecutiveData(regionDashboard.Region, allRegionalSites);
                regionDashboard.CompRegion = GetCompExecutiveData(regionDashboard.CompRegion, allRegionalSites);
                regionDashboard.PerStoreAverage = GetExecutiveDataCompanyAverages(regionDashboard.PerStoreAverage, allRegionalSites);

                // Get the number of stores that have uploaded recently
                int compWithSalesCount = 0;
                int compCount = 0;

                foreach (Site site in allRegionalSites)
                {
                    if (site.Comp)
                    {
                        if (site.Column18 > 0)
                        {
                            compWithSalesCount++;
                        }

                        compCount++;
                    }
                }

                regionDashboard.CompWithSalesCount = compWithSalesCount;
                regionDashboard.CompCount = compCount;

                exDashboard.RegionalList.Add(regionDashboard);
            }

            return exDashboard;
        }

        public static ExecutiveStoreDashboard GetStoreData(IList<Site> allSites)
        {
            var exDashboard = new ExecutiveStoreDashboard();
            exDashboard.StoreList = new List<ExecutiveData>();

            foreach (var site in allSites)
            {
                var storeDashboard = new ExecutiveStoreDashboard();

                var executiveData = GetExecutiveData(storeDashboard.Store, site);

                executiveData.Name = site.Name;
                executiveData.LastUpdated = site.LastUpdated;

                exDashboard.StoreList.Add(executiveData);        
            }

            return exDashboard;
        }

        private static ExecutiveData GetExecutiveData(ExecutiveData executiveData, IList<Site> allSites)
        {
            executiveData.Orders = (int)Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.FlashTickets, false); //Count
            executiveData.OrdersLastWeek = (int)Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.OrdersLastWeek, false); //Count; 
            executiveData.OrdersLastYear = (int)Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.OrdersLastYear, false); //Count; 

            executiveData.OrdersVsLastWeek = Calculations.GetComparableValues(executiveData.OrdersLastWeek, executiveData.Orders);
            executiveData.OrdersVsLastYear = Calculations.GetComparableValues(executiveData.OrdersLastYear, executiveData.Orders);

            executiveData.Sales = (Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.FlashSalesNetVAT, true)) / 100;
            executiveData.SalesLastWeek = (Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.NetSalesLastWeek, true)) / 100;
            executiveData.SalesLastYear = (Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.NetSalesLastYear, true)) / 100;

            executiveData.SalesVsLastWeek = Calculations.GetComparableValues(executiveData.SalesLastWeek, executiveData.Sales);
            executiveData.SalesVsLastYear = Calculations.GetComparableValues(executiveData.SalesLastYear, executiveData.Sales);

            executiveData.Drivers = (int)Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.TotalDeliverySales, false);
            executiveData.DriversDelivering = executiveData.Drivers - (int)Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.DeliveredInLessThan35Min, false);

            executiveData.AverageDriveTime = (int)Calculations.GetComputedAverageForColumnAgainstValue(allSites, ColumnsEnum.AverageDriveTime, 60, Calculations.Condition.Divide, false); //Time (divide againt 60)

            ExecutiveDashboard.CalculateCompResults(executiveData, allSites);

            return executiveData;
        }
        
        private static void CalculateCompResults(ExecutiveData executiveData, IList<Site> allSites)
        {
            double totalCompSales = 0;
            double totalCompSalesLastYear = 0;
            double totalCompOrders = 0;
            double totalCompOrdersLastYear = 0;

            // Caclulate totals
            foreach (Site site in allSites)
            {
                // Were there any sales last year?
                if (site.Comp && site.NetSalesLastYear > 0)
                {
                    totalCompSales += site.FlashSalesNetVAT * site.HeadOffice.ExchangeRate;
                    totalCompSalesLastYear += site.NetSalesLastYear * site.HeadOffice.ExchangeRate;

                    totalCompOrders += site.FlashTickets;
                    totalCompOrdersLastYear += site.OrdersLastYear;
                }
            }

            executiveData.CompSales = totalCompSales / 100;
            executiveData.CompSalesVsLastYear = Calculations.GetComparableValues(totalCompSalesLastYear, totalCompSales, 1);
            executiveData.CompOrders = totalCompOrders;
            executiveData.CompOrdersVsLastYear = Calculations.GetComparableValues(totalCompOrdersLastYear, totalCompOrders, 1);
        }

        private static ExecutiveData GetCompExecutiveData(ExecutiveData executiveData, IList<Site> allSites)
        {
            executiveData.Orders = (int)Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.FlashTickets, true, false); //Count
            executiveData.OrdersLastWeek = (int)Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.OrdersLastWeek, true, false); //Count; 
            executiveData.OrdersLastYear = (int)Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.OrdersLastYear, true, false); //Count; 

            executiveData.OrdersVsLastWeek = Calculations.GetComparableValues(executiveData.OrdersLastWeek, executiveData.Orders);
            executiveData.OrdersVsLastYear = Calculations.GetComparableValues(executiveData.OrdersLastYear, executiveData.Orders);

            executiveData.Sales = (Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.FlashSalesNetVAT, true, true)) / 100;
            executiveData.SalesLastWeek = (Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.NetSalesLastWeek, true, true)) / 100;
            executiveData.SalesLastYear = (Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.NetSalesLastYear, true, true)) / 100;

            executiveData.SalesVsLastWeek = Calculations.GetComparableValues(executiveData.SalesLastWeek, executiveData.Sales);
            executiveData.SalesVsLastYear = Calculations.GetComparableValues(executiveData.SalesLastYear, executiveData.Sales);

            executiveData.Drivers = (int)Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.TotalDeliverySales, true, false);
            executiveData.DriversDelivering = executiveData.Drivers - (int)Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.DeliveredInLessThan35Min, true, false);

            executiveData.AverageDriveTime = (int)Calculations.GetComputedAverageForColumnAgainstValue(allSites, ColumnsEnum.AverageDriveTime, 60, Calculations.Condition.Divide, true, false); //Time (divide againt 60)

            ExecutiveDashboard.CalculateCompResults(executiveData, allSites);

            return executiveData;
        }

        private static ExecutiveData GetExecutiveData(ExecutiveData executiveData, Site site)
        {
            if(site.Column16 > 0)
            {
                executiveData.Sales = ((double)site.Column16 * site.ExchangeRate) / 100;            
            }

            executiveData.SalesLastWeek = ((double)site.Column17 * site.ExchangeRate) / 100;
            executiveData.SalesLastYear = ((double)site.Column18 * site.ExchangeRate) / 100;

            executiveData.SalesVsLastWeek = Calculations.GetComparableValues(executiveData.SalesLastWeek, executiveData.Sales);
            executiveData.SalesVsLastYear = Calculations.GetComparableValues(executiveData.SalesLastYear, executiveData.Sales);

            executiveData.Orders = site.Column19; //Count
            executiveData.OrdersLastWeek = site.Column14;//Count
            executiveData.OrdersLastYear = site.Column15;//Count

            executiveData.OrdersVsLastWeek = Calculations.GetComparableValues(executiveData.OrdersLastWeek, executiveData.Orders);
            executiveData.OrdersVsLastYear = Calculations.GetComparableValues(executiveData.OrdersLastYear, executiveData.Orders);

            executiveData.LessThan30Min = Calculations.GetComputedAverageForColumn(site, ColumnsEnum.DeliveredInLessThan30Min, ColumnsEnum.TotalDeliveryOrders, Calculations.Condition.Divide, false, false) * 100; // % calculated value (divide agains col 3)
            executiveData.LessThan45Min = Calculations.GetComputedAverageForColumn(site, ColumnsEnum.DeliveredInLessThan45Min, ColumnsEnum.TotalDeliveryOrders, Calculations.Condition.Divide, false, false) * 100;//% calculated value (divide agains col 3)

            executiveData.Make = (int)Calculations.GetComputedAverageForColumnAgainstValue(site, (int)ColumnsEnum.Make, 60, Calculations.Condition.Divide, false); //Time (divide againt 60)
            executiveData.OTD = (int)Calculations.GetComputedAverageForColumnAgainstValue(site, (int)ColumnsEnum.OTDInstore, 60, Calculations.Condition.Divide, false); //Time (divide againt 60)
            executiveData.TTD = (int)Calculations.GetComputedAverageForColumnAgainstValue(site, (int)ColumnsEnum.ToTheDoorTime, 60, Calculations.Condition.Divide, false); //Time (divide againt 60)

            executiveData.Drivers = site.Column2;//Count
            executiveData.DriversDelivering = site.Column2 - site.Column6; //Count

            if(site.Column13 > 0)
                executiveData.OPR = Math.Round(((double)site.Column13 / 100), 1); //Count

            executiveData.AverageDriveTime = (int)Calculations.GetComputedAverageForColumnAgainstValue(site, 10, 60, Calculations.Condition.Divide, false); //Time (divide againt 60)
            executiveData.AverageSpend = Math.Round(Calculations.GetComputedAverageForColumn(site, ColumnsEnum.FlashSalesNetVAT, ColumnsEnum.FlashTickets, Calculations.Condition.Divide, true, false) / 100, 2); //calculated value (divide against col 20)

            return executiveData;
        }

        private static ExecutiveData GetExecutiveDataCompanyAverages(ExecutiveData executiveData, IList<Site> sites)
        {
            // Sales
            executiveData.Sales = ExecutiveDashboard.CalculateAverageSalesMoneyPerStore(sites);
            
            // Orders
            executiveData.Orders = ExecutiveDashboard.CalculateOrders(sites);

            // < 30 mins
            executiveData.LessThan30Min = ExecutiveDashboard.CalculateLessThan30Min(sites);
            
            // < 45 mins
            executiveData.LessThan45Min = ExecutiveDashboard.CalculateLessThan45Min(sites);

            // Time to Make (amount of time since the order was taken to order was made)
            executiveData.Make = ExecutiveDashboard.CalculateMake(sites);

            // Out The Door (amount of time since the order was taken to order left the shop)
            executiveData.OTD = ExecutiveDashboard.CalculateOutTheDoor(sites);

            // To The Door (amount of time since the order was taken to order was delivered)
            executiveData.TTD = ExecutiveDashboard.CalculateToTheDoor(sites);

            // Average Drive Time
            executiveData.AverageDriveTime = ExecutiveDashboard.CalculateAverageDriverTime(sites);

            // Drivers
            executiveData.Drivers = ExecutiveDashboard.CalculateDrivers(sites);

            // Drivers Delivering
            executiveData.DriversDelivering = ExecutiveDashboard.CalculateDriversDelivering(sites);

            // Orders per run (how many orders on average the drivers took each time)
            executiveData.OPR = ExecutiveDashboard.CalculateOPR(sites);

            // Average Spend
            executiveData.AverageSpend = ExecutiveDashboard.CalculateAverageSpend(sites);

            return executiveData;
        }

#region Calculations

        private static double CalculateAverageSalesMoneyPerStore(IList<Site> allSites)
        {
            double totalSales = 0;
            int siteCount = 0;

            foreach (Site site in allSites)
            {
                int sales = ColumnData.GetColumnValue(site, (int)ColumnsEnum.FlashSalesNetVAT);
                
                if (sales != 0)
                {
                    totalSales += sales * site.ExchangeRate;
                    siteCount++;
                }
            }

            double totalSalesPence = 0;
            
            if (siteCount != 0)
            {
                totalSalesPence = totalSales / siteCount;
            }

            double totalSalesPounds = totalSalesPence / 100;

            return Math.Round(totalSalesPounds, 2);
        }

        private static double CalculateOrders(IList<Site> allSites)
        {
            double averageForColumn = Calculations.GetAverageForColumn(allSites, ColumnsEnum.FlashTickets, false);
            
            return Math.Round(averageForColumn, 1);
        }

        private static double CalculateLessThan30Min(IList<Site> allSites)
        {
            double computedAverageForColumn = Calculations.GetComputedAverageForColumn(
                allSites,
                ColumnsEnum.DeliveredInLessThan30Min,
                ColumnsEnum.TotalDeliveryOrders,
                Calculations.Condition.Divide,
                false);

            return computedAverageForColumn * 100; // % calculated value (divide agains col 3)
        }

        private static double CalculateLessThan45Min(IList<Site> allSites)
        {
            double computedAverageForColumn = Calculations.GetComputedAverageForColumn(
                allSites,
                ColumnsEnum.DeliveredInLessThan45Min,
                ColumnsEnum.TotalDeliveryOrders,
                Calculations.Condition.Divide,
                false);

            return computedAverageForColumn * 100;//% calculated value (divide agains col 3)
        }

        private static double CalculateMake(IList<Site> allSites)
        {
            double averageSeconds = Calculations.X(allSites, ColumnsEnum.TotalDeliveryOrders, ColumnsEnum.Make);
            double averageMinutes = averageSeconds / 60;

            return averageMinutes;
        }

        private static double CalculateOutTheDoor(IList<Site> allSites)
        {
            double averageSeconds = Calculations.X(allSites, ColumnsEnum.TotalDeliveryOrders, ColumnsEnum.OTDInstore);
            double averageMinutes = averageSeconds / 60;

            return averageMinutes;
        }

        private static double CalculateToTheDoor(IList<Site> allSites)
        {
            double averageSeconds = Calculations.X(allSites, ColumnsEnum.TotalDeliveryOrders, ColumnsEnum.ToTheDoorTime);
            double averageMinutes = averageSeconds / 60;

            return averageMinutes;
        }

        private static double CalculateAverageDriverTime(IList<Site> allSites)
        {
            double averageSeconds = Calculations.X(allSites, ColumnsEnum.TotalDeliveryOrders, ColumnsEnum.AverageDriveTime);
            double averageMinutes = averageSeconds / 60;

            return averageMinutes;
        }

        private static double CalculateDrivers(IList<Site> allSites)
        {
            return Calculations.GetTotalForColumn(allSites, ColumnsEnum.TotalDeliverySales, false) / allSites.Count;
        }

        private static double CalculateDriversDelivering(IList<Site> allSites)
        {
            double drivers = Calculations.GetTotalForColumn(allSites, ColumnsEnum.TotalDeliverySales, false);
            double x = Calculations.GetTotalForColumn(allSites, ColumnsEnum.DeliveredInLessThan35Min, false);

            double totalDriversDelivering = drivers - x;

            double averageDriversDelivering = totalDriversDelivering / allSites.Count;

            return Math.Round(averageDriversDelivering, 2);            
        }
        
        private static double CalculateOPR(IList<Site> allSites)
        {
            double average = Calculations.X(allSites, ColumnsEnum.TotalDeliveryOrders, ColumnsEnum.OrdersPerDeliveryDriver);
            average = average / 100;

            return average;
        }

        //private static int CalculateDriveTime(IList<Site> allSites)
        //{
        //    return (int)Calculations.GetComputedAverageForColumnAgainstValue(allSites, ColumnsEnum.AverageDriveTime, 60, Calculations.Condition.Divide, false); //Time (divide againt 60)
        //}
        
        private static double CalculateAverageSpend(IList<Site> allSites)
        {
            double totalSalesValue = Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.FlashSalesNetVAT, true);
            double totalOrderCount = Calculations.GetTotalForColumn(allSites, (int)ColumnsEnum.FlashTickets, false);

            double averageSpend = 0;
            
            if (totalOrderCount != 0)
            {
                double totalSalesPence = totalSalesValue / totalOrderCount;

                // Convert the average to pounds and pence
                double totalSalesPounds = totalSalesPence / 100;

                // Round to 2 decimal places
                averageSpend = Math.Round(totalSalesPounds, 2);
            }
            
            return averageSpend;
        }

#endregion Calculations

        public static void AddSite(DateTime? forDateTime, Site site, IDictionary<int, HistoricalData> historicData, IList<Site> sites)
        {
            // Are we showing live data or historic data?
            if (forDateTime.HasValue)
            {
                // We are showing historic data
                Site historicSite = new Site();
                historicSite.Id = site.Id;
                historicSite.Name = site.Name;
                historicSite.HeadOffice = site.HeadOffice;
                historicSite.ExchangeRate = site.ExchangeRate;
                historicSite.Enabled = site.Enabled;

                HistoricalData historicalData = null;

                // Do we have historic data for this store for the specified trading day?
                if (historicData.TryGetValue(site.SiteId, out historicalData))
                {
                    // Use the historic data
                    historicSite.IsReportingToday = true;
                    historicSite.Column1 = historicalData.Column1;
                    historicSite.Column2 = historicalData.Column2;
                    historicSite.Column3 = historicalData.Column3;
                    historicSite.Column4 = historicalData.Column4;
                    historicSite.Column5 = historicalData.Column5;
                    historicSite.Column6 = historicalData.Column6;
                    historicSite.Column7 = historicalData.Column7;
                    historicSite.Column8 = historicalData.Column8;
                    historicSite.Column9 = historicalData.Column9;
                    historicSite.Column10 = historicalData.Column10;
                    historicSite.Column11 = historicalData.Column11;
                    historicSite.Column12 = historicalData.Column12;
                    historicSite.Column13 = historicalData.Column13;
                    historicSite.Column14 = historicalData.Column14;
                    historicSite.Column15 = historicalData.Column15;
                    historicSite.Column16 = historicalData.Column16;
                    historicSite.Column17 = historicalData.Column17;
                    historicSite.Column18 = historicalData.Column18;
                    historicSite.Column19 = historicalData.Column19;
                    historicSite.Column20 = historicalData.Column20;
                    historicSite.LastUpdated = historicalData.LastUpdated;
                    historicSite.Comp = site.Comp;
                }
                else
                {
                    // We want to see the store in the list but we don't have historic data for it for this specific trading day
                    historicSite.IsReportingToday = false;

                    historicSite.Column1 = 0;
                    historicSite.Column2 = 0;
                    historicSite.Column3 = 0;
                    historicSite.Column4 = 0;
                    historicSite.Column5 = 0;
                    historicSite.Column6 = 0;
                    historicSite.Column7 = 0;
                    historicSite.Column8 = 0;
                    historicSite.Column9 = 0;
                    historicSite.Column10 = 0;
                    historicSite.Column11 = 0;
                    historicSite.Column12 = 0;
                    historicSite.Column13 = 100; // Orders per delivery driver is always 100 for some reason - average?
                    historicSite.Column14 = 0;
                    historicSite.Column15 = 0;
                    historicSite.Column16 = 0;
                    historicSite.Column17 = 0;
                    historicSite.Column18 = 0;
                    historicSite.Column19 = 0;
                    historicSite.Column20 = 0;
                }

                sites.Add(historicSite);
            }
            else
            {
                // We are showing live data
                sites.Add(site);
            }
        }
    }
}
