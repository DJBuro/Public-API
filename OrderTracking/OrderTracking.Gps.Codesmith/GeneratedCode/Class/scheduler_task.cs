
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Schedulertask

	/// <summary>
	/// Schedulertask object for NHibernate mapped table 'scheduler_task'.
	/// </summary>
	public partial  class Schedulertask
		{
		#region Member Variables
		
		protected int? _id;
		protected string _taskname;
		protected string _taskgroup;
		protected int? _execcount;
		protected DateTime? _lastexectimestamp;
		protected DateTime? _nextexectimestamp;
		protected short _state;
		protected string _taskassemblyname;
		protected string _tasktypename;
		protected Scheduler _scheduler = new Scheduler();
		
		
		protected IList _schedulertasktriggers;
		protected IList _schedulertaskparameters;

		#endregion

		#region Constructors

		public Schedulertask() { }

		public Schedulertask( string taskname, string taskgroup, int? execcount, DateTime? lastexectimestamp, DateTime? nextexectimestamp, short state, string taskassemblyname, string tasktypename, Scheduler scheduler )
		{
			this._taskname = taskname;
			this._taskgroup = taskgroup;
			this._execcount = execcount;
			this._lastexectimestamp = lastexectimestamp;
			this._nextexectimestamp = nextexectimestamp;
			this._state = state;
			this._taskassemblyname = taskassemblyname;
			this._tasktypename = tasktypename;
			this._scheduler = scheduler;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Taskname
		{
			get { return _taskname; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Taskname", value, value.ToString());
				_taskname = value;
			}
		}

		public string Taskgroup
		{
			get { return _taskgroup; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Taskgroup", value, value.ToString());
				_taskgroup = value;
			}
		}

		public int? Execcount
		{
			get { return _execcount; }
			set { _execcount = value; }
		}

		public DateTime? Lastexectimestamp
		{
			get { return _lastexectimestamp; }
			set { _lastexectimestamp = value; }
		}

		public DateTime? Nextexectimestamp
		{
			get { return _nextexectimestamp; }
			set { _nextexectimestamp = value; }
		}

		public short State
		{
			get { return _state; }
			set { _state = value; }
		}

		public string Taskassemblyname
		{
			get { return _taskassemblyname; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Taskassemblyname", value, value.ToString());
				_taskassemblyname = value;
			}
		}

		public string Tasktypename
		{
			get { return _tasktypename; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Tasktypename", value, value.ToString());
				_tasktypename = value;
			}
		}

		public Scheduler Scheduler
		{
			get { return _scheduler; }
			set { _scheduler = value; }
		}

		public IList scheduler_task_triggers
		{
			get
			{
				if (_schedulertasktriggers==null)
				{
					_schedulertasktriggers = new ArrayList();
				}
				return _schedulertasktriggers;
			}
			set { _schedulertasktriggers = value; }
		}

		public IList scheduler_task_parameters
		{
			get
			{
				if (_schedulertaskparameters==null)
				{
					_schedulertaskparameters = new ArrayList();
				}
				return _schedulertaskparameters;
			}
			set { _schedulertaskparameters = value; }
		}


		#endregion
		
	}

	#endregion
}



