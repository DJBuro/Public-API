using Postal;

namespace MyAndromeda.Web.Models.Emails
{
    public class ResetPasswordNotificationEmail : Email 
    {
        public ResetPasswordNotificationEmail() : base()
        {
        }

        public ResetPasswordNotificationEmail(string viewName) : base(viewName)
        {
        }

        public string Email { get; set; }

        public string FirstName { get; set; }
    }
}