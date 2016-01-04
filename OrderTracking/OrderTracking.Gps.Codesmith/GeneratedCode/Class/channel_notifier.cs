
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Channelnotifier

	/// <summary>
	/// Channelnotifier object for NHibernate mapped table 'channel_notifier'.
	/// </summary>
	public partial  class Channelnotifier
		{
		#region Member Variables
		
		protected int? _id;
		protected Notifier _notifier = new Notifier();
		protected Gateeventchannel _gateeventchannel = new Gateeventchannel();
		
		

		#endregion

		#region Constructors

		public Channelnotifier() { }

		public Channelnotifier( Notifier notifier, Gateeventchannel gateeventchannel )
		{
			this._notifier = notifier;
			this._gateeventchannel = gateeventchannel;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public Notifier Notifier
		{
			get { return _notifier; }
			set { _notifier = value; }
		}

		public Gateeventchannel Gateeventchannel
		{
			get { return _gateeventchannel; }
			set { _gateeventchannel = value; }
		}


		#endregion
		
	}

	#endregion
}



