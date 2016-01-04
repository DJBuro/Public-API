
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Messageprovider

	/// <summary>
	/// Messageprovider object for NHibernate mapped table 'message_provider'.
	/// </summary>
	public partial  class Messageprovider
		{
		#region Member Variables
		
		protected int? _id;
		protected string _botype;
		protected string _msgprovname;
		protected short _enabled;
		protected DateTime? _created;
		protected string _url;
		protected string _username;
		protected string _password;
		protected int? _callinterval;
		protected long? _customlong;
		protected string _customstring;
		protected int? _calltimeout;
		protected string _routelabel;
		protected int? _defaultprovider;
		protected Loadabletype _loadabletype = new Loadabletype();
		
		
		protected IList _providermessagequeues;
		protected IList _msgprovsettingses;

		#endregion

		#region Constructors

		public Messageprovider() { }

		public Messageprovider( string botype, string msgprovname, short enabled, DateTime? created, string url, string username, string password, int? callinterval, long? customlong, string customstring, int? calltimeout, string routelabel, int? defaultprovider, Loadabletype loadabletype )
		{
			this._botype = botype;
			this._msgprovname = msgprovname;
			this._enabled = enabled;
			this._created = created;
			this._url = url;
			this._username = username;
			this._password = password;
			this._callinterval = callinterval;
			this._customlong = customlong;
			this._customstring = customstring;
			this._calltimeout = calltimeout;
			this._routelabel = routelabel;
			this._defaultprovider = defaultprovider;
			this._loadabletype = loadabletype;
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
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public string Msgprovname
		{
			get { return _msgprovname; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Msgprovname", value, value.ToString());
				_msgprovname = value;
			}
		}

		public short Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public string Url
		{
			get { return _url; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Url", value, value.ToString());
				_url = value;
			}
		}

		public string Username
		{
			get { return _username; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Username", value, value.ToString());
				_username = value;
			}
		}

		public string Password
		{
			get { return _password; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Password", value, value.ToString());
				_password = value;
			}
		}

		public int? Callinterval
		{
			get { return _callinterval; }
			set { _callinterval = value; }
		}

		public long? Customlong
		{
			get { return _customlong; }
			set { _customlong = value; }
		}

		public string Customstring
		{
			get { return _customstring; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Customstring", value, value.ToString());
				_customstring = value;
			}
		}

		public int? Calltimeout
		{
			get { return _calltimeout; }
			set { _calltimeout = value; }
		}

		public string Routelabel
		{
			get { return _routelabel; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Routelabel", value, value.ToString());
				_routelabel = value;
			}
		}

		public int? Defaultprovider
		{
			get { return _defaultprovider; }
			set { _defaultprovider = value; }
		}

		public Loadabletype Loadabletype
		{
			get { return _loadabletype; }
			set { _loadabletype = value; }
		}

		public IList provider_message_queues
		{
			get
			{
				if (_providermessagequeues==null)
				{
					_providermessagequeues = new ArrayList();
				}
				return _providermessagequeues;
			}
			set { _providermessagequeues = value; }
		}

		public IList msg_prov_settingses
		{
			get
			{
				if (_msgprovsettingses==null)
				{
					_msgprovsettingses = new ArrayList();
				}
				return _msgprovsettingses;
			}
			set { _msgprovsettingses = value; }
		}


		#endregion
		
	}

	#endregion
}



