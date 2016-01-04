
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Outgoingwebservice

	/// <summary>
	/// Outgoingwebservice object for NHibernate mapped table 'outgoing_web_service'.
	/// </summary>
	public partial  class Outgoingwebservice
		{
		#region Member Variables
		
		protected int? _id;
		protected string _namespace;
		protected string _url;
		protected string _username;
		protected string _password;
		protected double? _callinterval;
		protected long? _customlong;
		protected string _customstring;
		protected int? _timeout;
		
		

		#endregion

		#region Constructors

		public Outgoingwebservice() { }

		public Outgoingwebservice( string namespace, string url, string username, string password, double? callinterval, long? customlong, string customstring, int? timeout )
		{
			this._namespace = namespace;
			this._url = url;
			this._username = username;
			this._password = password;
			this._callinterval = callinterval;
			this._customlong = customlong;
			this._customstring = customstring;
			this._timeout = timeout;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Namespace
		{
			get { return _namespace; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Namespace", value, value.ToString());
				_namespace = value;
			}
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

		public double? Callinterval
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

		public int? Timeout
		{
			get { return _timeout; }
			set { _timeout = value; }
		}


		#endregion
		
	}

	#endregion
}



