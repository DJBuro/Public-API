
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Applicationdefreportviewer

	/// <summary>
	/// Applicationdefreportviewer object for NHibernate mapped table 'application_def_report_viewer'.
	/// </summary>
	public partial  class Applicationdefreportviewer
		{
		#region Member Variables
		
		protected Applicationdef _applicationdef = new Applicationdef();
		
		

		#endregion

		#region Constructors

		public Applicationdefreportviewer() { }

		public Applicationdefreportviewer( Applicationdef applicationdef )
		{
			this._applicationdef = applicationdef;
		}

		#endregion

		#region Public Properties


		public Applicationdef Applicationdef
		{
			get { return _applicationdef; }
			set { _applicationdef = value; }
		}


		#endregion
		
	}

	#endregion
}



