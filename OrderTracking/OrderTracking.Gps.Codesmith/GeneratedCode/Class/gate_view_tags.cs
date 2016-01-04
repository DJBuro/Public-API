
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateviewtag

	/// <summary>
	/// Gateviewtag object for NHibernate mapped table 'gate_view_tags'.
	/// </summary>
	public partial  class Gateviewtag
		{
		#region Member Variables
		
		protected Gateview _gateview = new Gateview();
		
		

		#endregion

		#region Constructors

		public Gateviewtag() { }

		public Gateviewtag( Gateview gateview )
		{
			this._gateview = gateview;
		}

		#endregion

		#region Public Properties


		public Gateview Gateview
		{
			get { return _gateview; }
			set { _gateview = value; }
		}


		#endregion
		
	}

	#endregion
}



