
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Item

	/// <summary>
	/// Item object for NHibernate mapped table 'tbl_Item'.
	/// </summary>
    public class Item : Entity.Entity
		{
		#region Member Variables
		
		protected int _quantity;
		protected string _name;
		protected Order _orderId;
		
		
		//protected IList _itemIdOrderItems;

		#endregion

		#region Constructors

		public Item() { }

        public Item(int quantity, string name, Order order)
		{
			this._quantity = quantity;
			this._name = name;
			this._orderId = order;
		}

		#endregion

		#region Public Properties

		public int Quantity
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

        public Order Order
        {
            get { return _orderId; }
            set { _orderId = value; }
        }


		#endregion
		
	}

	#endregion
}



