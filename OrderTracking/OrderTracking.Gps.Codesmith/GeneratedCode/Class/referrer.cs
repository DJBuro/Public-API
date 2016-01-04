
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Referrer

	/// <summary>
	/// Referrer object for NHibernate mapped table 'referrer'.
	/// </summary>
	public partial  class Referrer
		{
		#region Member Variables
		
		protected int? _id;
		protected string _refurl;
		
		

		#endregion

		#region Constructors

		public Referrer() { }

		public Referrer( string refurl )
		{
			this._refurl = refurl;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Refurl
		{
			get { return _refurl; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Refurl", value, value.ToString());
				_refurl = value;
			}
		}


		#endregion
		
	}

	#endregion
}



