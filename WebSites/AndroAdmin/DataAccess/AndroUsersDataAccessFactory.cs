using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroUsersDataAccess.DataAccess;

namespace AndroAdmin.DataAccess
{
    public class AndroUsersDataAccessFactory
    {
        public static IAndroUserDAO GetAndroUserDAO()
        {
            return new AndroUsersDataAccess.EntityFramework.DataAccess.AndroUserDAO();
        }

        public static ISecurityGroupDAO GetAndroSecurityGroupDAO()
        {
            return new AndroUsersDataAccess.EntityFramework.DataAccess.SecurityGroupDAO();
        }

        public static IPermissionDAO GetPermissionsDAO()
        {
            return new AndroUsersDataAccess.EntityFramework.DataAccess.PermissionDAO();
        }
    }
}