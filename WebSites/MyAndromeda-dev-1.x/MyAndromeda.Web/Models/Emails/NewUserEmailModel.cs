using MyAndromeda.Data.Domain;
using Postal;
using System;
using System.Linq;

namespace MyAndromeda.Web.Models.Emails
{
    public class NewUserEmail : Email
    {
        public NewUserEmail() : base() { }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ChainDomainModel Chain { get; set; }
    }
}