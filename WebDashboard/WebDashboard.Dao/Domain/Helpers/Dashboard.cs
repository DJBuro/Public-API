using System.Collections.Generic;

namespace WebDashboard.Dao.Domain.Helpers
{
    public class Dashboard
    {
        public string SiteName { get; set; }
        public string HeadOfficeMessage { get; set; }
        public IList<Column> Columns{ get; set; }
        public IList<Scroller> Scrollers { get; set; }
    }

    public class Scroller
    {
        public string Name { get; set; }
        public string Data { get; set; }

        public Scroller(string Name,string Data)
        {
            this.Name = Name;
            this.Data = Data;
        }

        public Scroller()
        {
            
        }
    }

    public class Column
    {
        public string Name { get; set; }
        public Data Data { get; set; }
        public int SiteRank { get; set; }
        public int RegionRank { get; set; }
        public IList<SiteRank> TopSites;
        public IList<SiteRank> BottomSites;
    }

    public class Data
    {
        public decimal CurrentValue { get; set; }
        public int MaxValue { get; set; }
        public string Name { get; set; }
        public decimal Benchmark { get; set; }
        public string DisplayValue { get; set; }
        public bool IsReverse { get; set; }
        public bool IsGauge { get; set; }
    }


    public class SiteRank
    {
        public int siteId { get; set; }
        public int rank { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public decimal indicator { get; set; }

        public SiteRank(int siteId, int rank, string siteName, string value, decimal indicator)
        {
            this.siteId = siteId;
            this.rank = rank;
            this.name = siteName;
            this.value = value;
            this.indicator = indicator;
        }

        public SiteRank()
        {

        }
    }
}