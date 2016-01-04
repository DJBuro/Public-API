
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region RamesesAddress

	/// <summary>
	/// RamesesAddress object for NHibernate mapped table 'tbl_RamesesAddress'.
	/// </summary>
	public partial  class RamesesAddress
		{
		#region Member Variables
		
		protected int? _id;
		protected int? _contactID;
		protected string _contact;
		protected int? _addressType;
		protected int? _optFlag;
		protected int? _addressID;
		protected int? _postOfficeID;
		protected string _subAddress;
		protected string _org1;
		protected string _org2;
		protected string _org3;
		protected string _prem1;
		protected string _prem2;
		protected string _prem3;
		protected string _roadNum;
		protected string _roadName;
		protected string _locality;
		protected string _town;
		protected string _county;
		protected string _postCode;
		protected string _grid;
		protected string _refno;
		protected string _directions;
		protected string _dps;
		protected string _pafType;
		protected int? _flags;
		protected int? _timesOrdered;
		
		
		protected IList _ramesesAddressIdRamesesAddressLoyaltyCards;

		#endregion

		#region Constructors

		public RamesesAddress() { }

		public RamesesAddress( int? contactID, string contact, int? addressType, int? optFlag, int? addressID, int? postOfficeID, string subAddress, string org1, string org2, string org3, string prem1, string prem2, string prem3, string roadNum, string roadName, string locality, string town, string county, string postCode, string grid, string refno, string directions, string dps, string pafType, int? flags, int? timesOrdered )
		{
			this._contactID = contactID;
			this._contact = contact;
			this._addressType = addressType;
			this._optFlag = optFlag;
			this._addressID = addressID;
			this._postOfficeID = postOfficeID;
			this._subAddress = subAddress;
			this._org1 = org1;
			this._org2 = org2;
			this._org3 = org3;
			this._prem1 = prem1;
			this._prem2 = prem2;
			this._prem3 = prem3;
			this._roadNum = roadNum;
			this._roadName = roadName;
			this._locality = locality;
			this._town = town;
			this._county = county;
			this._postCode = postCode;
			this._grid = grid;
			this._refno = refno;
			this._directions = directions;
			this._dps = dps;
			this._pafType = pafType;
			this._flags = flags;
			this._timesOrdered = timesOrdered;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? ContactID
		{
			get { return _contactID; }
			set { _contactID = value; }
		}

		public string Contact
		{
			get { return _contact; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Contact", value, value.ToString());
				_contact = value;
			}
		}

		public int? AddressType
		{
			get { return _addressType; }
			set { _addressType = value; }
		}

		public int? OptFlag
		{
			get { return _optFlag; }
			set { _optFlag = value; }
		}

		public int? AddressID
		{
			get { return _addressID; }
			set { _addressID = value; }
		}

		public int? PostOfficeID
		{
			get { return _postOfficeID; }
			set { _postOfficeID = value; }
		}

		public string SubAddress
		{
			get { return _subAddress; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for SubAddress", value, value.ToString());
				_subAddress = value;
			}
		}

		public string Org1
		{
			get { return _org1; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Org1", value, value.ToString());
				_org1 = value;
			}
		}

		public string Org2
		{
			get { return _org2; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Org2", value, value.ToString());
				_org2 = value;
			}
		}

		public string Org3
		{
			get { return _org3; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Org3", value, value.ToString());
				_org3 = value;
			}
		}

		public string Prem1
		{
			get { return _prem1; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Prem1", value, value.ToString());
				_prem1 = value;
			}
		}

		public string Prem2
		{
			get { return _prem2; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Prem2", value, value.ToString());
				_prem2 = value;
			}
		}

		public string Prem3
		{
			get { return _prem3; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Prem3", value, value.ToString());
				_prem3 = value;
			}
		}

		public string RoadNum
		{
			get { return _roadNum; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for RoadNum", value, value.ToString());
				_roadNum = value;
			}
		}

		public string RoadName
		{
			get { return _roadName; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for RoadName", value, value.ToString());
				_roadName = value;
			}
		}

		public string Locality
		{
			get { return _locality; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Locality", value, value.ToString());
				_locality = value;
			}
		}

		public string Town
		{
			get { return _town; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Town", value, value.ToString());
				_town = value;
			}
		}

		public string County
		{
			get { return _county; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for County", value, value.ToString());
				_county = value;
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

		public string Grid
		{
			get { return _grid; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Grid", value, value.ToString());
				_grid = value;
			}
		}

		public string Refno
		{
			get { return _refno; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Refno", value, value.ToString());
				_refno = value;
			}
		}

		public string Directions
		{
			get { return _directions; }
			set
			{
				if ( value != null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for Directions", value, value.ToString());
				_directions = value;
			}
		}

		public string Dps
		{
			get { return _dps; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Dps", value, value.ToString());
				_dps = value;
			}
		}

		public string PafType
		{
			get { return _pafType; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for PafType", value, value.ToString());
				_pafType = value;
			}
		}

		public int? Flags
		{
			get { return _flags; }
			set { _flags = value; }
		}

		public int? TimesOrdered
		{
			get { return _timesOrdered; }
			set { _timesOrdered = value; }
		}

		public IList RamesesAddressIdRamesesAddressLoyaltyCards
		{
			get
			{
				if (_ramesesAddressIdRamesesAddressLoyaltyCards==null)
				{
					_ramesesAddressIdRamesesAddressLoyaltyCards = new ArrayList();
				}
				return _ramesesAddressIdRamesesAddressLoyaltyCards;
			}
			set { _ramesesAddressIdRamesesAddressLoyaltyCards = value; }
		}


		#endregion
		
	}

	#endregion
}



