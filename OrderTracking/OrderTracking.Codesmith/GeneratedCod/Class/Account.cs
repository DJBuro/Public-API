
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Account

	/// <summary>
	/// Account object for NHibernate mapped table 'tbl_Account'.
	/// </summary>
	public partial  class Account
		{
		#region Member Variables
		
		protected long? _id;
		protected string _userName;
		protected string _password;
		protected bool _gpsEnabled;
		protected Store _storeId = new Store();
		
		

		#endregion

		#region Constructors

		public Account() { }

		public Account( string userName, string password, bool gpsEnabled, Store storeId )
		{
			this._userName = userName;
			this._password = password;
			this._gpsEnabled = gpsEnabled;
			this._storeId = storeId;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

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

		public Store StoreId
		{
			get { return _storeId; }
			set { _storeId = value; }
		}


		#endregion
		
	}

	#endregion
}



