
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region User

	/// <summary>
	/// User object for NHibernate mapped table 'users'.
	/// </summary>
	public partial  class User
		{
		#region Member Variables
		
		protected int? _id;
		protected string _username;
		protected string _name;
		protected string _surname;
		protected string _password;
		protected string _email;
		protected short _active;
		protected string _description;
		protected string _sourceaddress;
		protected DateTime? _created;
		protected string _botype;
		
		
		protected IList _owneridtrackrecorders;
		protected Taguser _taguser;
		protected IList _owneridsmsproxies;
		protected Userattribute _userattribute;
		protected IList _clientprovidermessages;
		protected IList _senderprovidermessages;
		protected IList _owneriddevices;
		protected IList _clientcmdqueueitems;
		protected IList _workspaces;
		protected IList _sendercmdqueueitems;
		protected IList _temporarycredentials;
		protected IList _usersettingses;
		protected IList _gatemessages;
		protected Uicontrolstate _uicontrolstate;
		protected IList _gateeventexpressionstates;
		protected IList _owneridtrackinfos;
		protected Gateeventexpressionevaluatoruser _gateeventexpressionevaluatoruser;

		#endregion

		#region Constructors

		public User() { }

		public User( string username, string name, string surname, string password, string email, short active, string description, string sourceaddress, DateTime? created, string botype )
		{
			this._username = username;
			this._name = name;
			this._surname = surname;
			this._password = password;
			this._email = email;
			this._active = active;
			this._description = description;
			this._sourceaddress = sourceaddress;
			this._created = created;
			this._botype = botype;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Username
		{
			get { return _username; }
			set
			{
				if ( value != null && value.Length > 15)
					throw new ArgumentOutOfRangeException("Invalid value for Username", value, value.ToString());
				_username = value;
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Surname
		{
			get { return _surname; }
			set
			{
				if ( value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Surname", value, value.ToString());
				_surname = value;
			}
		}

		public string Password
		{
			get { return _password; }
			set
			{
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for Password", value, value.ToString());
				_password = value;
			}
		}

		public string Email
		{
			get { return _email; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Email", value, value.ToString());
				_email = value;
			}
		}

		public short Active
		{
			get { return _active; }
			set { _active = value; }
		}

		public string Description
		{
			get { return _description; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
				_description = value;
			}
		}

		public string Sourceaddress
		{
			get { return _sourceaddress; }
			set
			{
				if ( value != null && value.Length > 15)
					throw new ArgumentOutOfRangeException("Invalid value for Sourceaddress", value, value.ToString());
				_sourceaddress = value;
			}
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
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

		public IList owner_idtrack_recorders
		{
			get
			{
				if (_owneridtrackrecorders==null)
				{
					_owneridtrackrecorders = new ArrayList();
				}
				return _owneridtrackrecorders;
			}
			set { _owneridtrackrecorders = value; }
		}

		public Taguser Taguser
		{
			get { return _taguser; }
			set { _taguser = value; }
		}

		public IList owner_idsms_proxies
		{
			get
			{
				if (_owneridsmsproxies==null)
				{
					_owneridsmsproxies = new ArrayList();
				}
				return _owneridsmsproxies;
			}
			set { _owneridsmsproxies = value; }
		}

		public Userattribute Userattribute
		{
			get { return _userattribute; }
			set { _userattribute = value; }
		}

		public IList client_provider_messages
		{
			get
			{
				if (_clientprovidermessages==null)
				{
					_clientprovidermessages = new ArrayList();
				}
				return _clientprovidermessages;
			}
			set { _clientprovidermessages = value; }
		}

		public IList sender_provider_messages
		{
			get
			{
				if (_senderprovidermessages==null)
				{
					_senderprovidermessages = new ArrayList();
				}
				return _senderprovidermessages;
			}
			set { _senderprovidermessages = value; }
		}

		public IList owner_iddevices
		{
			get
			{
				if (_owneriddevices==null)
				{
					_owneriddevices = new ArrayList();
				}
				return _owneriddevices;
			}
			set { _owneriddevices = value; }
		}

		public IList client_cmd_queue_items
		{
			get
			{
				if (_clientcmdqueueitems==null)
				{
					_clientcmdqueueitems = new ArrayList();
				}
				return _clientcmdqueueitems;
			}
			set { _clientcmdqueueitems = value; }
		}

		public IList workspaces
		{
			get
			{
				if (_workspaces==null)
				{
					_workspaces = new ArrayList();
				}
				return _workspaces;
			}
			set { _workspaces = value; }
		}

		public IList sender_cmd_queue_items
		{
			get
			{
				if (_sendercmdqueueitems==null)
				{
					_sendercmdqueueitems = new ArrayList();
				}
				return _sendercmdqueueitems;
			}
			set { _sendercmdqueueitems = value; }
		}

		public IList temporary_credentials
		{
			get
			{
				if (_temporarycredentials==null)
				{
					_temporarycredentials = new ArrayList();
				}
				return _temporarycredentials;
			}
			set { _temporarycredentials = value; }
		}

		public IList user_settingses
		{
			get
			{
				if (_usersettingses==null)
				{
					_usersettingses = new ArrayList();
				}
				return _usersettingses;
			}
			set { _usersettingses = value; }
		}

		public IList gate_messages
		{
			get
			{
				if (_gatemessages==null)
				{
					_gatemessages = new ArrayList();
				}
				return _gatemessages;
			}
			set { _gatemessages = value; }
		}

		public Uicontrolstate Uicontrolstate
		{
			get { return _uicontrolstate; }
			set { _uicontrolstate = value; }
		}

		public IList gate_event_expression_states
		{
			get
			{
				if (_gateeventexpressionstates==null)
				{
					_gateeventexpressionstates = new ArrayList();
				}
				return _gateeventexpressionstates;
			}
			set { _gateeventexpressionstates = value; }
		}

		public IList owner_idtrack_infos
		{
			get
			{
				if (_owneridtrackinfos==null)
				{
					_owneridtrackinfos = new ArrayList();
				}
				return _owneridtrackinfos;
			}
			set { _owneridtrackinfos = value; }
		}

		public Gateeventexpressionevaluatoruser Gateeventexpressionevaluatoruser
		{
			get { return _gateeventexpressionevaluatoruser; }
			set { _gateeventexpressionevaluatoruser = value; }
		}


		#endregion
		
	}

	#endregion
}



