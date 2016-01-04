
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Unit

	/// <summary>
	/// Unit object for NHibernate mapped table 'unit'.
	/// </summary>
	public partial  class Unit
		{
		#region Member Variables
		
		protected string _id;
		protected string _symbol;
		protected string _measure;
		
		
		protected IList _msgfields;

		#endregion

		#region Constructors

		public Unit() { }

		public Unit( string symbol, string measure )
		{
			this._symbol = symbol;
			this._measure = measure;
		}

		#endregion

		#region Public Properties

		public string Id
		{
			get {return _id;}
			set
			{
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for Id", value, value.ToString());
				_id = value;
			}
		}

		public string Symbol
		{
			get { return _symbol; }
			set
			{
				if ( value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Symbol", value, value.ToString());
				_symbol = value;
			}
		}

		public string Measure
		{
			get { return _measure; }
			set
			{
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for Measure", value, value.ToString());
				_measure = value;
			}
		}

		public IList msg_fields
		{
			get
			{
				if (_msgfields==null)
				{
					_msgfields = new ArrayList();
				}
				return _msgfields;
			}
			set { _msgfields = value; }
		}


		#endregion
		
	}

	#endregion
}



