
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Providermessage

	/// <summary>
	/// Providermessage object for NHibernate mapped table 'provider_message'.
	/// </summary>
	public partial  class Providermessage
		{
		#region Member Variables
		
		protected long? _id;
		protected int? _clientdeviceid;
		protected string _clientaddress;
		protected string _message;
		protected int? _deliverystatus;
		protected short _outgoing;
		protected DateTime? _timestampclient;
		protected DateTime? _timestampqueued;
		protected DateTime? _timestampdelivered;
		protected DateTime? _timestamplasttry;
		protected int? _retrycount;
		protected string _transport;
		protected User _client = new User();
		protected User _sender = new User();
		protected Providermessagequeue _queueid = new Providermessagequeue();
		
		
		protected Smsmessage _smsmessage;

		#endregion

		#region Constructors

		public Providermessage() { }

		public Providermessage( int? clientdeviceid, string clientaddress, string message, int? deliverystatus, short outgoing, DateTime? timestampclient, DateTime? timestampqueued, DateTime? timestampdelivered, DateTime? timestamplasttry, int? retrycount, string transport, User client, User sender, Providermessagequeue queueid )
		{
			this._clientdeviceid = clientdeviceid;
			this._clientaddress = clientaddress;
			this._message = message;
			this._deliverystatus = deliverystatus;
			this._outgoing = outgoing;
			this._timestampclient = timestampclient;
			this._timestampqueued = timestampqueued;
			this._timestampdelivered = timestampdelivered;
			this._timestamplasttry = timestamplasttry;
			this._retrycount = retrycount;
			this._transport = transport;
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

		public int? Clientdeviceid
		{
			get { return _clientdeviceid; }
			set { _clientdeviceid = value; }
		}

		public string Clientaddress
		{
			get { return _clientaddress; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Clientaddress", value, value.ToString());
				_clientaddress = value;
			}
		}

		public string Message
		{
			get { return _message; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Message", value, value.ToString());
				_message = value;
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

		public string Transport
		{
			get { return _transport; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Transport", value, value.ToString());
				_transport = value;
			}
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

		public Providermessagequeue queue_id
		{
			get { return _queueid; }
			set { _queueid = value; }
		}

		public Smsmessage Smsmessage
		{
			get { return _smsmessage; }
			set { _smsmessage = value; }
		}


		#endregion
		
	}

	#endregion
}



