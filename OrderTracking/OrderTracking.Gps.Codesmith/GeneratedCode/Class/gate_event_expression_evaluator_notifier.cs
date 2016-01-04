
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateeventexpressionevaluatornotifier

	/// <summary>
	/// Gateeventexpressionevaluatornotifier object for NHibernate mapped table 'gate_event_expression_evaluator_notifier'.
	/// </summary>
	public partial  class Gateeventexpressionevaluatornotifier
		{
		#region Member Variables
		
		protected Gateeventexpressionevaluator _gateeventexpressionevaluator = new Gateeventexpressionevaluator();
		
		

		#endregion

		#region Constructors

		public Gateeventexpressionevaluatornotifier() { }

		public Gateeventexpressionevaluatornotifier( Gateeventexpressionevaluator gateeventexpressionevaluator )
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



