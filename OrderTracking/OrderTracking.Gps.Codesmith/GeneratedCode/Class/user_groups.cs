
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Usergroup

	/// <summary>
	/// Usergroup object for NHibernate mapped table 'user_groups'.
	/// </summary>
	public partial  class Usergroup
		{
		#region Member Variables
		
		protected int? _grouprightid;
		protected int? _adminrightid;
		protected short _enablepublictracks;
		
		

		#endregion

		#region Constructors

		public Usergroup() { }

		public Usergroup( int? grouprightid, int? adminrightid, short enablepublictracks )
		{
			this._grouprightid = grouprightid;
			this._adminrightid = adminrightid;
			this._enablepublictracks = enablepublictracks;
		}

		#endregion

		#region Public Properties


		public int? Grouprightid
		{
			get { return _grouprightid; }
			set { _grouprightid = value; }
		}

		public int? Adminrightid
		{
			get { return _adminrightid; }
			set { _adminrightid = value; }
		}

		public short Enablepublictracks
		{
			get { return _enablepublictracks; }
			set { _enablepublictracks = value; }
		}


		#endregion
		
	}

	#endregion
}



