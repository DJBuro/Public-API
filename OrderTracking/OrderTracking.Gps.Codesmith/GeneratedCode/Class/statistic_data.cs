
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Statisticdata

	/// <summary>
	/// Statisticdata object for NHibernate mapped table 'statistic_data'.
	/// </summary>
	public partial  class Statisticdata
		{
		#region Member Variables
		
		protected int? _id;
		protected string _value;
		
		

		#endregion

		#region Constructors

		public Statisticdata() { }

		public Statisticdata( string value )
		{
			this._value = value;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Value
		{
			get { return _value; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Value", value, value.ToString());
				_value = value;
			}
		}


		#endregion
		
	}

	#endregion
}



