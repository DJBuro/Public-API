
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Groupright

	/// <summary>
	/// Groupright object for NHibernate mapped table 'group_rights'.
	/// </summary>
	public partial  class Groupright
		{
		#region Member Variables
		
		protected string _botype;
		
		

		#endregion

		#region Constructors

		public Groupright() { }

		public Groupright( string botype )
		{
			this._botype = botype;
		}

		#endregion

		#region Public Properties


		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}


		#endregion
		
	}

	#endregion
}



