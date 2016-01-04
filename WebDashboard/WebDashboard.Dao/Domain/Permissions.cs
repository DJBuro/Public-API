
namespace WebDashboard.Dao.Domain
{
	#region Permission

	/// <summary>
	/// Permission object for NHibernate mapped table 'tbl_Permissions'.
	/// </summary>
    public class Permission : Entity.Entity
		{
		#region Member Variables
		
		protected User _userId;
		protected Site _siteId;

		#endregion

		#region Constructors

		public Permission() { }

		public Permission( User user, Site site )
		{
			this._userId = user;
			this._siteId = site;
		}

		#endregion

		#region Public Properties

		public User User
		{
			get { return _userId; }
			set { _userId = value; }
		}

		public Site Site
		{
			get { return _siteId; }
			set { _siteId = value; }
		}


		#endregion
		
	}

	#endregion
}



