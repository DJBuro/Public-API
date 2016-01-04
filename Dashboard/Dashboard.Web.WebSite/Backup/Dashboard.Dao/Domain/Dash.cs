using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dashboard.Dao.Domain.Helpers;

namespace Dashboard.Dao.Domain
{
    public class Dash
    {

        protected IList<DashData> _DashDataItem;
        public IList<DashData> DashData
        {
            get
            {
                if (_DashDataItem == null)
                {
                    _DashDataItem = new List<DashData>();
                }
                return _DashDataItem;
            }
            set { _DashDataItem = value; }
        }

    }

    public class DashData
    {
        public string IndicatorName { get; set; }
        public double IndicatorValue { get; set; }
        public string ChartColor { get; set; }

        public int? DisplayOrder { get; set; }
        public string ChartLegend{ get; set;}

        public string ChartIndicatorValue { get; set; }

        public string IndicatorBenchmark { get; set; } //note, change db to be int?

        public int ChartType { get; set; }

        public int SiteRank { get; set; }
        public int RegionRank { get; set; }



        public IList<RankSite> TopSites { get; set; }
        public IList<RankSite> BottomSites { get; set; }

      

    }
}
