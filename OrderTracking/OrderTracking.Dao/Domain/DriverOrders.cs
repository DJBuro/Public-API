
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region DriverOrder

	/// <summary>
	/// DriverOrder object for NHibernate mapped table 'tbl_DriverOrders'.
	/// </summary>
    public class DriverOrder : Entity.Entity
		{
		#region Member Variables
		
		protected Driver _driverId;
		protected Order _orderId;

		#endregion

		#region Constructors

		public DriverOrder() { }

		public DriverOrder( Driver driver, Order order )
		{
			this._driverId = driver;
			this._orderId = order;
		}

		#endregion

		#region Public Properties

		public Driver Driver
		{
			get { return _driverId; }
			set { _driverId = value; }
		}

		public Order Order
		{
			get { return _orderId; }
			set { _orderId = value; }
		}


		#endregion
		
	}

	#endregion
}



