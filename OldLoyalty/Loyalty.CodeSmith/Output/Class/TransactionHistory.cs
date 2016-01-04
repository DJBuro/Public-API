
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region TransactionHistory

	/// <summary>
	/// TransactionHistory object for NHibernate mapped table 'tbl_TransactionHistory'.
	/// </summary>
	public partial  class TransactionHistory
		{
		#region Member Variables
		
		protected int? _id;
		protected DateTime? _dateTimeOrdered;
		protected int? _orderId;
		protected int? _loyaltyPointsRedeemed;
		protected int? _loyaltyPointsAdded;
		protected int? _loyaltyPointsValue;
		protected int? _orderTotal;
		protected LoyaltyCard _loyaltyCardId = new LoyaltyCard();
		protected Site _siteId = new Site();
		protected OrderType _orderTypeId;
		
		
		protected IList _transactionHistoryIdOrderItemHistories;

		#endregion

		#region Constructors

		public TransactionHistory() { }

		public TransactionHistory( DateTime? dateTimeOrdered, int? orderId, int? loyaltyPointsRedeemed, int? loyaltyPointsAdded, int? loyaltyPointsValue, int? orderTotal, LoyaltyCard loyaltyCardId, Site siteId, OrderType orderTypeId )
		{
			this._dateTimeOrdered = dateTimeOrdered;
			this._orderId = orderId;
			this._loyaltyPointsRedeemed = loyaltyPointsRedeemed;
			this._loyaltyPointsAdded = loyaltyPointsAdded;
			this._loyaltyPointsValue = loyaltyPointsValue;
			this._orderTotal = orderTotal;
			this._loyaltyCardId = loyaltyCardId;
			this._siteId = siteId;
			this._orderTypeId = orderTypeId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public DateTime? DateTimeOrdered
		{
			get { return _dateTimeOrdered; }
			set { _dateTimeOrdered = value; }
		}

		public int? OrderId
		{
			get { return _orderId; }
			set { _orderId = value; }
		}

		public int? LoyaltyPointsRedeemed
		{
			get { return _loyaltyPointsRedeemed; }
			set { _loyaltyPointsRedeemed = value; }
		}

		public int? LoyaltyPointsAdded
		{
			get { return _loyaltyPointsAdded; }
			set { _loyaltyPointsAdded = value; }
		}

		public int? LoyaltyPointsValue
		{
			get { return _loyaltyPointsValue; }
			set { _loyaltyPointsValue = value; }
		}

		public int? OrderTotal
		{
			get { return _orderTotal; }
			set { _orderTotal = value; }
		}

		public LoyaltyCard LoyaltyCardId
		{
			get { return _loyaltyCardId; }
			set { _loyaltyCardId = value; }
		}

		public Site SiteId
		{
			get { return _siteId; }
			set { _siteId = value; }
		}

		public OrderType OrderTypeId
		{
			get { return _orderTypeId; }
			set { _orderTypeId = value; }
		}

		public IList TransactionHistoryIdOrderItemHistories
		{
			get
			{
				if (_transactionHistoryIdOrderItemHistories==null)
				{
					_transactionHistoryIdOrderItemHistories = new ArrayList();
				}
				return _transactionHistoryIdOrderItemHistories;
			}
			set { _transactionHistoryIdOrderItemHistories = value; }
		}


		#endregion
		
	}

	#endregion
}



