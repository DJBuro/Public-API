
using Cacd.DAO.NHibernate.OrderTracking;

namespace Cacd.DAO.Factory
{
	public class OrderTrackingHibernateDAOFactory : AbstractHibernateDAOFactory{ 

			private AccountDAO _AccountDAO;
			private ApnDAO _ApnDAO;
			private CommandTypeDAO _CommandTypeDAO;
			private CoordinateDAO _CoordinateDAO;
			private CustomerDAO _CustomerDAO;
			private CustomerOrderDAO _CustomerOrderDAO;
			private DriverDAO _DriverDAO;
			private DriverOrderDAO _DriverOrderDAO;
			private ItemDAO _ItemDAO;
			private LogDAO _LogDAO;
			private OrderDAO _OrderDAO;
			private OrderStatusDAO _OrderStatusDAO;
			private PostCodeDAO _PostCodeDAO;
			private SmsCredentialDAO _SmsCredentialDAO;
			private StatusDAO _StatusDAO;
			private StoreDAO _StoreDAO;
			private TrackerDAO _TrackerDAO;
			private TrackerCommandDAO _TrackerCommandDAO;
			private TrackerStatusDAO _TrackerStatusDAO;
			private TrackerTypeDAO _TrackerTypeDAO;
		

			public AccountDAO AccountDAO
			{
				get
				{
					if (_AccountDAO == null) _AccountDAO = new AccountDAO(this.ConnectionDetails,this.SessionManager);
					return _AccountDAO;
				}
			}
			public ApnDAO ApnDAO
			{
				get
				{
					if (_ApnDAO == null) _ApnDAO = new ApnDAO(this.ConnectionDetails,this.SessionManager);
					return _ApnDAO;
				}
			}
			public CommandTypeDAO CommandTypeDAO
			{
				get
				{
					if (_CommandTypeDAO == null) _CommandTypeDAO = new CommandTypeDAO(this.ConnectionDetails,this.SessionManager);
					return _CommandTypeDAO;
				}
			}
			public CoordinateDAO CoordinateDAO
			{
				get
				{
					if (_CoordinateDAO == null) _CoordinateDAO = new CoordinateDAO(this.ConnectionDetails,this.SessionManager);
					return _CoordinateDAO;
				}
			}
			public CustomerDAO CustomerDAO
			{
				get
				{
					if (_CustomerDAO == null) _CustomerDAO = new CustomerDAO(this.ConnectionDetails,this.SessionManager);
					return _CustomerDAO;
				}
			}
			public CustomerOrderDAO CustomerOrderDAO
			{
				get
				{
					if (_CustomerOrderDAO == null) _CustomerOrderDAO = new CustomerOrderDAO(this.ConnectionDetails,this.SessionManager);
					return _CustomerOrderDAO;
				}
			}
			public DriverDAO DriverDAO
			{
				get
				{
					if (_DriverDAO == null) _DriverDAO = new DriverDAO(this.ConnectionDetails,this.SessionManager);
					return _DriverDAO;
				}
			}
			public DriverOrderDAO DriverOrderDAO
			{
				get
				{
					if (_DriverOrderDAO == null) _DriverOrderDAO = new DriverOrderDAO(this.ConnectionDetails,this.SessionManager);
					return _DriverOrderDAO;
				}
			}
			public ItemDAO ItemDAO
			{
				get
				{
					if (_ItemDAO == null) _ItemDAO = new ItemDAO(this.ConnectionDetails,this.SessionManager);
					return _ItemDAO;
				}
			}
			public LogDAO LogDAO
			{
				get
				{
					if (_LogDAO == null) _LogDAO = new LogDAO(this.ConnectionDetails,this.SessionManager);
					return _LogDAO;
				}
			}
			public OrderDAO OrderDAO
			{
				get
				{
					if (_OrderDAO == null) _OrderDAO = new OrderDAO(this.ConnectionDetails,this.SessionManager);
					return _OrderDAO;
				}
			}
			public OrderStatusDAO OrderStatusDAO
			{
				get
				{
					if (_OrderStatusDAO == null) _OrderStatusDAO = new OrderStatusDAO(this.ConnectionDetails,this.SessionManager);
					return _OrderStatusDAO;
				}
			}
			public PostCodeDAO PostCodeDAO
			{
				get
				{
					if (_PostCodeDAO == null) _PostCodeDAO = new PostCodeDAO(this.ConnectionDetails,this.SessionManager);
					return _PostCodeDAO;
				}
			}
			public SmsCredentialDAO SmsCredentialDAO
			{
				get
				{
					if (_SmsCredentialDAO == null) _SmsCredentialDAO = new SmsCredentialDAO(this.ConnectionDetails,this.SessionManager);
					return _SmsCredentialDAO;
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
			public StoreDAO StoreDAO
			{
				get
				{
					if (_StoreDAO == null) _StoreDAO = new StoreDAO(this.ConnectionDetails,this.SessionManager);
					return _StoreDAO;
				}
			}
			public TrackerDAO TrackerDAO
			{
				get
				{
					if (_TrackerDAO == null) _TrackerDAO = new TrackerDAO(this.ConnectionDetails,this.SessionManager);
					return _TrackerDAO;
				}
			}
			public TrackerCommandDAO TrackerCommandDAO
			{
				get
				{
					if (_TrackerCommandDAO == null) _TrackerCommandDAO = new TrackerCommandDAO(this.ConnectionDetails,this.SessionManager);
					return _TrackerCommandDAO;
				}
			}
			public TrackerStatusDAO TrackerStatusDAO
			{
				get
				{
					if (_TrackerStatusDAO == null) _TrackerStatusDAO = new TrackerStatusDAO(this.ConnectionDetails,this.SessionManager);
					return _TrackerStatusDAO;
				}
			}
			public TrackerTypeDAO TrackerTypeDAO
			{
				get
				{
					if (_TrackerTypeDAO == null) _TrackerTypeDAO = new TrackerTypeDAO(this.ConnectionDetails,this.SessionManager);
					return _TrackerTypeDAO;
				}
			}
	}
}
		




