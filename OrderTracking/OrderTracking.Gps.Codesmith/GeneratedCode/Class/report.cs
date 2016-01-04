
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Report

	/// <summary>
	/// Report object for NHibernate mapped table 'report'.
	/// </summary>
	public partial  class Report
		{
		#region Member Variables
		
		protected int? _id;
		protected string _reportname;
		protected string _reportdescription;
		protected string _botype;
		protected Reportviewer _reportviewer = new Reportviewer();
		protected Application _application = new Application();
		
		
		protected IList _reportparameters;

		#endregion

		#region Constructors

		public Report() { }

		public Report( string reportname, string reportdescription, string botype, Reportviewer reportviewer, Application application )
		{
			this._reportname = reportname;
			this._reportdescription = reportdescription;
			this._botype = botype;
			this._reportviewer = reportviewer;
			this._application = application;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Reportname
		{
			get { return _reportname; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Reportname", value, value.ToString());
				_reportname = value;
			}
		}

		public string Reportdescription
		{
			get { return _reportdescription; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Reportdescription", value, value.ToString());
				_reportdescription = value;
			}
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public Reportviewer Reportviewer
		{
			get { return _reportviewer; }
			set { _reportviewer = value; }
		}

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
		}

		public IList report_parameters
		{
			get
			{
				if (_reportparameters==null)
				{
					_reportparameters = new ArrayList();
				}
				return _reportparameters;
			}
			set { _reportparameters = value; }
		}


		#endregion
		
	}

	#endregion
}



