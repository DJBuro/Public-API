


namespace OrderTracking.Gps.Dao.Domain
{
    #region Usergroup

    /// <summary>
    /// Usergroup object for NHibernate mapped table 'user_groups'.
    /// </summary>
    public class UserGroup
    {
        #region Member Variables
        protected int? _id;
        //protected int? _userid;
        protected int? _groupid;
        protected int? _grouprightid;
        protected int? _adminrightid;
        protected short _enablepublictracks;

        #endregion

        #region Constructors

        public UserGroup() { }

 
        public UserGroup(int? groupid, int? grouprightid, int? adminrightid, short enablepublictracks)
        {
            this._groupid = groupid;
            this._grouprightid = grouprightid;
            this._adminrightid = adminrightid;
            this._enablepublictracks = enablepublictracks;
        }

        #endregion

        #region Public Properties

        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        //public int? Userid
        //{
        //    get { return _userid; }
        //    set { _userid = value; }
        //}

        public int? Groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }

        public int? Grouprightid
        {
            get { return _grouprightid; }
            set { _grouprightid = value; }
        }

        public int? Adminrightid
        {
            get { return _adminrightid; }
            set { _adminrightid = value; }
        }

        public short Enablepublictracks
        {
            get { return _enablepublictracks; }
            set { _enablepublictracks = value; }
        }


        #endregion

    }

    #endregion


    //#region UserGroup

    ///// <summary>
    ///// UserGroup object for NHibernate mapped table 'user_groups'.
    ///// </summary>
    //public class UserGroup
    //{
    //    #region Member Variables

    //    protected int? _id;
    //    protected int? _userid;
    //    protected int? _groupid;
    //    protected int? _grouprightid;
    //    protected int? _adminrightid;
    //    protected short _enablepublictracks;



    //    #endregion

    //    #region Constructors

    //    public UserGroup() { }

    //    public UserGroup(int? userid, int? groupid, int? grouprightid, int? adminrightid, short enablepublictracks)
    //    {
    //        this._userid = userid;
    //        this._groupid = groupid;
    //        this._grouprightid = grouprightid;
    //        this._adminrightid = adminrightid;
    //        this._enablepublictracks = enablepublictracks;
    //    }

    //    #endregion

    //    #region Public Properties

    //    public int? Id
    //    {
    //        get { return _id; }
    //        set { _id = value; }
    //    }

    //    public int? Userid
    //    {
    //        get { return _userid; }
    //        set { _userid = value; }
    //    }

    //    public int? Groupid
    //    {
    //        get { return _groupid; }
    //        set { _groupid = value; }
    //    }

    //    public int? Grouprightid
    //    {
    //        get { return _grouprightid; }
    //        set { _grouprightid = value; }
    //    }

    //    public int? Adminrightid
    //    {
    //        get { return _adminrightid; }
    //        set { _adminrightid = value; }
    //    }

    //    public short Enablepublictracks
    //    {
    //        get { return _enablepublictracks; }
    //        set { _enablepublictracks = value; }
    //    }


    //    #endregion

    //}

    //#endregion

}



