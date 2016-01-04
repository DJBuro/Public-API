
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Schedulertasktrigger

	/// <summary>
	/// Schedulertasktrigger object for NHibernate mapped table 'scheduler_task_trigger'.
	/// </summary>
	public partial  class Schedulertasktrigger
		{
		#region Member Variables
		
		protected int? _id;
		protected string _triggerassemblyname;
		protected string _triggertypename;
		protected string _triggerdata;
		protected Schedulertask _schedulertask = new Schedulertask();
		
		

		#endregion

		#region Constructors

		public Schedulertasktrigger() { }

		public Schedulertasktrigger( string triggerassemblyname, string triggertypename, string triggerdata, Schedulertask schedulertask )
		{
			this._triggerassemblyname = triggerassemblyname;
			this._triggertypename = triggertypename;
			this._triggerdata = triggerdata;
			this._schedulertask = schedulertask;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Triggerassemblyname
		{
			get { return _triggerassemblyname; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Triggerassemblyname", value, value.ToString());
				_triggerassemblyname = value;
			}
		}

		public string Triggertypename
		{
			get { return _triggertypename; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Triggertypename", value, value.ToString());
				_triggertypename = value;
			}
		}

		public string Triggerdata
		{
			get { return _triggerdata; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Triggerdata", value, value.ToString());
				_triggerdata = value;
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



