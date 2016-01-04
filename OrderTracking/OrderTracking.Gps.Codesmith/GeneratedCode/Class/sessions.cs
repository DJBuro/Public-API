
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Session

	/// <summary>
	/// Session object for NHibernate mapped table 'sessions'.
	/// </summary>
	public partial  class Session
		{
		#region Member Variables
		
		protected int? _id;
		protected int? _userid;
		protected DateTime? _timestamp;
		protected DateTime? _expire;
		protected DateTime? _created;
		protected string _ipaddress;
		protected string _botype;
		protected int? _deviceid;
		
		

		#endregion

		#region Constructors

		public Session() { }

		public Session( int? userid, DateTime? timestamp, DateTime? expire, DateTime? created, string ipaddress, string botype, int? deviceid )
		{
			this._userid = userid;
			this._timestamp = timestamp;
			this._expire = expire;
			this._created = created;
			this._ipaddress = ipaddress;
			this._botype = botype;
			this._deviceid = deviceid;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Userid
		{
			get { return _userid; }
			set { _userid = value; }
		}

		public DateTime? Timestamp
		{
			get { return _timestamp; }
			set { _timestamp = value; }
		}

		public DateTime? Expire
		{
			get { return _expire; }
			set { _expire = value; }
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public string Ipaddress
		{
			get { return _ipaddress; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Ipaddress", value, value.ToString());
				_ipaddress = value;
			}
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

		public int? Deviceid
		{
			get { return _deviceid; }
			set { _deviceid = value; }
		}


		#endregion
		
	}

	#endregion
}



