
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region IndicatorDefinition

	/// <summary>
	/// IndicatorDefinition object for NHibernate mapped table 'IndicatorDefinitions'.
	/// </summary>
	public class IndicatorDefinition : Entity
		{
		#region Member Variables
		
		protected string _indicatorName;
		protected string _indicatorValueType;
		protected int? _columnNumber;
		protected int? _interfaceColumnNumber;
		protected string _indicatorFormat;
		protected int? _divisor;
		protected bool _useFormula;
		protected int? _formulaColumnNumber;
		protected string _formulaExpression;
		protected bool _reverseSortingOrder;
		protected string _benchMark;
		protected string _summaryType;
		protected HeadOffice _headOffice;
		protected bool _useColumn;
		protected bool _showGauge;
		
		
		protected IList _indicatorTranslationses;

		#endregion

		#region Constructors

		public IndicatorDefinition() { }

		public IndicatorDefinition( string indicatorName, string indicatorValueType, int? columnNumber, int? interfaceColumnNumber, string indicatorFormat, int? divisor, bool useFormula, int? formulaColumnNumber, string formulaExpression, bool reverseSortingOrder, string benchMark, string summaryType, HeadOffice headOffice, bool useColumn, bool showGauge  )
		{
			this._indicatorName = indicatorName;
			this._indicatorValueType = indicatorValueType;
			this._columnNumber = columnNumber;
			this._interfaceColumnNumber = interfaceColumnNumber;
			this._indicatorFormat = indicatorFormat;
			this._divisor = divisor;
			this._useFormula = useFormula;
			this._formulaColumnNumber = formulaColumnNumber;
			this._formulaExpression = formulaExpression;
			this._reverseSortingOrder = reverseSortingOrder;
			this._benchMark = benchMark;
			this._summaryType = summaryType;
			this._headOffice = headOffice;
			this._useColumn = useColumn;
			this._showGauge = showGauge;
		}

		#endregion

		#region Public Properties

		public string IndicatorName
		{
			get { return _indicatorName; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for IndicatorName", value, value.ToString());
				_indicatorName = value;
			}
		}

		public string IndicatorValueType
		{
			get { return _indicatorValueType; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for IndicatorValueType", value, value.ToString());
				_indicatorValueType = value;
			}
		}

		public int? ColumnNumber
		{
			get { return _columnNumber; }
			set { _columnNumber = value; }
		}

		public int? InterfaceColumnNumber
		{
			get { return _interfaceColumnNumber; }
			set { _interfaceColumnNumber = value; }
		}

		public string IndicatorFormat
		{
			get { return _indicatorFormat; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for IndicatorFormat", value, value.ToString());
				_indicatorFormat = value;
			}
		}

		public int? Divisor
		{
			get { return _divisor; }
			set { _divisor = value; }
		}

		public bool UseFormula
		{
			get { return _useFormula; }
			set { _useFormula = value; }
		}

		public int? FormulaColumnNumber
		{
			get { return _formulaColumnNumber; }
			set { _formulaColumnNumber = value; }
		}

		public string FormulaExpression
		{
			get { return _formulaExpression; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for FormulaExpression", value, value.ToString());
				_formulaExpression = value;
			}
		}

		public bool ReverseSortingOrder
		{
			get { return _reverseSortingOrder; }
			set { _reverseSortingOrder = value; }
		}

		public string BenchMark
		{
			get { return _benchMark; }
			set
			{
				if ( value != null && value.Length > 8)
					throw new ArgumentOutOfRangeException("Invalid value for BenchMark", value, value.ToString());
				_benchMark = value;
			}
		}

		public string SummaryType
		{
			get { return _summaryType; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for SummaryType", value, value.ToString());
				_summaryType = value;
			}
		}

		public HeadOffice HeadOffice
		{
			get { return _headOffice; }
			set { _headOffice = value; }
		}

		public IList IndicatorTranslations
		{
			get
			{
				if (_indicatorTranslationses==null)
				{
					_indicatorTranslationses = new ArrayList();
				}
				return _indicatorTranslationses;
			}
			set { _indicatorTranslationses = value; }
		}
		public bool UseColumn
		{
			get { return _useColumn; }
			set { _useColumn = value; }
		}
		public bool ShowGauge
		{
			get { return _showGauge; }
			set { _showGauge = value; }
		}



		#endregion
		
	}

	#endregion
}



