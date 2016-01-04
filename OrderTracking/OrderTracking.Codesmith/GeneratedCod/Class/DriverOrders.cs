
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
		
		protected long? _id;
		protected Order _orderId = new Order();
		protected Driver _driverId = new Driver();
		
		

		#endregion

		#region Constructors

		public DriverOrder() { }

		public DriverOrder( Order orderId, Driver driverId )
		{
			this._orderId = orderId;
			this._driverId = driverId;
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

		public Driver DriverId
		{
			get { return _driverId; }
			set { _driverId = value; }
		}


		#endregion
		
	}

	#endregion
}



