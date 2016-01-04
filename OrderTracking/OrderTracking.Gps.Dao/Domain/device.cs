
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Device

	/// <summary>
	/// Device object for NHibernate mapped table 'device'.
	/// </summary>
	public class Device
		{
		#region Member Variables
		
		protected int? _id;
		protected string _devicename;
		protected string _botype;
		protected DateTime? _created;
		protected int? _hideposition;
		protected double? _proximity;
		protected string _iMEI;
		protected string _phonenumber;
		protected string _lastip;
		protected int? _lastport;
		protected string _staticip;
		protected int? _staticport;
		protected double? _longitude;
		protected double? _latitude;
		protected double? _groundspeed;
		protected double? _altitude;
		protected double? _heading;
		protected DateTime? _timestamp;
		protected int? _milliseconds;
		protected string _protocolid;
		protected int? _protocolversionid;
		protected short _valid;
		protected string _apn;
		protected string _gprsusername;
		protected string _gprspassword;
		protected int? _devdefid;
		protected string _outgoingtransport;
		protected string _email;
        protected User _ownerid = new User();
        protected Msgfielddictionary _msgfielddictionary;
        //protected Mobilenetwork _mobilenetwork;
		
		
		protected IList _gaterecordlatests;
		protected IList _cmdqueues;
		protected IList _clientcmdqueueitems;

		#endregion

		#region Constructors

		public Device() { }

        public Device(string devicename, string botype, DateTime? created, int? hideposition, double? proximity, string iMEI, string phonenumber, string lastip, int? lastport, string staticip, int? staticport, double? longitude, double? latitude, double? groundspeed, double? altitude, double? heading, DateTime? timestamp, int? milliseconds, string protocolid, int? protocolversionid, short valid, string apn, string gprsusername, string gprspassword, int? devdefid, string outgoingtransport, string email, User ownerid, Msgfielddictionary msgfielddictionary)
		{
			this._devicename = devicename;
			this._botype = botype;
			this._created = created;
			this._hideposition = hideposition;
			this._proximity = proximity;
			this._iMEI = iMEI;
			this._phonenumber = phonenumber;
			this._lastip = lastip;
			this._lastport = lastport;
			this._staticip = staticip;
			this._staticport = staticport;
			this._longitude = longitude;
			this._latitude = latitude;
			this._groundspeed = groundspeed;
			this._altitude = altitude;
			this._heading = heading;
			this._timestamp = timestamp;
			this._milliseconds = milliseconds;
			this._protocolid = protocolid;
			this._protocolversionid = protocolversionid;
			this._valid = valid;
			this._apn = apn;
			this._gprsusername = gprsusername;
			this._gprspassword = gprspassword;
			this._devdefid = devdefid;
			this._outgoingtransport = outgoingtransport;
			this._email = email;
            this._ownerid = ownerid;
            this._msgfielddictionary = msgfielddictionary;
            //this._mobilenetwork = mobilenetwork;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Devicename
		{
			get { return _devicename; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Devicename", value, value.ToString());
				_devicename = value;
			}
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public int? Hideposition
		{
			get { return _hideposition; }
			set { _hideposition = value; }
		}

		public double? Proximity
		{
			get { return _proximity; }
			set { _proximity = value; }
		}

		public string IMEI
		{
			get { return _iMEI; }
			set
			{
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for IMEI", value, value.ToString());
				_iMEI = value;
			}
		}

		public string Phonenumber
		{
			get { return _phonenumber; }
			set
			{
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for Phonenumber", value, value.ToString());
				_phonenumber = value;
			}
		}

		public string Lastip
		{
			get { return _lastip; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Lastip", value, value.ToString());
				_lastip = value;
			}
		}

		public int? Lastport
		{
			get { return _lastport; }
			set { _lastport = value; }
		}

		public string Staticip
		{
			get { return _staticip; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Staticip", value, value.ToString());
				_staticip = value;
			}
		}

		public int? Staticport
		{
			get { return _staticport; }
			set { _staticport = value; }
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

		public double? Groundspeed
		{
			get { return _groundspeed; }
			set { _groundspeed = value; }
		}

		public double? Altitude
		{
			get { return _altitude; }
			set { _altitude = value; }
		}

		public double? Heading
		{
			get { return _heading; }
			set { _heading = value; }
		}

		public DateTime? Timestamp
		{
			get { return _timestamp; }
			set { _timestamp = value; }
		}

		public int? Milliseconds
		{
			get { return _milliseconds; }
			set { _milliseconds = value; }
		}

		public string Protocolid
		{
			get { return _protocolid; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Protocolid", value, value.ToString());
				_protocolid = value;
			}
		}

		public int? Protocolversionid
		{
			get { return _protocolversionid; }
			set { _protocolversionid = value; }
		}

		public short Valid
		{
			get { return _valid; }
			set { _valid = value; }
		}

		public string Apn
		{
			get { return _apn; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Apn", value, value.ToString());
				_apn = value;
			}
		}

		public string Gprsusername
		{
			get { return _gprsusername; }
			set
			{
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for Gprsusername", value, value.ToString());
				_gprsusername = value;
			}
		}

		public string Gprspassword
		{
			get { return _gprspassword; }
			set
			{
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for Gprspassword", value, value.ToString());
				_gprspassword = value;
			}
		}

		public int? Devdefid
		{
			get { return _devdefid; }
			set { _devdefid = value; }
		}

		public string Outgoingtransport
		{
			get { return _outgoingtransport; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Outgoingtransport", value, value.ToString());
				_outgoingtransport = value;
			}
		}

		public string Email
		{
			get { return _email; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Email", value, value.ToString());
				_email = value;
			}
		}
       
		public User owner_id
		{
			get { return _ownerid; }
			set { _ownerid = value; }
		}

		public Msgfielddictionary Msgfielddictionary
		{
			get { return _msgfielddictionary; }
			set { _msgfielddictionary = value; }
		}
 /*
		public Mobilenetwork Mobilenetwork
		{
			get { return _mobilenetwork; }
			set { _mobilenetwork = value; }
		}

		public IList gate_record_latests
		{
			get
			{
				if (_gaterecordlatests==null)
				{
					_gaterecordlatests = new ArrayList();
				}
				return _gaterecordlatests;
			}
			set { _gaterecordlatests = value; }
		}

		public IList cmd_queues
		{
			get
			{
				if (_cmdqueues==null)
				{
					_cmdqueues = new ArrayList();
				}
				return _cmdqueues;
			}
			set { _cmdqueues = value; }
		}

		public IList client_cmd_queue_items
		{
			get
			{
				if (_clientcmdqueueitems==null)
				{
					_clientcmdqueueitems = new ArrayList();
				}
				return _clientcmdqueueitems;
			}
			set { _clientcmdqueueitems = value; }
		}
        */

		#endregion
		
	}

	#endregion
}



