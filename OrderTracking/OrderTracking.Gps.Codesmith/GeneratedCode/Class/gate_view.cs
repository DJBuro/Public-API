
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateview

	/// <summary>
	/// Gateview object for NHibernate mapped table 'gate_view'.
	/// </summary>
	public partial  class Gateview
		{
		#region Member Variables
		
		protected int? _id;
		protected string _viewname;
		protected string _viewdescription;
		protected int? _statusfilter;
		protected short _matchalltags;
		protected string _botype;
		protected Application _application = new Application();
		
		
		protected Gateviewtag _gateviewtag;

		#endregion

		#region Constructors

		public Gateview() { }

		public Gateview( string viewname, string viewdescription, int? statusfilter, short matchalltags, string botype, Application application )
		{
			this._viewname = viewname;
			this._viewdescription = viewdescription;
			this._statusfilter = statusfilter;
			this._matchalltags = matchalltags;
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

		public string Viewname
		{
			get { return _viewname; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Viewname", value, value.ToString());
				_viewname = value;
			}
		}

		public string Viewdescription
		{
			get { return _viewdescription; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Viewdescription", value, value.ToString());
				_viewdescription = value;
			}
		}

		public int? Statusfilter
		{
			get { return _statusfilter; }
			set { _statusfilter = value; }
		}

		public short Matchalltags
		{
			get { return _matchalltags; }
			set { _matchalltags = value; }
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

		public Gateviewtag Gateviewtag
		{
			get { return _gateviewtag; }
			set { _gateviewtag = value; }
		}


		#endregion
		
	}

	#endregion
}



