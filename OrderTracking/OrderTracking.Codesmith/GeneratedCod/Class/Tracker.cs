
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Tracker

	/// <summary>
	/// Tracker object for NHibernate mapped table 'tbl_Tracker'.
	/// </summary>
	public partial  class Tracker
		{
		#region Member Variables
		
		protected long? _id;
		protected string _name;
		protected int? _batteryLevel;
		protected string _iMEI;
		protected string _serialNumber;
		protected string _phoneNumber;
		protected bool _active;
		protected Coordinate _coordinates = new Coordinate();
		protected TrackerStatus _status = new TrackerStatus();
		protected TrackerType _typeId = new TrackerType();
		protected Store _storeId = new Store();
		protected Driver _driverId = new Driver();
		protected Apn _apnId = new Apn();
		
		

		#endregion

		#region Constructors

		public Tracker() { }

		public Tracker( string name, int? batteryLevel, string iMEI, string serialNumber, string phoneNumber, bool active, Coordinate coordinates, TrackerStatus status, TrackerType typeId, Store storeId, Driver driverId, Apn apnId )
		{
			this._name = name;
			this._batteryLevel = batteryLevel;
			this._iMEI = iMEI;
			this._serialNumber = serialNumber;
			this._phoneNumber = phoneNumber;
			this._active = active;
			this._coordinates = coordinates;
			this._status = status;
			this._typeId = typeId;
			this._storeId = storeId;
			this._driverId = driverId;
			this._apnId = apnId;
		}

		#endregion

		#region Public Properties

		public long? Id
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

		public int? BatteryLevel
		{
			get { return _batteryLevel; }
			set { _batteryLevel = value; }
		}

		public string IMEI
		{
			get { return _iMEI; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for IMEI", value, value.ToString());
				_iMEI = value;
			}
		}

		public string SerialNumber
		{
			get { return _serialNumber; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for SerialNumber", value, value.ToString());
				_serialNumber = value;
			}
		}

		public string PhoneNumber
		{
			get { return _phoneNumber; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for PhoneNumber", value, value.ToString());
				_phoneNumber = value;
			}
		}

		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}

		public Coordinate Coordinates
		{
			get { return _coordinates; }
			set { _coordinates = value; }
		}

		public TrackerStatus Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public TrackerType TypeId
		{
			get { return _typeId; }
			set { _typeId = value; }
		}

		public Store StoreId
		{
			get { return _storeId; }
			set { _storeId = value; }
		}

		public Driver DriverId
		{
			get { return _driverId; }
			set { _driverId = value; }
		}

		public Apn ApnId
		{
			get { return _apnId; }
			set { _apnId = value; }
		}


		#endregion
		
	}

	#endregion
}



