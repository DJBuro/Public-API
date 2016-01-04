
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Mobilenetwork

	/// <summary>
	/// Mobilenetwork object for NHibernate mapped table 'mobile_network'.
	/// </summary>
	public partial  class Mobilenetwork
		{
		#region Member Variables
		
		protected int? _id;
		protected string _operator;
		protected string _username;
		protected string _password;
		protected string _apn;
		protected string _dns1;
		protected string _dns2;
		protected string _description;
		protected Country _country = new Country();
		
		
		protected IList _devices;

		#endregion

		#region Constructors

		public Mobilenetwork() { }

		public Mobilenetwork( string operator, string username, string password, string apn, string dns1, string dns2, string description, Country country )
		{
			this._operator = operator;
			this._username = username;
			this._password = password;
			this._apn = apn;
			this._dns1 = dns1;
			this._dns2 = dns2;
			this._description = description;
			this._country = country;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Operator
		{
			get { return _operator; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Operator", value, value.ToString());
				_operator = value;
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
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for Password", value, value.ToString());
				_password = value;
			}
		}

		public string Apn
		{
			get { return _apn; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Apn", value, value.ToString());
				_apn = value;
			}
		}

		public string Dns1
		{
			get { return _dns1; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Dns1", value, value.ToString());
				_dns1 = value;
			}
		}

		public string Dns2
		{
			get { return _dns2; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Dns2", value, value.ToString());
				_dns2 = value;
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

		public Country Country
		{
			get { return _country; }
			set { _country = value; }
		}

		public IList devices
		{
			get
			{
				if (_devices==null)
				{
					_devices = new ArrayList();
				}
				return _devices;
			}
			set { _devices = value; }
		}


		#endregion
		
	}

	#endregion
}



