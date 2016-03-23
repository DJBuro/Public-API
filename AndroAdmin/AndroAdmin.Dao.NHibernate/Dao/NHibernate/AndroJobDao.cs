using AndroAdmin.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;

namespace AndroAdmin.Dao.NHibernate
{
    public class AndroJobDao : HibernateDaoSupport, IAndroJob
    {
        public AndroAdminHibernateDAOFactory AndroAdminHibernateDaoFactory { get; set; }
    }
}
