using System.Collections.Generic;
using System.Text;
using WebDashboard.Dao.Domain.Helpers;
using WebDashboard.Dao.Domain;
using System.Linq;

namespace WebDashboard.Mvc.Helpers
{
    public static class DashboardData
    {
        #region GaugeData

        public static Dashboard Get(Site site, IList<Indicator> indicators, IList<Site> allSites)
        {
            var dashboard = new Dashboard {SiteName = site.Name, Columns = new List<Column>()};

            foreach (var indicator in indicators)
            {
                //todo:WTF? there is no 3
                //make sure we are displaying this data
                if (indicator.IndicatorType.Id.Value == 3) continue;

                var column = new Column
                                 {
                                     Name = indicator.LongName ?? indicator.Definition.ShortName,
                                     Data = Gauge(site, indicator),
                                     SiteRank = SiteRanking(site, allSites, indicator),
                                     RegionRank = RegionRanking(site, allSites, indicator),
                                     TopSites = ColumnRankings(allSites, indicator, true).ToList(),
                                     BottomSites = ColumnRankings(allSites, indicator, false).ToList(),
                                 };

                dashboard.Columns.Add(column);
            }

            dashboard.Scrollers = new List<Scroller>();

            foreach (var indicator in indicators)
            {
                if(indicator.DisplayAsScroller)
                {
                    dashboard.Scrollers.Add(ScrollingTickets(allSites, indicator));
                }
            }
            //make sure the 1st 5 are gauges, as Flex doesn't care...
            dashboard.Columns = dashboard.Columns.OrderByDescending(c => c.Data.IsGauge).ToList();

            return dashboard;
        }


        public static Scroller ScrollingTickets(IList<Site> allSites, Indicator indicator)
        {
            var scroller = new Scroller();
            var sb = new StringBuilder();

            var dic = new Dictionary<string, decimal>();

            foreach (var site in allSites)
            {
                var columnData = ColumnData.GetColumnValue(site, indicator.Definition.ColumnNumber);

                var data = ColumnData.GetData(site, indicator, columnData);

                if(data == 0 && indicator.AllowZero == false)
                    continue;

                dic.Add(site.Name, data);              
            }

            
            foreach (var keyValuePair in dic.OrderBy(c => c.Value))
            {
                sb.Append(keyValuePair.Key + ":" + keyValuePair.Value + "   ");
            }

            scroller.Name = indicator.ShortName ?? indicator.Definition.ShortName;
            scroller.Data = sb.ToString();
            
            return scroller;
        }

        private static IList<SiteRank> ColumnRankings(IEnumerable<Site> allSites, Indicator indicator, bool isTopSite)
        {
            var siteRanks = new List<SiteRank>();

            IList<SiteRank> activeSites = new List<SiteRank>();

            foreach (var site in allSites)
            {
                var columnData = ColumnData.GetColumnValue(site, indicator.Definition.ColumnNumber);
                var columnValue = ColumnData.GetData(site, indicator, columnData);

                if(columnValue == 0 && indicator.AllowZero ==false)
                    continue;

                var displayValue = ColumnData.DisplayValue(indicator, columnValue);

                activeSites.Add(new SiteRank(site.Id.Value, 0, site.Name, displayValue, columnValue));

            }


            activeSites = activeSites.OrderBy(c => c.indicator).ToList();

            if(activeSites.Count < 3)
            {
                var i = 3 - activeSites.Count;

                for (var j = 0; j < i; j++)
                {
                    activeSites.Add(new SiteRank(0,0,"Not reporting","0",0));
                }
            }

            if(!indicator.ReverseSort)
            {
                activeSites = activeSites.OrderBy(c => c.indicator).ToList();
            }

            var siteCount = activeSites.Count;

            foreach (var list in activeSites)
            {
                var indexOf = activeSites.IndexOf(list); 
                siteRanks.Add(new SiteRank(list.siteId, indexOf+1,list.name,list.value, list.indicator));
            }

            if (!isTopSite)
                siteRanks.Reverse();

            if (!indicator.ReverseSort)
                siteRanks.Reverse();

            if (siteRanks.Count > 2)
            {
                siteRanks = siteRanks.Take(3).ToList();  
            }
                            
            if (!isTopSite)
            {
                for (var i = 0; i < siteRanks.Count; i++)
                {
                   siteRanks[i].rank = siteCount--;
                }

                siteRanks.Reverse();
            }
            else
            {
                for (var i = 0; i < siteRanks.Count; i++)
                {
                    siteRanks[i].rank = i+1;
                }
            }


            return siteRanks;
        }

        private static int SiteRanking(Site site,  IEnumerable<Site> allSites,Indicator indicator)
        {
            return ColumnData.RankSite(site, allSites, indicator);
       }

        private static int RegionRanking(Site site, IEnumerable<Site> allSites, Indicator indicator)
        {
            var regionalSites = allSites.OfType<Site>().Where(c => c.Region.Id.Value == site.Region.Id.Value);

            return ColumnData.RankSite(site, regionalSites, indicator);
        }

        private static Data Gauge(Site site, Indicator indicator)
        {
            var gauge = new Data
                            {
                                Benchmark = indicator.BenchMark,
                                IsReverse = indicator.ReverseSort,
                                Name = indicator.LongName ?? indicator.Definition.ShortName,
                                MaxValue = indicator.MaxValue
                            };

            switch (indicator.IndicatorType.Id)
            {
                case 1:
                    gauge.IsGauge = false;
                    break;
                case 2:
                    gauge.IsGauge = true;
                    break;
            }

            gauge.CurrentValue = ColumnData.GetGaugeData(site, indicator);
            gauge.DisplayValue = ColumnData.DisplayValue(indicator, gauge.CurrentValue);

            return gauge;
        }

        #endregion

    }
}
