using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MyAndromeda.SendGridService
{
    public class SendGridEmailSettings : ISendGridEmailSettings
    {
        private SmtpSection settings;
        private SmtpSection Settings 
        {
            get 
            {
                if (settings != null) { return settings; }
                var config = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;

                return (settings = config);
            } 
        }

        public string UserName
        {
            get
            {
                
                return this.Settings.Network.UserName;
            }
        }

        public string Password
        {
            get
            {
                return this.Settings.Network.Password;
            }
        }
    }
}
