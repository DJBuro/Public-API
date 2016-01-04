using System.Net.Mail;
using MyAndromeda.Core;
using Postal;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace MyAndromeda.SendGridService
{
    public interface IMyAndromedaEmailService: IEmailService, ITransientDependency
    {
    }

    public class MyAndromedaEmailService : IMyAndromedaEmailService
    {
        private EmailService postalEmailService;
        private ISendGridEmailSettings sendGridEmailSettings;
        public MyAndromedaEmailService(ISendGridEmailSettings sendGridEmailSettings) 
        {
            this.sendGridEmailSettings = sendGridEmailSettings;
            this.postalEmailService = new EmailService();
        }

        public void Send(Email email)
        {
            var emailMessage = postalEmailService.CreateMailMessage(email);

            SendGridMail.SendGrid myMessage = SendGridMail.SendGrid.GetInstance();

            myMessage.SetMyAndromedaDefaults();
            myMessage.Copy(emailMessage);
            myMessage.From = emailMessage.From;
            myMessage.ReplyTo = emailMessage.ReplyToList.ToArray(); 
                //new MailAddress[] { emailMessage.ReplyTo };
            
            var credentials = new NetworkCredential(sendGridEmailSettings.UserName, sendGridEmailSettings.Password);
            var transportWeb = SendGridMail.Web.GetInstance(credentials);

            transportWeb.Deliver(myMessage);
        }

        public Task SendAsync(Email email)
        {
            var emailMessage = postalEmailService.CreateMailMessage(email);

            SendGridMail.SendGrid myMessage = SendGridMail.SendGrid.GetInstance();
            
            myMessage.SetMyAndromedaDefaults();
            myMessage.Copy(emailMessage);
            myMessage.From = emailMessage.From;
            myMessage.ReplyTo = emailMessage.ReplyToList.ToArray();
            //myMessage.From = emailMessage.From;

            var credentials = new NetworkCredential(sendGridEmailSettings.UserName, sendGridEmailSettings.Password);
            var transportWeb = SendGridMail.Web.GetInstance(credentials);
            
            return transportWeb.DeliverAsync(myMessage);
        }

        public MailMessage CreateMailMessage(Email email)
        {
            var emailMessage = postalEmailService.CreateMailMessage(email);

            return emailMessage;
        }
    }

    public static class Extensions 
    {

        public static void SetMyAndromedaDefaults(this SendGridMail.SendGrid email) 
        {
            email.EnableClickTracking();
            email.EnableGravatar();
            email.EnableOpenTracking();
        }
        
        public static void Copy(this SendGridMail.SendGrid email, MailMessage message) 
        {
            //email.To = message.To.ToArray();
            foreach (var to in message.To) 
            {
                email.AddTo(to.Address);
            }
            foreach (var to in message.CC) 
            {
                email.AddCc(to.Address);
            }
            foreach (var to in message.Bcc) 
            {
                email.AddBcc(to.Address);
            }

            email.Subject = message.Subject;
            email.Html = message.Body;
        } 
    }
}
