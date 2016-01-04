
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Trackdatamod

	/// <summary>
	/// Trackdatamod object for NHibernate mapped table 'track_data_mod'.
	/// </summary>
	public partial  class Trackdatamod
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
		protected short _valid;
		protected short _deleted;
		protected Trackdata _trackdata = new Trackdata();
		protected Trackinfo _trackinfo = new Trackinfo();
		protected Postprocessor _postprocessor = new Postprocessor();
		
		

		#endregion

		#region Constructors

		public Trackdatamod() { }

		public Trackdatamod( double? longitude, double? latitude, double? altitude, double? heading, double? groundspeed, DateTime? timestamp, int? milliseconds, short valid, short deleted, Trackdata trackdata, Trackinfo trackinfo, Postprocessor postprocessor )
		{
			this._longitude = longitude;
			this._latitude = latitude;
			this._altitude = altitude;
			this._heading = heading;
			this._groundspeed = groundspeed;
			this._timestamp = timestamp;
			this._milliseconds = milliseconds;
			this._valid = valid;
			this._deleted = deleted;
			this._trackdata = trackdata;
			this._trackinfo = trackinfo;
			this._postprocessor = postprocessor;
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

		public short Valid
		{
			get { return _valid; }
			set { _valid = value; }
		}

		public short Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}

		public Trackdata Trackdata
		{
			get { return _trackdata; }
			set { _trackdata = value; }
		}

		public Trackinfo Trackinfo
		{
			get { return _trackinfo; }
			set { _trackinfo = value; }
		}

		public Postprocessor Postprocessor
		{
			get { return _postprocessor; }
			set { _postprocessor = value; }
		}


		#endregion
		
	}

	#endregion
}



