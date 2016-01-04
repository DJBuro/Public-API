
using AndroAdmin.Dao.Domain;
using NHibISessionFactory = NHibernate.ISessionFactory;

namespace AndroAdmin.Dao.NHibernate.Dao.Factory
{
    /// <summary>
    /// AndroUser object for NHibernate mapped table 'tbl_AndroUser'.
    /// </summary>
    public class AndroUserDAO : GenericNHibernateDAO<AndroUser,int?>
    {
        public AndroUserDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    /// <summary>
    /// AndroUserPermission object for NHibernate mapped table 'tbl_AndroUserPermission'.
    /// </summary>
    public  class AndroUserPermissionDAO : GenericNHibernateDAO<AndroUserPermission,int?>
    {
        public AndroUserPermissionDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

    /// <summary>
    /// Project object for NHibernate mapped table 'tbl_Project'.
    /// </summary>
    public  class ProjectDAO : GenericNHibernateDAO<Project,int?>
    {
        public ProjectDAO(NHibISessionFactory _SessionFactory)
            : base(_SessionFactory)
        {
        }
    }

}