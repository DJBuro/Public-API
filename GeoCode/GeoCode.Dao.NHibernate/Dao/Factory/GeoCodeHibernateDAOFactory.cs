using GeoCode.Dao.NHibernate.Dao.Factory;

namespace GeoCode.Dao.NHibernate.Dao.Factory
{
    public class GeoCodeHibernateDAOFactory : AbstractHibernateDAOFactory
    {

        //private CoordinateDAO _CoordinateDAO;
        private PostCodeDAO _PostCodeDAO;

        //public CoordinateDAO CoordinateDAO
        //{
        //    get
        //    {
        //        if (_CoordinateDAO == null) _CoordinateDAO = new CoordinateDAO(this.SessionFactory);
        //        return _CoordinateDAO;
        //    }
        //}

        public PostCodeDAO PostCodeDAO
        {
            get
            {
                if (_PostCodeDAO == null) _PostCodeDAO = new PostCodeDAO(this.SessionFactory);
                return _PostCodeDAO;
            }
        }
    }
}