
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region CustomerOrder

	/// <summary>
	/// CustomerOrder object for NHibernate mapped table 'tbl_CustomerOrders'.
	/// </summary>
	public partial  class CustomerOrder
		{
		#region Member Variables
		
		protected long? _id;
		protected Order _orderId = new Order();
		protected Customer _customerId = new Customer();
		
		

		#endregion

		#region Constructors

		public CustomerOrder() { }

		public CustomerOrder( Order orderId, Customer customerId )
		{
			this._orderId = orderId;
			this._customerId = customerId;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public Order OrderId
		{
			get { return _orderId; }
			set { _orderId = value; }
		}

		public Customer CustomerId
		{
			get { return _customerId; }
			set { _customerId = value; }
		}


		#endregion
		
	}

	#endregion
}



