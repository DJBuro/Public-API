
using Dashboard.Dao.NHibernate.Dao.Factory;


namespace Dashboard.Dao.NHibernate.Dao.Factory
{
    public class DashboardHibernateDAOFactory : AbstractHibernateDAOFactory{ 

            private DashboardDataDAO _DashboardDataDAO;
            private HeadOfficeDAO _HeadOfficeDAO;
            private IndicatorDefinitionDAO _IndicatorDefinitionDAO;
            private IndicatorTranslationDAO _IndicatorTranslationDAO;
            private InterfaceTextDAO _InterfaceTextDAO;
            private InterfaceTextLangaugeDAO _InterfaceTextLangaugeDAO;
            private LanguageDAO _LanguageDAO;
			private PermissionDAO _PermissionDAO;
            private PreviousDayDashboardDataDAO _PreviousDayDashboardDataDAO;
            private RegionDAO _RegionDAO;
            private SiteMessageDAO _SiteMessageDAO;
            private SiteDAO _SiteDAO;
            private SitesRegionDAO _SitesRegionDAO;
            private TradingTimeDAO _TradingTimeDAO;
			private UserDAO _UserDAO;
		

        public DashboardDataDAO DashboardDataDAO
        {
            get
            {
                if (_DashboardDataDAO == null) _DashboardDataDAO = new DashboardDataDAO(this.SessionFactory);
                return _DashboardDataDAO;
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
        public IndicatorDefinitionDAO IndicatorDefinitionDAO
        {
            get
            {
                if (_IndicatorDefinitionDAO == null) _IndicatorDefinitionDAO = new IndicatorDefinitionDAO(this.SessionFactory);
                return _IndicatorDefinitionDAO;
            }
        }
        public IndicatorTranslationDAO IndicatorTranslationDAO
        {
            get
            {
                if (_IndicatorTranslationDAO == null) _IndicatorTranslationDAO = new IndicatorTranslationDAO(this.SessionFactory);
                return _IndicatorTranslationDAO;
            }
        }
        public InterfaceTextDAO InterfaceTextDAO
        {
            get
            {
                if (_InterfaceTextDAO == null) _InterfaceTextDAO = new InterfaceTextDAO(this.SessionFactory);
                return _InterfaceTextDAO;
            }
        }
        public InterfaceTextLangaugeDAO InterfaceTextLangaugeDAO
        {
            get
            {
                if (_InterfaceTextLangaugeDAO == null) _InterfaceTextLangaugeDAO = new InterfaceTextLangaugeDAO(this.SessionFactory);
                return _InterfaceTextLangaugeDAO;
            }
        }
        public LanguageDAO LanguageDAO
        {
            get
            {
                if (_LanguageDAO == null) _LanguageDAO = new LanguageDAO(this.SessionFactory);
                return _LanguageDAO;
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
        public PreviousDayDashboardDataDAO PreviousDayDashboardDataDAO
        {
            get
            {
                if (_PreviousDayDashboardDataDAO == null) _PreviousDayDashboardDataDAO = new PreviousDayDashboardDataDAO(this.SessionFactory);
                return _PreviousDayDashboardDataDAO;
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
        public SiteMessageDAO SiteMessageDAO
        {
            get
            {
                if (_SiteMessageDAO == null) _SiteMessageDAO = new SiteMessageDAO(this.SessionFactory);
                return _SiteMessageDAO;
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
        public SitesRegionDAO SitesRegionDAO
        {
            get
            {
                if (_SitesRegionDAO == null) _SitesRegionDAO = new SitesRegionDAO(this.SessionFactory);
                return _SitesRegionDAO;
            }
        }
        public TradingTimeDAO TradingTimeDAO
        {
            get
            {
                if (_TradingTimeDAO == null) _TradingTimeDAO = new TradingTimeDAO(this.SessionFactory);
                return _TradingTimeDAO;
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
	}
}