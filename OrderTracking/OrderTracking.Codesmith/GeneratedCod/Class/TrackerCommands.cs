
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region TrackerCommand

	/// <summary>
	/// TrackerCommand object for NHibernate mapped table 'tbl_TrackerCommands'.
	/// </summary>
	public partial  class TrackerCommand
		{
		#region Member Variables
		
		protected long? _id;
		protected byte? _priority;
		protected string _name;
		protected string _command;
		protected TrackerType _trackerTypeId = new TrackerType();
		protected CommandType _commandTypeId = new CommandType();
		
		

		#endregion

		#region Constructors

		public TrackerCommand() { }

		public TrackerCommand( byte? priority, string name, string command, TrackerType trackerTypeId, CommandType commandTypeId )
		{
			this._priority = priority;
			this._name = name;
			this._command = command;
			this._trackerTypeId = trackerTypeId;
			this._commandTypeId = commandTypeId;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public byte? Priority
		{
			get { return _priority; }
			set { _priority = value; }
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Command
		{
			get { return _command; }
			set
			{
				if ( value != null && value.Length > 150)
					throw new ArgumentOutOfRangeException("Invalid value for Command", value, value.ToString());
				_command = value;
			}
		}

		public TrackerType TrackerTypeId
		{
			get { return _trackerTypeId; }
			set { _trackerTypeId = value; }
		}

		public CommandType CommandTypeId
		{
			get { return _commandTypeId; }
			set { _commandTypeId = value; }
		}


		#endregion
		
	}

	#endregion
}



