using OrderTracking.Dao.Domain;
using NHibISessionFactory = NHibernate.ISessionFactory;

namespace OrderTracking.Dao.NHibernate.Dao.Factory
{
	#region AccountDAO
	/// <summary>
	/// Account object for NHibernate mapped table 'tbl_Account'.
	/// </summary>
	public class AccountDAO : GenericNHibernateDAO<Account,long?>
	{
        public AccountDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
		{
		}
	}

	#endregion
	#region ApnDAO

	/// <summary>
	/// Apn object for NHibernate mapped table 'tbl_Apn'.
	/// </summary>
	public class ApnDAO : GenericNHibernateDAO<Apn,long?>
	{
        public ApnDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
		{
		}
	}

	#endregion
	#region CommandTypeDAO

	/// <summary>
	/// CommandType object for NHibernate mapped table 'tbl_CommandType'.
	/// </summary>
	public class CommandTypeDAO : GenericNHibernateDAO<CommandType,long?>
	{
        public CommandTypeDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
		{
		}
	}

	#endregion
	#region CoordinateDAO

	/// <summary>
	/// Coordinates object for NHibernate mapped table 'tbl_Coordinates'.
	/// </summary>
	public class CoordinateDAO : GenericNHibernateDAO<Coordinates,long?>
	{
        public CoordinateDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
		{
		}
	}

	#endregion
	#region CustomerDAO

	/// <summary>
	/// Customer object for NHibernate mapped table 'tbl_Customer'.
	/// </summary>
	public class CustomerDAO : GenericNHibernateDAO<Customer,long?>
    {
        public CustomerDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }
	
	#endregion
	#region CustomerOrderDAO

    /// <summary>
    /// CustomerOrder object for NHibernate mapped table 'tbl_CustomerOrders'.
    /// </summary>
	public class CustomerOrderDAO : GenericNHibernateDAO<CustomerOrder,long?>
    {
        public CustomerOrderDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    #endregion
    #region ClientDAO

    /// <summary>
    /// Client object for NHibernate mapped table 'Client'.
    /// </summary>
    public class ClientDAO : GenericNHibernateDAO<Client, long?>
    {
        public ClientDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    #endregion
    #region ClientStoreDAO

    /// <summary>
    /// ClientStore object for NHibernate mapped table 'ClientStore'.
    /// </summary>
    public class ClientStoreDAO : GenericNHibernateDAO<ClientStore, long?>
    {
        public ClientStoreDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    #endregion
    #region ClientWebsiteCustomContentDAO

    /// <summary>
    /// ClientWebsiteCustomContent object for NHibernate mapped table 'ClientWebsiteCustomContent'.
    /// </summary>
    public class ClientWebsiteCustomContentDAO : GenericNHibernateDAO<ClientWebsiteCustomContent, long?>
    {
        public ClientWebsiteCustomContentDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    #endregion
    #region DriverDAO

    /// <summary>
    /// Driver object for NHibernate mapped table 'tbl_Driver'.
    /// </summary>
    public class DriverDAO : GenericNHibernateDAO<Driver,long?>
    {
        public DriverDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }
    #endregion
    #region DriverOrderDAO

    /// <summary>
    /// DriverOrder object for NHibernate mapped table 'tbl_DriverOrders'.
    /// </summary>
    public class DriverOrderDAO : GenericNHibernateDAO<DriverOrder,long?>
    {
        public DriverOrderDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }
    #endregion
    #region ItemDAO

    /// <summary>
    /// Item object for NHibernate mapped table 'tbl_Item'.
    /// </summary>
    public class ItemDAO : GenericNHibernateDAO<Item,long?>
    {
        public ItemDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    #endregion
    #region LogDAO

    /// <summary>
    /// Log object for NHibernate mapped table 'tbl_Log'.
    /// </summary>
    public class LogDAO : GenericNHibernateDAO<Log,long?>
    {
        public LogDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    #endregion
    #region OrderDAO

    /// <summary>
    /// Order object for NHibernate mapped table 'tbl_Order'.
    /// </summary>
    public class OrderDAO : GenericNHibernateDAO<Order,long?>
    {
        public OrderDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    #endregion
    #region OrderStatusDAO

    /// <summary>
    /// OrderStatus object for NHibernate mapped table 'tbl_OrderStatus'.
    /// </summary>
    public class OrderStatusDAO : GenericNHibernateDAO<OrderStatus,long?>
	{
        public OrderStatusDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
	    {
	    }
    }

    #endregion
    #region SmsCredentialDAO

    /// <summary>
    /// SmsCredential object for NHibernate mapped table 'tbl_SmsCredentials'.
    /// </summary>
    public class SmsCredentialDAO : GenericNHibernateDAO<SmsCredential,long?>
	{
        public SmsCredentialDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
	    {
	    }
    }

    #endregion
    #region StatusDAO

    /// <summary>
    /// Status object for NHibernate mapped table 'tbl_Status'.
    /// </summary>
    public class StatusDAO : GenericNHibernateDAO<Status,long?>
    {
        public StatusDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    #endregion
    #region StoreDAO

    /// <summary>
    /// Store object for NHibernate mapped table 'tbl_Store'.
    /// </summary>
    public class StoreDAO : GenericNHibernateDAO<Store,long?>
	{
        public StoreDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
	    }
    }

    #endregion
    #region TrackerDAO

    /// <summary>
    /// Tracker object for NHibernate mapped table 'tbl_Tracker'.
    /// </summary>
    public class TrackerDAO : GenericNHibernateDAO<Tracker,long?>
	{
        public TrackerDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
	    {
	    }
    }

    #endregion
    #region TrackerCommandDAO

    /// <summary>
    /// TrackerCommand object for NHibernate mapped table 'tbl_TrackerCommands'.
    /// </summary>
    public class TrackerCommandDAO : GenericNHibernateDAO<TrackerCommand,long?>
	{
        public TrackerCommandDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
	    {
	    }
    }

    #endregion
    #region TrackerStatusDAO

    /// <summary>
    /// TrackerStatus object for NHibernate mapped table 'tbl_TrackerStatus'.
    /// </summary>
    public class TrackerStatusDAO : GenericNHibernateDAO<TrackerStatus,long?>
	{
        public TrackerStatusDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
	    {
	    }
    }

    #endregion
    #region TrackerTypeDAO

    /// <summary>
    /// TrackerType object for NHibernate mapped table 'tbl_TrackerType'.
    /// </summary>
    public class TrackerTypeDAO : GenericNHibernateDAO<TrackerType,long?>
	{
        public TrackerTypeDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
	    {
	    }
    }

    #endregion
}