
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Evaluatorcondition

	/// <summary>
	/// Evaluatorcondition object for NHibernate mapped table 'evaluator_condition'.
	/// </summary>
	public partial  class Evaluatorcondition
		{
		#region Member Variables
		
		protected int? _id;
		protected string _botype;
		protected string _name;
		protected DateTime? _created;
		protected Loadabletype _loadabletype = new Loadabletype();
		
		
		protected IList _evaluatorconditiondayofweekperiods;
		protected IList _evaluatorconditioneventdurations;

		#endregion

		#region Constructors

		public Evaluatorcondition() { }

		public Evaluatorcondition( string botype, string name, DateTime? created, Loadabletype loadabletype )
		{
			this._botype = botype;
			this._name = name;
			this._created = created;
			this._loadabletype = loadabletype;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
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

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public Loadabletype Loadabletype
		{
			get { return _loadabletype; }
			set { _loadabletype = value; }
		}

		public IList evaluator_condition_dayofweek_periods
		{
			get
			{
				if (_evaluatorconditiondayofweekperiods==null)
				{
					_evaluatorconditiondayofweekperiods = new ArrayList();
				}
				return _evaluatorconditiondayofweekperiods;
			}
			set { _evaluatorconditiondayofweekperiods = value; }
		}

		public IList evaluator_condition_event_durations
		{
			get
			{
				if (_evaluatorconditioneventdurations==null)
				{
					_evaluatorconditioneventdurations = new ArrayList();
				}
				return _evaluatorconditioneventdurations;
			}
			set { _evaluatorconditioneventdurations = value; }
		}


		#endregion
		
	}

	#endregion
}



