using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dashboard.Dao.Domain;

namespace Dashboard.Web.Mvc.Utilities
{
    public static class Authorization
    {
        public static bool CanDo(User user, string siteId)
        {
            if(user.HeadOfficeUser)
                return true;

            foreach (Permission permission in user.Permissions)
            {
                if (permission.Site.Id.Value.ToString() == siteId)
                {
                    return true;
                }
            }

            return false;

        }

    }
}
