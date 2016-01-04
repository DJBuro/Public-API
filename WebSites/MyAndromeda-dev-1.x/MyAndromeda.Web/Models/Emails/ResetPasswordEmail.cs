using Postal;
using System;
using System.Linq;

namespace MyAndromeda.Web.Models.Emails
{
    public class ResetPasswordEmail : Email
    {
        public ResetPasswordEmail() : base() { }
        public ResetPasswordEmail(string viewName): base(viewName) { }

        public string ValidationCode { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
    }
}