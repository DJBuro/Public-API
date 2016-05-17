using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Core;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Framework.Notification
{

    public interface INotifier : ITransientDependency
    {
        /// <summary>
        /// Add a exception into the list.
        /// </summary>
        /// <param name="exception">The exception.</param>
        void Exception(Exception exception);

        /// <summary>
        /// Determines whether this instance has exception.
        /// </summary>
        /// <returns></returns>
        bool HasException();

        /// <summary>
        /// Determines whether this instance has errors.
        /// </summary>
        /// <returns></returns>
        bool HasErrors();

        /// <summary>
        /// Determines whether this instance has notifications.
        /// </summary>
        /// <returns></returns>
        bool HasNotifications();

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message, bool notifyOthersInStore = false);

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(string message);

        /// <summary>
        /// Notifies the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Notify(string message, bool notifyOthersInStore = false);

        /// <summary>
        /// Notifies the specified message as success.
        /// </summary>
        /// <param name="message">The message.</param>
        void Success(string message, bool notifyOthersInStore = false);
    }

    public class Notifier : INotifier 
    {
        
        private readonly Lazy<ControllerContext> controllerContexthost;
        private readonly INotifierEvent[] notifierEvents;
        private readonly IWorkContext workContext;

        public Notifier(Lazy<ControllerContext> controllerContexthost, INotifierEvent[] notifierEvents, IWorkContext workContext) 
        {
            this.workContext = workContext;
            this.controllerContexthost = controllerContexthost;
            this.notifierEvents = notifierEvents;
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message)
        {
            this.DebugMessages.Add(message);
        }

        /// <summary>
        /// Add a exception into the list.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Exception(Exception exception)
        {
            this.Exceptions.Add(exception);
        }

        public bool HasDebugMessages() 
        {
            return this.DebugMessages.Count > 0;
        }

        public bool HasException()
        {
            return this.Exceptions.Count > 0;
        }

        public bool HasErrors()
        {
            return this.ErrorMessages.Count > 0;
        }

        public bool HasNotifications()
        {
            return this.NotifyMessages.Count > 0;
        }

        private ICollection<string> DebugMessages 
        {
            get 
            {
                var collection = GrabCollection<string>(controllerContexthost.Value, DebugKey);
                
                return collection;
            }
        }

        private ICollection<string> NotifyMessages 
        {
            get
            {
                var collection = GrabCollection<string>(controllerContexthost.Value, MessageKey);

                return collection;
            } 
        }

        private ICollection<string> ErrorMessages
        {
            get 
            {
                var collection = GrabCollection<string>(controllerContexthost.Value, ErrorMessageKey);

                return collection;
            }
        }

        private ICollection<Exception> Exceptions
        {
            get 
            {
                var collection = GrabCollection<Exception>(controllerContexthost.Value, ExceptionKey);
                    
                return collection;
            }
        }

        /// <summary>
        /// Stores a message for the user.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Notify(string message, bool notifyOthersInStore = false)
        {
            var context = new NotificationContext() 
            {
                Message = message,
                NotifyOthersLoggedIntoStore = notifyOthersInStore,
                UserName = this.workContext.CurrentUser.Available ? this.workContext.CurrentUser.User.Username :  ""
            };

            foreach (var ev in this.notifierEvents) 
            {
                ev.OnNotify(context);
            }

            if (this.NotifyMessages != null) 
            {
                this.NotifyMessages.Add(message);
            }
        }

        /// <summary>
        /// Notifies the specified message as success.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Success(string message, bool notifyOthersInStore = false)
        {
            var context = new NotificationContext()
            {
                Message = message,
                NotifyOthersLoggedIntoStore = notifyOthersInStore,
                UserName = this.workContext.CurrentUser.Available ? this.workContext.CurrentUser.User.Username : ""
            };

            foreach (var ev in this.notifierEvents) 
            {
                ev.OnSuccess(context);
            }

            if (this.NotifyMessages != null) 
            { 
                this.NotifyMessages.Add(message);
            }
        }

        /// <summary>
        /// Stores an Error for the user.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message, bool notifyOthersInStore = false)
        {
            var context = new NotificationContext()
            {
                Message = message,
                NotifyOthersLoggedIntoStore = notifyOthersInStore,
                UserName = this.workContext.CurrentUser.Available ? this.workContext.CurrentUser.User.Username : ""
            };

            foreach (var ev in this.notifierEvents) 
            {
                ev.OnError(context);
            }

            if (this.ErrorMessages != null) 
            { 
                this.ErrorMessages.Add(message);
            }
        }

        public static readonly string MessageKey = "iNotifier.messages";
        public static readonly string DebugKey = "iNotifier.debug";
        public static readonly string ErrorMessageKey = "iNotifier.errors";
        public static readonly string ExceptionKey = "iNotifier.exceptions";

        public static ICollection<T> GrabCollection<T>(ControllerContext controllerContext, string collectionKey) 
        {
            if (controllerContext == null || controllerContext.Controller == null) { return null; }

            var controllerCollection = controllerContext.Controller.TempData[collectionKey];
            if (controllerCollection != null)
            {
                return controllerCollection as ICollection<T>;
            }

            controllerCollection = controllerContext.Controller.TempData[collectionKey] = new Collection<T>();

            return controllerCollection as ICollection<T>;
        }
    }
}
