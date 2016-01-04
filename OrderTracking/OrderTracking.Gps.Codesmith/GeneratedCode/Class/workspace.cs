
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Workspace

	/// <summary>
	/// Workspace object for NHibernate mapped table 'workspace'.
	/// </summary>
	public partial  class Workspace
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _state;
		protected int? _shared;
		protected DateTime? _created;
		protected Application _application = new Application();
		protected User _user = new User();
		
		

		#endregion

		#region Constructors

		public Workspace() { }

		public Workspace( string name, string state, int? shared, DateTime? created, Application application, User user )
		{
			this._name = name;
			this._state = state;
			this._shared = shared;
			this._created = created;
			this._application = application;
			this._user = user;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string State
		{
			get { return _state; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for State", value, value.ToString());
				_state = value;
			}
		}

		public int? Shared
		{
			get { return _shared; }
			set { _shared = value; }
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
		}

		public User User
		{
			get { return _user; }
			set { _user = value; }
		}


		#endregion
		
	}

	#endregion
}



