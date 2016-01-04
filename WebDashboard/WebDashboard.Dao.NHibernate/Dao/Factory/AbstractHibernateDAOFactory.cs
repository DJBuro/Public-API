using NHibISessionFactory = NHibernate.ISessionFactory;

namespace WebDashboard.Dao.NHibernate.Dao.Factory
{
    public abstract class AbstractHibernateDAOFactory
    {
        public void CloseSession()
        {
            this.CloseSession();
        }

        public NHibISessionFactory SessionFactory { get; set; }
    }
}
