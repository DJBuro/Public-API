using GeoCode.Dao.Domain;
using NHibISessionFactory = NHibernate.ISessionFactory;

namespace GeoCode.Dao.NHibernate.Dao.Factory
{
   
    /// <summary>
    /// Coordinates object for NHibernate mapped table 'tbl_Coordinates'.
    /// </summary>
    public class CoordinateDAO : GenericNHibernateDAO<Coordinates, long?>
    {
        public CoordinateDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }   

    public class PostCodeDAO : GenericNHibernateDAO<PostCode, long?>
    {
        public PostCodeDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }
}