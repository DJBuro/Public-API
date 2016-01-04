using System;
using System.Collections;

namespace WebDashboard.Dao.Domain
{
	#region User

	/// <summary>
	/// User object for NHibernate mapped table 'tbl_User'.
	/// </summary>
    public class User : Entity.Entity
		{
		#region Constructors

		public User() { }

		public User( 
		    string emailAddress, 
		    string password, 
		    bool storeUser,
		    bool headOfficeUser, 
		    bool adminUser,
		    bool active, 
		    HeadOffice headOfficeId )
		{
			this._emailAddress = emailAddress;
			this._password = password;
            this._storeUser = storeUser;
			this._headOfficeUser = headOfficeUser;
            this._adminUser = adminUser;
			this._enabled = active;
			this._headOfficeId = headOfficeId;
		}

		#endregion

		#region Public Properties

        protected string _emailAddress;
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

        protected string _password;
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

        protected bool _storeUser;
        public bool StoreUser
        {
            get { return _storeUser; }
            set { _storeUser = value; }
        }

        protected bool _groupUser;
        public bool IsExecutiveDashboardGroupUser
        {
            get { return _groupUser; }
            set { _groupUser = value; }
        }

        protected bool _headOfficeUser;
		public bool IsExecutiveDashboardUser
		{
			get { return _headOfficeUser; }
			set { _headOfficeUser = value; }
		}

        protected bool _adminUser;
        public bool IsAdministrator
        {
            get { return _adminUser; }
            set { _adminUser = value; }
        }

        protected bool _enabled;
		public bool Active
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

        protected HeadOffice _headOfficeId;
		public HeadOffice HeadOffice
		{
			get { return _headOfficeId; }
			set { _headOfficeId = value; }
		}

        protected IList _userIdPermissionses;
        public IList UserPermissions
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



