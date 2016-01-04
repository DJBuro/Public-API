
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Evaluatorconditiondayofweekperiod

	/// <summary>
	/// Evaluatorconditiondayofweekperiod object for NHibernate mapped table 'evaluator_condition_dayofweek_period'.
	/// </summary>
	public partial  class Evaluatorconditiondayofweekperiod
		{
		#region Member Variables
		
		protected int? _id;
		protected long? _starttimeofday;
		protected long? _stoptimeofday;
		protected int? _dayofweek;
		protected int? _evaluationmethod;
		protected Evaluatorcondition _evaluatorcondition = new Evaluatorcondition();
		
		

		#endregion

		#region Constructors

		public Evaluatorconditiondayofweekperiod() { }

		public Evaluatorconditiondayofweekperiod( long? starttimeofday, long? stoptimeofday, int? dayofweek, int? evaluationmethod, Evaluatorcondition evaluatorcondition )
		{
			this._starttimeofday = starttimeofday;
			this._stoptimeofday = stoptimeofday;
			this._dayofweek = dayofweek;
			this._evaluationmethod = evaluationmethod;
			this._evaluatorcondition = evaluatorcondition;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public long? Starttimeofday
		{
			get { return _starttimeofday; }
			set { _starttimeofday = value; }
		}

		public long? Stoptimeofday
		{
			get { return _stoptimeofday; }
			set { _stoptimeofday = value; }
		}

		public int? Dayofweek
		{
			get { return _dayofweek; }
			set { _dayofweek = value; }
		}

		public int? Evaluationmethod
		{
			get { return _evaluationmethod; }
			set { _evaluationmethod = value; }
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



