
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Evaluatorconditioneventduration

	/// <summary>
	/// Evaluatorconditioneventduration object for NHibernate mapped table 'evaluator_condition_event_duration'.
	/// </summary>
	public partial  class Evaluatorconditioneventduration
		{
		#region Member Variables
		
		protected int? _id;
		protected long? _eventduration;
		protected string _relationaloperator;
		protected Evaluatorcondition _evaluatorcondition = new Evaluatorcondition();
		
		

		#endregion

		#region Constructors

		public Evaluatorconditioneventduration() { }

		public Evaluatorconditioneventduration( long? eventduration, string relationaloperator, Evaluatorcondition evaluatorcondition )
		{
			this._eventduration = eventduration;
			this._relationaloperator = relationaloperator;
			this._evaluatorcondition = evaluatorcondition;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public long? Eventduration
		{
			get { return _eventduration; }
			set { _eventduration = value; }
		}

		public string Relationaloperator
		{
			get { return _relationaloperator; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Relationaloperator", value, value.ToString());
				_relationaloperator = value;
			}
		}

		public Evaluatorcondition Evaluatorcondition
		{
			get { return _evaluatorcondition; }
			set { _evaluatorcondition = value; }
		}


		#endregion
		
	}

	#endregion
}



