
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Store

	/// <summary>
	/// Store object for NHibernate mapped table 'tbl_Store'.
	/// </summary>
	public partial  class Store
		{
		#region Member Variables
		
		protected long? _id;
		protected string _externalStoreId;
		protected string _name;
		protected short _deliveryRadius;
		protected Coordinate _coordinates = new Coordinate();
		
		
		protected IList _storeIdOrders;
		protected IList _storeIdDrivers;
		protected IList _storeIdAccounts;
		protected IList _storeIdTrackers;

		#endregion

		#region Constructors

		public Store() { }

		public Store( string externalStoreId, string name, short deliveryRadius, Coordinate coordinates )
		{
			this._externalStoreId = externalStoreId;
			this._name = name;
			this._deliveryRadius = deliveryRadius;
			this._coordinates = coordinates;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string ExternalStoreId
		{
			get { return _externalStoreId; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for ExternalStoreId", value, value.ToString());
				_externalStoreId = value;
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public short DeliveryRadius
		{
			get { return _deliveryRadius; }
			set { _deliveryRadius = value; }
		}

		public Coordinate Coordinates
		{
			get { return _coordinates; }
			set { _coordinates = value; }
		}

		public IList StoreIdOrders
		{
			get
			{
				if (_storeIdOrders==null)
				{
					_storeIdOrders = new ArrayList();
				}
				return _storeIdOrders;
			}
			set { _storeIdOrders = value; }
		}

		public IList StoreIdDrivers
		{
			get
			{
				if (_storeIdDrivers==null)
				{
					_storeIdDrivers = new ArrayList();
				}
				return _storeIdDrivers;
			}
			set { _storeIdDrivers = value; }
		}

		public IList StoreIdAccounts
		{
			get
			{
				if (_storeIdAccounts==null)
				{
					_storeIdAccounts = new ArrayList();
				}
				return _storeIdAccounts;
			}
			set { _storeIdAccounts = value; }
		}

		public IList StoreIdTrackers
		{
			get
			{
				if (_storeIdTrackers==null)
				{
					_storeIdTrackers = new ArrayList();
				}
				return _storeIdTrackers;
			}
			set { _storeIdTrackers = value; }
		}


		#endregion
		
	}

	#endregion
}



