using NHibISessionFactory = NHibernate.ISessionFactory;

namespace OrderTracking.Dao.NHibernate.Dao.Factory
{
    public abstract class AbstractHibernateDAOFactory
    {
        public void CloseSession()
        {
            //todo: test, old code below.
            //this.Close;
            this.SessionFactory.Close();
        }
        
        private NHibISessionFactory _SessionFactory;

        public NHibISessionFactory SessionFactory
        {
            get { return _SessionFactory; }
            set { _SessionFactory = value; }
        }
    }
}
