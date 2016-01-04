
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Schedulertaskparameter

	/// <summary>
	/// Schedulertaskparameter object for NHibernate mapped table 'scheduler_task_parameter'.
	/// </summary>
	public partial  class Schedulertaskparameter
		{
		#region Member Variables
		
		protected int? _id;
		protected string _parametername;
		protected string _parameterassemblyname;
		protected string _parametertypename;
		protected string _parameterdata;
		protected Schedulertask _schedulertask = new Schedulertask();
		
		

		#endregion

		#region Constructors

		public Schedulertaskparameter() { }

		public Schedulertaskparameter( string parametername, string parameterassemblyname, string parametertypename, string parameterdata, Schedulertask schedulertask )
		{
			this._parametername = parametername;
			this._parameterassemblyname = parameterassemblyname;
			this._parametertypename = parametertypename;
			this._parameterdata = parameterdata;
			this._schedulertask = schedulertask;
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

		public Schedulertask Schedulertask
		{
			get { return _schedulertask; }
			set { _schedulertask = value; }
		}


		#endregion
		
	}

	#endregion
}



