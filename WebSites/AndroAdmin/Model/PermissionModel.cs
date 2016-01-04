using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using AndroUsersDataAccess.Domain;

namespace AndroAdmin.Model
{
    public class PermissionModel
    {
        public Permission Permission { get; set; }
        public bool Selected { get; set; }
    }
}