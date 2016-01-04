
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateeventstate

	/// <summary>
	/// Gateeventstate object for NHibernate mapped table 'gate_event_state'.
	/// </summary>
	public partial  class Gateeventstate
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		
		
		protected IList _gateeventlogs;

		#endregion

		#region Constructors

		public Gateeventstate() { }

		public Gateeventstate( string name, string description )
		{
			this._name = name;
			this._description = description;
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

		public IList gate_event_logs
		{
			get
			{
				if (_gateeventlogs==null)
				{
					_gateeventlogs = new ArrayList();
				}
				return _gateeventlogs;
			}
			set { _gateeventlogs = value; }
		}


		#endregion
		
	}

	#endregion
}



