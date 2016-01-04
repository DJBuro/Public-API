using System;
using WebDashboard.Dao.Domain;
using NHibISessionFactory = NHibernate.ISessionFactory;
using ValueType=WebDashboard.Dao.Domain.ValueType;

namespace WebDashboard.Dao.NHibernate.Dao.Factory
{
    /// <summary>
    /// Definition object for NHibernate mapped table 'tbl_Definition'.
    /// </summary>
    public  class DefinitionDAO : GenericNHibernateDAO<Definition,int?>
    {
        public DefinitionDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }    
    
    /// <summary>
    /// Definition object for NHibernate mapped table 'tbl_DivisorType'.
    /// </summary>
    public  class DivisorTypeDAO : GenericNHibernateDAO<DivisorType,int?>
    {
        public DivisorTypeDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    /// <summary>
    /// HeadOffice object for NHibernate mapped table 'tbl_HeadOffice'.
    /// </summary>
    public  class HeadOfficeDAO : GenericNHibernateDAO<HeadOffice,int?>
    {
        public HeadOfficeDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

		#region HistoricalDataDAO

		/// <summary>
		/// HistoricalData object for NHibernate mapped table 'tbl_HistoricalData'.
		/// </summary>
		public  class HistoricalDataDAO : GenericNHibernateDAO<HistoricalData,int?>
			{
			public HistoricalDataDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
            {
			}
		}
	
		#endregion
		#region IndicatorDAO

		/// <summary>
		/// Indicator object for NHibernate mapped table 'tbl_Indicator'.
		/// </summary>
		public  class IndicatorDAO : GenericNHibernateDAO<Indicator,int?>
			{
            public IndicatorDAO(NHibISessionFactory _SessionFactory)
                : base(_SessionFactory)
            {
			}
		}
	
		#endregion

    /// <summary>
    /// IndicatorType object for NHibernate mapped table 'tbl_IndicatorType'.
    /// </summary>
    public  class IndicatorTypeDAO : GenericNHibernateDAO<IndicatorType,int?>
    {
        public IndicatorTypeDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    /// <summary>
    /// Permission object for NHibernate mapped table 'tbl_Permissions'.
    /// </summary>
    public  class PermissionDAO : GenericNHibernateDAO<Permission,int?>
    {
        public PermissionDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    /// <summary>
    /// Region object for NHibernate mapped table 'tbl_Region'.
    /// </summary>
    public  class RegionDAO : GenericNHibernateDAO<Region,int?>
    {
        public RegionDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    /// <summary>
    /// Site object for NHibernate mapped table 'tbl_Site'.
    /// </summary>
    public  class SiteDAO : GenericNHibernateDAO<Site,int?>
    {
        public SiteDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    /// <summary>
    /// SiteType object for NHibernate mapped table 'tbl_SiteType'.
    /// </summary>
    public  class SiteTypeDAO : GenericNHibernateDAO<SiteType,int?>
    {
        public SiteTypeDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }


    /// <summary>
    /// User object for NHibernate mapped table 'tbl_User'.
    /// </summary>
    public  class UserDAO : GenericNHibernateDAO<User,int?>
    {
        public UserDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    /// <summary>
    /// ValueType object for NHibernate mapped table 'tbl_ValueType'.
    /// </summary>
    public  class ValueTypeDAO : GenericNHibernateDAO<ValueType,int?>
    {
        public ValueTypeDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    /// <summary>
    /// UserRegion object for NHibernate mapped table 'tbl_UserRegion'.
    /// </summary>
    public class UserRegionDAO : GenericNHibernateDAO<UserRegion, int?>
    {
        public UserRegionDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }
    
    /// <summary>
    /// ValueType object for NHibernate mapped table 'tbl_ValueType'.
    /// </summary>
    public class LogDAO : GenericNHibernateDAO<Log, int?>
    {
        public LogDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    /// <summary>
    /// GroupExchangeRate object for NHibernate mapped table 'tbl_GroupExchangeRate'.
    /// </summary>
    public class GroupExchangeRateDAO : GenericNHibernateDAO<GroupExchangeRate, int?>
    {
        public GroupExchangeRateDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }
}