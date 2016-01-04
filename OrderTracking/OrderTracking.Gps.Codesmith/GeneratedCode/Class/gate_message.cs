
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gatemessage

	/// <summary>
	/// Gatemessage object for NHibernate mapped table 'gate_message'.
	/// </summary>
	public partial  class Gatemessage
		{
		#region Member Variables
		
		protected long? _id;
		protected long? _trackdataid;
		protected DateTime? _servertimestamp;
		protected int? _servertimestampms;
		protected int? _deviceid;
		protected User _user = new User();
		
		
		protected IList _gateevents;
		protected IList _gatemessagerecords;

		#endregion

		#region Constructors

		public Gatemessage() { }

		public Gatemessage( long? trackdataid, DateTime? servertimestamp, int? servertimestampms, int? deviceid, User user )
		{
			this._trackdataid = trackdataid;
			this._servertimestamp = servertimestamp;
			this._servertimestampms = servertimestampms;
			this._deviceid = deviceid;
			this._user = user;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public long? Trackdataid
		{
			get { return _trackdataid; }
			set { _trackdataid = value; }
		}

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

		public int? Deviceid
		{
			get { return _deviceid; }
			set { _deviceid = value; }
		}

		public User User
		{
			get { return _user; }
			set { _user = value; }
		}

		public IList gate_events
		{
			get
			{
				if (_gateevents==null)
				{
					_gateevents = new ArrayList();
				}
				return _gateevents;
			}
			set { _gateevents = value; }
		}

		public IList gate_message_records
		{
			get
			{
				if (_gatemessagerecords==null)
				{
					_gatemessagerecords = new ArrayList();
				}
				return _gatemessagerecords;
			}
			set { _gatemessagerecords = value; }
		}


		#endregion
		
	}

	#endregion
}



