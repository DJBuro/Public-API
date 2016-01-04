
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region DriverOrder

	/// <summary>
	/// DriverOrder object for NHibernate mapped table 'tbl_DriverOrders'.
	/// </summary>
	public partial  class DriverOrder
		{
		#region Member Variables
		
		protected int? _id;
		protected Driver _driverId = new Driver();
		protected Order _orderId = new Order();
		
		

		#endregion

		#region Constructors

		public DriverOrder() { }

		public DriverOrder( Driver driverId, Order orderId )
		{
			this._driverId = driverId;
			this._orderId = orderId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public Driver DriverId
		{
			get { return _driverId; }
			set { _driverId = value; }
		}

		public Order OrderId
		{
			get { return _orderId; }
			set { _orderId = value; }
		}


		#endregion
		
	}

	#endregion
}



