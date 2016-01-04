using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dashboard.Dao.Domain.Helpers
{
    public class Display
    {
        public IList<Gauge> Gauges { get; set; }

    }

    public class FlexDashboard
    {
        public string HeadOfficeMessage { get; set; }
        public IList<Gauge> Gauges { get; set; }
        public string scrollingOtd { get; set; }
        public string scrollingOpd { get; set; }
        public string scrollingTickets { get; set; }
    }


    //todo: rename/refactor class
    public class SiteRank
    {
        public int rank { get; set;}
        public string name { get; set;}
        public decimal indicator { get; set; }
        public string indicatorPrefix { get; set; }
        public string indictatorSuffix { get; set; }
        public decimal? indicatorDivisor { get; set; }

        public SiteRank(int rank, string siteName, decimal indicator, string indicatorPrefix, string indictatorSuffix, decimal? indicatorDivisor)
		{
			this.rank = rank;
			this.name = siteName;
            this.indicator = indicator;
            this.indicatorPrefix = indicatorPrefix;
            this.indictatorSuffix = indictatorSuffix;
            this.indicatorDivisor = indicatorDivisor;
		}

        public SiteRank()
        {
            
        }
    }

}
