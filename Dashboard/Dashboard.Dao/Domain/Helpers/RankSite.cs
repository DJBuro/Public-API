using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dashboard.Dao.Domain.Helpers
{
    // todo: depreciate
    public class RankSite
    {

        protected int _rank;
        protected DashboardData _data;
        protected string _data2;
        protected Site _site;

        public RankSite() { }

        public RankSite(int rank, Site site, DashboardData data, string data2)
		{
			this._rank = rank;
			this._site = site;
			this._data = data;
            this._data2 = data2;
		}

        public int Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }

        public Site Site
        {
            get { return _site; }
            set { _site = value; }
        }

        public DashboardData Data
        {
            get { return _data; }
            set{_data = value;}
        }

        public string Data2
        {
            get { return _data2; }
            set { _data2 = value; }
        }
    }
}
