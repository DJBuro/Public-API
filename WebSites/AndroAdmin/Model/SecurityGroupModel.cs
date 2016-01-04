using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroUsersDataAccess.Domain;

namespace AndroAdmin.Model
{
    public class SecurityGroupModel
    {
        public SecurityGroup SecurityGroup { get; set; }
        public List<PermissionModel> Permissions { get; set; }
        public bool Selected { get; set; }
    }
}