
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region CommandType

	/// <summary>
	/// CommandType object for NHibernate mapped table 'tbl_CommandType'.
	/// </summary>
	public partial  class CommandType
		{
		#region Member Variables
		
		protected long? _id;
		protected string _name;
		protected string _description;
		
		
		protected IList _commandTypeIdTrackerCommandses;

		#endregion

		#region Constructors

		public CommandType() { }

		public CommandType( string name, string description )
		{
			this._name = name;
			this._description = description;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
				_description = value;
			}
		}

		public IList CommandTypeIdTrackerCommandses
		{
			get
			{
				if (_commandTypeIdTrackerCommandses==null)
				{
					_commandTypeIdTrackerCommandses = new ArrayList();
				}
				return _commandTypeIdTrackerCommandses;
			}
			set { _commandTypeIdTrackerCommandses = value; }
		}


		#endregion
		
	}

	#endregion
}



