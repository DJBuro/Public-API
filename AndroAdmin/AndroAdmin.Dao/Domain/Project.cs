
using System;
using System.Collections;


namespace AndroAdmin.Dao.Domain
{
	#region Project

	/// <summary>
	/// Project object for NHibernate mapped table 'tbl_Project'.
	/// </summary>
	public class Project : Entity.Entity
		{
		#region Member Variables
		
		protected string _name;
		protected string _url;
		protected IList _projectAndroUserPermissions;

		#endregion

		#region Constructors

		public Project() { }

		public Project( string name, string url )
		{
			this._name = name;
			this._url = url;
		}

		#endregion

		#region Public Properties

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Url
		{
			get { return _url; }
			set
			{
				if ( value != null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for Url", value, value.ToString());
				_url = value;
			}
		}

		public IList UserPermissions
		{
			get
			{
				if (_projectAndroUserPermissions==null)
				{
					_projectAndroUserPermissions = new ArrayList();
				}
				return _projectAndroUserPermissions;
			}
			set { _projectAndroUserPermissions = value; }
		}


		#endregion
		
	}

	#endregion
}



