
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Item

	/// <summary>
	/// Item object for NHibernate mapped table 'tbl_Item'.
	/// </summary>
	public partial  class Item
		{
		#region Member Variables
		
		protected long? _id;
		protected int? _quantity;
		protected string _name;
		protected Order _orderId = new Order();
		
		

		#endregion

		#region Constructors

		public Item() { }

		public Item( int? quantity, string name, Order orderId )
		{
			this._quantity = quantity;
			this._name = name;
			this._orderId = orderId;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Quantity
		{
			get { return _quantity; }
			set { _quantity = value; }
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

		public Order OrderId
		{
			get { return _orderId; }
			set { _orderId = value; }
		}


		#endregion
		
	}

	#endregion
}



