
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region User

	/// <summary>
	/// User object for NHibernate mapped table 'Users'.
	/// </summary>
    public class User : Entity
		{
		#region Member Variables
		
		protected string _emailAddress;
		protected string _password;
		protected bool _headOfficeUser;
		protected bool _enabled;
		protected HeadOffice _headOfficeId = new HeadOffice();

		protected IList _userIdPermissionses;

		#endregion

		#region Constructors

		public User() { }

		public User( string emailAddress, string password, bool enabled, HeadOffice headOfficeId )
		{
			this._emailAddress = emailAddress;
			this._password = password;
			this._enabled = enabled;
			this._headOfficeId = headOfficeId;
		}

		#endregion

		#region Public Properties

		public string EmailAddress
		{
			get { return _emailAddress; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for EmailAddress", value, value.ToString());
				_emailAddress = value;
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

		public bool HeadOfficeUser
		{
			get { return _headOfficeUser; }
			set { _headOfficeUser = value; }
		}

		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		public HeadOffice HeadOffice
		{
			get { return _headOfficeId; }
			set { _headOfficeId = value; }
		}

		public IList Permissions
		{
			get
			{
				if (_userIdPermissionses==null)
				{
					_userIdPermissionses = new ArrayList();
				}
				return _userIdPermissionses;
			}
			set { _userIdPermissionses = value; }
		}


		#endregion
		
	}

	#endregion
}



