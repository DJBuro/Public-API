
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region Permission

	/// <summary>
	/// Permission object for NHibernate mapped table 'Permissions'.
	/// </summary>
    public class Permission : Entity
		{
		#region Member Variables
		
		protected Site _siteId = new Site();
		protected User _userId = new User();


		#endregion

		#region Constructors

		public Permission() { }

		public Permission( Site siteId, User userId )
		{
			this._siteId = siteId;
			this._userId = userId;
		}

		#endregion

		#region Public Properties

		public Site Site
		{
			get { return _siteId; }
			set { _siteId = value; }
		}

		public User User
		{
			get { return _userId; }
			set { _userId = value; }
		}


		#endregion
		
	}

	#endregion
}



