
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Geofence

	/// <summary>
	/// Geofence object for NHibernate mapped table 'geofence'.
	/// </summary>
	public partial  class Geofence
		{
		#region Member Variables
		
		protected int? _id;
		protected string _geofencename;
		protected string _geofencedescription;
		protected double? _minlongitude;
		protected double? _maxlongitude;
		protected double? _minlatitude;
		protected double? _maxlatitude;
		protected double? _minaltitude;
		protected double? _maxaltitude;
		protected string _botype;
		protected DateTime? _created;
		protected Application _application = new Application();
		
		

		#endregion

		#region Constructors

		public Geofence() { }

		public Geofence( string geofencename, string geofencedescription, double? minlongitude, double? maxlongitude, double? minlatitude, double? maxlatitude, double? minaltitude, double? maxaltitude, string botype, DateTime? created, Application application )
		{
			this._geofencename = geofencename;
			this._geofencedescription = geofencedescription;
			this._minlongitude = minlongitude;
			this._maxlongitude = maxlongitude;
			this._minlatitude = minlatitude;
			this._maxlatitude = maxlatitude;
			this._minaltitude = minaltitude;
			this._maxaltitude = maxaltitude;
			this._botype = botype;
			this._created = created;
			this._application = application;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Geofencename
		{
			get { return _geofencename; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Geofencename", value, value.ToString());
				_geofencename = value;
			}
		}

		public string Geofencedescription
		{
			get { return _geofencedescription; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Geofencedescription", value, value.ToString());
				_geofencedescription = value;
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

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
		}


		#endregion
		
	}

	#endregion

	#region Geofencecircular

	/// <summary>
	/// Geofencecircular object for NHibernate mapped table 'geofence_circular'.
	/// </summary>
	public class Geofencecircular : Geofence
	{
		#region Member Variables

		protected double? _radius;
		protected double? _centerlongitude;
		protected double? _centerlatitude;

		#endregion

		#region Constructors

		public Geofencecircular() : base() { }

		public Geofencecircular( string geofencename, string geofencedescription, double? minlongitude, double? maxlongitude, double? minlatitude, double? maxlatitude, double? minaltitude, double? maxaltitude, string botype, DateTime? created, Application application, double? radius, double? centerlongitude, double? centerlatitude ) : base(geofencename, geofencedescription, minlongitude, maxlongitude, minlatitude, maxlatitude, minaltitude, maxaltitude, botype, created, application)
		{
			this._radius = radius;
			this._centerlongitude = centerlongitude;
			this._centerlatitude = centerlatitude;
		}

		#endregion

		#region Public Properties

		public double? Radius
		{
			get { return _radius; }
			set { _radius = value; }
		}

		public double? Centerlongitude
		{
			get { return _centerlongitude; }
			set { _centerlongitude = value; }
		}

		public double? Centerlatitude
		{
			get { return _centerlatitude; }
			set { _centerlatitude = value; }
		}

		#endregion
	}

	#endregion

	#region Geofenceeventargument

	/// <summary>
	/// Geofenceeventargument object for NHibernate mapped table 'geofence_event_argument'.
	/// </summary>
	public class Geofenceeventargument : Geofence
	{
		#region Member Variables

		protected int? _geofenceaction;
		protected Gateeventargument _gateeventargument;

		#endregion

		#region Constructors

		public Geofenceeventargument() : base() { }

		public Geofenceeventargument( string geofencename, string geofencedescription, double? minlongitude, double? maxlongitude, double? minlatitude, double? maxlatitude, double? minaltitude, double? maxaltitude, string botype, DateTime? created, Application application, int? geofenceaction, Gateeventargument gateeventargument ) : base(geofencename, geofencedescription, minlongitude, maxlongitude, minlatitude, maxlatitude, minaltitude, maxaltitude, botype, created, application)
		{
			this._geofenceaction = geofenceaction;
			this.Gateeventargument = gateeventargument;
		}

		#endregion

		#region Public Properties

		public int? Geofenceaction
		{
			get { return _geofenceaction; }
			set { _geofenceaction = value; }
		}

		public Gateeventargument Gateeventargument
		{
			get { return _gateeventargument; }
			set { _gateeventargument = value; }
		}

		#endregion
	}

	#endregion

	#region Taggeofence

	/// <summary>
	/// Taggeofence object for NHibernate mapped table 'tag_geofences'.
	/// </summary>
	public class Taggeofence : Geofence
	{
		#region Member Variables

		protected DateTime? _servertimestamp;
		protected int? _servertimestampms;
		protected Tag _tag;

		#endregion

		#region Constructors

		public Taggeofence() : base() { }

		public Taggeofence( string geofencename, string geofencedescription, double? minlongitude, double? maxlongitude, double? minlatitude, double? maxlatitude, double? minaltitude, double? maxaltitude, string botype, DateTime? created, Application application, DateTime? servertimestamp, int? servertimestampms, Tag tag ) : base(geofencename, geofencedescription, minlongitude, maxlongitude, minlatitude, maxlatitude, minaltitude, maxaltitude, botype, created, application)
		{
			this._servertimestamp = servertimestamp;
			this._servertimestampms = servertimestampms;
			this.Tag = tag;
		}

		#endregion

		#region Public Properties

		public DateTime? Servertimestamp
		{
			get { return _servertimestamp; }
			set { _servertimestamp = value; }
		}

		public int? Servertimestampms
		{
			get { return _servertimestampms; }
			set { _servertimestampms = value; }
		}

		public Tag Tag
		{
			get { return _tag; }
			set { _tag = value; }
		}

		#endregion
	}

	#endregion

	#region Geofencepolygon

	/// <summary>
	/// Geofencepolygon object for NHibernate mapped table 'geofence_polygon'.
	/// </summary>
	public class Geofencepolygon : Geofence
	{
		#region Member Variables

		protected string _vertices;

		#endregion

		#region Constructors

		public Geofencepolygon() : base() { }

		public Geofencepolygon( string geofencename, string geofencedescription, double? minlongitude, double? maxlongitude, double? minlatitude, double? maxlatitude, double? minaltitude, double? maxaltitude, string botype, DateTime? created, Application application, string vertices ) : base(geofencename, geofencedescription, minlongitude, maxlongitude, minlatitude, maxlatitude, minaltitude, maxaltitude, botype, created, application)
		{
			this._vertices = vertices;
		}

		#endregion

		#region Public Properties

		public string Vertices
		{
			get { return _vertices; }
			set { _vertices = value; }
		}

		#endregion
	}

	#endregion
}



