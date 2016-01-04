
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateevent

	/// <summary>
	/// Gateevent object for NHibernate mapped table 'gate_event'.
	/// </summary>
	public partial  class Gateevent
		{
		#region Member Variables
		
		protected long? _id;
		protected int? _userid;
		protected string _cause;
		protected int? _gateeventexpressionevaluatorid;
		protected short _stage;
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
		protected short _scheduled;
		protected Gatemessage _gatemessage = new Gatemessage();
		protected Gateeventchannel _gateeventchannel = new Gateeventchannel();
		protected Application _application = new Application();
		
		
		protected IList _gateeventlogs;
		protected IList _gateeventarguments;

		#endregion

		#region Constructors

		public Gateevent() { }

		public Gateevent( int? userid, string cause, int? gateeventexpressionevaluatorid, short stage, double? minlongitude, double? maxlongitude, double? minlatitude, double? maxlatitude, double? minaltitude, double? maxaltitude, DateTime? mintimestamp, int? minmilliseconds, DateTime? maxtimestamp, int? maxmilliseconds, short scheduled, Gatemessage gatemessage, Gateeventchannel gateeventchannel, Application application )
		{
			this._userid = userid;
			this._cause = cause;
			this._gateeventexpressionevaluatorid = gateeventexpressionevaluatorid;
			this._stage = stage;
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
			this._scheduled = scheduled;
			this._gatemessage = gatemessage;
			this._gateeventchannel = gateeventchannel;
			this._application = application;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Userid
		{
			get { return _userid; }
			set { _userid = value; }
		}

		public string Cause
		{
			get { return _cause; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Cause", value, value.ToString());
				_cause = value;
			}
		}

		public int? Gateeventexpressionevaluatorid
		{
			get { return _gateeventexpressionevaluatorid; }
			set { _gateeventexpressionevaluatorid = value; }
		}

		public short Stage
		{
			get { return _stage; }
			set { _stage = value; }
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

		public short Scheduled
		{
			get { return _scheduled; }
			set { _scheduled = value; }
		}

		public Gatemessage Gatemessage
		{
			get { return _gatemessage; }
			set { _gatemessage = value; }
		}

		public Gateeventchannel Gateeventchannel
		{
			get { return _gateeventchannel; }
			set { _gateeventchannel = value; }
		}

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
		}

		public IList gate_event_logs
		{
			get
			{
				if (_gateeventlogs==null)
				{
					_gateeventlogs = new ArrayList();
				}
				return _gateeventlogs;
			}
			set { _gateeventlogs = value; }
		}

		public IList gate_event_arguments
		{
			get
			{
				if (_gateeventarguments==null)
				{
					_gateeventarguments = new ArrayList();
				}
				return _gateeventarguments;
			}
			set { _gateeventarguments = value; }
		}


		#endregion
		
	}

	#endregion
}



