
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateeventexpressionevaluator

	/// <summary>
	/// Gateeventexpressionevaluator object for NHibernate mapped table 'gate_event_expression_evaluator'.
	/// </summary>
	public partial  class Gateeventexpressionevaluator
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		protected short _deleted;
		protected string _expressionbooloperator;
		protected DateTime? _created;
		protected string _endexpressionbooloperator;
		protected Application _application = new Application();
		
		
		protected Gateeventexpressionevaluatorchannel _gateeventexpressionevaluatorchannel;
		protected Gateeventexpressionevaluatornotifier _gateeventexpressionevaluatornotifier;
		protected Gateeventexpressionevaluatorevaluatorcondition _gateeventexpressionevaluatorevaluatorcondition;
		protected IList _gateeventexpressions;
		protected Gateeventexpressionevaluatoruser _gateeventexpressionevaluatoruser;

		#endregion

		#region Constructors

		public Gateeventexpressionevaluator() { }

		public Gateeventexpressionevaluator( string name, string description, short deleted, string expressionbooloperator, DateTime? created, string endexpressionbooloperator, Application application )
		{
			this._name = name;
			this._description = description;
			this._deleted = deleted;
			this._expressionbooloperator = expressionbooloperator;
			this._created = created;
			this._endexpressionbooloperator = endexpressionbooloperator;
			this._application = application;
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

		public short Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}

		public string Expressionbooloperator
		{
			get { return _expressionbooloperator; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Expressionbooloperator", value, value.ToString());
				_expressionbooloperator = value;
			}
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public string Endexpressionbooloperator
		{
			get { return _endexpressionbooloperator; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Endexpressionbooloperator", value, value.ToString());
				_endexpressionbooloperator = value;
			}
		}

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
		}

		public Gateeventexpressionevaluatorchannel Gateeventexpressionevaluatorchannel
		{
			get { return _gateeventexpressionevaluatorchannel; }
			set { _gateeventexpressionevaluatorchannel = value; }
		}

		public Gateeventexpressionevaluatornotifier Gateeventexpressionevaluatornotifier
		{
			get { return _gateeventexpressionevaluatornotifier; }
			set { _gateeventexpressionevaluatornotifier = value; }
		}

		public Gateeventexpressionevaluatorevaluatorcondition Gateeventexpressionevaluatorevaluatorcondition
		{
			get { return _gateeventexpressionevaluatorevaluatorcondition; }
			set { _gateeventexpressionevaluatorevaluatorcondition = value; }
		}

		public IList gate_event_expressions
		{
			get
			{
				if (_gateeventexpressions==null)
				{
					_gateeventexpressions = new ArrayList();
				}
				return _gateeventexpressions;
			}
			set { _gateeventexpressions = value; }
		}

		public Gateeventexpressionevaluatoruser Gateeventexpressionevaluatoruser
		{
			get { return _gateeventexpressionevaluatoruser; }
			set { _gateeventexpressionevaluatoruser = value; }
		}


		#endregion
		
	}

	#endregion
}



