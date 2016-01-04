using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using MyAndromedaDataAccess.Domain.Marketing;
using Ninject.Extensions.Logging;
using MyAndromeda.Logging;

namespace MyAndromeda.Framework.Messaging
{
    public class MailSender : IMailSender
    {
        private readonly IMyAndromedaLogger logger;
        
        SmtpClient smtpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailSender" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="emailContext">The email context.</param>
        public MailSender(IMyAndromedaLogger logger)
        { 
            this.logger = logger;
        }

        public EmailSettings Settings { get; set; }

        public void Send(IMessage message, Guid guid)
        {
            if (message.Type != "Email")
            {
                return;
            }

            this.Send(new[] { message.To }, message.Subject, message.Body, guid);
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="settings">The settings.</param>
        public void Send(IMessage message, EmailSettings settings, Guid guid)
        {
            this.Settings = settings;
            this.Send(message, guid);
        }

        /// <summary>
        /// Sends the specified to.
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        public void Send(IEnumerable<string> toAddress, string subject, string message, Guid token)
        {
            if (toAddress == null || toAddress.Count() == 0)
            {
                throw new ArgumentException("Requires to address(es).");
            }

            if (string.IsNullOrWhiteSpace(subject)) 
            {
                throw new ArgumentException("Requires subject");
            }

            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.From = new MailAddress(this.Settings.From ?? "marketing@androtech.com");
              
            foreach (var address in toAddress) 
            {
                mailMessage.To.Add(new MailAddress(address));
            }

            this.logger.Debug(string.Format("Sending an email to: {0}", toAddress.ToString()));

            this.SetupSmtpSettings();
            this.SendMyEmail(mailMessage);
        }
        
        /// <summary>
        /// Sends the specified to.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="settings">The settings.</param>
        public void Send(IEnumerable<string> to, string subject, string message, EmailSettings settings, Guid guid)
        {
            this.Settings = settings;
            this.Send(to, subject, message, guid);
        }

        private void SetupSmtpSettings() 
        {
            if (this.smtpClient != null)
            {
                return;
            }

            this.smtpClient = new SmtpClient(this.Settings.Host);
            if (!string.IsNullOrWhiteSpace(this.Settings.PickupFolder))
            {
                this.smtpClient.PickupDirectoryLocation = this.Settings.PickupFolder;
                this.smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

                this.logger.Debug("Delivery method is set to folder");
            }

            this.smtpClient.EnableSsl = this.Settings.SSL;

            if (this.Settings.Authenticated)
            {
                this.smtpClient.Credentials = new System.Net.NetworkCredential(this.Settings.UserName, this.Settings.Password);
                this.logger.Debug("Delivery network credentials set");
            }

            //this.smtpClient.SendCompleted += (sender, ars) => 
            //{
            //    var token = ars.UserState;  
            //};

            //this.smtpClient.ServicePoint.ConnectionLimit = 3;
        }

        private void SendMyEmail(MailMessage mailMessage)
        {
            this.smtpClient.Send(mailMessage);
            //smtpClient.SendAsync(mailMessage, token);
            //return smtpClient.SendMailAsync(mailMessage);
        }
    }
}