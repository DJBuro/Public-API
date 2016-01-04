using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroAdmin.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public List<SecurityGroupModel> SecurityGroups { get; set; }
    }
}