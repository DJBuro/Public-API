
using System;
using System.Collections;


namespace WebDashboard.Dao.Domain
{
    #region UserRegion

    /// <summary>
	/// Site object for NHibernate mapped table 'tbl_UserRegion'.
	/// </summary>
    public class UserRegion : Entity.Entity
	{
	    #region Member Variables
    	
	    protected int _userId;
	    protected int _regionId;

	    #endregion

	    #region Constructors

	    public UserRegion() { }

        public UserRegion(int userId, int regionId)
	    {
            this._userId = userId;
            this._regionId = regionId;
	    }

	    #endregion

	    #region Public Properties

        public int UserId
	    {
            get { return _userId; }
            set { _userId = value; }
	    }

        public int RegionId
        {
            get { return _regionId; }
            set { _regionId = value; }
        }

		#endregion
	}
	#endregion
}



