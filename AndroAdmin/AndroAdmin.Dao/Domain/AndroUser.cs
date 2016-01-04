
using System;
using System.Collections;


namespace AndroAdmin.Dao.Domain
{
	#region AndroUser

	/// <summary>
	/// AndroUser object for NHibernate mapped table 'tbl_AndroUser'.
	/// </summary>
	public class AndroUser : Entity.Entity
		{
		#region Member Variables
		
		protected string _firstName;
		protected string _surName;
		protected string _password;
		protected string _emailAddress;
		protected bool _active;
		protected DateTime _created;
		
		
		protected IList _androUserAndroUserPermissions;

		#endregion

		#region Constructors

		public AndroUser() { }

		public AndroUser( string firstName, string surName, string password, string emailAddress, bool active, DateTime created)
		{
			this._firstName = firstName;
			this._surName = surName;
			this._password = password;
			this._emailAddress = emailAddress;
			this._active = active;
			this._created = created;
		}

		#endregion

		#region Public Properties

		public string FirstName
		{
			get { return _firstName; }
			set
			{
				if ( value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for FirstName", value, value.ToString());
				_firstName = value;
			}
		}

		public string SurName
		{
			get { return _surName; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for SurName", value, value.ToString());
				_surName = value;
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

		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}

		public DateTime Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public IList UserPermissions
		{
			get
			{
				if (_androUserAndroUserPermissions==null)
				{
					_androUserAndroUserPermissions = new ArrayList();
				}
				return _androUserAndroUserPermissions;
			}
			set { _androUserAndroUserPermissions = value; }
		}


		#endregion
		
	}

	#endregion
}



