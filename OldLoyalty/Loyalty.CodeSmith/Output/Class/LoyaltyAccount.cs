
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region LoyaltyAccount

	/// <summary>
	/// LoyaltyAccount object for NHibernate mapped table 'tbl_LoyaltyAccount'.
	/// </summary>
	public partial  class LoyaltyAccount
		{
		#region Member Variables
		
		protected int? _id;
		protected int? _points;
		protected DateTime? _dateTimeCreated;
		protected Site _siteId = new Site();
		
		
		protected IList _loyaltyAccountIdLoyaltyUsers;
		protected IList _loyaltyAccountIdLoyaltyCards;
		protected IList _loyaltyAccountIdAccountAddresses;
		protected IList _loyaltyAccountIdCompanyLoyaltyAccounts;
		protected IList _loyaltyAccountIdLoyaltyAccountStatuses;

		#endregion

		#region Constructors

		public LoyaltyAccount() { }

		public LoyaltyAccount( int? points, DateTime? dateTimeCreated, Site siteId )
		{
			this._points = points;
			this._dateTimeCreated = dateTimeCreated;
			this._siteId = siteId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Points
		{
			get { return _points; }
			set { _points = value; }
		}

		public DateTime? DateTimeCreated
		{
			get { return _dateTimeCreated; }
			set { _dateTimeCreated = value; }
		}

		public Site SiteId
		{
			get { return _siteId; }
			set { _siteId = value; }
		}

		public IList LoyaltyAccountIdLoyaltyUsers
		{
			get
			{
				if (_loyaltyAccountIdLoyaltyUsers==null)
				{
					_loyaltyAccountIdLoyaltyUsers = new ArrayList();
				}
				return _loyaltyAccountIdLoyaltyUsers;
			}
			set { _loyaltyAccountIdLoyaltyUsers = value; }
		}

		public IList LoyaltyAccountIdLoyaltyCards
		{
			get
			{
				if (_loyaltyAccountIdLoyaltyCards==null)
				{
					_loyaltyAccountIdLoyaltyCards = new ArrayList();
				}
				return _loyaltyAccountIdLoyaltyCards;
			}
			set { _loyaltyAccountIdLoyaltyCards = value; }
		}

		public IList LoyaltyAccountIdAccountAddresses
		{
			get
			{
				if (_loyaltyAccountIdAccountAddresses==null)
				{
					_loyaltyAccountIdAccountAddresses = new ArrayList();
				}
				return _loyaltyAccountIdAccountAddresses;
			}
			set { _loyaltyAccountIdAccountAddresses = value; }
		}

		public IList LoyaltyAccountIdCompanyLoyaltyAccounts
		{
			get
			{
				if (_loyaltyAccountIdCompanyLoyaltyAccounts==null)
				{
					_loyaltyAccountIdCompanyLoyaltyAccounts = new ArrayList();
				}
				return _loyaltyAccountIdCompanyLoyaltyAccounts;
			}
			set { _loyaltyAccountIdCompanyLoyaltyAccounts = value; }
		}

		public IList LoyaltyAccountIdLoyaltyAccountStatuses
		{
			get
			{
				if (_loyaltyAccountIdLoyaltyAccountStatuses==null)
				{
					_loyaltyAccountIdLoyaltyAccountStatuses = new ArrayList();
				}
				return _loyaltyAccountIdLoyaltyAccountStatuses;
			}
			set { _loyaltyAccountIdLoyaltyAccountStatuses = value; }
		}


		#endregion
		
	}

	#endregion
}



