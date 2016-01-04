
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Notifiertag

	/// <summary>
	/// Notifiertag object for NHibernate mapped table 'notifier_tags'.
	/// </summary>
	public partial  class Notifiertag
		{
		#region Member Variables
		
		protected Tag _tag = new Tag();
		
		

		#endregion

		#region Constructors

		public Notifiertag() { }

		public Notifiertag( Tag tag )
		{
			this._tag = tag;
		}

		#endregion

		#region Public Properties


		public Tag Tag
		{
			get { return _tag; }
			set { _tag = value; }
		}


		#endregion
		
	}

	#endregion
}



