
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Scheduler

	/// <summary>
	/// Scheduler object for NHibernate mapped table 'scheduler'.
	/// </summary>
	public partial  class Scheduler
		{
		#region Member Variables
		
		protected int? _id;
		protected string _schedulername;
		protected string _botype;
		
		
		protected IList _schedulertasks;

		#endregion

		#region Constructors

		public Scheduler() { }

		public Scheduler( string schedulername, string botype )
		{
			this._schedulername = schedulername;
			this._botype = botype;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Schedulername
		{
			get { return _schedulername; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Schedulername", value, value.ToString());
				_schedulername = value;
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

		public IList scheduler_tasks
		{
			get
			{
				if (_schedulertasks==null)
				{
					_schedulertasks = new ArrayList();
				}
				return _schedulertasks;
			}
			set { _schedulertasks = value; }
		}


		#endregion
		
	}

	#endregion
}



