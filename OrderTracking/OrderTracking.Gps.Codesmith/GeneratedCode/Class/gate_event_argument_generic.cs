
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateeventargumentgeneric

	/// <summary>
	/// Gateeventargumentgeneric object for NHibernate mapped table 'gate_event_argument_generic'.
	/// </summary>
	public partial  class Gateeventargumentgeneric
		{
		#region Member Variables
		
		protected int? _id;
		protected int? _intvalue;
		protected double? _dblvalue;
		protected byte? _boolvalue;
		protected string _strvalue;
		protected Gateeventargument _gateeventargument = new Gateeventargument();
		
		

		#endregion

		#region Constructors

		public Gateeventargumentgeneric() { }

		public Gateeventargumentgeneric( int? intvalue, double? dblvalue, byte? boolvalue, string strvalue, Gateeventargument gateeventargument )
		{
			this._intvalue = intvalue;
			this._dblvalue = dblvalue;
			this._boolvalue = boolvalue;
			this._strvalue = strvalue;
			this._gateeventargument = gateeventargument;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Intvalue
		{
			get { return _intvalue; }
			set { _intvalue = value; }
		}

		public double? Dblvalue
		{
			get { return _dblvalue; }
			set { _dblvalue = value; }
		}

		public byte? Boolvalue
		{
			get { return _boolvalue; }
			set { _boolvalue = value; }
		}

		public string Strvalue
		{
			get { return _strvalue; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Strvalue", value, value.ToString());
				_strvalue = value;
			}
		}

		public Gateeventargument Gateeventargument
		{
			get { return _gateeventargument; }
			set { _gateeventargument = value; }
		}


		#endregion
		
	}

	#endregion
}



