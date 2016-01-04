
using System;
using System.Collections;
using System.Xml.Serialization;


namespace OrderTracking.Dao.Domain
{
	#region Order

	/// <summary>
	/// Order object for NHibernate mapped table 'tbl_Order'.
	/// </summary>
    public class Order : Entity.Entity
		{
		#region Member Variables
		
		protected string _externalOrderId;
		protected string _name;
		protected string _ticketNumber;
		protected string _extraInformation;
		protected DateTime? _proximityDelivered;
		protected Store _storeId;
		
		
		protected IList _orderIdCustomerOrderses;
		protected IList _orderIdDriverOrderses;
		protected IList _orderIdOrderStatuses;
		protected IList _orderIdItems;

		#endregion

		#region Constructors

		public Order() 
		{ 
		}

		public Order(
		    string externalOrderId, 
		    string name, 
		    string ticketNumber, 
		    string extraInformation, 
		    DateTime? proximityDelivered, 
		    Store store )
		{
			this._externalOrderId = externalOrderId;
			this._name = name;
			this._ticketNumber = ticketNumber;
			this._extraInformation = extraInformation;
			this._proximityDelivered = proximityDelivered;
			this._storeId = store;
		}

		#endregion

		#region Public Properties

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

		public Store Store
		{
			get { return _storeId; }
			set { _storeId = value; }
		}

        public IList CustomerOrder
        {
            get
            {
                if (_orderIdCustomerOrderses == null)
                {
                    _orderIdCustomerOrderses = new ArrayList();
                }
                return _orderIdCustomerOrderses;
            }
            set { _orderIdCustomerOrderses = value; }
        }

        //public IList OrderIdDriverOrderses
        //{
        //    get
        //    {
        //        if (_orderIdDriverOrderses==null)
        //        {
        //            _orderIdDriverOrderses = new ArrayList();
        //        }
        //        return _orderIdDriverOrderses;
        //    }
        //    set { _orderIdDriverOrderses = value; }
        //}

		public IList OrderStatus
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

		public IList Items
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


		#endregion
		
	}

	#endregion
}



