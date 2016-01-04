
using System;


namespace OrderTracking.Dao.Domain
{
	#region SmsCredential

	/// <summary>
	/// SmsCredential object for NHibernate mapped table 'tbl_SmsCredentials'.
	/// </summary>
    public class SmsCredential : Entity.Entity
		{
		#region Member Variables
		
		protected string _username;
		protected string _password;

        #endregion

		#region Constructors

		public SmsCredential() { }

		public SmsCredential( string username, string password)
		{
			this._username = username;
			this._password = password;
		}

		#endregion

		#region Public Properties

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

		#endregion
		
	}

	#endregion
}



