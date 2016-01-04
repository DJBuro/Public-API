
using Cacd.DAO.NHibernate.Loyalty;

namespace Cacd.DAO.Factory
{
	public class LoyaltyHibernateDAOFactory : AbstractHibernateDAOFactory{ 

			private AccountAddressDAO _AccountAddressDAO;
			private AccountStatusDAO _AccountStatusDAO;
			private CompanyDAO _CompanyDAO;
			private CompanyLoyaltyAccountDAO _CompanyLoyaltyAccountDAO;
			private CompanyUserTitleDAO _CompanyUserTitleDAO;
			private CountryDAO _CountryDAO;
			private CountryCurrencyDAO _CountryCurrencyDAO;
			private CurrencyDAO _CurrencyDAO;
			private LoyaltyAccountDAO _LoyaltyAccountDAO;
			private LoyaltyAccountStatusDAO _LoyaltyAccountStatusDAO;
			private LoyaltyCardDAO _LoyaltyCardDAO;
			private LoyaltyCardStatusDAO _LoyaltyCardStatusDAO;
			private LoyaltyLogDAO _LoyaltyLogDAO;
			private LoyaltyUserDAO _LoyaltyUserDAO;
			private OrderItemHistoryDAO _OrderItemHistoryDAO;
			private OrderTypeDAO _OrderTypeDAO;
			private RamesesAddressDAO _RamesesAddressDAO;
			private RamesesAddressLoyaltyCardDAO _RamesesAddressLoyaltyCardDAO;
			private SiteDAO _SiteDAO;
			private StatusDAO _StatusDAO;
			private TransactionHistoryDAO _TransactionHistoryDAO;
			private UserTitleDAO _UserTitleDAO;
		

			public AccountAddressDAO AccountAddressDAO
			{
				get
				{
					if (_AccountAddressDAO == null) _AccountAddressDAO = new AccountAddressDAO(this.ConnectionDetails,this.SessionManager);
					return _AccountAddressDAO;
				}
			}
			public AccountStatusDAO AccountStatusDAO
			{
				get
				{
					if (_AccountStatusDAO == null) _AccountStatusDAO = new AccountStatusDAO(this.ConnectionDetails,this.SessionManager);
					return _AccountStatusDAO;
				}
			}
			public CompanyDAO CompanyDAO
			{
				get
				{
					if (_CompanyDAO == null) _CompanyDAO = new CompanyDAO(this.ConnectionDetails,this.SessionManager);
					return _CompanyDAO;
				}
			}
			public CompanyLoyaltyAccountDAO CompanyLoyaltyAccountDAO
			{
				get
				{
					if (_CompanyLoyaltyAccountDAO == null) _CompanyLoyaltyAccountDAO = new CompanyLoyaltyAccountDAO(this.ConnectionDetails,this.SessionManager);
					return _CompanyLoyaltyAccountDAO;
				}
			}
			public CompanyUserTitleDAO CompanyUserTitleDAO
			{
				get
				{
					if (_CompanyUserTitleDAO == null) _CompanyUserTitleDAO = new CompanyUserTitleDAO(this.ConnectionDetails,this.SessionManager);
					return _CompanyUserTitleDAO;
				}
			}
			public CountryDAO CountryDAO
			{
				get
				{
					if (_CountryDAO == null) _CountryDAO = new CountryDAO(this.ConnectionDetails,this.SessionManager);
					return _CountryDAO;
				}
			}
			public CountryCurrencyDAO CountryCurrencyDAO
			{
				get
				{
					if (_CountryCurrencyDAO == null) _CountryCurrencyDAO = new CountryCurrencyDAO(this.ConnectionDetails,this.SessionManager);
					return _CountryCurrencyDAO;
				}
			}
			public CurrencyDAO CurrencyDAO
			{
				get
				{
					if (_CurrencyDAO == null) _CurrencyDAO = new CurrencyDAO(this.ConnectionDetails,this.SessionManager);
					return _CurrencyDAO;
				}
			}
			public LoyaltyAccountDAO LoyaltyAccountDAO
			{
				get
				{
					if (_LoyaltyAccountDAO == null) _LoyaltyAccountDAO = new LoyaltyAccountDAO(this.ConnectionDetails,this.SessionManager);
					return _LoyaltyAccountDAO;
				}
			}
			public LoyaltyAccountStatusDAO LoyaltyAccountStatusDAO
			{
				get
				{
					if (_LoyaltyAccountStatusDAO == null) _LoyaltyAccountStatusDAO = new LoyaltyAccountStatusDAO(this.ConnectionDetails,this.SessionManager);
					return _LoyaltyAccountStatusDAO;
				}
			}
			public LoyaltyCardDAO LoyaltyCardDAO
			{
				get
				{
					if (_LoyaltyCardDAO == null) _LoyaltyCardDAO = new LoyaltyCardDAO(this.ConnectionDetails,this.SessionManager);
					return _LoyaltyCardDAO;
				}
			}
			public LoyaltyCardStatusDAO LoyaltyCardStatusDAO
			{
				get
				{
					if (_LoyaltyCardStatusDAO == null) _LoyaltyCardStatusDAO = new LoyaltyCardStatusDAO(this.ConnectionDetails,this.SessionManager);
					return _LoyaltyCardStatusDAO;
				}
			}
			public LoyaltyLogDAO LoyaltyLogDAO
			{
				get
				{
					if (_LoyaltyLogDAO == null) _LoyaltyLogDAO = new LoyaltyLogDAO(this.ConnectionDetails,this.SessionManager);
					return _LoyaltyLogDAO;
				}
			}
			public LoyaltyUserDAO LoyaltyUserDAO
			{
				get
				{
					if (_LoyaltyUserDAO == null) _LoyaltyUserDAO = new LoyaltyUserDAO(this.ConnectionDetails,this.SessionManager);
					return _LoyaltyUserDAO;
				}
			}
			public OrderItemHistoryDAO OrderItemHistoryDAO
			{
				get
				{
					if (_OrderItemHistoryDAO == null) _OrderItemHistoryDAO = new OrderItemHistoryDAO(this.ConnectionDetails,this.SessionManager);
					return _OrderItemHistoryDAO;
				}
			}
			public OrderTypeDAO OrderTypeDAO
			{
				get
				{
					if (_OrderTypeDAO == null) _OrderTypeDAO = new OrderTypeDAO(this.ConnectionDetails,this.SessionManager);
					return _OrderTypeDAO;
				}
			}
			public RamesesAddressDAO RamesesAddressDAO
			{
				get
				{
					if (_RamesesAddressDAO == null) _RamesesAddressDAO = new RamesesAddressDAO(this.ConnectionDetails,this.SessionManager);
					return _RamesesAddressDAO;
				}
			}
			public RamesesAddressLoyaltyCardDAO RamesesAddressLoyaltyCardDAO
			{
				get
				{
					if (_RamesesAddressLoyaltyCardDAO == null) _RamesesAddressLoyaltyCardDAO = new RamesesAddressLoyaltyCardDAO(this.ConnectionDetails,this.SessionManager);
					return _RamesesAddressLoyaltyCardDAO;
				}
			}
			public SiteDAO SiteDAO
			{
				get
				{
					if (_SiteDAO == null) _SiteDAO = new SiteDAO(this.ConnectionDetails,this.SessionManager);
					return _SiteDAO;
				}
			}
			public StatusDAO StatusDAO
			{
				get
				{
					if (_StatusDAO == null) _StatusDAO = new StatusDAO(this.ConnectionDetails,this.SessionManager);
					return _StatusDAO;
				}
			}
			public TransactionHistoryDAO TransactionHistoryDAO
			{
				get
				{
					if (_TransactionHistoryDAO == null) _TransactionHistoryDAO = new TransactionHistoryDAO(this.ConnectionDetails,this.SessionManager);
					return _TransactionHistoryDAO;
				}
			}
			public UserTitleDAO UserTitleDAO
			{
				get
				{
					if (_UserTitleDAO == null) _UserTitleDAO = new UserTitleDAO(this.ConnectionDetails,this.SessionManager);
					return _UserTitleDAO;
				}
			}
	}
}
		




