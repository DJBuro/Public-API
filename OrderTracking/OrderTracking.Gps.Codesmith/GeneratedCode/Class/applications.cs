
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Application

	/// <summary>
	/// Application object for NHibernate mapped table 'applications'.
	/// </summary>
	public partial  class Application
		{
		#region Member Variables
		
		protected int? _id;
		protected string _botype;
		protected string _name;
		protected string _description;
		protected int? _maxusers;
		protected DateTime? _expire;
		protected DateTime? _created;
		
		
		protected IList _tags;
		protected IList _scriptpluginapplications;
		protected IList _gateviews;
		protected IList _notifiers;
		protected IList _gateevents;
		protected IList _geofences;
		protected IList _appsettingses;
		protected IList _workspaces;
		protected IList _temporarycredentials;
		protected IList _apptemplateses;
		protected IList _groupses;
		protected Uicontrolstate _uicontrolstate;
		protected IList _reports;
		protected IList _gateeventexpressionevaluators;
		protected Geocoderapplication _geocoderapplication;
		protected IList _gateeventchanneldictionaries;

		#endregion

		#region Constructors

		public Application() { }

		public Application( string botype, string name, string description, int? maxusers, DateTime? expire, DateTime? created )
		{
			this._botype = botype;
			this._name = name;
			this._description = description;
			this._maxusers = maxusers;
			this._expire = expire;
			this._created = created;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
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

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
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

		public int? Maxusers
		{
			get { return _maxusers; }
			set { _maxusers = value; }
		}

		public DateTime? Expire
		{
			get { return _expire; }
			set { _expire = value; }
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public IList tags
		{
			get
			{
				if (_tags==null)
				{
					_tags = new ArrayList();
				}
				return _tags;
			}
			set { _tags = value; }
		}

		public IList script_plugin_applications
		{
			get
			{
				if (_scriptpluginapplications==null)
				{
					_scriptpluginapplications = new ArrayList();
				}
				return _scriptpluginapplications;
			}
			set { _scriptpluginapplications = value; }
		}

		public IList gate_views
		{
			get
			{
				if (_gateviews==null)
				{
					_gateviews = new ArrayList();
				}
				return _gateviews;
			}
			set { _gateviews = value; }
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

		public IList gate_events
		{
			get
			{
				if (_gateevents==null)
				{
					_gateevents = new ArrayList();
				}
				return _gateevents;
			}
			set { _gateevents = value; }
		}

		public IList geofences
		{
			get
			{
				if (_geofences==null)
				{
					_geofences = new ArrayList();
				}
				return _geofences;
			}
			set { _geofences = value; }
		}

		public IList app_settingses
		{
			get
			{
				if (_appsettingses==null)
				{
					_appsettingses = new ArrayList();
				}
				return _appsettingses;
			}
			set { _appsettingses = value; }
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

		public IList app_templateses
		{
			get
			{
				if (_apptemplateses==null)
				{
					_apptemplateses = new ArrayList();
				}
				return _apptemplateses;
			}
			set { _apptemplateses = value; }
		}

		public IList groupses
		{
			get
			{
				if (_groupses==null)
				{
					_groupses = new ArrayList();
				}
				return _groupses;
			}
			set { _groupses = value; }
		}

		public Uicontrolstate Uicontrolstate
		{
			get { return _uicontrolstate; }
			set { _uicontrolstate = value; }
		}

		public IList reports
		{
			get
			{
				if (_reports==null)
				{
					_reports = new ArrayList();
				}
				return _reports;
			}
			set { _reports = value; }
		}

		public IList gate_event_expression_evaluators
		{
			get
			{
				if (_gateeventexpressionevaluators==null)
				{
					_gateeventexpressionevaluators = new ArrayList();
				}
				return _gateeventexpressionevaluators;
			}
			set { _gateeventexpressionevaluators = value; }
		}

		public Geocoderapplication Geocoderapplication
		{
			get { return _geocoderapplication; }
			set { _geocoderapplication = value; }
		}

		public IList gate_event_channel_dictionaries
		{
			get
			{
				if (_gateeventchanneldictionaries==null)
				{
					_gateeventchanneldictionaries = new ArrayList();
				}
				return _gateeventchanneldictionaries;
			}
			set { _gateeventchanneldictionaries = value; }
		}


		#endregion
		
	}

	#endregion
}



