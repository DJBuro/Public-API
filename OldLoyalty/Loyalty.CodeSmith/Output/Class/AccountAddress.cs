
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region AccountAddress

	/// <summary>
	/// AccountAddress object for NHibernate mapped table 'tbl_AccountAddress'.
	/// </summary>
	public partial  class AccountAddress
		{
		#region Member Variables
		
		protected int? _id;
		protected string _addressLineOne;
		protected string _addressLineTwo;
		protected string _addressLineThree;
		protected string _addressLineFour;
		protected string _townCity;
		protected string _countyProvince;
		protected string _postCode;
		protected LoyaltyAccount _loyaltyAccountId = new LoyaltyAccount();
		protected Country _countryId = new Country();
		
		

		#endregion

		#region Constructors

		public AccountAddress() { }

		public AccountAddress( string addressLineOne, string addressLineTwo, string addressLineThree, string addressLineFour, string townCity, string countyProvince, string postCode, LoyaltyAccount loyaltyAccountId, Country countryId )
		{
			this._addressLineOne = addressLineOne;
			this._addressLineTwo = addressLineTwo;
			this._addressLineThree = addressLineThree;
			this._addressLineFour = addressLineFour;
			this._townCity = townCity;
			this._countyProvince = countyProvince;
			this._postCode = postCode;
			this._loyaltyAccountId = loyaltyAccountId;
			this._countryId = countryId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string AddressLineOne
		{
			get { return _addressLineOne; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for AddressLineOne", value, value.ToString());
				_addressLineOne = value;
			}
		}

		public string AddressLineTwo
		{
			get { return _addressLineTwo; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for AddressLineTwo", value, value.ToString());
				_addressLineTwo = value;
			}
		}

		public string AddressLineThree
		{
			get { return _addressLineThree; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for AddressLineThree", value, value.ToString());
				_addressLineThree = value;
			}
		}

		public string AddressLineFour
		{
			get { return _addressLineFour; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for AddressLineFour", value, value.ToString());
				_addressLineFour = value;
			}
		}

		public string TownCity
		{
			get { return _townCity; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for TownCity", value, value.ToString());
				_townCity = value;
			}
		}

		public string CountyProvince
		{
			get { return _countyProvince; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for CountyProvince", value, value.ToString());
				_countyProvince = value;
			}
		}

		public string PostCode
		{
			get { return _postCode; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for PostCode", value, value.ToString());
				_postCode = value;
			}
		}

		public LoyaltyAccount LoyaltyAccountId
		{
			get { return _loyaltyAccountId; }
			set { _loyaltyAccountId = value; }
		}

		public Country CountryId
		{
			get { return _countryId; }
			set { _countryId = value; }
		}


		#endregion
		
	}

	#endregion
}



