
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Providermessagequeue

	/// <summary>
	/// Providermessagequeue object for NHibernate mapped table 'provider_message_queue'.
	/// </summary>
	public partial  class Providermessagequeue
		{
		#region Member Variables
		
		protected int? _id;
		protected Messageprovider _messageprovider = new Messageprovider();
		
		
		protected IList _queueidprovidermessages;

		#endregion

		#region Constructors

		public Providermessagequeue() { }

		public Providermessagequeue( Messageprovider messageprovider )
		{
			this._messageprovider = messageprovider;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public Messageprovider Messageprovider
		{
			get { return _messageprovider; }
			set { _messageprovider = value; }
		}

		public IList queue_idprovider_messages
		{
			get
			{
				if (_queueidprovidermessages==null)
				{
					_queueidprovidermessages = new ArrayList();
				}
				return _queueidprovidermessages;
			}
			set { _queueidprovidermessages = value; }
		}


		#endregion
		
	}

	#endregion
}



