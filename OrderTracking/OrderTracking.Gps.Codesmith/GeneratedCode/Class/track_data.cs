
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Trackdata

	/// <summary>
	/// Trackdata object for NHibernate mapped table 'track_data'.
	/// </summary>
	public partial  class Trackdata
		{
		#region Member Variables
		
		protected long? _id;
		protected double? _longitude;
		protected double? _latitude;
		protected double? _altitude;
		protected double? _heading;
		protected double? _groundspeed;
		protected DateTime? _timestamp;
		protected int? _milliseconds;
		protected double? _distancenext;
		protected short _valid;
		protected Trackinfo _trackinfo = new Trackinfo();
		
		
		protected IList _trackdatamods;

		#endregion

		#region Constructors

		public Trackdata() { }

		public Trackdata( double? longitude, double? latitude, double? altitude, double? heading, double? groundspeed, DateTime? timestamp, int? milliseconds, double? distancenext, short valid, Trackinfo trackinfo )
		{
			this._longitude = longitude;
			this._latitude = latitude;
			this._altitude = altitude;
			this._heading = heading;
			this._groundspeed = groundspeed;
			this._timestamp = timestamp;
			this._milliseconds = milliseconds;
			this._distancenext = distancenext;
			this._valid = valid;
			this._trackinfo = trackinfo;
		}

		#endregion

		#region Public Properties

		public long? Id
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

		public double? Groundspeed
		{
			get { return _groundspeed; }
			set { _groundspeed = value; }
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

		public double? Distancenext
		{
			get { return _distancenext; }
			set { _distancenext = value; }
		}

		public short Valid
		{
			get { return _valid; }
			set { _valid = value; }
		}

		public Trackinfo Trackinfo
		{
			get { return _trackinfo; }
			set { _trackinfo = value; }
		}

		public IList track_data_mods
		{
			get
			{
				if (_trackdatamods==null)
				{
					_trackdatamods = new ArrayList();
				}
				return _trackdatamods;
			}
			set { _trackdatamods = value; }
		}


		#endregion
		
	}

	#endregion
}



