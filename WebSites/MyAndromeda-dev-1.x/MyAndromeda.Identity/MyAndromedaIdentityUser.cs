using System;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace MyAndromeda.Identity
{
    /// <summary>
    /// implement lots of interface on Microsoft.AspNet.Identity.I.....
    /// </summary>
    public class MyAndromedaIdentityUser : IUser
    {
        public MyAndromedaIdentityUser(string id, string userName) 
        {
            this.Id = id;
            this.UserName = userName;
        }

        public string Id { get; private set; }

        public string UserName { get; set; }
    }
}