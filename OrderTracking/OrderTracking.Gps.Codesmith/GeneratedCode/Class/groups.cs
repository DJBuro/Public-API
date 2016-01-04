
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Group

	/// <summary>
	/// Group object for NHibernate mapped table 'groups'.
	/// </summary>
	public partial  class Group
		{
		#region Member Variables
		
		protected int? _id;
		protected string _groupname;
		protected string _groupdescription;
		protected string _botype;
		protected DateTime? _created;
		protected short _publicflag;
		protected Application _application = new Application();
		
		
		protected IList _grouprouterules;
		protected Mapsgroup _mapsgroup;
		protected IList _groupsettingses;
		protected IList _msgfielddictionaries;
		protected IList _groupreferrerlogs;

		#endregion

		#region Constructors

		public Group() { }

		public Group( string groupname, string groupdescription, string botype, DateTime? created, short publicflag, Application application )
		{
			this._groupname = groupname;
			this._groupdescription = groupdescription;
			this._botype = botype;
			this._created = created;
			this._publicflag = publicflag;
			this._application = application;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Groupname
		{
			get { return _groupname; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Groupname", value, value.ToString());
				_groupname = value;
			}
		}

		public string Groupdescription
		{
			get { return _groupdescription; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Groupdescription", value, value.ToString());
				_groupdescription = value;
			}
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public short Publicflag
		{
			get { return _publicflag; }
			set { _publicflag = value; }
		}

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
		}

		public IList group_route_rules
		{
			get
			{
				if (_grouprouterules==null)
				{
					_grouprouterules = new ArrayList();
				}
				return _grouprouterules;
			}
			set { _grouprouterules = value; }
		}

		public Mapsgroup Mapsgroup
		{
			get { return _mapsgroup; }
			set { _mapsgroup = value; }
		}

		public IList group_settingses
		{
			get
			{
				if (_groupsettingses==null)
				{
					_groupsettingses = new ArrayList();
				}
				return _groupsettingses;
			}
			set { _groupsettingses = value; }
		}

		public IList msg_field_dictionaries
		{
			get
			{
				if (_msgfielddictionaries==null)
				{
					_msgfielddictionaries = new ArrayList();
				}
				return _msgfielddictionaries;
			}
			set { _msgfielddictionaries = value; }
		}

		public IList group_referrer_logs
		{
			get
			{
				if (_groupreferrerlogs==null)
				{
					_groupreferrerlogs = new ArrayList();
				}
				return _groupreferrerlogs;
			}
			set { _groupreferrerlogs = value; }
		}


		#endregion
		
	}

	#endregion
}



