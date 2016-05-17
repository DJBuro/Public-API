using MyAndromeda.Core;

namespace MyAndromeda.Framework.Notification
{
    public interface INotifierEvent : ITransientDependency 
    {
        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void OnError(NotificationContext context);

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void OnDebug(NotificationContext context);

        /// <summary>
        /// Notifies the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void OnNotify(NotificationContext context);

        /// <summary>
        /// Called when [success].
        /// </summary>
        /// <param name="message">The message.</param>
        void OnSuccess(NotificationContext context);
    }

    public class NotificationContext 
    {
        public string UserName { get; set; }
        public string Message { get; set; }

        public bool NotifyOthersLoggedIntoChain { get; set; }
        public bool NotifyOthersLoggedIntoStore { get; set; }
    }
}