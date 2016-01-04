
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Tag

	/// <summary>
	/// Tag object for NHibernate mapped table 'tag'.
	/// </summary>
	public partial  class Tag
		{
		#region Member Variables
		
		protected int? _id;
		protected string _tagname;
		protected string _tagdescription;
		protected string _botype;
		protected Application _application = new Application();
		
		
		protected Taguser _taguser;
		protected Notifiertag _notifiertag;
		protected Taggeofence _taggeofence;
		protected IList _geofenceeventexpressions;

		#endregion

		#region Constructors

		public Tag() { }

		public Tag( string tagname, string tagdescription, string botype, Application application )
		{
			this._tagname = tagname;
			this._tagdescription = tagdescription;
			this._botype = botype;
			this._application = application;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Tagname
		{
			get { return _tagname; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Tagname", value, value.ToString());
				_tagname = value;
			}
		}

		public string Tagdescription
		{
			get { return _tagdescription; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Tagdescription", value, value.ToString());
				_tagdescription = value;
			}
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
		}

		public Taguser Taguser
		{
			get { return _taguser; }
			set { _taguser = value; }
		}

		public Notifiertag Notifiertag
		{
			get { return _notifiertag; }
			set { _notifiertag = value; }
		}

		public Taggeofence Taggeofence
		{
			get { return _taggeofence; }
			set { _taggeofence = value; }
		}

		public IList geofence_event_expressions
		{
			get
			{
				if (_geofenceeventexpressions==null)
				{
					_geofenceeventexpressions = new ArrayList();
				}
				return _geofenceeventexpressions;
			}
			set { _geofenceeventexpressions = value; }
		}


		#endregion
		
	}

	#endregion
}



