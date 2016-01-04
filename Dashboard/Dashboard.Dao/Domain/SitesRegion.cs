
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region SitesRegion

	/// <summary>
	/// SitesRegion object for NHibernate mapped table 'SitesRegions'.
	/// </summary>
	public class SitesRegion : Entity
		{
		#region Member Variables
		
		protected Site _site;
		protected Region _region;
		
		

		#endregion

		#region Constructors

		public SitesRegion() { }

		public SitesRegion( Site site, Region region )
		{
			this._site = site;
			this._region = region;
		}

		#endregion

		#region Public Properties

		public Site Site
		{
			get { return _site; }
			set { _site = value; }
		}

		public Region Region
		{
			get { return _region; }
			set { _region = value; }
		}


		#endregion
		
	}

	#endregion
}



