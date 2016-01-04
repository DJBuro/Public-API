
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Adminright

	/// <summary>
	/// Adminright object for NHibernate mapped table 'admin_rights'.
	/// </summary>
	public partial  class Adminright
		{
		#region Member Variables
		
		protected string _botype;
		
		

		#endregion

		#region Constructors

		public Adminright() { }

		public Adminright( string botype )
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



