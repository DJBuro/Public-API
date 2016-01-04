
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Attribute

	/// <summary>
	/// Attribute object for NHibernate mapped table 'attribute'.
	/// </summary>
	public partial  class Attribute
		{
		#region Member Variables
		
		protected int? _id;
		protected string _attributename;
		protected string _attributetype;
		protected int? _intvalue;
		protected double? _doublevalue;
		protected string _stringvalue;
		protected short _boolvalue;
		
		
		protected Userattribute _userattribute;

		#endregion

		#region Constructors

		public Attribute() { }

		public Attribute( string attributename, string attributetype, int? intvalue, double? doublevalue, string stringvalue, short boolvalue )
		{
			this._attributename = attributename;
			this._attributetype = attributetype;
			this._intvalue = intvalue;
			this._doublevalue = doublevalue;
			this._stringvalue = stringvalue;
			this._boolvalue = boolvalue;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Attributename
		{
			get { return _attributename; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Attributename", value, value.ToString());
				_attributename = value;
			}
		}

		public string Attributetype
		{
			get { return _attributetype; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Attributetype", value, value.ToString());
				_attributetype = value;
			}
		}

		public int? Intvalue
		{
			get { return _intvalue; }
			set { _intvalue = value; }
		}

		public double? Doublevalue
		{
			get { return _doublevalue; }
			set { _doublevalue = value; }
		}

		public string Stringvalue
		{
			get { return _stringvalue; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Stringvalue", value, value.ToString());
				_stringvalue = value;
			}
		}

		public short Boolvalue
		{
			get { return _boolvalue; }
			set { _boolvalue = value; }
		}

		public Userattribute Userattribute
		{
			get { return _userattribute; }
			set { _userattribute = value; }
		}


		#endregion
		
	}

	#endregion
}



