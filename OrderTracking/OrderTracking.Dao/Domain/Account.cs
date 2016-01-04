
using System;
using System.Collections;

namespace OrderTracking.Dao.Domain
{
	#region Account

	/// <summary>
	/// Account object for NHibernate mapped table 'tbl_Account'.
	/// </summary>
	public class Account : Entity.Entity
		{
		#region Member Variables
		
		protected string _userName;
		protected string _password;
		protected bool _gpsEnabled;
		protected Store _store;
		

		#endregion

		#region Constructors

		public Account() { }

		public Account( string userName, string password, bool gpsEnabled, Store store )
		{
			this._userName = userName;
			this._password = password;
			this._gpsEnabled = gpsEnabled;
			this._store = store;
		}

		#endregion

		#region Public Properties

		public string UserName
		{
			get { return _userName; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for UserName", value, value.ToString());
				_userName = value;
			}
		}

		public string Password
		{
			get { return _password; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Password", value, value.ToString());
				_password = value;
			}
		}

		public bool GpsEnabled
		{
			get { return _gpsEnabled; }
			set { _gpsEnabled = value; }
		}

		public Store Store
		{
			get { return _store; }
			set { _store = value; }
		}


		#endregion
		
	}

	#endregion
}



