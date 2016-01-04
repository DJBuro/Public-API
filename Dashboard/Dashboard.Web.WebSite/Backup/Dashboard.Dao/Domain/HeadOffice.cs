
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region HeadOffice

	/// <summary>
	/// HeadOffice object for NHibernate mapped table 'HeadOffice'.
	/// </summary>
	public class HeadOffice : Entity
		{
		#region Member Variables

		protected string _headOfficeName;
		protected InterfaceTextLangauge _language;
		
		
		protected IList _regionses;
		protected IList _indicatorDefinitionses;
		protected IList _tradingTimeses;
		protected IList _siteMessageses;
		protected IList _siteses;
		protected IList _headOfficeIdDashboardUsers;

		#endregion

		#region Constructors

		public HeadOffice() { }

		public HeadOffice( string headOfficeName, InterfaceTextLangauge language )
		{
			this._headOfficeName = headOfficeName;
			this._language = language;
		}

		#endregion

		#region Public Properties

		public string HeadOfficeName
		{
			get { return _headOfficeName; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for HeadOfficeName", value, value.ToString());
				_headOfficeName = value;
			}
		}

		public InterfaceTextLangauge Language
		{
			get { return _language; }
			set { _language = value; }
		}

		public IList Regions
		{
			get
			{
				if (_regionses==null)
				{
					_regionses = new ArrayList();
				}
				return _regionses;
			}
			set { _regionses = value; }
		}

		public IList IndicatorDefinitions
		{
			get
			{
				if (_indicatorDefinitionses==null)
				{
					_indicatorDefinitionses = new ArrayList();
				}
				return _indicatorDefinitionses;
			}
			set { _indicatorDefinitionses = value; }
		}

		public IList TradingTimes
		{
			get
			{
				if (_tradingTimeses==null)
				{
					_tradingTimeses = new ArrayList();
				}
				return _tradingTimeses;
			}
			set { _tradingTimeses = value; }
		}

		public IList SiteMessages
		{
			get
			{
				if (_siteMessageses==null)
				{
					_siteMessageses = new ArrayList();
				}
				return _siteMessageses;
			}
			set { _siteMessageses = value; }
		}

		public IList Sites
		{
			get
			{
				if (_siteses==null)
				{
					_siteses = new ArrayList();
				}
				return _siteses;
			}
			set { _siteses = value; }
		}

		public IList DashboardUsers
		{
			get
			{
				if (_headOfficeIdDashboardUsers==null)
				{
					_headOfficeIdDashboardUsers = new ArrayList();
				}
				return _headOfficeIdDashboardUsers;
			}
			set { _headOfficeIdDashboardUsers = value; }
		}

		#endregion
		
	}

	#endregion
}



