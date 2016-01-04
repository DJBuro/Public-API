
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Reportparameter

	/// <summary>
	/// Reportparameter object for NHibernate mapped table 'report_parameter'.
	/// </summary>
	public partial  class Reportparameter
		{
		#region Member Variables
		
		protected int? _id;
		protected string _parametername;
		protected string _parametertypename;
		protected string _parameterassemblyname;
		protected string _parameterdata;
		protected Report _report = new Report();
		
		

		#endregion

		#region Constructors

		public Reportparameter() { }

		public Reportparameter( string parametername, string parametertypename, string parameterassemblyname, string parameterdata, Report report )
		{
			this._parametername = parametername;
			this._parametertypename = parametertypename;
			this._parameterassemblyname = parameterassemblyname;
			this._parameterdata = parameterdata;
			this._report = report;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Parametername
		{
			get { return _parametername; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Parametername", value, value.ToString());
				_parametername = value;
			}
		}

		public string Parametertypename
		{
			get { return _parametertypename; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Parametertypename", value, value.ToString());
				_parametertypename = value;
			}
		}

		public string Parameterassemblyname
		{
			get { return _parameterassemblyname; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Parameterassemblyname", value, value.ToString());
				_parameterassemblyname = value;
			}
		}

		public string Parameterdata
		{
			get { return _parameterdata; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Parameterdata", value, value.ToString());
				_parameterdata = value;
			}
		}

		public Report Report
		{
			get { return _report; }
			set { _report = value; }
		}


		#endregion
		
	}

	#endregion
}



