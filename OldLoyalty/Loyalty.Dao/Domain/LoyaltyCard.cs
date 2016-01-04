
using System;
using System.Collections;
using System.Runtime.Serialization;


namespace Loyalty.Dao.Domain
{
	#region LoyaltyCard

	/// <summary>
	/// LoyaltyCard object for NHibernate mapped table 'tbl_LoyaltyCard'.
	/// </summary>
	public class LoyaltyCard : Entity
		{
		#region Member Variables
		
		protected string _cardNumber;
		//protected DateTime? _dateTimeCreated;
		protected string _pin;
		protected LoyaltyAccount _loyaltyAccountId;
		
		protected IList _loyaltyCardIdTransactionHistories;
		protected IList _loyaltyCardIdRamesesAddressLoyaltyCards;
		protected IList _loyaltyCardIdLoyaltyCardStatuses;

		#endregion

		#region Constructors

		public LoyaltyCard() { }

		public LoyaltyCard( string cardNumber, LoyaltyAccount loyaltyAccount)
		{
			this._cardNumber = cardNumber;
			this._pin = null;
			this._loyaltyAccountId = loyaltyAccount;
		}

        public LoyaltyCard(string cardNumber, string pin, LoyaltyAccount loyaltyAccount)
        {
            this._cardNumber = cardNumber;
            this._pin = pin;
            this._loyaltyAccountId = loyaltyAccount;
        }

		#endregion

		#region Public Properties

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

        //public DateTime? DateTimeCreated
        //{
        //    get { return _dateTimeCreated; }
        //    set { _dateTimeCreated = value; }
        //}

		public LoyaltyAccount LoyaltyAccount
		{
			get { return _loyaltyAccountId; }
			set { _loyaltyAccountId = value; }
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

		public IList TransactionHistory
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

		public IList RamesesAddressLoyaltyCards
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

		public IList LoyaltyCardStatus
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

		#endregion
		
	}

	#endregion
}



