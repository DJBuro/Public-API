
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Smsproxyqueue

	/// <summary>
	/// Smsproxyqueue object for NHibernate mapped table 'sms_proxy_queue'.
	/// </summary>
	public partial  class Smsproxyqueue
		{
		#region Member Variables
		
		protected int? _id;
		protected int? _smsproxyid;
		
		

		#endregion

		#region Constructors

		public Smsproxyqueue() { }

		public Smsproxyqueue( int? smsproxyid )
		{
			this._smsproxyid = smsproxyid;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Smsproxyid
		{
			get { return _smsproxyid; }
			set { _smsproxyid = value; }
		}


		#endregion
		
	}

	#endregion
}



