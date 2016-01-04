using NHibISessionFactory = NHibernate.ISessionFactory;

namespace GeoCode.Dao.NHibernate.Dao.Factory
{
    public abstract class AbstractHibernateDAOFactory
    {
        public void CloseSession()
        {
            //todo: test, old code below.
            //this.Close;
            this.SessionFactory.Close();
        }

        public NHibISessionFactory SessionFactory { get; set; }
    }
}