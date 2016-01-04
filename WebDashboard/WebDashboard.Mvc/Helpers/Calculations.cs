using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using WebDashboard.Dao.Domain;
using WebDashboard.Dao.Domain.Helpers;

namespace WebDashboard.Mvc.Helpers
{
    public static class Calculations
    {
        public static double GetComparableValues(double historicValue, double currentValue)
        {
            return Calculations.GetComparableValues(historicValue, currentValue, 0);
        }

        public static double GetComparableValues(double historicValue, double currentValue, int decimalPlaces)
        {
            if (historicValue > 0 & currentValue > 0)
            {
                var d = currentValue - historicValue;
                var val = Math.Round((d / historicValue) * 100, decimalPlaces);

                return val; //% change
            }

            return 0;
        }

        public static double GetComparableValues(int historicValue, int currentValue)
        {
            return GetComparableValues((double)historicValue, (double)currentValue);
        }

        public static List<Site> Convert(this IList regionalSites)
        {
            var allSites = new List<Site>();

            foreach (Site site in regionalSites)
            {
                allSites.Add(site);
            }

            return allSites;
        }

        public enum Condition
        {
            Divide,
            Multiply
        }

        public static double GetAverageForColumn(IList<Site> allSites, ColumnsEnum columnNumber, bool isCurrency)
        {
            var amount = Calculations.GetTotalForColumn(allSites, (int)columnNumber, isCurrency);

            if (amount > 0)
            {
                return amount / allSites.Count;
            }

            return amount;
        }

        public static double GetTotalForColumn(IList<Site> allSites, ColumnsEnum columnNumber, bool isCurrency)
        {
            return Calculations.GetTotalForColumn(allSites, (int)columnNumber, isCurrency);
        }

        public static double GetTotalForColumn(IList<Site> allSites, int columnNumber, bool isCurrency)
        {
            return Calculations.GetTotalForColumn(allSites, columnNumber, false, isCurrency);
        }

        public static double GetTotalForColumn(IList<Site> allSites, int columnNumber, bool compOnly, bool isCurrency)
        {
            double total = 0;

            foreach (Site site in allSites)
            {

                if (!compOnly || (compOnly && site.Comp))
                {
                    total += isCurrency ?
                        (ColumnData.GetColumnValue(site, columnNumber) * site.HeadOffice.ExchangeRate) :
                        ColumnData.GetColumnValue(site, columnNumber);
                }
            }

            return total;
        }

        public static double GetAverageTotalForColumn(IList<Site> allSites, int columnNumber, bool isCurrency)
        {
            double total = 0;
            int siteCount = 0;

            foreach (var site in allSites)
            {
                if (site.IsReportingToday)
                {
                    int siteValue = ColumnData.GetColumnValue(site, columnNumber);
                    if (siteValue > 0)
                    {
                        if (isCurrency)
                        {
                            total += (siteValue * site.HeadOffice.ExchangeRate);
                        }
                        else
                        {
                            total += siteValue;
                        }
                        siteCount++;
                    }
                }
            }

            if (total > 0)
            {
                total = total / siteCount;
            }

            return total;
        }

        public static double GetComputedAverageForColumn(
            IList<Site> allSites,
            ColumnsEnum columnNumber,
            ColumnsEnum computedColumnNumber,
            Condition condition,
            bool isCurrency)
        {
            double totalPercentage = 0;
            double storeCount = 0;

            double totalColumnValue = 0;
            double totalComputedValue = 0;

            foreach (var site in allSites)
            {
                int columnValue = ColumnData.GetColumnValue(site, (int)columnNumber);
                int computedColumnValue = ColumnData.GetColumnValue(site, (int)computedColumnNumber);

                switch (condition)
                {
                    case Condition.Divide:
                        if (computedColumnValue != 0 && columnValue != 0)
                        {
                            if (isCurrency)
                            {
                                totalColumnValue += columnValue * site.ExchangeRate;
                                totalComputedValue += computedColumnValue * site.ExchangeRate;
                            }
                            else
                            {
                                totalColumnValue += columnValue;
                                totalComputedValue += computedColumnValue;
                            }
                            
                            storeCount++;
                        }
                        break;
                }
            }

            switch (condition)
            {
                case Condition.Divide:
                    if (totalComputedValue != 0 && totalColumnValue != 0)
                    {
                        totalPercentage = Math.Round((double)totalColumnValue / (double)totalComputedValue, 2);
                    }
                    break;
            }

            return totalPercentage;
        }

        public static double GetComputedAverageForColumn(
            Site site,
            ColumnsEnum columnNumber,
            ColumnsEnum computedColumnNumber,
            Condition condition,
            bool isCurrency,
            bool computedIsCurrency)
        {
            double total = 0;
            double divisor = 0;

            double columnValue = ColumnData.GetColumnValue(site, (int)columnNumber);
            double computedColumn = ColumnData.GetColumnValue(site, (int)computedColumnNumber);

            switch (condition)
            {
                case Condition.Divide:
                    if (computedColumn != 0 & columnValue != 0)
                    {
                        if (isCurrency)
                        {
                            columnValue = columnValue * site.ExchangeRate;
                        }

                        if (computedIsCurrency)
                        {
                            computedColumn = computedColumn * site.ExchangeRate;
                        }
                        
                        total += (columnValue / computedColumn);
                        divisor++;
                    }
                    break;
            }

            if (total > 0)
            {
                total = Math.Round(total / divisor, 2);
            }

            return total;
        }

        public static double GetComputedAverageForColumnAgainstValue(
            IList<Site> allSites, 
            ColumnsEnum columnNumber, 
            double value,
            Condition condition,
            bool isCurrency)
        {
            return Calculations.GetComputedAverageForColumnAgainstValue(allSites, columnNumber, value, condition, false, isCurrency);
        }

        public static double GetComputedAverageForColumnAgainstValue(
            IList<Site> allSites, 
            ColumnsEnum columnNumber, 
            double value, 
            Condition condition,
            bool compOnly,
            bool isCurrency)
        {
            double total = 0;
            double divisor = 0;

            double totalColumnValue = 0;

            foreach (var site in allSites)
            {
                if (!compOnly || (compOnly && site.Comp))
                {
                    double columnValue = ColumnData.GetColumnValue(site, (int)columnNumber);

                    switch (condition)
                    {
                        case Condition.Divide:
                            if (columnValue != 0)
                            {
                                if (isCurrency)
                                {
                                    totalColumnValue += (columnValue * site.ExchangeRate);
                                }
                                else
                                {
                                    totalColumnValue += columnValue;
                                }
                                
                                divisor++;
                            }
                            break;
                    }
                }
            }

            switch (condition)
            {
                case Condition.Divide:
                    if (totalColumnValue != 0)
                    {
                        total += totalColumnValue / value;
                    }
                    break;
            }

            if (total > 0)
            {
                total = Math.Round(total / divisor, 2);
            }

            return total;
        }

        public static double GetComputedAverageForColumnAgainstValue(
            Site site, 
            int columnNumber, 
            double value,
            Condition condition,
            bool isCurrency)
        {
            double total = 0;
            double divisor = 0;

            var columnValue = ColumnData.GetColumnValue(site, columnNumber);

            switch (condition)
            {
                case Condition.Divide:
                    if (columnValue != 0)
                    {
                        if (isCurrency)
                        {
                            total += (columnValue * site.ExchangeRate) / value;
                        }
                        else
                        {
                            total += columnValue / value;
                        }
                        divisor++;
                    }
                    break;
            }

            if (total > 0)
            {
                total = Math.Round(total / divisor, 2);
            }

            return total;
        }

        public static IList<ExecutiveData> OrderBy(string orderby, string type, IList<ExecutiveData> executiveData)
        {
            switch (orderby)
            {
                case "Name":
                    return type != "Desc" ? executiveData.OrderBy(c => c.Name).ToList() : executiveData.OrderByDescending(c => c.Name).ToList();
                case "Orders":
                    return type != "Desc" ? executiveData.OrderBy(c => c.Orders).ToList() : executiveData.OrderByDescending(c => c.Orders).ToList();
                case "OrdersLW":
                    return type != "Desc" ? executiveData.OrderBy(c => c.OrdersVsLastWeek).ToList() : executiveData.OrderByDescending(c => c.OrdersVsLastWeek).ToList();
                case "OrdersLY":
                    return type != "Desc" ? executiveData.OrderBy(c => c.OrdersVsLastYear).ToList() : executiveData.OrderByDescending(c => c.OrdersVsLastYear).ToList();
                case "Sales":
                    return type != "Desc" ? executiveData.OrderBy(c => c.Sales).ToList() : executiveData.OrderByDescending(c => c.Sales).ToList();
                case "SalesLW":
                    return type != "Desc" ? executiveData.OrderBy(c => c.SalesVsLastWeek).ToList() : executiveData.OrderByDescending(c => c.SalesVsLastWeek).ToList();
                case "SalesLY":
                    return type != "Desc" ? executiveData.OrderBy(c => c.SalesVsLastYear).ToList() : executiveData.OrderByDescending(c => c.SalesVsLastYear).ToList();
                case "30min":
                    return type != "Desc" ? executiveData.OrderBy(c => c.LessThan30Min).ToList() : executiveData.OrderByDescending(c => c.LessThan30Min).ToList();
                case "45min":
                    return type != "Desc" ? executiveData.OrderBy(c => c.LessThan45Min).ToList() : executiveData.OrderByDescending(c => c.LessThan45Min).ToList();
                case "Make":
                    return type != "Desc" ? executiveData.OrderBy(c => c.Make).ToList() : executiveData.OrderByDescending(c => c.Make).ToList();
                case "OTD":
                    return type != "Desc" ? executiveData.OrderBy(c => c.OTD).ToList() : executiveData.OrderByDescending(c => c.OTD).ToList();
                case "OPR":
                    return type != "Desc" ? executiveData.OrderBy(c => c.OPR).ToList() : executiveData.OrderByDescending(c => c.OPR).ToList();
                case "TTD":
                    return type != "Desc" ? executiveData.OrderBy(c => c.TTD).ToList() : executiveData.OrderByDescending(c => c.TTD).ToList();
                case "AverageDriveTime":
                    return type != "Desc" ? executiveData.OrderBy(c => c.AverageDriveTime).ToList() : executiveData.OrderByDescending(c => c.AverageDriveTime).ToList();
                case "Drivers":
                    return type != "Desc" ? executiveData.OrderBy(c => c.Drivers).ToList() : executiveData.OrderByDescending(c => c.Drivers).ToList();
                case "Road":
                    return type != "Desc" ? executiveData.OrderBy(c => c.DriversDelivering).ToList() : executiveData.OrderByDescending(c => c.DriversDelivering).ToList();
                case "Avg":
                    return type != "Desc" ? executiveData.OrderBy(c => c.AverageSpend).ToList() : executiveData.OrderByDescending(c => c.AverageSpend).ToList();
            }

            return null;
        }

        public static void CleanupData(IList<Site> sites, bool removeDisabledSites)
        {
            DateTime dateTimeNow = DateTime.Now;

            // Trading day is 6:30am to 6:30am the next day
            // Only use data that was uploaded during the current trading day
            DateTime startOfTradingDay = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 7, 0, 0);

            // Before 6:30am is technically the previous trading day so 
            // we only use data uploaded after 6:30am the previous day
            if (dateTimeNow.Hour < 7) // || (dateTimeNow.Hour == 6 && dateTimeNow.Minute < 30))
            {
                startOfTradingDay = startOfTradingDay.AddDays(-1);
            }

            List<Site> removeSites = new List<Site>();

            foreach (Site site in sites)
            {
                if (removeDisabledSites && !site.Enabled)
                {
                    removeSites.Add(site);
                }
                else if (site.LastUpdated < startOfTradingDay)
                {
                    site.IsReportingToday = false;
                    
                    site.Column1 = 0;
                    site.Column2 = 0;
                    site.Column3 = 0;
                    site.Column4 = 0;
                    site.Column5 = 0;
                    site.Column6 = 0;
                    site.Column7 = 0;
                    site.Column8 = 0;
                    site.Column9 = 0;
                    site.Column10 = 0;
                    site.Column11 = 0;
                    site.Column12 = 0;
                    site.Column13 = 100; // Orders per delivery driver is always 100 for some reason - average?
                    site.Column14 = 0;
                    site.Column15 = 0;
                    site.Column16 = 0;
                    site.Column17 = 0;
                    site.Column18 = 0;
                    site.Column19 = 0;
                    site.Column20 = 0;
                }
                else
                {
                    site.IsReportingToday = true;
                }
            }

            foreach (Site removeSite in removeSites)
            {
                sites.Remove(removeSite);
            }
        }

        public static double X(IList<Site> allSites, ColumnsEnum valueColumn, ColumnsEnum weightingColumn)
        {
            int totalValueWeighted = 0;
            double totalValue = 0;

            foreach (var site in allSites)
            {
                int value = ColumnData.GetColumnValue(site, (int)valueColumn);
                int weighting = ColumnData.GetColumnValue(site, (int)weightingColumn);

                if (value != 0 && weighting != 0)
                {
                    totalValueWeighted += (value * weighting);
                    totalValue += value;
                }
            }

            double average = 0;
            
            if (totalValue != 0)
            {
                // Get the average for all stores
                average = totalValueWeighted / totalValue;
            }

            return average;
        }

        public static void SetCompanyExchangeRate(HeadOffice company, IList<GroupExchangeRate> groupExchangeRates)
        {
            // Does this company have a currency code?
            if (company.CurrencyCode != null &&
                company.CurrencyCode.Length > 0)
            {
                // Does the company have an exchange rate that needs to be applied to all values
                foreach (GroupExchangeRate groupExchangeRate in groupExchangeRates)
                {
                    if (groupExchangeRate.GroupId == company.GroupId &&
                        groupExchangeRate.CurrencyCode == company.CurrencyCode)
                    {
                        company.ExchangeRate = groupExchangeRate.ExchangeRate;
                        break;
                    }
                }
            }
        }
    }
}
