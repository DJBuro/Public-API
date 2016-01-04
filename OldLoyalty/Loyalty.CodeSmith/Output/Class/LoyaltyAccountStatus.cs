
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region LoyaltyAccountStatus

	/// <summary>
	/// LoyaltyAccountStatus object for NHibernate mapped table 'tbl_LoyaltyAccountStatus'.
	/// </summary>
	public partial  class LoyaltyAccountStatus
		{
		#region Member Variables
		
		protected int? _id;
		protected string _reason;
		protected DateTime? _dateTimeCreated;
		protected LoyaltyAccount _loyaltyAccountId = new LoyaltyAccount();
		protected AccountStatus _accountStatusId = new AccountStatus();
		
		

		#endregion

		#region Constructors

		public LoyaltyAccountStatus() { }

		public LoyaltyAccountStatus( string reason, DateTime? dateTimeCreated, LoyaltyAccount loyaltyAccountId, AccountStatus accountStatusId )
		{
			this._reason = reason;
			this._dateTimeCreated = dateTimeCreated;
			this._loyaltyAccountId = loyaltyAccountId;
			this._accountStatusId = accountStatusId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Reason
		{
			get { return _reason; }
			set
			{
				if ( value != null && value.Length > 250)
					throw new ArgumentOutOfRangeException("Invalid value for Reason", value, value.ToString());
				_reason = value;
			}
		}

		public DateTime? DateTimeCreated
		{
			get { return _dateTimeCreated; }
			set { _dateTimeCreated = value; }
		}

		public LoyaltyAccount LoyaltyAccountId
		{
			get { return _loyaltyAccountId; }
			set { _loyaltyAccountId = value; }
		}

		public AccountStatus AccountStatusId
		{
			get { return _accountStatusId; }
			set { _accountStatusId = value; }
		}


		#endregion
		
	}

	#endregion
}



