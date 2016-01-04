
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Trackrecorder

	/// <summary>
	/// Trackrecorder object for NHibernate mapped table 'track_recorder'.
	/// </summary>
	public partial  class Trackrecorder
		{
		#region Member Variables
		
		protected int? _id;
		protected string _botype;
		protected string _name;
		protected int? _trackinfoid;
		protected int? _recording;
		protected long? _lasttrackdataid;
		protected double? _timefilter;
		protected double? _distancefilter;
		protected double? _directionfilter;
		protected double? _directionthreshold;
		protected double? _speedfilter;
		protected double? _restarttime;
		protected double? _restartdistance;
		protected string _trackcategoryid;
		protected long? _lastuncertaintrackdataid;
		protected double? _restartinterval;
		protected double? _restartintervaloffset;
		protected double? _smstimefilter;
		protected short _motion;
		protected User _ownerid = new User();
		
		

		#endregion

		#region Constructors

		public Trackrecorder() { }

		public Trackrecorder( string botype, string name, int? trackinfoid, int? recording, long? lasttrackdataid, double? timefilter, double? distancefilter, double? directionfilter, double? directionthreshold, double? speedfilter, double? restarttime, double? restartdistance, string trackcategoryid, long? lastuncertaintrackdataid, double? restartinterval, double? restartintervaloffset, double? smstimefilter, short motion, User ownerid )
		{
			this._botype = botype;
			this._name = name;
			this._trackinfoid = trackinfoid;
			this._recording = recording;
			this._lasttrackdataid = lasttrackdataid;
			this._timefilter = timefilter;
			this._distancefilter = distancefilter;
			this._directionfilter = directionfilter;
			this._directionthreshold = directionthreshold;
			this._speedfilter = speedfilter;
			this._restarttime = restarttime;
			this._restartdistance = restartdistance;
			this._trackcategoryid = trackcategoryid;
			this._lastuncertaintrackdataid = lastuncertaintrackdataid;
			this._restartinterval = restartinterval;
			this._restartintervaloffset = restartintervaloffset;
			this._smstimefilter = smstimefilter;
			this._motion = motion;
			this._ownerid = ownerid;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
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

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public int? Trackinfoid
		{
			get { return _trackinfoid; }
			set { _trackinfoid = value; }
		}

		public int? Recording
		{
			get { return _recording; }
			set { _recording = value; }
		}

		public long? Lasttrackdataid
		{
			get { return _lasttrackdataid; }
			set { _lasttrackdataid = value; }
		}

		public double? Timefilter
		{
			get { return _timefilter; }
			set { _timefilter = value; }
		}

		public double? Distancefilter
		{
			get { return _distancefilter; }
			set { _distancefilter = value; }
		}

		public double? Directionfilter
		{
			get { return _directionfilter; }
			set { _directionfilter = value; }
		}

		public double? Directionthreshold
		{
			get { return _directionthreshold; }
			set { _directionthreshold = value; }
		}

		public double? Speedfilter
		{
			get { return _speedfilter; }
			set { _speedfilter = value; }
		}

		public double? Restarttime
		{
			get { return _restarttime; }
			set { _restarttime = value; }
		}

		public double? Restartdistance
		{
			get { return _restartdistance; }
			set { _restartdistance = value; }
		}

		public string Trackcategoryid
		{
			get { return _trackcategoryid; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Trackcategoryid", value, value.ToString());
				_trackcategoryid = value;
			}
		}

		public long? Lastuncertaintrackdataid
		{
			get { return _lastuncertaintrackdataid; }
			set { _lastuncertaintrackdataid = value; }
		}

		public double? Restartinterval
		{
			get { return _restartinterval; }
			set { _restartinterval = value; }
		}

		public double? Restartintervaloffset
		{
			get { return _restartintervaloffset; }
			set { _restartintervaloffset = value; }
		}

		public double? Smstimefilter
		{
			get { return _smstimefilter; }
			set { _smstimefilter = value; }
		}

		public short Motion
		{
			get { return _motion; }
			set { _motion = value; }
		}

		public User owner_id
		{
			get { return _ownerid; }
			set { _ownerid = value; }
		}


		#endregion
		
	}

	#endregion
}



