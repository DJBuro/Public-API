using System;
using System.Linq;

namespace MyAndromeda.Core.User
{
    public class MyAndromedaUser
    {
        public MyAndromedaUser() { }

        public int Id { get; set; }
        public string Username { get; set; }

        public string Firstname { get; set; }
        public string Surname { get; set; }

        public bool Enabled { get; set; }

        //not this easy any more
        //public List<Site> Sites { get; set; }
    }
}
