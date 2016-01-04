
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region CustomerOrder

	/// <summary>
	/// CustomerOrder object for NHibernate mapped table 'tbl_CustomerOrders'.
	/// </summary>
    public class CustomerOrder : Entity.Entity
		{
		#region Member Variables
		
		protected Order _orderId;
		protected Customer _customerId;

		#endregion

		#region Constructors

		public CustomerOrder() { }

		public CustomerOrder( Order order, Customer customer )
		{
			this._orderId = order;
			this._customerId = customer;
		}

		#endregion

		#region Public Properties

		public Order Order
		{
			get { return _orderId; }
			set { _orderId = value; }
		}

		public Customer Customer
		{
			get { return _customerId; }
			set { _customerId = value; }
		}


		#endregion
		
	}

	#endregion
}



