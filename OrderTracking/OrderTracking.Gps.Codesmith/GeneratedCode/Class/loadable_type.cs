
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Loadabletype

	/// <summary>
	/// Loadabletype object for NHibernate mapped table 'loadable_type'.
	/// </summary>
	public partial  class Loadabletype
		{
		#region Member Variables
		
		protected int? _id;
		protected string _assemblyname;
		protected string _typename;
		protected string _typedescription;
		protected string _basetypename;
		protected string _basetypedescription;
		protected short _deleted;
		protected DateTime? _created;
		protected string _version;
		
		
		protected IList _reportviewers;
		protected IList _serviceplugins;
		protected IList _gatecommands;
		protected IList _notifiers;
		protected IList _listeners;
		protected IList _messageproviders;
		protected IList _postprocessors;
		protected IList _evaluatorconditions;
		protected IList _devicedefs;
		protected IList _applicationdefs;
		protected IList _recorderrules;
		protected IList _gateeventexpressions;
		protected IList _applicationrules;

		#endregion

		#region Constructors

		public Loadabletype() { }

		public Loadabletype( string assemblyname, string typename, string typedescription, string basetypename, string basetypedescription, short deleted, DateTime? created, string version )
		{
			this._assemblyname = assemblyname;
			this._typename = typename;
			this._typedescription = typedescription;
			this._basetypename = basetypename;
			this._basetypedescription = basetypedescription;
			this._deleted = deleted;
			this._created = created;
			this._version = version;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Assemblyname
		{
			get { return _assemblyname; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Assemblyname", value, value.ToString());
				_assemblyname = value;
			}
		}

		public string Typename
		{
			get { return _typename; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Typename", value, value.ToString());
				_typename = value;
			}
		}

		public string Typedescription
		{
			get { return _typedescription; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Typedescription", value, value.ToString());
				_typedescription = value;
			}
		}

		public string Basetypename
		{
			get { return _basetypename; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Basetypename", value, value.ToString());
				_basetypename = value;
			}
		}

		public string Basetypedescription
		{
			get { return _basetypedescription; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Basetypedescription", value, value.ToString());
				_basetypedescription = value;
			}
		}

		public short Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public string Version
		{
			get { return _version; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Version", value, value.ToString());
				_version = value;
			}
		}

		public IList report_viewers
		{
			get
			{
				if (_reportviewers==null)
				{
					_reportviewers = new ArrayList();
				}
				return _reportviewers;
			}
			set { _reportviewers = value; }
		}

		public IList service_plugins
		{
			get
			{
				if (_serviceplugins==null)
				{
					_serviceplugins = new ArrayList();
				}
				return _serviceplugins;
			}
			set { _serviceplugins = value; }
		}

		public IList gate_commands
		{
			get
			{
				if (_gatecommands==null)
				{
					_gatecommands = new ArrayList();
				}
				return _gatecommands;
			}
			set { _gatecommands = value; }
		}

		public IList notifiers
		{
			get
			{
				if (_notifiers==null)
				{
					_notifiers = new ArrayList();
				}
				return _notifiers;
			}
			set { _notifiers = value; }
		}

		public IList listeners
		{
			get
			{
				if (_listeners==null)
				{
					_listeners = new ArrayList();
				}
				return _listeners;
			}
			set { _listeners = value; }
		}

		public IList message_providers
		{
			get
			{
				if (_messageproviders==null)
				{
					_messageproviders = new ArrayList();
				}
				return _messageproviders;
			}
			set { _messageproviders = value; }
		}

		public IList post_processors
		{
			get
			{
				if (_postprocessors==null)
				{
					_postprocessors = new ArrayList();
				}
				return _postprocessors;
			}
			set { _postprocessors = value; }
		}

		public IList evaluator_conditions
		{
			get
			{
				if (_evaluatorconditions==null)
				{
					_evaluatorconditions = new ArrayList();
				}
				return _evaluatorconditions;
			}
			set { _evaluatorconditions = value; }
		}

		public IList device_defs
		{
			get
			{
				if (_devicedefs==null)
				{
					_devicedefs = new ArrayList();
				}
				return _devicedefs;
			}
			set { _devicedefs = value; }
		}

		public IList application_defs
		{
			get
			{
				if (_applicationdefs==null)
				{
					_applicationdefs = new ArrayList();
				}
				return _applicationdefs;
			}
			set { _applicationdefs = value; }
		}

		public IList recorder_rules
		{
			get
			{
				if (_recorderrules==null)
				{
					_recorderrules = new ArrayList();
				}
				return _recorderrules;
			}
			set { _recorderrules = value; }
		}

		public IList gate_event_expressions
		{
			get
			{
				if (_gateeventexpressions==null)
				{
					_gateeventexpressions = new ArrayList();
				}
				return _gateeventexpressions;
			}
			set { _gateeventexpressions = value; }
		}

		public IList application_rules
		{
			get
			{
				if (_applicationrules==null)
				{
					_applicationrules = new ArrayList();
				}
				return _applicationrules;
			}
			set { _applicationrules = value; }
		}


		#endregion
		
	}

	#endregion
}



