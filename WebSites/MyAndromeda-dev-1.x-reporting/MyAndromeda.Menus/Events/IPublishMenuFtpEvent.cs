using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Logging;

namespace MyAndromeda.Menus.Events
{
    public interface IPublishMenuFtpEvent : ITransientDependency 
    {
        /// <summary>
        /// Called when [started job].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="notifiyRoles">The notify roles.</param>
        void OnStartedJob(string message, params string[] notifyRoles);

        /// <summary>
        /// Called when [ended job].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="notifiyRoles">The notify roles.</param>
        void OnEndedJob(string message, params string[] notifyRoles);

        /// <summary>
        /// Called when [tasks starting].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="notifyRoles">The notify roles.</param>
        void OnTasksStarting(string message, params string[] notifyRoles);

        /// <summary>
        /// Called when [ended task].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="notifyRoles">The notify roles.</param>
        void OnEndedTask(string message, params string[] notifyRoles);

        /// <summary>
        /// Called when [started task].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="notifiyRoles">The notify roles.</param>
        void OnStartedTask(string message, params string[] notifyRoles);

        /// <summary>
        /// Called when [error].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="notifyRoles">The notify roles.</param>
        void OnError(string message, params string[] notifyRoles);
    }

    public class PublishMenuFtpEventLogger : IPublishMenuFtpEvent 
    {
        private readonly IMyAndromedaLogger logger;

        public PublishMenuFtpEventLogger(IMyAndromedaLogger logger)
        { 
            this.logger = logger;
        }

        public void OnStartedJob(string message, params string[] notifyRoles)
        {
            //this.logger.Info(message);
        }

        public void OnEndedJob(string message, params string[] notifyRoles)
        {
            //this.logger.Info(message);
        }

        public void OnTasksStarting(string message, params string[] notifyRoles)
        {
            //this.logger.Info(message);
        }

        public void OnEndedTask(string message, params string[] notifyRoles)
        {
            //this.logger.Info(message);
        }

        public void OnStartedTask(string message, params string[] notifyRoles)
        {
            //this.logger.Info(message);
        }

        public void OnError(string message, params string[] notifyRoles)
        {
            this.logger.Error(message);
        }
    }
}
