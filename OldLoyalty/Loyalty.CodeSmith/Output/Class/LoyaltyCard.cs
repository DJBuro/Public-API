
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region LoyaltyCard

	/// <summary>
	/// LoyaltyCard object for NHibernate mapped table 'tbl_LoyaltyCard'.
	/// </summary>
	public partial  class LoyaltyCard
		{
		#region Member Variables
		
		protected int? _id;
		protected string _cardNumber;
		protected DateTime? _dateTimeCreated;
		protected string _pin;
		protected LoyaltyAccount _loyaltyAccountId = new LoyaltyAccount();
		
		
		protected IList _loyaltyCardIdRamesesAddressLoyaltyCards;
		protected IList _loyaltyCardIdLoyaltyCardStatuses;
		protected IList _loyaltyCardIdTransactionHistories;

		#endregion

		#region Constructors

		public LoyaltyCard() { }

		public LoyaltyCard( string cardNumber, DateTime? dateTimeCreated, string pin, LoyaltyAccount loyaltyAccountId )
		{
			this._cardNumber = cardNumber;
			this._dateTimeCreated = dateTimeCreated;
			this._pin = pin;
			this._loyaltyAccountId = loyaltyAccountId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string CardNumber
		{
			get { return _cardNumber; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for CardNumber", value, value.ToString());
				_cardNumber = value;
			}
		}

		public DateTime? DateTimeCreated
		{
			get { return _dateTimeCreated; }
			set { _dateTimeCreated = value; }
		}

		public string Pin
		{
			get { return _pin; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Pin", value, value.ToString());
				_pin = value;
			}
		}

		public LoyaltyAccount LoyaltyAccountId
		{
			get { return _loyaltyAccountId; }
			set { _loyaltyAccountId = value; }
		}

		public IList LoyaltyCardIdRamesesAddressLoyaltyCards
		{
			get
			{
				if (_loyaltyCardIdRamesesAddressLoyaltyCards==null)
				{
					_loyaltyCardIdRamesesAddressLoyaltyCards = new ArrayList();
				}
				return _loyaltyCardIdRamesesAddressLoyaltyCards;
			}
			set { _loyaltyCardIdRamesesAddressLoyaltyCards = value; }
		}

		public IList LoyaltyCardIdLoyaltyCardStatuses
		{
			get
			{
				if (_loyaltyCardIdLoyaltyCardStatuses==null)
				{
					_loyaltyCardIdLoyaltyCardStatuses = new ArrayList();
				}
				return _loyaltyCardIdLoyaltyCardStatuses;
			}
			set { _loyaltyCardIdLoyaltyCardStatuses = value; }
		}

		public IList LoyaltyCardIdTransactionHistories
		{
			get
			{
				if (_loyaltyCardIdTransactionHistories==null)
				{
					_loyaltyCardIdTransactionHistories = new ArrayList();
				}
				return _loyaltyCardIdTransactionHistories;
			}
			set { _loyaltyCardIdTransactionHistories = value; }
		}


		#endregion
		
	}

	#endregion
}



