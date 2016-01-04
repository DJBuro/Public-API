using System;
using System.Collections.Generic;
using System.Linq;
using WebDashboard.Dao.Domain;
using WebDashboard.Dao.Domain.Helpers;

namespace WebDashboard.Mvc.Helpers
{
    public static class ColumnData
    {
        /// <summary>
        /// Suffix/Prefix the data to be displayed
        /// </summary>
        /// <param name="indicator"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        public static string DisplayValue(Indicator indicator, decimal columnValue)
        {
            if (indicator.ValueType.IsPrefix)
            {
                return indicator.ValueType.Value + columnValue;
            }
                
            return columnValue + indicator.ValueType.Value;
        }

        /// <summary>
        /// Rank a site based on its position
        /// </summary>
        /// <param name="site"></param>
        /// <param name="allSites"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public static int RankSite(Site site, IEnumerable<Site> allSites, Indicator indicator)
        {
            var rankedSites = new List<SiteRank>();

            foreach (var site2 in allSites)
            {
                var columnData = GetColumnValue(site2, indicator.Definition.ColumnNumber);
                var columnValue = GetData(site2, indicator, columnData);

                if (columnValue == 0 && indicator.AllowZero == false)
                    continue;

                rankedSites.Add(new SiteRank(site2.SiteId, 1, "", "", columnValue));
            }

            if (indicator.ReverseSort)
            {
                rankedSites = rankedSites.OrderBy(c => c.indicator).ToList();
            }
            else
            {
                rankedSites = rankedSites.OrderByDescending(c => c.indicator).ToList();
            }

            var siteRanking = 0;

            for (var i = 0; i < rankedSites.Count; i++)
            {
                if (site.SiteId != rankedSites[i].siteId) continue;
                siteRanking = i + 1;
                break;
            }

            return siteRanking;
        }

        /// <summary>
        /// Used for historical data, compares all sites
        /// </summary>
        /// <param name="allSites"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public static List<SiteRank> RankSites(IEnumerable<HistoricalData> allSites, Indicator indicator)
        {
            var rankedSites = new List<SiteRank>();

            foreach (var site in allSites)
            {
                var columnData = GetColumnValue(site, indicator.Definition.ColumnNumber);
                var columnValue = GetData(site, indicator, columnData);

                if (columnValue == 0 && indicator.AllowZero == false)
                    continue;

                rankedSites.Add(new SiteRank(site.SiteId, 1, site.Name,columnValue.ToString(), columnValue));
            }

            if (indicator.ReverseSort)
            {
                rankedSites = rankedSites.OrderBy(c => c.indicator).ToList();
            }
            else
            {
                rankedSites = rankedSites.OrderByDescending(c => c.indicator).ToList();
            }

            for (var i = 0; i < rankedSites.Count; i++)
            {
                rankedSites[i].rank = i + 1;
            }

            return rankedSites;
        }

        /// <summary>
        /// Calculates the data needed for a gauge
        /// </summary>
        /// <param name="site"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public static decimal GetGaugeData(Site site, Indicator indicator)
        {
            switch (indicator.Definition.ColumnNumber)
            {
                case 1: return GetData(site, indicator, site.Column1);

                case 2: return GetData(site, indicator, site.Column2);

                case 3: return GetData(site, indicator, site.Column3);

                case 4: return GetData(site, indicator, site.Column4);
                    
                case 5: return GetData(site, indicator, site.Column5);
                   
                case 8: return GetData(site, indicator, site.Column8);
                    
                case 9: return GetData(site, indicator, site.Column9);

                case 10: return GetData(site, indicator, site.Column10);
                    
                case 11: return GetData(site, indicator, site.Column11);
                    
                case 12: return GetData(site, indicator, site.Column12);
                    
                case 13: return GetData(site, indicator, site.Column13);

                case 14: return GetData(site, indicator, site.Column14);

                case 15: return GetData(site, indicator, site.Column15);

                case 16: return GetData(site, indicator, site.Column16);

                case 17: return GetData(site, indicator, site.Column17);

                case 18: return GetData(site, indicator, site.Column18);

                case 19: return GetData(site, indicator, site.Column19);
                   
                case 20: return GetData(site, indicator, site.Column20);
            }

            return 0;
        }

        /// <summary>
        /// Returns the calcuated value of a column
        /// </summary>
        /// <param name="site"></param>
        /// <param name="indicator"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        public static decimal GetData(Site site, Indicator indicator, decimal columnValue)
        {
            decimal data = 0;

            switch (indicator.DivisorType.Id)
            {
                case 1: //divide by another columns values;
                    var divisorColumn = GetColumnValue(site, indicator.DivisorColumn);
                    if (divisorColumn == 0)
                        return data;

                    data = Math.Round(columnValue / divisorColumn, 2);
                    return AdjustForData(data, indicator);

                case 2: //divide by the maximum value
                    data = Math.Round(columnValue / indicator.MaxValue, 2);
                    return AdjustForData(data, indicator);

                case 3:// we just need the value of the column
                    data = Math.Round(columnValue, 2);
                    return AdjustForData(data, indicator);    

                case 4://this is a custom value to divide by
                    data = Math.Round(columnValue / indicator.CustomDivisorValue, 2);
                    return data == 0 ? data : AdjustForData(data, indicator);
            }

            return data;
        }

        /// <summary>
        /// Returns the calcuated value of a column
        /// </summary>
        /// <param name="site"></param>
        /// <param name="indicator"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        public static decimal GetData(HistoricalData site, Indicator indicator, decimal columnValue)
        {
            decimal data = 0;

            switch (indicator.DivisorType.Id)
            {
                case 1: //divide by another columns values;
                    var divisorColumn = GetColumnValue(site, indicator.DivisorColumn);
                    if (divisorColumn == 0)
                        return data;

                    data = Math.Round(columnValue / divisorColumn, 2);
                    return AdjustForData(data, indicator);

                case 2: //divide by the maximum value
                    data = Math.Round(columnValue / indicator.MaxValue, 2);
                    return AdjustForData(data, indicator);

                case 3:// we just need the value of the column
                    data = Math.Round(columnValue, 2);
                    return AdjustForData(data, indicator);

                case 4://this is a custom value to divide by
                    data = Math.Round(columnValue / indicator.CustomDivisorValue, 2);
                    return data == 0 ? data : AdjustForData(data, indicator);
            }

            return data;
        }


        /// <summary>
        /// Rameses doesn't believe in decimal places, or time formats other than seconds
        /// </summary>
        /// <returns></returns>
        public static decimal AdjustForData(decimal data, Indicator indicator)
        {
            //todo: if ever any free time, sort this out.
            //Check if we are using Money
            if (indicator.ValueType.Id == 1 || 
                indicator.ValueType.Id == 2 || 
                indicator.ValueType.Id == 9 ||
                indicator.ValueType.Id == 10 ||
                indicator.ValueType.Id == 11 ||
                indicator.ValueType.Id == 13 ||
                indicator.ValueType.Id == 15)
            {
                return Math.Round(data / 100, 2);
            }

            //percentage values for time
            if (indicator.ValueType.Id == 6)
            {
                 return data * 100;
            }

            //OPD -- there is no value type
            if (indicator.ValueType.Id == 7)
            {
                return Math.Round(data / 100, 2);
            }

            return data;
        }

        /// <summary>
        /// Returns the value of a column for a site
        /// </summary>
        /// <param name="site"></param>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public static int GetColumnValue(Site site, int columnNumber)
        {
            switch (columnNumber)
            {
                case 1: return site.Column1;

                case 2: return site.Column2;
                    
                case 3: return site.Column3;

                case 4: return site.Column4;
                    
                case 5: return site.Column5;
                    
                case 6: return site.Column6;
                    
                case 7: return site.Column7;
                    
                case 8: return site.Column8;
                    
                case 9: return site.Column9;
                    
                case 10: return site.Column10;

                case 11: return site.Column11;
                    
                case 12: return site.Column12;
                    
                case 13: return site.Column13;
                    
                case 14: return site.Column14;
                    
                case 15: return site.Column15;
                    
                case 16: return site.Column16;
                    
                case 17: return site.Column17;
                    
                case 18: return site.Column18;
                    
                case 19: return site.Column19;
                    
                case 20: return site.Column20;
                    
            }

            return 0;
        }


        /// <summary>
        /// Returns the value of a column for a site
        /// </summary>
        /// <param name="site"></param>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public static int GetColumnValue(HistoricalData site, int columnNumber)
        {
            switch (columnNumber)
            {
                case 1: return site.Column1;

                case 2: return site.Column2;

                case 3: return site.Column3;

                case 4: return site.Column4;

                case 5: return site.Column5;

                case 6: return site.Column6;

                case 7: return site.Column7;

                case 8: return site.Column8;

                case 9: return site.Column9;

                case 10: return site.Column10;

                case 11: return site.Column11;

                case 12: return site.Column12;

                case 13: return site.Column13;

                case 14: return site.Column14;

                case 15: return site.Column15;

                case 16: return site.Column16;

                case 17: return site.Column17;

                case 18: return site.Column18;

                case 19: return site.Column19;

                case 20: return site.Column20;

            }

            return 0;
        }

        /// <summary>
        /// Orders sites by a column number
        /// </summary>
        /// <param name="sites"></param>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public static List<Site> OrderByColumn(IEnumerable<Site> sites, int columnNumber)
        {
            switch (columnNumber)
            {
                case 1: return sites.OrderBy(c => c.Column1).ToList();
                    
                case 2: return sites.OrderBy(c => c.Column2).ToList();
                    
                case 3: return sites.OrderBy(c => c.Column3).ToList();
                    
                case 4: return sites.OrderBy(c => c.Column4).ToList();
                    
                case 5: return sites.OrderBy(c => c.Column5).ToList();
                    
                case 6: return sites.OrderBy(c => c.Column6).ToList();
                    
                case 7: return sites.OrderBy(c => c.Column7).ToList();
                    
                case 8: return sites.OrderBy(c => c.Column8).ToList();
                    
                case 9: return sites.OrderBy(c => c.Column9).ToList();
                    
                case 10: return sites.OrderBy(c => c.Column10).ToList();
                    
                case 11: return sites.OrderBy(c => c.Column11).ToList();
                    
                case 12: return sites.OrderBy(c => c.Column12).ToList();
                    
                case 13: return sites.OrderBy(c => c.Column13).ToList();
                    
                case 14: return sites.OrderBy(c => c.Column14).ToList();
                    
                case 15: return sites.OrderBy(c => c.Column15).ToList();
                    
                case 16: return sites.OrderBy(c => c.Column16).ToList();
                    
                case 17: return sites.OrderBy(c => c.Column17).ToList();
                    
                case 18: return sites.OrderBy(c => c.Column18).ToList();
                    
                case 19: return sites.OrderBy(c => c.Column19).ToList();
                    
                case 20: return sites.OrderBy(c => c.Column20).ToList();
                    
            }


            return null;
        }


        public static void RemoveZeroValues(List<Site> sites, int columnNumber)
        {
            switch (columnNumber)
            {
                case 1: sites.RemoveAll(c => c.Column1 == 0);
                     break;

                case 2: sites.RemoveAll(c => c.Column2 == 0);
                     break;
                    
                case 3: sites.RemoveAll(c => c.Column3 == 0);
                     break;

                case 4: sites.RemoveAll(c => c.Column4 == 0);
                     break;

                case 5: sites.RemoveAll(c => c.Column5 == 0);
                     break;

                case 6: sites.RemoveAll(c => c.Column6 == 0);
                     break;

                case 7: sites.RemoveAll(c => c.Column7 == 0);
                     break;

                case 8: sites.RemoveAll(c => c.Column8 == 0);
                     break;

                case 9: sites.RemoveAll(c => c.Column9 == 0);
                     break;

                case 10: sites.RemoveAll(c => c.Column10 == 0);
                     break;

                case 11: sites.RemoveAll(c => c.Column11 == 0);
                     break;

                case 12: sites.RemoveAll(c => c.Column12 == 0);
                     break;
 
                case 13: sites.RemoveAll(c => c.Column13 == 0);
                     break;

                case 14: sites.RemoveAll(c => c.Column14 == 0);
                     break;

                case 15: sites.RemoveAll(c => c.Column15 == 0);
                     break;

                case 16: sites.RemoveAll(c => c.Column16 == 0);
                     break;

                case 17: sites.RemoveAll(c => c.Column17 == 0);
                     break;

                case 18: sites.RemoveAll(c => c.Column18 == 0);
                     break;

                case 19: sites.RemoveAll(c => c.Column19 == 0);
                     break;

                case 20: sites.RemoveAll(c => c.Column20 == 0);
                    break;

            }

        }
    }
}
