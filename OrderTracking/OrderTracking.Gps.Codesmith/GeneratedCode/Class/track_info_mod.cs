
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Trackinfomod

	/// <summary>
	/// Trackinfomod object for NHibernate mapped table 'track_info_mod'.
	/// </summary>
	public partial  class Trackinfomod
		{
		#region Member Variables
		
		protected int? _id;
		protected int? _trackinfoid;
		protected string _name;
		protected string _description;
		protected int? _ownerid;
		protected string _trackcategoryid;
		protected double? _minlongitude;
		protected double? _maxlongitude;
		protected double? _minlatitude;
		protected double? _maxlatitude;
		protected double? _minaltitude;
		protected double? _maxaltitude;
		protected DateTime? _mintimestamp;
		protected int? _minmilliseconds;
		protected DateTime? _maxtimestamp;
		protected int? _maxmilliseconds;
		protected double? _totaldistance;
		protected Postprocessor _postprocessor = new Postprocessor();
		
		

		#endregion

		#region Constructors

		public Trackinfomod() { }

		public Trackinfomod( int? trackinfoid, string name, string description, int? ownerid, string trackcategoryid, double? minlongitude, double? maxlongitude, double? minlatitude, double? maxlatitude, double? minaltitude, double? maxaltitude, DateTime? mintimestamp, int? minmilliseconds, DateTime? maxtimestamp, int? maxmilliseconds, double? totaldistance, Postprocessor postprocessor )
		{
			this._trackinfoid = trackinfoid;
			this._name = name;
			this._description = description;
			this._ownerid = ownerid;
			this._trackcategoryid = trackcategoryid;
			this._minlongitude = minlongitude;
			this._maxlongitude = maxlongitude;
			this._minlatitude = minlatitude;
			this._maxlatitude = maxlatitude;
			this._minaltitude = minaltitude;
			this._maxaltitude = maxaltitude;
			this._mintimestamp = mintimestamp;
			this._minmilliseconds = minmilliseconds;
			this._maxtimestamp = maxtimestamp;
			this._maxmilliseconds = maxmilliseconds;
			this._totaldistance = totaldistance;
			this._postprocessor = postprocessor;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Trackinfoid
		{
			get { return _trackinfoid; }
			set { _trackinfoid = value; }
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

		public string Description
		{
			get { return _description; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
				_description = value;
			}
		}

		public int? Ownerid
		{
			get { return _ownerid; }
			set { _ownerid = value; }
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

		public double? Minlongitude
		{
			get { return _minlongitude; }
			set { _minlongitude = value; }
		}

		public double? Maxlongitude
		{
			get { return _maxlongitude; }
			set { _maxlongitude = value; }
		}

		public double? Minlatitude
		{
			get { return _minlatitude; }
			set { _minlatitude = value; }
		}

		public double? Maxlatitude
		{
			get { return _maxlatitude; }
			set { _maxlatitude = value; }
		}

		public double? Minaltitude
		{
			get { return _minaltitude; }
			set { _minaltitude = value; }
		}

		public double? Maxaltitude
		{
			get { return _maxaltitude; }
			set { _maxaltitude = value; }
		}

		public DateTime? Mintimestamp
		{
			get { return _mintimestamp; }
			set { _mintimestamp = value; }
		}

		public int? Minmilliseconds
		{
			get { return _minmilliseconds; }
			set { _minmilliseconds = value; }
		}

		public DateTime? Maxtimestamp
		{
			get { return _maxtimestamp; }
			set { _maxtimestamp = value; }
		}

		public int? Maxmilliseconds
		{
			get { return _maxmilliseconds; }
			set { _maxmilliseconds = value; }
		}

		public double? Totaldistance
		{
			get { return _totaldistance; }
			set { _totaldistance = value; }
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



