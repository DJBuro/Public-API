
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateeventexpressionevaluatorevaluatorcondition

	/// <summary>
	/// Gateeventexpressionevaluatorevaluatorcondition object for NHibernate mapped table 'gate_event_expression_evaluator_evaluator_condition'.
	/// </summary>
	public partial  class Gateeventexpressionevaluatorevaluatorcondition
		{
		#region Member Variables
		
		protected Gateeventexpressionevaluator _gateeventexpressionevaluator = new Gateeventexpressionevaluator();
		
		

		#endregion

		#region Constructors

		public Gateeventexpressionevaluatorevaluatorcondition() { }

		public Gateeventexpressionevaluatorevaluatorcondition( Gateeventexpressionevaluator gateeventexpressionevaluator )
		{
			this._gateeventexpressionevaluator = gateeventexpressionevaluator;
		}

		#endregion

		#region Public Properties


		public Gateeventexpressionevaluator Gateeventexpressionevaluator
		{
			get { return _gateeventexpressionevaluator; }
			set { _gateeventexpressionevaluator = value; }
		}


		#endregion
		
	}

	#endregion
}



