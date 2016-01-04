
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Scriptplugin

	/// <summary>
	/// Scriptplugin object for NHibernate mapped table 'script_plugin'.
	/// </summary>
	public partial  class Scriptplugin
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _category;
		protected string _description;
		protected string _filepath;
		protected string _version;
		protected int? _loadorder;
		protected int? _deleted;
		protected DateTime? _created;
		
		
		protected IList _scriptpages;
		protected IList _scriptfiles;
		protected IList _scriptpluginapplications;
		protected IList _scriptservices;

		#endregion

		#region Constructors

		public Scriptplugin() { }

		public Scriptplugin( string name, string category, string description, string filepath, string version, int? loadorder, int? deleted, DateTime? created )
		{
			this._name = name;
			this._category = category;
			this._description = description;
			this._filepath = filepath;
			this._version = version;
			this._loadorder = loadorder;
			this._deleted = deleted;
			this._created = created;
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
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Category
		{
			get { return _category; }
			set
			{
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for Category", value, value.ToString());
				_category = value;
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

		public string Filepath
		{
			get { return _filepath; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Filepath", value, value.ToString());
				_filepath = value;
			}
		}

		public string Version
		{
			get { return _version; }
			set
			{
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for Version", value, value.ToString());
				_version = value;
			}
		}

		public int? Loadorder
		{
			get { return _loadorder; }
			set { _loadorder = value; }
		}

		public int? Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public IList script_pages
		{
			get
			{
				if (_scriptpages==null)
				{
					_scriptpages = new ArrayList();
				}
				return _scriptpages;
			}
			set { _scriptpages = value; }
		}

		public IList script_files
		{
			get
			{
				if (_scriptfiles==null)
				{
					_scriptfiles = new ArrayList();
				}
				return _scriptfiles;
			}
			set { _scriptfiles = value; }
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

		public IList script_services
		{
			get
			{
				if (_scriptservices==null)
				{
					_scriptservices = new ArrayList();
				}
				return _scriptservices;
			}
			set { _scriptservices = value; }
		}


		#endregion
		
	}

	#endregion
}



