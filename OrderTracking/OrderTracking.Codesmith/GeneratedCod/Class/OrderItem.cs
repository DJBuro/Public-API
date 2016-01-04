
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region OrderItem

	/// <summary>
	/// OrderItem object for NHibernate mapped table 'tbl_OrderItem'.
	/// </summary>
	public partial  class OrderItem
		{
		#region Member Variables
		
		protected long? _id;
		protected Item _itemId = new Item();
		protected Order _orderId = new Order();
		
		

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

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public Item ItemId
		{
			get { return _itemId; }
			set { _itemId = value; }
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



