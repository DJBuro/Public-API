using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebDashboard.Dao.Domain;
using WebDashboard.Dao.Domain.Helpers;

namespace Dashboard.Web.Mvc.Helpers
{
    public static class BuildDashboardData
    {

        #region GaugeData

        public static Display GetData(Site site, IList<IndicatorDefinition> indicatorDefinitions,List<DashboardData> allSiteData)
        {
            var display = new Display();

            display.Gauges = new List<Gauge>();
            
            var dashboardData = new DashboardData();

            foreach (DashboardData data in site.DashboardData)
            {
                dashboardData = data;
            }

            var allRegionData = new List<DashboardData>();

            foreach (SitesRegion sitesRegion in site.SitesRegions)
            {
                foreach (SitesRegion c in sitesRegion.Region.SitesRegions)
                {
                    if (c.Site.DashboardData.Count != 0)
                    {
                        allRegionData.Add((DashboardData)c.Site.DashboardData[0]);
                    }
                }
            }

            foreach (var definition in indicatorDefinitions.OrderBy(c => c.InterfaceColumnNumber))
            {
                //todo: move to DB... also with Prefix/suffix indicators etc.
                string moneyFormat = "€";

                if(site.HeadOffice.Id == 143 | site.HeadOffice.Id == 163)
                {
                    moneyFormat = "£";
                }


                if (definition.UseColumn)
                {
                    decimal value=0;
                
                    var translation = (IndicatorTranslation) definition.IndicatorTranslations[0];
                    
                    var gauge = new Gauge {ShowGauge = true};

                    //todo: refactor/clean up - too much repetitive coding
                    //todo: const: string the indicatory types
                    switch (definition.ColumnNumber)
                    {
                        case 1: //NYP: average order

                            if (dashboardData.Column1.HasValue)
                            {
                                
                                value = 0;

                                if (dashboardData.Column1.Value != 0 | dashboardData.Column20.Value != 0)
                                {
                                    if (dashboardData.Column1.Value > 0)
                                        value = Math.Round((((decimal)dashboardData.Column1.Value / (decimal)dashboardData.Column20.Value) / definition.Divisor.Value), 2);
                                }

                                var siteRanks = new List<SiteRank>();
                                
                                foreach (DashboardData list in allSiteData)
                                {
                                    decimal xy = 0;
                                    if (list.Column20.Value > 0)
                                    {
                                        xy = (list.Column1.Value/list.Column20.Value) * 100 ;
                                    }

                                    var rank = new SiteRank
                                                    {
                                                        indicator = xy,
                                                        name = list.Site.SiteName,
                                                        rank = 0
                                                    };
                                    siteRanks.Add(rank);
                                }

                                gauge.CurrentValue = value;
                                gauge.Name = translation.TranslatedIndicatorName;
                                gauge.MaxValue = definition.Divisor.Value;
                                gauge.Benchmark = definition.BenchMark;
                                gauge.PrefixIndicator = moneyFormat;
                                gauge.SuffixIndicator = "";
                                
                                //TODO:definition.ReverseSortingOrder should be applied below
                                gauge.TopSite =RankingHelper.RankSites(siteRanks.OrderByDescending(c => c.indicator).ToList(), definition.Divisor.Value * 100, moneyFormat,"", definition.ReverseSortingOrder);

                                if (gauge.TopSite.Count > 0 )
                                {
                                    if (gauge.TopSite[0].indicator > gauge.MaxValue)
                                        gauge.MaxValue = (int) gauge.TopSite[0].indicator;
                                }
                                if (gauge.TopSite.Count > 2)
                                {
                                    gauge.BottomSite = gauge.TopSite.OrderByDescending(c => c.rank).ToList().GetRange(
                                        0, 3);
                                    gauge.BottomSite = gauge.BottomSite.Reverse().ToList();
                                }

                                gauge.SiteRank = gauge.TopSite.ToList().FindIndex(c => c.name == dashboardData.Site.SiteName) + 1;                               

                                gauge.RegionRank = RankingHelper.GetRegionRanking(site, allRegionData.OrderByDescending(c => c.Column1).ToList());

                                gauge.TopSite = gauge.TopSite.Take(3).ToList();

                                gauge.ShowGauge = definition.ShowGauge;
                                gauge.IsReverse = definition.ReverseSortingOrder;
                            }

                            break;

                        case 5: //NYP %>30min (all use col 5)

                            if (dashboardData.Column5.HasValue)
                            {
                                if (dashboardData.Column3.Value > 0 & dashboardData.Column5.Value > 0)
                                    value = Math.Round(((decimal)dashboardData.Column5.Value / (decimal)dashboardData.Column3.Value), 2) * 100;

                                var siteRanks = new List<SiteRank>();

                                foreach (DashboardData list in allSiteData)
                                {
                                    var rank = new SiteRank
                                    {
                                        indicator = list.Column5.Value,
                                        indicatorDivisor = list.Column3.Value,
                                        name = list.Site.SiteName,
                                        rank = 0
                                    };
                                    siteRanks.Add(rank);
                                }

                                gauge.CurrentValue = value;
                                gauge.Name = translation.TranslatedIndicatorName;
                                gauge.MaxValue = definition.Divisor.Value;
                                gauge.Benchmark = definition.BenchMark;
                                gauge.PrefixIndicator = "";
                                gauge.SuffixIndicator = "%";

                                gauge.TopSite =RankingHelper.RankSites(siteRanks.OrderByDescending(c => c.indicator).ToList(), definition.Divisor.Value, "", "%", definition.ReverseSortingOrder);
                                
                                if (gauge.TopSite.Count > 2)
                                {
                                    gauge.BottomSite = gauge.TopSite.OrderByDescending(c => c.rank).ToList().GetRange(
                                        0, 3);
                                    gauge.BottomSite = gauge.BottomSite.Reverse().ToList();
                                }

                                gauge.SiteRank = gauge.TopSite.ToList().FindIndex(c => c.name == dashboardData.Site.SiteName) + 1;

                                gauge.TopSite = gauge.TopSite.Take(3).ToList();

                                //todo: sort as above - siteRank
                                gauge.RegionRank = RankingHelper.GetRegionRanking(site, allRegionData.OrderByDescending(c => c.Column5).ToList());

                                gauge.ShowGauge = definition.ShowGauge;
                                gauge.IsReverse = definition.ReverseSortingOrder;
                            }
                            break;

                        case 8: // NYP: Instore / TBBC & PJ  : OTD (On Time Delivery)

                            if (dashboardData.Column8.HasValue)
                            {
                                if (dashboardData.Column8.Value > 0)
                                    value = Math.Round((decimal)dashboardData.Column8.Value / (decimal)definition.Divisor.Value, 2);

                                var siteRanks = new List<SiteRank>();

                                foreach (DashboardData list in allSiteData)
                                {
                                    var rank = new SiteRank
                                    {
                                        indicator = list.Column8.Value,
                                        name = list.Site.SiteName,
                                        rank = 0
                                    };
                                    siteRanks.Add(rank);
                                }

                                gauge.CurrentValue = value;
                                gauge.Name = translation.TranslatedIndicatorName;
                                gauge.MaxValue = definition.Divisor.Value;
                                gauge.Benchmark = definition.BenchMark;
                                gauge.PrefixIndicator = "";
                                gauge.SuffixIndicator = "";

                                gauge.TopSite = RankingHelper.RankSites(siteRanks.OrderByDescending(c => c.indicator).ToList(), definition.Divisor.Value, "", "", definition.ReverseSortingOrder);
                                
                                if (gauge.TopSite.Count > 2)
                                {
                                    gauge.BottomSite = gauge.TopSite.OrderByDescending(c => c.rank).ToList().GetRange(
                                        0, 3);
                                    gauge.BottomSite = gauge.BottomSite.Reverse().ToList();
                                }
                                //If there is a 4+ tie for first
                                if (gauge.SiteRank == 0)
                                    gauge.SiteRank = 1;

                                gauge.SiteRank = gauge.TopSite.ToList().FindIndex(c => c.name == dashboardData.Site.SiteName) + 1;
                                gauge.RegionRank = RankingHelper.GetRegionRanking(site, allRegionData.OrderByDescending(c => c.Column8).ToList());

                                gauge.TopSite = gauge.TopSite.Take(3).ToList();

                                gauge.ShowGauge = definition.ShowGauge;
                                gauge.IsReverse = definition.ReverseSortingOrder;
                            }

                            break;

                        case 9: // TBBC & PJ : TTD (To The Door) / NYP: Door delivery time (Bezorgtijd)

                            if (dashboardData.Column9.HasValue)
                            {

                                if (dashboardData.Column9.Value > 0)
                                    value = Math.Round((decimal)dashboardData.Column9.Value / (decimal)definition.Divisor.Value, 2);

                                var siteRanks = new List<SiteRank>();

                                foreach (DashboardData list in allSiteData)
                                {
                                    var rank = new SiteRank
                                    {
                                        indicator = list.Column9.Value,
                                        name = list.Site.SiteName,
                                        rank = 0
                                    };
                                    siteRanks.Add(rank);
                                }

                                gauge.CurrentValue = value;
                                gauge.Name = translation.TranslatedIndicatorName;
                                gauge.MaxValue = definition.Divisor.Value;
                                gauge.Benchmark = definition.BenchMark;
                                gauge.PrefixIndicator = "";
                                gauge.SuffixIndicator = "min";

                                gauge.TopSite =RankingHelper.RankSites(siteRanks.OrderByDescending(c => c.indicator).ToList(), definition.Divisor.Value, "", "",true);
                                gauge.IsReverse = true;

                                if (gauge.TopSite.Count > 2)
                                {
                                    gauge.BottomSite = gauge.TopSite.OrderByDescending(c => c.rank).ToList().GetRange(
                                        0, 3);
                                    gauge.BottomSite = gauge.BottomSite.Reverse().ToList();
                                }
                                //If there is a 4+ tie for first
                                if (gauge.SiteRank == 0)
                                    gauge.SiteRank = 1;

                                gauge.SiteRank = gauge.TopSite.ToList().FindIndex(c => c.name == dashboardData.Site.SiteName) + 1;
                                gauge.RegionRank = RankingHelper.GetRegionRanking(site, allRegionData.OrderByDescending(c => c.Column9).ToList());

                                gauge.TopSite = gauge.TopSite.Take(3).ToList();

                                gauge.ShowGauge = definition.ShowGauge;
                                gauge.IsReverse = definition.ReverseSortingOrder;
                            }
                            break;

                        case 11: // TBBC & PJ :MAKE / NYP : not used

                            if (dashboardData.Column11.HasValue)
                            {

                                if (dashboardData.Column11.Value > 0)
                                    value = Math.Round((decimal)dashboardData.Column11.Value / (decimal)definition.Divisor.Value,2);


                                var siteRanks = new List<SiteRank>();

                                foreach (DashboardData list in allSiteData)
                                {
                                    var rank = new SiteRank
                                    {
                                        indicator = list.Column11.Value,
                                        name = list.Site.SiteName,
                                        rank = 0
                                    };
                                    siteRanks.Add(rank);
                                }

                                gauge.CurrentValue = value;
                                gauge.Name = translation.TranslatedIndicatorName;
                                gauge.MaxValue = definition.Divisor.Value;
                                gauge.Benchmark = definition.BenchMark;
                                gauge.PrefixIndicator = "";
                                gauge.SuffixIndicator = "min";

                                gauge.TopSite = RankingHelper.RankSites(siteRanks.OrderByDescending(c => c.indicator).ToList(), definition.Divisor.Value, "", "", definition.ReverseSortingOrder);
                                
                                if (gauge.TopSite.Count > 2)
                                {
                                    gauge.BottomSite = gauge.TopSite.OrderByDescending(c => c.rank).ToList().GetRange(
                                        0, 3);
                                    gauge.BottomSite = gauge.BottomSite.Reverse().ToList();
                                }
                                //If there is a 4+ tie for first
                                if (gauge.SiteRank == 0)
                                    gauge.SiteRank = 1;

                                gauge.SiteRank = gauge.TopSite.ToList().FindIndex(c => c.name == dashboardData.Site.SiteName) + 1;
                                gauge.RegionRank = RankingHelper.GetRegionRanking(site, allRegionData.OrderByDescending(c => c.Column11).ToList());

                                gauge.TopSite = gauge.TopSite.Take(3).ToList();

                                gauge.ShowGauge = definition.ShowGauge;
                                gauge.IsReverse = definition.ReverseSortingOrder;
                            
                            }

                            break;

                        case 12: //NYP : %<20min

                            if (dashboardData.Column12.HasValue)
                            {

                                if (dashboardData.Column3.Value > 0 & dashboardData.Column12.Value > 0)
                                    value = Math.Round(((decimal)dashboardData.Column12.Value / (decimal)dashboardData.Column3.Value), 2) * 100;

                                var siteRanks = new List<SiteRank>();

                                foreach (DashboardData list in allSiteData)
                                {
                                    var rank = new SiteRank
                                                   {
                                                       indicator = list.Column12.Value,
                                                       indicatorDivisor = list.Column3.Value,
                                                       name = list.Site.SiteName,
                                                       rank = 0
                                                    };
                                    siteRanks.Add(rank);
                                }

                                gauge.CurrentValue = value;
                                gauge.Name = translation.TranslatedIndicatorName;
                                gauge.MaxValue = definition.Divisor.Value;
                                gauge.Benchmark = definition.BenchMark;
                                gauge.PrefixIndicator = "";
                                gauge.SuffixIndicator = "%";

                                gauge.TopSite = RankingHelper.RankSites(siteRanks.OrderByDescending(c => c.indicator).ToList(), definition.Divisor.Value, "", "%", definition.ReverseSortingOrder);

                                if (gauge.TopSite.Count > 2)
                                {
                                    gauge.BottomSite = gauge.TopSite.OrderByDescending(c => c.rank).ToList().GetRange(
                                        0, 3);
                                    
                                    gauge.BottomSite = gauge.BottomSite.Reverse().ToList();
                                }
                                gauge.SiteRank = gauge.TopSite.ToList().FindIndex(c => c.name == dashboardData.Site.SiteName) + 1;

                                

                                //If there is a 4+ tie for first
                                //if (gauge.SiteRank == 0)
                                //    gauge.SiteRank = 1;

                                //todo: sort as above - siteRank
                                gauge.RegionRank = RankingHelper.GetRegionRanking(site, allRegionData.OrderByDescending(c => c.Column12).ToList());

                                gauge.TopSite = gauge.TopSite.Take(3).ToList();

                                gauge.ShowGauge = definition.ShowGauge;
                                gauge.IsReverse = definition.ReverseSortingOrder;
                            }

                            break;

                        case 13: //NYP & PJ & TBBC: OPR (Orders Per Driver)

                            if (dashboardData.Column13.HasValue)
                            {

                                if (dashboardData.Column13.Value > 0 )
                                    value = Math.Round(((decimal)dashboardData.Column13.Value / 100), 2) ;


                                var siteRanks = new List<SiteRank>();

                                foreach (DashboardData list in allSiteData)
                                {
                                    var rank = new SiteRank
                                    {
                                        indicator = list.Column13.Value,
                                        name = list.Site.SiteName,
                                        rank = 0
                                    };
                                    siteRanks.Add(rank);
                                }

                                gauge.CurrentValue = value;
                                gauge.Name = translation.TranslatedIndicatorName;
                                gauge.MaxValue = definition.Divisor.Value;
                                gauge.Benchmark = definition.BenchMark;
                                gauge.PrefixIndicator = "";
                                gauge.SuffixIndicator = "";

                                //note: since reverse is true for this:
                                gauge.TopSite =
                                    RankingHelper.RankSites(siteRanks.OrderBy(c => c.indicator).ToList(), 100, "", "",true);

                                gauge.IsReverse = true;

                                if (gauge.TopSite.Count > 2)
                                {
                                    gauge.BottomSite = gauge.TopSite.OrderByDescending(c => c.rank).ToList().GetRange(
                                        0, 3);
                                    gauge.BottomSite = gauge.BottomSite.Reverse().ToList();
                                }

                                gauge.SiteRank = gauge.BottomSite.ToList().FindIndex(c => c.name == dashboardData.Site.SiteName) + 1;
                                
                                //If there is a 4+ tie for first
                                if (gauge.SiteRank == 0)
                                    gauge.SiteRank = 1;
                                
                                gauge.RegionRank = RankingHelper.GetRegionRanking(site, allRegionData.OrderByDescending(c => c.Column13).ToList());

                                gauge.TopSite = gauge.TopSite.Take(3).ToList();

                                gauge.ShowGauge = definition.ShowGauge;
                                gauge.IsReverse = definition.ReverseSortingOrder;
                            }
                            break;

                        case 20: //NYP & PJ & TBBC: Tickets

                            if (dashboardData.Column20.HasValue)
                            {
                                value = dashboardData.Column20.Value;

                                var siteRanks = new List<SiteRank>();

                               
                                foreach (DashboardData list in allSiteData)
                                {
                                    var rank = new SiteRank
                                    {
                                        indicator = list.Column20.Value,
                                        name = list.Site.SiteName,
                                        rank = 0
                                    };
                                    siteRanks.Add(rank);
                                }

                                gauge.CurrentValue = value;
                                gauge.Name = translation.TranslatedIndicatorName;
                                gauge.MaxValue = definition.Divisor.Value;
                                gauge.Benchmark = definition.BenchMark;
                                gauge.PrefixIndicator = "";
                                gauge.SuffixIndicator = "";

                                gauge.TopSite =
                                    RankingHelper.RankSites(siteRanks.OrderByDescending(c => c.indicator).ToList(), 1, "", "", definition.ReverseSortingOrder);

                                if (gauge.TopSite.Count > 2)
                                {
                                    gauge.BottomSite = gauge.TopSite.OrderByDescending(c => c.rank).ToList().GetRange(
                                        0, 3);
                                    gauge.BottomSite = gauge.BottomSite.Reverse().ToList();
                                }
                                gauge.SiteRank = gauge.TopSite.ToList().FindIndex(c => c.name == dashboardData.Site.SiteName) + 1;
                                gauge.RegionRank = RankingHelper.GetRegionRanking(site, allRegionData.OrderByDescending(c => c.Column20).ToList());

                                //If there is a 4+ tie for first
                                //if (gauge.SiteRank == 0)
                                //    gauge.SiteRank = 1;

                                //if the top site is has done more than 100 tickets... all sites will be compared to it.
                               // if (gauge.TopSite.Count > 0 && gauge.TopSite[0].indicator > 100)
                               //     gauge.MaxValue = Convert.ToInt32(gauge.TopSite[0].indicator);

                                gauge.TopSite = gauge.TopSite.Take(3).ToList();

                                gauge.ShowGauge = definition.ShowGauge;
                                gauge.IsReverse = definition.ReverseSortingOrder;
                            }

                            break;

                        default:
                            break;
                    }

                    display.Gauges.Add(gauge);
                }

            }

            //note: reorder, as we want the first 5 to be ones that want gauges.
            display.Gauges = display.Gauges.OrderByDescending(c => c.ShowGauge).ToList();

            return display;
        }

        #endregion
    }



    #region Scrolling
    
    public static class ScrollingHelper
    {
        public static string GetScrollingTickets(List<DashboardData> allData, List<IndicatorDefinition> indicatorDefinitions)
        {
            allData = allData.OrderBy(c => c.Site.SiteName).ToList();

            var stringBuilder = new StringBuilder();

            foreach (var data in allData)
            {              
                foreach (var definition in indicatorDefinitions)
                {
                    switch (definition.ColumnNumber)
                    {
                        case 20:
                            {
                               stringBuilder.Append(data.Site.SiteName + " " + data.Column20.Value + " - ");
                                break;
                            }
                    }  
                }
            }

            return stringBuilder.ToString();
        }

        public static string GetScrollingOPD(List<DashboardData> allData, List<IndicatorDefinition> indicatorDefinitions)
        {
            allData = allData.OrderBy(c => c.Site.SiteName).ToList();

            var stringBuilder = new StringBuilder();

            foreach (var data in allData)
            {
                foreach (var definition in indicatorDefinitions)
                {
                    switch (definition.ColumnNumber)
                    {
                        case 13:
                            {
                                stringBuilder.Append(data.Site.SiteName);

                                stringBuilder.Append(" " + string.Format(definition.IndicatorFormat, Convert.ToDecimal(data.Column13.Value) / 100) + " - ");
                                break;
                            }
                   }
                }
            }

            return stringBuilder.ToString();
        }

        //OTD : Out The Door
        public static string GetScrollingOTD(List<DashboardData> allData, List<IndicatorDefinition> indicatorDefinitions)
        {
            allData = allData.OrderBy(c => c.Site.SiteName).ToList();

            var stringBuilder = new StringBuilder();

            foreach (var data in allData)
            {
                foreach (var definition in indicatorDefinitions)
                {
                    switch (definition.ColumnNumber)
                    {
                        case 8:
                            {
                                stringBuilder.Append(data.Site.SiteName);

                                if (data.Column8.Value == 0)
                                {
                                    stringBuilder.Append(" 0 - ");
                                }
                                else
                                {
                                    stringBuilder.Append(" " +
                                                         string.Format(definition.IndicatorFormat, Convert.ToDecimal(data.Column8.Value)/ definition.Divisor ) + " - ");

                                }
                                break;
                            }
                    }
                }
            }

            return stringBuilder.ToString();
        }
/*
        public static IList<SiteScroller> ScrollingSite(List<DashboardData> allData, List<IndicatorDefinition> indicatorDefinitions)
        {
            var scrollerData = new List<SiteScroller>();

            

            allData = allData.OrderBy(c => c.Site.SiteName).ToList();

            foreach (DashboardData data in allData)
            {

                var siteScroller = new SiteScroller();
                siteScroller.LocationName = data.Site.SiteName;

                foreach (var definition in indicatorDefinitions)
                {

                    switch (definition.ColumnNumber)
                    {
                        case 1:
                            //System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol + ??
                            siteScroller.Avg = (string.Format(definition.IndicatorFormat, data.Column1.Value / definition.Divisor));
                            break;

                        case 8:

                            siteScroller.InStore = (string.Format(definition.IndicatorFormat, data.Column8.Value / definition.Divisor));
                            break;
                            
                        case 13:

                            siteScroller.OPD = (string.Format(definition.IndicatorFormat, Convert.ToDecimal(data.Column13.Value) / definition.Divisor));

                            break;

                        case 20:

                            siteScroller.Tick = data.Column20.Value.ToString();
                            break;

                    }

                }

                scrollerData.Add(siteScroller);
            }

            

            return scrollerData;
        }
*/
    }

#endregion

    public static class RankingHelper
    {

        public static int GetRegionRanking(Site site, List<DashboardData> allRegionData)
        {
            var ranking = allRegionData.FindIndex(c => c.Site.Id == site.Id);
            
            return ranking + 1;//note: 0 based index
        }

        /// <summary>
        /// If you have reverse rankings eg, the lower the better
        /// </summary>
        /// <param name="columnData"></param>
        /// <param name="divisorValue"></param>
        /// <param name="indictatorSuffix"></param>
        /// <param name="isReverseRanking"></param>
        /// <returns></returns>
        public static IList<SiteRank> RankSites(List<SiteRank> columnData, int divisorValue, string indicatorPrefix, string indictatorSuffix, bool isReverseRanking)
        {
            var siteRanks = new List<SiteRank>();
            var i = 1;
            var y = columnData.Count;

            //if we are using % against another column
            foreach (SiteRank data in columnData)
            {
                 if (data.indicatorDivisor.HasValue & data.indicator > 0)
                 {
                    //data.indicator = data.indicator *100; // = Math.Round(((decimal) data.indicator/(decimal) data.indicatorDivisor.Value), 2)*100;
                    data.indicator = Math.Round(((decimal) data.indicator/(decimal) data.indicatorDivisor.Value), 2)*10000;
                 }
             }
            
            if (columnData.Count > 3) //make sure we have enough to play with
            {
                var list = columnData.OrderByDescending(c => c.indicator);

                if (isReverseRanking)
                {
                    foreach (var data in list) // reverse ranking, so count backwards
                    {
                        var siteRanking = Math.Round(data.indicator/divisorValue, 2);

                        if (siteRanking > 0)
                        {
                            siteRanks.Add(new SiteRank(y--, data.name, siteRanking, indicatorPrefix, indictatorSuffix, null));
                        }
                    }
                    siteRanks.Reverse();

                    //since there may be less than the y (as some > 0)... re-index the completed list
                    var newIndex = 1;

                    foreach (SiteRank siteRank in siteRanks)
                    {
                        siteRank.rank = newIndex++;
                    }

                }
                else
                {
                    foreach (var data in list)
                    {
                       var siteRanking = Math.Round(data.indicator/divisorValue, 2);
                       
                        if (siteRanking > 0)
                        {
                            siteRanks.Add(new SiteRank(i++, data.name, siteRanking, indicatorPrefix, indictatorSuffix, null));
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < 3; j++)
                {
                    siteRanks.Add(new SiteRank(j + 1, "no recent data", 0, indicatorPrefix, indictatorSuffix, null));
                }
            }

            return siteRanks; //note: return the entire list, as we use it for Site Ranking.
        }

        
    }

}
