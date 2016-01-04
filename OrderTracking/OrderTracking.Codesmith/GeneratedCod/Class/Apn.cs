
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Apn

	/// <summary>
	/// Apn object for NHibernate mapped table 'tbl_Apn'.
	/// </summary>
	public partial  class Apn
		{
		#region Member Variables
		
		protected long? _id;
		protected string _provider;
		protected string _name;
		protected string _username;
		protected string _password;
		
		
		protected IList _apnIdTrackers;

		#endregion

		#region Constructors

		public Apn() { }

		public Apn( string provider, string name, string username, string password )
		{
			this._provider = provider;
			this._name = name;
			this._username = username;
			this._password = password;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Provider
		{
			get { return _provider; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Provider", value, value.ToString());
				_provider = value;
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Username
		{
			get { return _username; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Username", value, value.ToString());
				_username = value;
			}
		}

		public string Password
		{
			get { return _password; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Password", value, value.ToString());
				_password = value;
			}
		}

		public IList ApnIdTrackers
		{
			get
			{
				if (_apnIdTrackers==null)
				{
					_apnIdTrackers = new ArrayList();
				}
				return _apnIdTrackers;
			}
			set { _apnIdTrackers = value; }
		}


		#endregion
		
	}

	#endregion
}



