
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Cmdqueueitem

	/// <summary>
	/// Cmdqueueitem object for NHibernate mapped table 'cmd_queue_item'.
	/// </summary>
	public partial  class Cmdqueueitem
		{
		#region Member Variables
		
		protected long? _id;
		protected int? _gatecommandid;
		protected string _gatecommandname;
		protected int? _stepcurrent;
		protected int? _stepmax;
		protected string _stepdesc;
		protected string _errordesc;
		protected string _customstate;
		protected int? _deliverystatus;
		protected short _outgoing;
		protected DateTime? _timestampclient;
		protected DateTime? _timestampqueued;
		protected DateTime? _timestampdelivered;
		protected DateTime? _timestamplasttry;
		protected int? _retrycount;
		protected Device _client = new Device();
		protected User _client = new User();
		protected User _sender = new User();
		protected Cmdqueue _queueid = new Cmdqueue();
		
		
		protected IList _cmdargs;

		#endregion

		#region Constructors

		public Cmdqueueitem() { }

		public Cmdqueueitem( int? gatecommandid, string gatecommandname, int? stepcurrent, int? stepmax, string stepdesc, string errordesc, string customstate, int? deliverystatus, short outgoing, DateTime? timestampclient, DateTime? timestampqueued, DateTime? timestampdelivered, DateTime? timestamplasttry, int? retrycount, Device client, User client, User sender, Cmdqueue queueid )
		{
			this._gatecommandid = gatecommandid;
			this._gatecommandname = gatecommandname;
			this._stepcurrent = stepcurrent;
			this._stepmax = stepmax;
			this._stepdesc = stepdesc;
			this._errordesc = errordesc;
			this._customstate = customstate;
			this._deliverystatus = deliverystatus;
			this._outgoing = outgoing;
			this._timestampclient = timestampclient;
			this._timestampqueued = timestampqueued;
			this._timestampdelivered = timestampdelivered;
			this._timestamplasttry = timestamplasttry;
			this._retrycount = retrycount;
			this._client = client;
			this._client = client;
			this._sender = sender;
			this._queueid = queueid;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Gatecommandid
		{
			get { return _gatecommandid; }
			set { _gatecommandid = value; }
		}

		public string Gatecommandname
		{
			get { return _gatecommandname; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Gatecommandname", value, value.ToString());
				_gatecommandname = value;
			}
		}

		public int? Stepcurrent
		{
			get { return _stepcurrent; }
			set { _stepcurrent = value; }
		}

		public int? Stepmax
		{
			get { return _stepmax; }
			set { _stepmax = value; }
		}

		public string Stepdesc
		{
			get { return _stepdesc; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Stepdesc", value, value.ToString());
				_stepdesc = value;
			}
		}

		public string Errordesc
		{
			get { return _errordesc; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Errordesc", value, value.ToString());
				_errordesc = value;
			}
		}

		public string Customstate
		{
			get { return _customstate; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Customstate", value, value.ToString());
				_customstate = value;
			}
		}

		public int? Deliverystatus
		{
			get { return _deliverystatus; }
			set { _deliverystatus = value; }
		}

		public short Outgoing
		{
			get { return _outgoing; }
			set { _outgoing = value; }
		}

		public DateTime? Timestampclient
		{
			get { return _timestampclient; }
			set { _timestampclient = value; }
		}

		public DateTime? Timestampqueued
		{
			get { return _timestampqueued; }
			set { _timestampqueued = value; }
		}

		public DateTime? Timestampdelivered
		{
			get { return _timestampdelivered; }
			set { _timestampdelivered = value; }
		}

		public DateTime? Timestamplasttry
		{
			get { return _timestamplasttry; }
			set { _timestamplasttry = value; }
		}

		public int? Retrycount
		{
			get { return _retrycount; }
			set { _retrycount = value; }
		}

		public Device client_
		{
			get { return _client; }
			set { _client = value; }
		}

		public User client_
		{
			get { return _client; }
			set { _client = value; }
		}

		public User sender_
		{
			get { return _sender; }
			set { _sender = value; }
		}

		public Cmdqueue queue_id
		{
			get { return _queueid; }
			set { _queueid = value; }
		}

		public IList cmd_args
		{
			get
			{
				if (_cmdargs==null)
				{
					_cmdargs = new ArrayList();
				}
				return _cmdargs;
			}
			set { _cmdargs = value; }
		}


		#endregion
		
	}

	#endregion
}



