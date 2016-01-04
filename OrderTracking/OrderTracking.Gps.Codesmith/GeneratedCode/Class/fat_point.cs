
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Fatpoint

	/// <summary>
	/// Fatpoint object for NHibernate mapped table 'fat_point'.
	/// </summary>
	public partial  class Fatpoint
		{
		#region Member Variables
		
		protected long? _id;
		protected long? _starttrackdataid;
		protected long? _endtrackdataid;
		protected double? _longitude;
		protected double? _latitude;
		protected double? _altitude;
		protected DateTime? _starttime;
		protected int? _starttimems;
		protected DateTime? _endtime;
		protected int? _endtimems;
		protected double? _errorradius;
		protected int? _builddots;
		protected Trackinfo _trackinfo = new Trackinfo();
		
		

		#endregion

		#region Constructors

		public Fatpoint() { }

		public Fatpoint( long? starttrackdataid, long? endtrackdataid, double? longitude, double? latitude, double? altitude, DateTime? starttime, int? starttimems, DateTime? endtime, int? endtimems, double? errorradius, int? builddots, Trackinfo trackinfo )
		{
			this._starttrackdataid = starttrackdataid;
			this._endtrackdataid = endtrackdataid;
			this._longitude = longitude;
			this._latitude = latitude;
			this._altitude = altitude;
			this._starttime = starttime;
			this._starttimems = starttimems;
			this._endtime = endtime;
			this._endtimems = endtimems;
			this._errorradius = errorradius;
			this._builddots = builddots;
			this._trackinfo = trackinfo;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public long? Starttrackdataid
		{
			get { return _starttrackdataid; }
			set { _starttrackdataid = value; }
		}

		public long? Endtrackdataid
		{
			get { return _endtrackdataid; }
			set { _endtrackdataid = value; }
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

		public DateTime? Starttime
		{
			get { return _starttime; }
			set { _starttime = value; }
		}

		public int? Starttimems
		{
			get { return _starttimems; }
			set { _starttimems = value; }
		}

		public DateTime? Endtime
		{
			get { return _endtime; }
			set { _endtime = value; }
		}

		public int? Endtimems
		{
			get { return _endtimems; }
			set { _endtimems = value; }
		}

		public double? Errorradius
		{
			get { return _errorradius; }
			set { _errorradius = value; }
		}

		public int? Builddots
		{
			get { return _builddots; }
			set { _builddots = value; }
		}

		public Trackinfo Trackinfo
		{
			get { return _trackinfo; }
			set { _trackinfo = value; }
		}


		#endregion
		
	}

	#endregion
}



