
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region LoyaltyCardStatus

	/// <summary>
	/// LoyaltyCardStatus object for NHibernate mapped table 'tbl_LoyaltyCardStatus'.
	/// </summary>
	public partial  class LoyaltyCardStatus
		{
		#region Member Variables
		
		protected int? _id;
		protected LoyaltyCard _loyaltyCardId = new LoyaltyCard();
		protected Status _statusId = new Status();
		
		

		#endregion

		#region Constructors

		public LoyaltyCardStatus() { }

		public LoyaltyCardStatus( LoyaltyCard loyaltyCardId, Status statusId )
		{
			this._loyaltyCardId = loyaltyCardId;
			this._statusId = statusId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public LoyaltyCard LoyaltyCardId
		{
			get { return _loyaltyCardId; }
			set { _loyaltyCardId = value; }
		}

		public Status StatusId
		{
			get { return _statusId; }
			set { _statusId = value; }
		}


		#endregion
		
	}

	#endregion
}



