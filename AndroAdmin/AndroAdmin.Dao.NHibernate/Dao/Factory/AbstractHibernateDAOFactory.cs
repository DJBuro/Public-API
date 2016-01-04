using NHibISessionFactory = NHibernate.ISessionFactory;

namespace AndroAdmin.Dao.NHibernate.Dao.Factory
{
    public abstract class AbstractHibernateDAOFactory
    {
        public void CloseSession()
        {
            this.CloseSession();
        }

        private NHibISessionFactory _SessionFactory;

        public NHibISessionFactory SessionFactory
        {
            get { return _SessionFactory; }
            set { _SessionFactory = value; }
        }
    }
}
