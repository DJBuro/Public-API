
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateuser

	/// <summary>
	/// Gateuser object for NHibernate mapped table 'gate_user'.
	/// </summary>
	public partial  class Gateuser
		{
		#region Member Variables
		
		protected int? _id;
		protected double? _longitude;
		protected double? _latitude;
		protected double? _groundspeed;
		protected double? _altitude;
		protected double? _heading;
		protected DateTime? _timestamp;
		protected DateTime? _servertimestamp;
		protected DateTime? _deviceactivity;
		protected int? _delay;
		protected string _lasttransport;
		
		
		protected IList _gaterecordlatests;

		#endregion

		#region Constructors

		public Gateuser() { }

		public Gateuser( double? longitude, double? latitude, double? groundspeed, double? altitude, double? heading, DateTime? timestamp, DateTime? servertimestamp, DateTime? deviceactivity, int? delay, string lasttransport )
		{
			this._longitude = longitude;
			this._latitude = latitude;
			this._groundspeed = groundspeed;
			this._altitude = altitude;
			this._heading = heading;
			this._timestamp = timestamp;
			this._servertimestamp = servertimestamp;
			this._deviceactivity = deviceactivity;
			this._delay = delay;
			this._lasttransport = lasttransport;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
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

		public DateTime? Servertimestamp
		{
			get { return _servertimestamp; }
			set { _servertimestamp = value; }
		}

		public DateTime? Deviceactivity
		{
			get { return _deviceactivity; }
			set { _deviceactivity = value; }
		}

		public int? Delay
		{
			get { return _delay; }
			set { _delay = value; }
		}

		public string Lasttransport
		{
			get { return _lasttransport; }
			set
			{
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for Lasttransport", value, value.ToString());
				_lasttransport = value;
			}
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


		#endregion
		
	}

	#endregion
}



