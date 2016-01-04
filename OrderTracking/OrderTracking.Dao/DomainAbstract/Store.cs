
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
		
		protected int? _id;
		protected string _ramesesId;
		protected string _externalStoreId;
		protected string _name;
		protected double? _longitude;
		protected double? _latitude;
		protected string _monitor;
		protected bool _gPSEnabled;
		
		
		protected IList _storeIdDrivers;
		protected IList _storeIdOrders;

		#endregion

		#region Constructors

		public Store() { }

		public Store( string ramesesId, string externalStoreId, string name, double? longitude, double? latitude, string monitor, bool gPSEnabled )
		{
			this._ramesesId = ramesesId;
			this._externalStoreId = externalStoreId;
			this._name = name;
			this._longitude = longitude;
			this._latitude = latitude;
			this._monitor = monitor;
			this._gPSEnabled = gPSEnabled;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string RamesesId
		{
			get { return _ramesesId; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for RamesesId", value, value.ToString());
				_ramesesId = value;
			}
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

		public double? Longitude
		{
			get { return _longitude; }
			set { _longitude = value; }
		}

		public double? Latitude
		{
			get { return _latitude; }
			set { _latitude = value; }
		}

		public string Monitor
		{
			get { return _monitor; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Monitor", value, value.ToString());
				_monitor = value;
			}
		}

		public bool GPSEnabled
		{
			get { return _gPSEnabled; }
			set { _gPSEnabled = value; }
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


		#endregion
		
	}

	#endregion
}



