
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region Region

	/// <summary>
	/// Region object for NHibernate mapped table 'Regions'.
	/// </summary>
	public class Region : Entity
		{
		#region Member Variables

        protected string _regionName;
		protected HeadOffice _headOffice;
		
		
		protected IList _sitesRegionses;

		#endregion

		#region Constructors

		public Region() { }

		public Region( string regionName, HeadOffice headOffice )
		{
			this._regionName = regionName;
			this._headOffice = headOffice;
		}

		#endregion

		#region Public Properties

		public string RegionName
		{
			get { return _regionName; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for RegionName", value, value.ToString());
				_regionName = value;
			}
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


		#endregion
		
	}

	#endregion
}



