
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region RamesesAddressLoyaltyCard

	/// <summary>
	/// RamesesAddressLoyaltyCard object for NHibernate mapped table 'tbl_RamesesAddressLoyaltyCard'.
	/// </summary>
	public partial  class RamesesAddressLoyaltyCard
		{
		#region Member Variables
		
		protected int? _id;
		protected LoyaltyCard _loyaltyCardId = new LoyaltyCard();
		protected RamesesAddress _ramesesAddressId = new RamesesAddress();
		
		

		#endregion

		#region Constructors

		public RamesesAddressLoyaltyCard() { }

		public RamesesAddressLoyaltyCard( LoyaltyCard loyaltyCardId, RamesesAddress ramesesAddressId )
		{
			this._loyaltyCardId = loyaltyCardId;
			this._ramesesAddressId = ramesesAddressId;
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

		public RamesesAddress RamesesAddressId
		{
			get { return _ramesesAddressId; }
			set { _ramesesAddressId = value; }
		}


		#endregion
		
	}

	#endregion
}



