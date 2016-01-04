
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region OrderItem

	/// <summary>
	/// OrderItem object for NHibernate mapped table 'tbl_OrderItem'.
	/// </summary>
    public class OrderItem : Entity.Entity
		{
		#region Member Variables
		
		protected Item _itemId;
		protected Order _orderId;

		#endregion

		#region Constructors

		public OrderItem() { }

		public OrderItem( Item itemId, Order orderId )
		{
			this._itemId = itemId;
			this._orderId = orderId;
		}

		#endregion

		#region Public Properties

		public Item Item
		{
			get { return _itemId; }
			set { _itemId = value; }
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



