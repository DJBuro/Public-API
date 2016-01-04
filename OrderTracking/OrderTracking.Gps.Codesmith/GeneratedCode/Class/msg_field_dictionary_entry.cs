
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Msgfielddictionaryentry

	/// <summary>
	/// Msgfielddictionaryentry object for NHibernate mapped table 'msg_field_dictionary_entry'.
	/// </summary>
	public partial  class Msgfielddictionaryentry
		{
		#region Member Variables
		
		protected int? _id;
		protected double? _multiplicator;
		protected double? _constant;
		protected short _enabled;
		protected short _savewithpos;
		protected short _savechangesonly;
		protected string _multiplicatorformula;
		protected Msgfield _in = new Msgfield();
		protected Msgfield _out = new Msgfield();
		protected Msgfielddictionary _msgfielddictionary = new Msgfielddictionary();
		
		

		#endregion

		#region Constructors

		public Msgfielddictionaryentry() { }

		public Msgfielddictionaryentry( double? multiplicator, double? constant, short enabled, short savewithpos, short savechangesonly, string multiplicatorformula, Msgfield in, Msgfield out, Msgfielddictionary msgfielddictionary )
		{
			this._multiplicator = multiplicator;
			this._constant = constant;
			this._enabled = enabled;
			this._savewithpos = savewithpos;
			this._savechangesonly = savechangesonly;
			this._multiplicatorformula = multiplicatorformula;
			this._in = in;
			this._out = out;
			this._msgfielddictionary = msgfielddictionary;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public double? Multiplicator
		{
			get { return _multiplicator; }
			set { _multiplicator = value; }
		}

		public double? Constant
		{
			get { return _constant; }
			set { _constant = value; }
		}

		public short Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		public short Savewithpos
		{
			get { return _savewithpos; }
			set { _savewithpos = value; }
		}

		public short Savechangesonly
		{
			get { return _savechangesonly; }
			set { _savechangesonly = value; }
		}

		public string Multiplicatorformula
		{
			get { return _multiplicatorformula; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Multiplicatorformula", value, value.ToString());
				_multiplicatorformula = value;
			}
		}

		public Msgfield in_
		{
			get { return _in; }
			set { _in = value; }
		}

		public Msgfield out_
		{
			get { return _out; }
			set { _out = value; }
		}

		public Msgfielddictionary Msgfielddictionary
		{
			get { return _msgfielddictionary; }
			set { _msgfielddictionary = value; }
		}


		#endregion
		
	}

	#endregion
}



