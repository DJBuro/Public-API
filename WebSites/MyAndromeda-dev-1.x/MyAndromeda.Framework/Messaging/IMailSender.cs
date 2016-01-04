using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using MyAndromeda.Core;
using MyAndromeda.Data.Domain.Marketing;
using Ninject.Extensions.Logging;

namespace MyAndromeda.Framework.Messaging
{
    public interface IMailSender : IDependency
    {
        EmailSettings Settings { get; set; }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Send(IMessage message, Guid token);

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="settings">The settings.</param>
        void Send(IMessage message, EmailSettings settings, Guid token);

        /// <summary>
        /// Sends the specified to.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        void Send(IEnumerable<string> to, string subject, string message, Guid token);

        /// <summary>
        /// Sends the specified to.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="settings">The settings.</param>
        void Send(IEnumerable<string> to, string subject, string message, EmailSettings settings, Guid token);
    }
}
