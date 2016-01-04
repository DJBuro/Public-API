
using System;

namespace WebDashboard.Dao.Domain
{
	#region Indicator

	/// <summary>
	/// Indicator object for NHibernate mapped table 'tbl_Indicator'.
	/// </summary>
    public class Indicator : Entity.Entity
		{
		#region Member Variables
		
		protected decimal _benchMark;
		protected bool _reverseSort;
		protected int _divisorColumnId;
		protected bool _allowZero;
		protected int _maxValue;
		protected bool _displayAsScroller;
		protected string _longName;
		protected string _shortName;
		protected int _customDivisorValue;
		protected HeadOffice _headOffice;
		protected DivisorType _divisorType;
		protected Definition _definition;
		protected IndicatorType _indicatorType;
		protected ValueType _valueType;
		
		

		#endregion

		#region Constructors

		public Indicator() { }

		public Indicator( decimal benchMark, bool reverseSort, int divisorColumn, bool allowZero, int maxValue, bool displayAsScroller, string longName, string shortName, int customDivisorValue, HeadOffice headOffice, DivisorType divisorType, Definition definition, IndicatorType indicatorType, ValueType valueType )
		{
			this._benchMark = benchMark;
			this._reverseSort = reverseSort;
			this._divisorColumnId = divisorColumn;
			this._allowZero = allowZero;
			this._maxValue = maxValue;
			this._displayAsScroller = displayAsScroller;
			this._longName = longName;
			this._shortName = shortName;
			this._customDivisorValue = customDivisorValue;
			this._headOffice = headOffice;
			this._divisorType = divisorType;
			this._definition = definition;
			this._indicatorType = indicatorType;
			this._valueType = valueType;
		}

		#endregion

		#region Public Properties

		public decimal BenchMark
		{
			get { return _benchMark; }
			set { _benchMark = value; }
		}

		public bool ReverseSort
		{
			get { return _reverseSort; }
			set { _reverseSort = value; }
		}

		public int DivisorColumn
		{
			get { return _divisorColumnId; }
			set { _divisorColumnId = value; }
		}

		public bool AllowZero
		{
			get { return _allowZero; }
			set { _allowZero = value; }
		}

		public int MaxValue
		{
			get { return _maxValue; }
			set { _maxValue = value; }
		}

		public bool DisplayAsScroller
		{
			get { return _displayAsScroller; }
			set { _displayAsScroller = value; }
		}

		public string LongName
		{
			get { return _longName; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for LongName", value, value.ToString());
				_longName = value;
			}
		}

		public string ShortName
		{
			get { return _shortName; }
			set
			{
				if ( value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for ShortName", value, value.ToString());
				_shortName = value;
			}
		}

		public int CustomDivisorValue
		{
			get { return _customDivisorValue; }
			set { _customDivisorValue = value; }
		}

		public HeadOffice HeadOffice
		{
			get { return _headOffice; }
			set { _headOffice = value; }
		}

		public DivisorType DivisorType
		{
			get { return _divisorType; }
			set { _divisorType = value; }
		}

		public Definition Definition
		{
			get { return _definition; }
			set { _definition = value; }
		}

		public IndicatorType IndicatorType
		{
			get { return _indicatorType; }
			set { _indicatorType = value; }
		}

		public ValueType ValueType
		{
			get { return _valueType; }
			set { _valueType = value; }
		}


		#endregion
		
	}

	#endregion
}



