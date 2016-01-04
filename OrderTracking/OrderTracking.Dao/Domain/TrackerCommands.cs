
using System;


namespace OrderTracking.Dao.Domain
{
	#region TrackerCommand

	/// <summary>
	/// TrackerCommand object for NHibernate mapped table 'tbl_TrackerCommands'.
	/// </summary>
    public class TrackerCommand : Entity.Entity
		{
		#region Member Variables
		
		protected byte? _priority;
		protected string _name;
		protected string _command;
		protected TrackerType _trackerTypeId;
		protected CommandType _commandTypeId;
		
		

		#endregion

		#region Constructors

		public TrackerCommand() { }

		public TrackerCommand( byte? priority, string name, string command, TrackerType trackerType, CommandType commandType )
		{
			this._priority = priority;
			this._name = name;
			this._command = command;
			this._trackerTypeId = trackerType;
			this._commandTypeId = commandType;
		}

		#endregion

		#region Public Properties

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

		public TrackerType TrackerType
		{
			get { return _trackerTypeId; }
			set { _trackerTypeId = value; }
		}

		public CommandType CommandType
		{
			get { return _commandTypeId; }
			set { _commandTypeId = value; }
		}


		#endregion
		
	}

	#endregion
}



