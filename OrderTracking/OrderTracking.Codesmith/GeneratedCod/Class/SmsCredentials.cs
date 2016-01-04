
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region SmsCredential

	/// <summary>
	/// SmsCredential object for NHibernate mapped table 'tbl_SmsCredentials'.
	/// </summary>
	public partial  class SmsCredential
		{
		#region Member Variables
		
		protected long? _id;
		protected string _username;
		protected string _password;
		protected string _smsFrom;
		
		

		#endregion

		#region Constructors

		public SmsCredential() { }

		public SmsCredential( string username, string password, string smsFrom )
		{
			this._username = username;
			this._password = password;
			this._smsFrom = smsFrom;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Username
		{
			get { return _username; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Username", value, value.ToString());
				_username = value;
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

		public string SmsFrom
		{
			get { return _smsFrom; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for SmsFrom", value, value.ToString());
				_smsFrom = value;
			}
		}


		#endregion
		
	}

	#endregion
}



