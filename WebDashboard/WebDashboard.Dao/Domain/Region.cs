
using System;
using System.Collections;
using System.Collections.Generic;


namespace WebDashboard.Dao.Domain
{
	#region Region

	/// <summary>
	/// Region object for NHibernate mapped table 'tbl_Region'.
	/// </summary>
    public class Region : Entity.Entity
		{
		#region Member Variables

        protected string _regionName;
		protected HeadOffice _headOffice;
        protected string _timeZone;

        protected IList<Site> _regionIdSites;

		#endregion

		#region Constructors

		public Region() { }

		public Region( string regionName, string timeZone, HeadOffice headOffice )
		{
			this._regionName = regionName;
            this._timeZone = timeZone;
			this._headOffice = headOffice;
		}

		#endregion

		#region Public Properties

		public string Name
		{
			get { return _regionName; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_regionName = value;
			}
		}

        public string TimeZone
        {
            get { return _timeZone; }
            set { _timeZone = value; }
        }

		public HeadOffice HeadOffice
		{
			get { return _headOffice; }
			set { _headOffice = value; }
		}
        
        public IList<Site> RegionalSites
        {
            get
            {
                if (_regionIdSites == null)
                {
                    _regionIdSites = new List<Site>();
                }
                return _regionIdSites;
            }
            set { _regionIdSites = value; }
        }
        

		#endregion
		
	}

	#endregion
}



