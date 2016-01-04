
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Trackinfo

	/// <summary>
	/// Trackinfo object for NHibernate mapped table 'track_info'.
	/// </summary>
	public partial  class Trackinfo
		{
		#region Member Variables
		
		protected int? _id;
		protected string _botype;
		protected string _name;
		protected string _description;
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
		protected short _deleted;
		protected int? _dirtycount;
		protected Trackcategory _trackcategory = new Trackcategory();
		protected User _ownerid = new User();
		
		
		protected IList _trackdatas;
		protected IList _trackpostprocessorlogs;
		protected IList _trackdatamods;
		protected IList _fatpoints;

		#endregion

		#region Constructors

		public Trackinfo() { }

		public Trackinfo( string botype, string name, string description, double? minlongitude, double? maxlongitude, double? minlatitude, double? maxlatitude, double? minaltitude, double? maxaltitude, DateTime? mintimestamp, int? minmilliseconds, DateTime? maxtimestamp, int? maxmilliseconds, double? totaldistance, short deleted, int? dirtycount, Trackcategory trackcategory, User ownerid )
		{
			this._botype = botype;
			this._name = name;
			this._description = description;
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
			this._deleted = deleted;
			this._dirtycount = dirtycount;
			this._trackcategory = trackcategory;
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

		public short Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}

		public int? Dirtycount
		{
			get { return _dirtycount; }
			set { _dirtycount = value; }
		}

		public Trackcategory Trackcategory
		{
			get { return _trackcategory; }
			set { _trackcategory = value; }
		}

		public User owner_id
		{
			get { return _ownerid; }
			set { _ownerid = value; }
		}

		public IList track_datas
		{
			get
			{
				if (_trackdatas==null)
				{
					_trackdatas = new ArrayList();
				}
				return _trackdatas;
			}
			set { _trackdatas = value; }
		}

		public IList track_post_processor_logs
		{
			get
			{
				if (_trackpostprocessorlogs==null)
				{
					_trackpostprocessorlogs = new ArrayList();
				}
				return _trackpostprocessorlogs;
			}
			set { _trackpostprocessorlogs = value; }
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

		public IList fat_points
		{
			get
			{
				if (_fatpoints==null)
				{
					_fatpoints = new ArrayList();
				}
				return _fatpoints;
			}
			set { _fatpoints = value; }
		}


		#endregion
		
	}

	#endregion
}



