using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdminDataAccess.DataAccess;

namespace Andromeda.WebOrderTracking
{
    public class AndroAdminDataAccessFactory
    {
        public static ILogDAO GetLogDAO()
        {
            return new AndroAdminDataAccess.nHibernate.DataAccess.LogDAO();
        }
    }
}