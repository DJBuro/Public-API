
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Order

	/// <summary>
	/// Order object for NHibernate mapped table 'tbl_Order'.
	/// </summary>
	public partial  class Order
		{
		#region Member Variables
		
		protected long? _id;
		protected string _externalOrderId;
		protected string _name;
		protected string _ticketNumber;
		protected string _extraInformation;
		protected DateTime? _proximityDelivered;
		protected DateTime? _created;
		protected Store _storeId = new Store();
		
		
		protected IList _orderIdOrderStatuses;
		protected IList _orderIdItems;
		protected IList _orderIdCustomerOrderses;
		protected IList _orderIdDriverOrderses;

		#endregion

		#region Constructors

		public Order() { }

		public Order( string externalOrderId, string name, string ticketNumber, string extraInformation, DateTime? proximityDelivered, DateTime? created, Store storeId )
		{
			this._externalOrderId = externalOrderId;
			this._name = name;
			this._ticketNumber = ticketNumber;
			this._extraInformation = extraInformation;
			this._proximityDelivered = proximityDelivered;
			this._created = created;
			this._storeId = storeId;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string ExternalOrderId
		{
			get { return _externalOrderId; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for ExternalOrderId", value, value.ToString());
				_externalOrderId = value;
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string TicketNumber
		{
			get { return _ticketNumber; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for TicketNumber", value, value.ToString());
				_ticketNumber = value;
			}
		}

		public string ExtraInformation
		{
			get { return _extraInformation; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for ExtraInformation", value, value.ToString());
				_extraInformation = value;
			}
		}

		public DateTime? ProximityDelivered
		{
			get { return _proximityDelivered; }
			set { _proximityDelivered = value; }
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public Store StoreId
		{
			get { return _storeId; }
			set { _storeId = value; }
		}

		public IList OrderIdOrderStatuses
		{
			get
			{
				if (_orderIdOrderStatuses==null)
				{
					_orderIdOrderStatuses = new ArrayList();
				}
				return _orderIdOrderStatuses;
			}
			set { _orderIdOrderStatuses = value; }
		}

		public IList OrderIdItems
		{
			get
			{
				if (_orderIdItems==null)
				{
					_orderIdItems = new ArrayList();
				}
				return _orderIdItems;
			}
			set { _orderIdItems = value; }
		}

		public IList OrderIdCustomerOrderses
		{
			get
			{
				if (_orderIdCustomerOrderses==null)
				{
					_orderIdCustomerOrderses = new ArrayList();
				}
				return _orderIdCustomerOrderses;
			}
			set { _orderIdCustomerOrderses = value; }
		}

		public IList OrderIdDriverOrderses
		{
			get
			{
				if (_orderIdDriverOrderses==null)
				{
					_orderIdDriverOrderses = new ArrayList();
				}
				return _orderIdDriverOrderses;
			}
			set { _orderIdDriverOrderses = value; }
		}


		#endregion
		
	}

	#endregion
}



