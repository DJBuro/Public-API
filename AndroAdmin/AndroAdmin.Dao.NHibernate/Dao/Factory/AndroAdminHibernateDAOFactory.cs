


namespace AndroAdmin.Dao.NHibernate.Dao.Factory
{
    public class AndroAdminHibernateDAOFactory : AbstractHibernateDAOFactory{ 

        private AndroUserDAO _AndroUserDAO;
        private AndroUserPermissionDAO _AndroUserPermissionDAO;
        private ProjectDAO _ProjectDAO;
		

        public AndroUserDAO AndroUserDAO
        {
            get
            {
                if (_AndroUserDAO == null) _AndroUserDAO = new AndroUserDAO(this.SessionFactory);
                return _AndroUserDAO;
            }
        }
        public AndroUserPermissionDAO AndroUserPermissionDAO
        {
            get
            {
                if (_AndroUserPermissionDAO == null) _AndroUserPermissionDAO = new AndroUserPermissionDAO(this.SessionFactory);
                return _AndroUserPermissionDAO;
            }
        }
        public ProjectDAO ProjectDAO
        {
            get
            {
                if (_ProjectDAO == null) _ProjectDAO = new ProjectDAO(this.SessionFactory);
                return _ProjectDAO;
            }
        }
    }
}