
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Reportviewer

	/// <summary>
	/// Reportviewer object for NHibernate mapped table 'report_viewer'.
	/// </summary>
	public partial  class Reportviewer
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		protected string _reportviewertype;
		protected Loadabletype _loadabletype = new Loadabletype();
		
		
		protected IList _reports;

		#endregion

		#region Constructors

		public Reportviewer() { }

		public Reportviewer( string name, string description, string reportviewertype, Loadabletype loadabletype )
		{
			this._name = name;
			this._description = description;
			this._reportviewertype = reportviewertype;
			this._loadabletype = loadabletype;
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
				if ( value != null && value.Length > 64)
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

		public string Reportviewertype
		{
			get { return _reportviewertype; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Reportviewertype", value, value.ToString());
				_reportviewertype = value;
			}
		}

		public Loadabletype Loadabletype
		{
			get { return _loadabletype; }
			set { _loadabletype = value; }
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


		#endregion
		
	}

	#endregion
}



