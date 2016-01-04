
using System;
using System.Collections;
using System.Collections.Generic;
using Cacd.Core.Utils;
using Cacd.Domain.Directories;
using NHibernate;
using NHibernate.Expression;

namespace Cacd.DAO.NHibernate.Loyalty
{

		#region AccountAddressDAO

		/// <summary>
		/// AccountAddress object for NHibernate mapped table 'tbl_AccountAddress'.
		/// </summary>
		public  class AccountAddressDAO : GenericNHibernateDAO<AccountAddress,int?>
			{
			public AccountAddressDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region AccountStatusDAO

		/// <summary>
		/// AccountStatus object for NHibernate mapped table 'tbl_AccountStatus'.
		/// </summary>
		public  class AccountStatusDAO : GenericNHibernateDAO<AccountStatus,int?>
			{
			public AccountStatusDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CompanyDAO

		/// <summary>
		/// Company object for NHibernate mapped table 'tbl_Company'.
		/// </summary>
		public  class CompanyDAO : GenericNHibernateDAO<Company,int?>
			{
			public CompanyDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CompanyLoyaltyAccountDAO

		/// <summary>
		/// CompanyLoyaltyAccount object for NHibernate mapped table 'tbl_CompanyLoyaltyAccount'.
		/// </summary>
		public  class CompanyLoyaltyAccountDAO : GenericNHibernateDAO<CompanyLoyaltyAccount,int?>
			{
			public CompanyLoyaltyAccountDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CompanyUserTitleDAO

		/// <summary>
		/// CompanyUserTitle object for NHibernate mapped table 'tbl_CompanyUserTitles'.
		/// </summary>
		public  class CompanyUserTitleDAO : GenericNHibernateDAO<CompanyUserTitle,int?>
			{
			public CompanyUserTitleDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CountryDAO

		/// <summary>
		/// Country object for NHibernate mapped table 'tbl_Country'.
		/// </summary>
		public  class CountryDAO : GenericNHibernateDAO<Country,int?>
			{
			public CountryDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CountryCurrencyDAO

		/// <summary>
		/// CountryCurrency object for NHibernate mapped table 'tbl_CountryCurrency'.
		/// </summary>
		public  class CountryCurrencyDAO : GenericNHibernateDAO<CountryCurrency,int?>
			{
			public CountryCurrencyDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CurrencyDAO

		/// <summary>
		/// Currency object for NHibernate mapped table 'tbl_Currency'.
		/// </summary>
		public  class CurrencyDAO : GenericNHibernateDAO<Currency,int?>
			{
			public CurrencyDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region LoyaltyAccountDAO

		/// <summary>
		/// LoyaltyAccount object for NHibernate mapped table 'tbl_LoyaltyAccount'.
		/// </summary>
		public  class LoyaltyAccountDAO : GenericNHibernateDAO<LoyaltyAccount,int?>
			{
			public LoyaltyAccountDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region LoyaltyAccountStatusDAO

		/// <summary>
		/// LoyaltyAccountStatus object for NHibernate mapped table 'tbl_LoyaltyAccountStatus'.
		/// </summary>
		public  class LoyaltyAccountStatusDAO : GenericNHibernateDAO<LoyaltyAccountStatus,int?>
			{
			public LoyaltyAccountStatusDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region LoyaltyCardDAO

		/// <summary>
		/// LoyaltyCard object for NHibernate mapped table 'tbl_LoyaltyCard'.
		/// </summary>
		public  class LoyaltyCardDAO : GenericNHibernateDAO<LoyaltyCard,int?>
			{
			public LoyaltyCardDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region LoyaltyCardStatusDAO

		/// <summary>
		/// LoyaltyCardStatus object for NHibernate mapped table 'tbl_LoyaltyCardStatus'.
		/// </summary>
		public  class LoyaltyCardStatusDAO : GenericNHibernateDAO<LoyaltyCardStatus,int?>
			{
			public LoyaltyCardStatusDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region LoyaltyLogDAO

		/// <summary>
		/// LoyaltyLog object for NHibernate mapped table 'tbl_LoyaltyLog'.
		/// </summary>
		public  class LoyaltyLogDAO : GenericNHibernateDAO<LoyaltyLog,int?>
			{
			public LoyaltyLogDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region LoyaltyUserDAO

		/// <summary>
		/// LoyaltyUser object for NHibernate mapped table 'tbl_LoyaltyUser'.
		/// </summary>
		public  class LoyaltyUserDAO : GenericNHibernateDAO<LoyaltyUser,int?>
			{
			public LoyaltyUserDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region OrderItemHistoryDAO

		/// <summary>
		/// OrderItemHistory object for NHibernate mapped table 'tbl_OrderItemHistory'.
		/// </summary>
		public  class OrderItemHistoryDAO : GenericNHibernateDAO<OrderItemHistory,int?>
			{
			public OrderItemHistoryDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region OrderTypeDAO

		/// <summary>
		/// OrderType object for NHibernate mapped table 'tbl_OrderType'.
		/// </summary>
		public  class OrderTypeDAO : GenericNHibernateDAO<OrderType,int?>
			{
			public OrderTypeDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region RamesesAddressDAO

		/// <summary>
		/// RamesesAddress object for NHibernate mapped table 'tbl_RamesesAddress'.
		/// </summary>
		public  class RamesesAddressDAO : GenericNHibernateDAO<RamesesAddress,int?>
			{
			public RamesesAddressDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region RamesesAddressLoyaltyCardDAO

		/// <summary>
		/// RamesesAddressLoyaltyCard object for NHibernate mapped table 'tbl_RamesesAddressLoyaltyCard'.
		/// </summary>
		public  class RamesesAddressLoyaltyCardDAO : GenericNHibernateDAO<RamesesAddressLoyaltyCard,int?>
			{
			public RamesesAddressLoyaltyCardDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region SiteDAO

		/// <summary>
		/// Site object for NHibernate mapped table 'tbl_Site'.
		/// </summary>
		public  class SiteDAO : GenericNHibernateDAO<Site,int?>
			{
			public SiteDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region StatusDAO

		/// <summary>
		/// Status object for NHibernate mapped table 'tbl_Status'.
		/// </summary>
		public  class StatusDAO : GenericNHibernateDAO<Status,int?>
			{
			public StatusDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TransactionHistoryDAO

		/// <summary>
		/// TransactionHistory object for NHibernate mapped table 'tbl_TransactionHistory'.
		/// </summary>
		public  class TransactionHistoryDAO : GenericNHibernateDAO<TransactionHistory,int?>
			{
			public TransactionHistoryDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region UserTitleDAO

		/// <summary>
		/// UserTitle object for NHibernate mapped table 'tbl_UserTitle'.
		/// </summary>
		public  class UserTitleDAO : GenericNHibernateDAO<UserTitle,int?>
			{
			public UserTitleDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion

}





		

