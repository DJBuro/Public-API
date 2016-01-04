
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region OrderItemHistory

	/// <summary>
	/// OrderItemHistory object for NHibernate mapped table 'tbl_OrderItemHistory'.
	/// </summary>
	public partial  class OrderItemHistory
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected decimal? _itemPrice;
		protected int? _itemLoyaltyPoints;
		protected TransactionHistory _transactionHistoryId = new TransactionHistory();
		
		

		#endregion

		#region Constructors

		public OrderItemHistory() { }

		public OrderItemHistory( string name, decimal? itemPrice, int? itemLoyaltyPoints, TransactionHistory transactionHistoryId )
		{
			this._name = name;
			this._itemPrice = itemPrice;
			this._itemLoyaltyPoints = itemLoyaltyPoints;
			this._transactionHistoryId = transactionHistoryId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
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

		public decimal? ItemPrice
		{
			get { return _itemPrice; }
			set { _itemPrice = value; }
		}

		public int? ItemLoyaltyPoints
		{
			get { return _itemLoyaltyPoints; }
			set { _itemLoyaltyPoints = value; }
		}

		public TransactionHistory TransactionHistoryId
		{
			get { return _transactionHistoryId; }
			set { _transactionHistoryId = value; }
		}


		#endregion
		
	}

	#endregion
}



