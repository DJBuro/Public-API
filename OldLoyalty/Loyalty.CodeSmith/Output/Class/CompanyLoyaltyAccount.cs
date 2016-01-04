
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region CompanyLoyaltyAccount

	/// <summary>
	/// CompanyLoyaltyAccount object for NHibernate mapped table 'tbl_CompanyLoyaltyAccount'.
	/// </summary>
	public partial  class CompanyLoyaltyAccount
		{
		#region Member Variables
		
		protected int? _id;
		protected LoyaltyAccount _loyaltyAccountId = new LoyaltyAccount();
		protected Company _companyId = new Company();
		
		

		#endregion

		#region Constructors

		public CompanyLoyaltyAccount() { }

		public CompanyLoyaltyAccount( LoyaltyAccount loyaltyAccountId, Company companyId )
		{
			this._loyaltyAccountId = loyaltyAccountId;
			this._companyId = companyId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public LoyaltyAccount LoyaltyAccountId
		{
			get { return _loyaltyAccountId; }
			set { _loyaltyAccountId = value; }
		}

		public Company CompanyId
		{
			get { return _companyId; }
			set { _companyId = value; }
		}


		#endregion
		
	}

	#endregion
}



