
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region Country

	/// <summary>
	/// Country object for NHibernate mapped table 'tbl_Country'.
	/// </summary>
	public partial  class Country
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _iSOCode;
		
		
		protected IList _countryIdAccountAddresses;
		protected IList _countryIdCompanies;
		protected IList _countryIdSites;
		protected IList _countryIdCountryCurrencies;

		#endregion

		#region Constructors

		public Country() { }

		public Country( string name, string iSOCode )
		{
			this._name = name;
			this._iSOCode = iSOCode;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
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

		public string ISOCode
		{
			get { return _iSOCode; }
			set
			{
				if ( value != null && value.Length > 6)
					throw new ArgumentOutOfRangeException("Invalid value for ISOCode", value, value.ToString());
				_iSOCode = value;
			}
		}

		public IList CountryIdAccountAddresses
		{
			get
			{
				if (_countryIdAccountAddresses==null)
				{
					_countryIdAccountAddresses = new ArrayList();
				}
				return _countryIdAccountAddresses;
			}
			set { _countryIdAccountAddresses = value; }
		}

		public IList CountryIdCompanies
		{
			get
			{
				if (_countryIdCompanies==null)
				{
					_countryIdCompanies = new ArrayList();
				}
				return _countryIdCompanies;
			}
			set { _countryIdCompanies = value; }
		}

		public IList CountryIdSites
		{
			get
			{
				if (_countryIdSites==null)
				{
					_countryIdSites = new ArrayList();
				}
				return _countryIdSites;
			}
			set { _countryIdSites = value; }
		}

		public IList CountryIdCountryCurrencies
		{
			get
			{
				if (_countryIdCountryCurrencies==null)
				{
					_countryIdCountryCurrencies = new ArrayList();
				}
				return _countryIdCountryCurrencies;
			}
			set { _countryIdCountryCurrencies = value; }
		}


		#endregion
		
	}

	#endregion
}



