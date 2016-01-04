
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region Site

	/// <summary>
	/// Site object for NHibernate mapped table 'tbl_Site'.
	/// </summary>
	public partial  class Site
		{
		#region Member Variables
		
		protected int? _id;
		protected int? _ramesesSiteId;
		protected string _name;
		protected string _siteKey;
		protected string _sitePassword;
		protected Country _countryId = new Country();
		protected Company _companyId = new Company();
		
		
		protected IList _siteIdTransactionHistories;
		protected IList _siteIdLoyaltyAccounts;

		#endregion

		#region Constructors

		public Site() { }

		public Site( int? ramesesSiteId, string name, string siteKey, string sitePassword, Country countryId, Company companyId )
		{
			this._ramesesSiteId = ramesesSiteId;
			this._name = name;
			this._siteKey = siteKey;
			this._sitePassword = sitePassword;
			this._countryId = countryId;
			this._companyId = companyId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? RamesesSiteId
		{
			get { return _ramesesSiteId; }
			set { _ramesesSiteId = value; }
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string SiteKey
		{
			get { return _siteKey; }
			set
			{
				if ( value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for SiteKey", value, value.ToString());
				_siteKey = value;
			}
		}

		public string SitePassword
		{
			get { return _sitePassword; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for SitePassword", value, value.ToString());
				_sitePassword = value;
			}
		}

		public Country CountryId
		{
			get { return _countryId; }
			set { _countryId = value; }
		}

		public Company CompanyId
		{
			get { return _companyId; }
			set { _companyId = value; }
		}

		public IList SiteIdTransactionHistories
		{
			get
			{
				if (_siteIdTransactionHistories==null)
				{
					_siteIdTransactionHistories = new ArrayList();
				}
				return _siteIdTransactionHistories;
			}
			set { _siteIdTransactionHistories = value; }
		}

		public IList SiteIdLoyaltyAccounts
		{
			get
			{
				if (_siteIdLoyaltyAccounts==null)
				{
					_siteIdLoyaltyAccounts = new ArrayList();
				}
				return _siteIdLoyaltyAccounts;
			}
			set { _siteIdLoyaltyAccounts = value; }
		}


		#endregion
		
	}

	#endregion
}



