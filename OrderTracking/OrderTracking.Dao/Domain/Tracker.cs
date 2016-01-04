using System;

namespace OrderTracking.Dao.Domain
{
	#region Tracker

	/// <summary>
	/// Tracker object for NHibernate mapped table 'tbl_Tracker'.
	/// </summary>
	public class Tracker : Entity.Entity
    {
		#region Member Variables
		
		protected string _name;
        protected int _batteryLevel;
		protected string _iMEI;
		protected string _serialNumber;
		protected string _phoneNumber;
		protected bool _active;
		protected Coordinates _coordinates;
		protected TrackerStatus _status;
		protected Store _storeId;
		protected Driver _driverId;
		protected Apn _apnId;
		protected TrackerType _typeId;
		protected DateTime? _lastUpdate;
		protected Double? _speed;

		#endregion

        #region Public Properties

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
                _name = value;
            }
        }

        public int BatteryLevel
        {
            get { return _batteryLevel; }
            set { _batteryLevel = value; }
        }

        public string IMEI
        {
            get { return _iMEI; }
            set { _iMEI = value; }
        }

        public string SerialNumber
        {
            get { return _serialNumber; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for SerialNumber", value, value.ToString());
                _serialNumber = value;
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for PhoneNumber", value, value.ToString());
                _phoneNumber = value;
            }
        }

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public Coordinates Coordinates
        {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        public TrackerStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Store Store
        {
            get { return _storeId; }
            set { _storeId = value; }
        }

        public Driver Driver
        {
            get { return _driverId; }
            set { _driverId = value; }
        }

        public Apn Apn
        {
            get { return _apnId; }
            set { _apnId = value; }
        }

        public TrackerType Type
        {
            get { return _typeId; }
            set { _typeId = value; }
        }
        
        public DateTime? LastUpdate
        {
            get { return this._lastUpdate; }
            set { this._lastUpdate = value; }
        }
        
        public double? Speed
        {
            get { return this._speed; }
            set { this._speed = value; }
        }

        #endregion
        
		#region Constructors

		public Tracker() { }

        public Tracker(
            string name, 
            Driver driver, 
            TrackerStatus status, 
            Store store)
        {
            this._name = name;
            this._driverId = driver;
            this._status = status;
            this._storeId = store;
        }

        public Tracker(
            string name, 
            int batteryLevel, 
            string iMEI, 
            string serialNumber, 
            string phoneNumber, 
            bool active, 
            Apn apn, 
            TrackerType type, 
            Driver driver, 
            TrackerStatus status, 
            Store store, 
            Coordinates coordinates,
            DateTime? lastUpdate,
            double? speed)
        {

            this._name = name;
            this._batteryLevel = batteryLevel;
			this._iMEI = iMEI;
			this._serialNumber = serialNumber;
			this._phoneNumber = phoneNumber;
			this._active = active;
			this._coordinates = coordinates;
			this._status = status;
			this._storeId = store;
			this._driverId = driver;
			this._apnId = apn;
			this._typeId = type;
			this._lastUpdate = lastUpdate;
			this._speed = speed;
        }

		#endregion
	}

	#endregion
}



