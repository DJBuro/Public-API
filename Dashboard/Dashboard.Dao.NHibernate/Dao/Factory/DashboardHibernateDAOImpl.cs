
using System;
using System.Collections;
using Dashboard.Dao.Domain;

using NHibISessionFactory = NHibernate.ISessionFactory;

namespace Dashboard.Dao.NHibernate.Dao.Factory
{

		#region DashboardDataDAO

		/// <summary>
		/// DashboardData object for NHibernate mapped table 'DashboardData'.
		/// </summary>
		public  class DashboardDataDAO : GenericNHibernateDAO<DashboardData,int?>
			{
            public DashboardDataDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region HeadOfficeDAO

		/// <summary>
		/// HeadOffice object for NHibernate mapped table 'HeadOffice'.
		/// </summary>
		public  class HeadOfficeDAO : GenericNHibernateDAO<HeadOffice,int?>
			{
            public HeadOfficeDAO(NHibISessionFactory _SessionFactory)
                : base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region IndicatorDefinitionDAO

		/// <summary>
		/// IndicatorDefinition object for NHibernate mapped table 'IndicatorDefinitions'.
		/// </summary>
		public  class IndicatorDefinitionDAO : GenericNHibernateDAO<IndicatorDefinition,int?>
			{
			public IndicatorDefinitionDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region IndicatorTranslationDAO

		/// <summary>
		/// IndicatorTranslation object for NHibernate mapped table 'IndicatorTranslations'.
		/// </summary>
		public  class IndicatorTranslationDAO : GenericNHibernateDAO<IndicatorTranslation,int?>
			{
			public IndicatorTranslationDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region InterfaceTextDAO

		/// <summary>
		/// InterfaceText object for NHibernate mapped table 'InterfaceText'.
		/// </summary>
		public  class InterfaceTextDAO : GenericNHibernateDAO<InterfaceText,int?>
			{
			public InterfaceTextDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region InterfaceTextLangaugeDAO

		/// <summary>
		/// InterfaceTextLangauge object for NHibernate mapped table 'InterfaceTextLangauge'.
		/// </summary>
		public  class InterfaceTextLangaugeDAO : GenericNHibernateDAO<InterfaceTextLangauge,int?>
			{
			public InterfaceTextLangaugeDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region LanguageDAO

		/// <summary>
		/// Language object for NHibernate mapped table 'Language'.
		/// </summary>
		public  class LanguageDAO : GenericNHibernateDAO<Language,int?>
			{
			public LanguageDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region PermissionDAO

		/// <summary>
		/// Permission object for NHibernate mapped table 'Permissions'.
		/// </summary>
		public  class PermissionDAO : GenericNHibernateDAO<Permission,int?>
			{
                public PermissionDAO(NHibISessionFactory _SessionFactory)
                    : base(_SessionFactory)
                {
                }
            }
	
		#endregion
		#region PreviousDayDashboardDataDAO

		/// <summary>
		/// PreviousDayDashboardData object for NHibernate mapped table 'PreviousDayDashboardData'.
		/// </summary>
		public  class PreviousDayDashboardDataDAO : GenericNHibernateDAO<PreviousDayDashboardData,int?>
			{
			public PreviousDayDashboardDataDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region RegionDAO

		/// <summary>
		/// Region object for NHibernate mapped table 'Regions'.
		/// </summary>
		public  class RegionDAO : GenericNHibernateDAO<Region,int?>
			{
			public RegionDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region SiteMessageDAO

		/// <summary>
		/// SiteMessage object for NHibernate mapped table 'SiteMessages'.
		/// </summary>
		public  class SiteMessageDAO : GenericNHibernateDAO<SiteMessage,int?>
			{
			public SiteMessageDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region SiteDAO

		/// <summary>
		/// Site object for NHibernate mapped table 'Sites'.
		/// </summary>
		public  class SiteDAO : GenericNHibernateDAO<Site,int?>
			{
			public SiteDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region SitesRegionDAO

		/// <summary>
		/// SitesRegion object for NHibernate mapped table 'SitesRegions'.
		/// </summary>
		public  class SitesRegionDAO : GenericNHibernateDAO<SitesRegion,int?>
			{
			public SitesRegionDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region TradingTimeDAO

		/// <summary>
		/// TradingTime object for NHibernate mapped table 'TradingTimes'.
		/// </summary>
		public  class TradingTimeDAO : GenericNHibernateDAO<TradingTime,int?>
			{
			public TradingTimeDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion
		#region UserDAO

		/// <summary>
		/// User object for NHibernate mapped table 'Users'.
		/// </summary>
		public  class UserDAO : GenericNHibernateDAO<User,int?>
			{
			public UserDAO(NHibISessionFactory _SessionFactory): base(_SessionFactory)
			{
			}
		}
	
		#endregion

}





		

