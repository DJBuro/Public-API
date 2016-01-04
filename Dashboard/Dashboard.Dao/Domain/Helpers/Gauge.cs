using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dashboard.Dao.Domain.Helpers
{
    public class Gauge
    {
        public decimal CurrentValue { get; set; }
        public int MaxValue { get;set; }
        public string Name { get; set; }
        public string Benchmark { get; set; }
        public bool ShowGauge { get; set; }
        public bool IsReverse { get; set; }
        public string PrefixIndicator { get; set; }
        public string SuffixIndicator { get; set; }
        public int SiteRank { get; set; }
        public int RegionRank { get; set; }

        public IList<SiteRank> TopSite;
        public IList<SiteRank> BottomSite;


    }
}