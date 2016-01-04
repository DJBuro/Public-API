using WebDashboard.Dao.NHibernate.Dao.Factory;

namespace WebDashboard.DAO.Factory
{
	public class WebDashboardHibernateDAOFactory : AbstractHibernateDAOFactory
	{ 
		private DefinitionDAO _DefinitionDAO;
        private DivisorTypeDAO _DivisorTypeDAO;
		private HeadOfficeDAO _HeadOfficeDAO;
		private HistoricalDataDAO _HistoricalDataDAO;
		private IndicatorDAO _IndicatorDAO;
		private IndicatorTypeDAO _IndicatorTypeDAO;
		private PermissionDAO _PermissionDAO;
		private RegionDAO _RegionDAO;
		private SiteDAO _SiteDAO;
		private SiteTypeDAO _SiteTypeDAO;
		private UserDAO _UserDAO;
		private ValueTypeDAO _ValueTypeDAO;
        private UserRegionDAO _UserRegionDAO;
		private LogDAO _LogDAO;
		private GroupExchangeRateDAO _GroupExchangeRateDAO;        

		public DefinitionDAO DefinitionDAO
		{
			get
			{
                if (_DefinitionDAO == null) _DefinitionDAO = new DefinitionDAO(this.SessionFactory);
				    return _DefinitionDAO;
			}
		}
        public DivisorTypeDAO DivisorTypeDAO
		{
			get
			{
                if (_DivisorTypeDAO == null) _DivisorTypeDAO = new DivisorTypeDAO(this.SessionFactory);
                    return _DivisorTypeDAO;
			}
		}
		public HeadOfficeDAO HeadOfficeDAO
		{
			get
			{
                if (_HeadOfficeDAO == null) _HeadOfficeDAO = new HeadOfficeDAO(this.SessionFactory);
				    return _HeadOfficeDAO;
			}
		}

		public HistoricalDataDAO HistoricalDataDAO
		{
			get
			{
                if (_HistoricalDataDAO == null) _HistoricalDataDAO = new HistoricalDataDAO(this.SessionFactory);
				    return _HistoricalDataDAO;
			}
		}
		public IndicatorDAO IndicatorDAO
		{
			get
			{
                if (_IndicatorDAO == null) _IndicatorDAO = new IndicatorDAO(this.SessionFactory);
				    return _IndicatorDAO;
			}
		}
		public IndicatorTypeDAO IndicatorTypeDAO
		{
			get
			{
                if (_IndicatorTypeDAO == null) _IndicatorTypeDAO = new IndicatorTypeDAO(this.SessionFactory);
				    return _IndicatorTypeDAO;
			}
		}
		public PermissionDAO PermissionDAO
		{
			get
			{
                if (_PermissionDAO == null) _PermissionDAO = new PermissionDAO(this.SessionFactory);
				    return _PermissionDAO;
			}
		}
		public RegionDAO RegionDAO
		{
			get
			{
                if (_RegionDAO == null) _RegionDAO = new RegionDAO(this.SessionFactory);
				    return _RegionDAO;
			}
		}
		public SiteDAO SiteDAO
		{
			get
			{
                if (_SiteDAO == null) _SiteDAO = new SiteDAO(this.SessionFactory);
				    return _SiteDAO;
			}
		}
		public SiteTypeDAO SiteTypeDAO
		{
			get
			{
                if (_SiteTypeDAO == null) _SiteTypeDAO = new SiteTypeDAO(this.SessionFactory);
				    return _SiteTypeDAO;
			}
		}
	
		public UserDAO UserDAO
		{
			get
			{
                if (_UserDAO == null) _UserDAO = new UserDAO(this.SessionFactory);
				    return _UserDAO;
			}
		}
		public ValueTypeDAO ValueTypeDAO
		{
			get
			{
                if (_ValueTypeDAO == null) _ValueTypeDAO = new ValueTypeDAO(this.SessionFactory);
				    return _ValueTypeDAO;
			}
		}
        public UserRegionDAO UserRegionDAO
        {
            get
            {
                if (_UserRegionDAO == null) _UserRegionDAO = new UserRegionDAO(this.SessionFactory);
                    return _UserRegionDAO;
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
		
		public GroupExchangeRateDAO GroupExchangeRateDAO
		{
            get
            {
                if (_GroupExchangeRateDAO == null) 
                {
                    _GroupExchangeRateDAO = new GroupExchangeRateDAO(this.SessionFactory);
                }
                
                return _GroupExchangeRateDAO;
            }
		}
	}
}
		




