
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateeventexpression

	/// <summary>
	/// Gateeventexpression object for NHibernate mapped table 'gate_event_expression'.
	/// </summary>
	public partial  class Gateeventexpression
		{
		#region Member Variables
		
		protected int? _id;
		protected double? _valuedouble;
		protected short _valueboolean;
		protected double? _mindeltayfilter;
		protected int? _denominator;
		protected string _relationaloperator;
		protected string _mathoperator;
		protected string _valuestring;
		protected byte? _isendexpression;
		protected Gateeventexpressionevaluator _gateeventexpressionevaluator = new Gateeventexpressionevaluator();
		protected Loadabletype _loadabletype = new Loadabletype();
		protected Msgfield _msgfield = new Msgfield();
		
		
		protected Geofenceeventexpression _geofenceeventexpression;
		protected IList _gateeventexpressionstates;

		#endregion

		#region Constructors

		public Gateeventexpression() { }

		public Gateeventexpression( double? valuedouble, short valueboolean, double? mindeltayfilter, int? denominator, string relationaloperator, string mathoperator, string valuestring, byte? isendexpression, Gateeventexpressionevaluator gateeventexpressionevaluator, Loadabletype loadabletype, Msgfield msgfield )
		{
			this._valuedouble = valuedouble;
			this._valueboolean = valueboolean;
			this._mindeltayfilter = mindeltayfilter;
			this._denominator = denominator;
			this._relationaloperator = relationaloperator;
			this._mathoperator = mathoperator;
			this._valuestring = valuestring;
			this._isendexpression = isendexpression;
			this._gateeventexpressionevaluator = gateeventexpressionevaluator;
			this._loadabletype = loadabletype;
			this._msgfield = msgfield;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public double? Valuedouble
		{
			get { return _valuedouble; }
			set { _valuedouble = value; }
		}

		public short Valueboolean
		{
			get { return _valueboolean; }
			set { _valueboolean = value; }
		}

		public double? Mindeltayfilter
		{
			get { return _mindeltayfilter; }
			set { _mindeltayfilter = value; }
		}

		public int? Denominator
		{
			get { return _denominator; }
			set { _denominator = value; }
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

		public string Mathoperator
		{
			get { return _mathoperator; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Mathoperator", value, value.ToString());
				_mathoperator = value;
			}
		}

		public string Valuestring
		{
			get { return _valuestring; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Valuestring", value, value.ToString());
				_valuestring = value;
			}
		}

		public byte? Isendexpression
		{
			get { return _isendexpression; }
			set { _isendexpression = value; }
		}

		public Gateeventexpressionevaluator Gateeventexpressionevaluator
		{
			get { return _gateeventexpressionevaluator; }
			set { _gateeventexpressionevaluator = value; }
		}

		public Loadabletype Loadabletype
		{
			get { return _loadabletype; }
			set { _loadabletype = value; }
		}

		public Msgfield Msgfield
		{
			get { return _msgfield; }
			set { _msgfield = value; }
		}

		public Geofenceeventexpression Geofenceeventexpression
		{
			get { return _geofenceeventexpression; }
			set { _geofenceeventexpression = value; }
		}

		public IList gate_event_expression_states
		{
			get
			{
				if (_gateeventexpressionstates==null)
				{
					_gateeventexpressionstates = new ArrayList();
				}
				return _gateeventexpressionstates;
			}
			set { _gateeventexpressionstates = value; }
		}


		#endregion
		
	}

	#endregion
}



