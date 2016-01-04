namespace OrderTracking.Dao.NHibernate.Dao.Factory
{
    public class OrderTrackingHibernateDAOFactory : AbstractHibernateDAOFactory
    { 

		private AccountDAO _AccountDAO;
		private ApnDAO _ApnDAO;
		private CommandTypeDAO _CommandTypeDAO;
		private CoordinateDAO _CoordinateDAO;
		private CustomerDAO _CustomerDAO;
        private CustomerOrderDAO _CustomerOrderDAO;
        private ClientDAO _ClientDAO;
        private ClientStoreDAO _ClientStoreDAO;
        private ClientWebsiteCustomContentDAO _ClientWebsiteCustomContentDAO;
		private DriverDAO _DriverDAO;
		private DriverOrderDAO _DriverOrderDAO;
		private ItemDAO _ItemDAO;
		private LogDAO _LogDAO;
		private OrderDAO _OrderDAO;
		private OrderStatusDAO _OrderStatusDAO;
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
                if (_AccountDAO == null) _AccountDAO = new AccountDAO(this.SessionFactory);
				return _AccountDAO;
			}
		}
		public ApnDAO ApnDAO
		{
			get
			{
				if (_ApnDAO == null) _ApnDAO = new ApnDAO(this.SessionFactory);
				return _ApnDAO;
			}
		}
		public CommandTypeDAO CommandTypeDAO
		{
			get
			{
				if (_CommandTypeDAO == null) _CommandTypeDAO = new CommandTypeDAO(this.SessionFactory);
				return _CommandTypeDAO;
			}
		}
		public CoordinateDAO CoordinateDAO
		{
			get
			{
                if (_CoordinateDAO == null) _CoordinateDAO = new CoordinateDAO(this.SessionFactory);
				return _CoordinateDAO;
			}
		}

        public CustomerDAO CustomerDAO
        {
            get
            {
                if (_CustomerDAO == null) _CustomerDAO = new CustomerDAO(this.SessionFactory);
                return _CustomerDAO;
            }
        }
        public CustomerOrderDAO CustomerOrderDAO
        {
            get
            {
                if (_CustomerOrderDAO == null) _CustomerOrderDAO = new CustomerOrderDAO(this.SessionFactory);
                return _CustomerOrderDAO;
            }
        }
        public ClientDAO ClientDAO
        {
            get
            {
                if (_ClientDAO == null) _ClientDAO = new ClientDAO(this.SessionFactory);
                return _ClientDAO;
            }
        }
        public ClientStoreDAO ClientStoreDAO
        {
            get
            {
                if (_ClientStoreDAO == null) _ClientStoreDAO = new ClientStoreDAO(this.SessionFactory);
                return _ClientStoreDAO;
            }
        }
        public ClientWebsiteCustomContentDAO ClientWebsiteCustomContentDAO
        {
            get
            {
                if (_ClientWebsiteCustomContentDAO == null) _ClientWebsiteCustomContentDAO = new ClientWebsiteCustomContentDAO(this.SessionFactory);
                return _ClientWebsiteCustomContentDAO;
            }
        }
        public DriverDAO DriverDAO
        {
            get
            {
                if (_DriverDAO == null) _DriverDAO = new DriverDAO(this.SessionFactory);
                return _DriverDAO;
            }
        }
        public DriverOrderDAO DriverOrderDAO
        {
            get
            {
                if (_DriverOrderDAO == null) _DriverOrderDAO = new DriverOrderDAO(this.SessionFactory);
                return _DriverOrderDAO;
            }
        }
        public ItemDAO ItemDAO
        {
            get
            {
                if (_ItemDAO == null) _ItemDAO = new ItemDAO(this.SessionFactory);
                return _ItemDAO;
            }
        }
        public LogDAO LogDAO
        {
            get
            {
                if (_LogDAO == null) _LogDAO = new LogDAO(this.SessionFactory);
                return _LogDAO;
            }
        }
        public OrderDAO OrderDAO
        {
            get
            {
                if (_OrderDAO == null) _OrderDAO = new OrderDAO(this.SessionFactory);
                return _OrderDAO;
            }
        }
        public OrderStatusDAO OrderStatusDAO
        {
            get
            {
                if (_OrderStatusDAO == null) _OrderStatusDAO = new OrderStatusDAO(this.SessionFactory);
                return _OrderStatusDAO;
            }
        }
			public SmsCredentialDAO SmsCredentialDAO
			{
				get
				{
					if (_SmsCredentialDAO == null) _SmsCredentialDAO = new SmsCredentialDAO(this.SessionFactory);
					return _SmsCredentialDAO;
				}
			}
        public StatusDAO StatusDAO
        {
            get
            {
                if (_StatusDAO == null) _StatusDAO = new StatusDAO(this.SessionFactory);
                return _StatusDAO;
            }
        }
        public StoreDAO StoreDAO
        {
            get
            {
                if (_StoreDAO == null) _StoreDAO = new StoreDAO(this.SessionFactory);
                return _StoreDAO;
            }
        }

        public TrackerDAO TrackerDAO
        {
            get
            {
                if (_TrackerDAO == null) _TrackerDAO = new TrackerDAO(this.SessionFactory);
                return _TrackerDAO;
            }
        }
			public TrackerCommandDAO TrackerCommandDAO
			{
				get
				{
					if (_TrackerCommandDAO == null) _TrackerCommandDAO = new TrackerCommandDAO(this.SessionFactory);
					return _TrackerCommandDAO;
				}
			}
			public TrackerStatusDAO TrackerStatusDAO
			{
				get
				{
                    if (_TrackerStatusDAO == null) _TrackerStatusDAO = new TrackerStatusDAO(this.SessionFactory);
					return _TrackerStatusDAO;
				}
			}
			public TrackerTypeDAO TrackerTypeDAO
			{
				get
				{
					if (_TrackerTypeDAO == null) _TrackerTypeDAO = new TrackerTypeDAO(this.SessionFactory);
					return _TrackerTypeDAO;
				}
			}
	}
}