
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region Company

	/// <summary>
	/// Company object for NHibernate mapped table 'tbl_Company'.
	/// </summary>
	public partial  class Company
		{
		#region Member Variables
		
		protected int? _id;
		protected int? _ramesesHeadOfficeId;
		protected int? _ramesesCompanyId;
		protected string _name;
		protected int? _redemptionRatio;
		protected string _companyKey;
		protected string _companyPassword;
		protected Country _countryId = new Country();
		
		
		protected IList _companyIdCompanyLoyaltyAccounts;
		protected IList _companyIdSites;
		protected IList _companyIdCompanyUserTitleses;

		#endregion

		#region Constructors

		public Company() { }

		public Company( int? ramesesHeadOfficeId, int? ramesesCompanyId, string name, int? redemptionRatio, string companyKey, string companyPassword, Country countryId )
		{
			this._ramesesHeadOfficeId = ramesesHeadOfficeId;
			this._ramesesCompanyId = ramesesCompanyId;
			this._name = name;
			this._redemptionRatio = redemptionRatio;
			this._companyKey = companyKey;
			this._companyPassword = companyPassword;
			this._countryId = countryId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? RamesesHeadOfficeId
		{
			get { return _ramesesHeadOfficeId; }
			set { _ramesesHeadOfficeId = value; }
		}

		public int? RamesesCompanyId
		{
			get { return _ramesesCompanyId; }
			set { _ramesesCompanyId = value; }
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

		public int? RedemptionRatio
		{
			get { return _redemptionRatio; }
			set { _redemptionRatio = value; }
		}

		public string CompanyKey
		{
			get { return _companyKey; }
			set
			{
				if ( value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for CompanyKey", value, value.ToString());
				_companyKey = value;
			}
		}

		public string CompanyPassword
		{
			get { return _companyPassword; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for CompanyPassword", value, value.ToString());
				_companyPassword = value;
			}
		}

		public Country CountryId
		{
			get { return _countryId; }
			set { _countryId = value; }
		}

		public IList CompanyIdCompanyLoyaltyAccounts
		{
			get
			{
				if (_companyIdCompanyLoyaltyAccounts==null)
				{
					_companyIdCompanyLoyaltyAccounts = new ArrayList();
				}
				return _companyIdCompanyLoyaltyAccounts;
			}
			set { _companyIdCompanyLoyaltyAccounts = value; }
		}

		public IList CompanyIdSites
		{
			get
			{
				if (_companyIdSites==null)
				{
					_companyIdSites = new ArrayList();
				}
				return _companyIdSites;
			}
			set { _companyIdSites = value; }
		}

		public IList CompanyIdCompanyUserTitleses
		{
			get
			{
				if (_companyIdCompanyUserTitleses==null)
				{
					_companyIdCompanyUserTitleses = new ArrayList();
				}
				return _companyIdCompanyUserTitleses;
			}
			set { _companyIdCompanyUserTitleses = value; }
		}


		#endregion
		
	}

	#endregion
}



