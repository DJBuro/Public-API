using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroUsersDataAccess.Domain;

namespace AndroAdmin.Model
{
    public class UserListModel
    {
        public AndroUser AndroUser { get; set; }
        public string SecurityGroups { get; set; }

        public UserListModel ()
        {
            this.SecurityGroups = "";
        }
    }
}