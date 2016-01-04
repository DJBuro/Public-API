
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region Site

	/// <summary>
	/// Site object for NHibernate mapped table 'Sites'.
	/// </summary>
	public class Site : Entity
		{
		#region Member Variables
		
		protected string _siteName;
		protected string _iPAddress;
		protected bool _enabled;
		protected HeadOffice _headOffice;
		
		
		protected IList _sitesRegionses;
		protected IList _dashboardDatas;

		#endregion

		#region Constructors

		public Site() { }

		public Site( string siteName, string iPAddress, bool enabled, HeadOffice headOffice )
		{
			this._siteName = siteName;
			this._iPAddress = iPAddress;
			this._enabled = enabled;
			this._headOffice = headOffice;
		}

		#endregion

		#region Public Properties


		public string SiteName
		{
			get { return _siteName; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for SiteName", value, value.ToString());
				_siteName = value;
			}
		}

		public string IPAddress
		{
			get { return _iPAddress; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for IPAddress", value, value.ToString());
				_iPAddress = value;
			}
		}

		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		public HeadOffice HeadOffice
		{
			get { return _headOffice; }
			set { _headOffice = value; }
		}

		public IList SitesRegions
		{
			get
			{
				if (_sitesRegionses==null)
				{
					_sitesRegionses = new ArrayList();
				}
				return _sitesRegionses;
			}
			set { _sitesRegionses = value; }
		}

		public IList DashboardData
		{
			get
			{
				if (_dashboardDatas==null)
				{
					_dashboardDatas = new ArrayList();
				}
				return _dashboardDatas;
			}
			set { _dashboardDatas = value; }
		}


		#endregion
		
	}

	#endregion
}



