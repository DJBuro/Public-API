using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using AndroAdmin.Dao.Domain;

namespace AndroAdmin.Mvc.Utilities
{
    public static class Authorization
    {
        public static bool CanDo(AndroUser user, string siteId)
        {
            //TODO: extend fix, etc..

            if (user.Active)
                return true;
            /*
            foreach (Permission permission in user.Permissions)
            {
                if (permission.Site.Id.Value.ToString() == siteId)
                {
                    return true;
                }
            }
            */
            return false;

        }

    }
}