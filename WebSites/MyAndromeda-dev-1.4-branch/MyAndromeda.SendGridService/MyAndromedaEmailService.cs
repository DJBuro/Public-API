using System.Net.Mail;
using MyAndromeda.Core;
using MyAndromeda.Logging;
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
        private readonly IMyAndromedaLogger logger;

        private readonly EmailService postalEmailService;
        private readonly ISendGridEmailSettings sendGridEmailSettings;

        public MyAndromedaEmailService(
            IMyAndromedaLogger logger,
            ISendGridEmailSettings sendGridEmailSettings) 
        {
            this.logger = logger;
            this.sendGridEmailSettings = sendGridEmailSettings;
            this.postalEmailService = new EmailService();
        }

        public void Send(Email email)
        {
            var emailMessage = postalEmailService.CreateMailMessage(email);

            var credentials = new NetworkCredential(sendGridEmailSettings.UserName, sendGridEmailSettings.Password);
            var client = new SendGrid.Web(credentials);

            SendGrid.SendGridMessage myMessage = new SendGrid.SendGridMessage();

            myMessage.SetMyAndromedaDefaults();
            myMessage.Copy(emailMessage);
            myMessage.From = emailMessage.From;
            myMessage.ReplyTo = emailMessage.ReplyToList.ToArray();

            try
            {
                var task = client.DeliverAsync(myMessage);
                task.Wait();
            }
            catch (Exception e)
            {
                this.logger.Error("Error sending a email to: " + myMessage.To);
                this.logger.Error(e);
                throw;
            }

            
        }

        public async Task SendAsync(Email email)
        {
            var emailMessage = postalEmailService.CreateMailMessage(email);

            var credentials = new NetworkCredential(sendGridEmailSettings.UserName, sendGridEmailSettings.Password);
            var client = new SendGrid.Web(credentials);

            SendGrid.SendGridMessage myMessage = new SendGrid.SendGridMessage();
            
            myMessage.SetMyAndromedaDefaults();
            myMessage.Copy(emailMessage);
            myMessage.From = emailMessage.From;
            myMessage.ReplyTo = emailMessage.ReplyToList.ToArray();
            //myMessage.From = emailMessage.From;
            try
            {
                await client.DeliverAsync(myMessage);
            }
            catch (Exception e)
            {
                this.logger.Error("Error sending a email async to: " + myMessage.To);
                this.logger.Error(e);
                throw;
            }
            
        }

        public MailMessage CreateMailMessage(Email email)
        {
            var emailMessage = postalEmailService.CreateMailMessage(email);

            return emailMessage;
        }
    }

    public static class Extensions 
    {

        public static void SetMyAndromedaDefaults(this SendGrid.SendGridMessage email) 
        {
            email.EnableClickTracking();
            email.EnableGravatar();
            email.EnableOpenTracking();
        }

        public static void Copy(this SendGrid.SendGridMessage email, MailMessage message) 
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
