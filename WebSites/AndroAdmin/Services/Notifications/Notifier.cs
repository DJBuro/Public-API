using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AndroAdmin.Services.Notifications
{
    /// <summary>
    /// Cut down version of MyAndromeda's postback notifier: see MyAndromeda.Framework.Notification
    /// </summary>
    public class Notifier
    {
        public static readonly string MessageKey = "iNotifier.messages";
        public static readonly string DebugKey = "iNotifier.debug";
        public static readonly string ErrorMessageKey = "iNotifier.errors";
        public static readonly string ExceptionKey = "iNotifier.exceptions";

        private readonly Controller controller;

        private Notifier() { }

        public Notifier(Controller controller): this()
        {
            this.controller = controller;
        }

        private ICollection<string> NotifyMessages
        {
            get
            {
                var collection = GrabCollection<string>(this.controller.ControllerContext, MessageKey);

                return collection;
            }
        }

        /// <summary>
        /// Stores a message for the user.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Notify(string message)
        {
            this.NotifyMessages.Add(message);
        }

        public static ICollection<T> GrabCollection<T>(ControllerContext controllerContext, string collectionKey)
        {
            var controllerCollection = controllerContext.Controller.TempData[collectionKey];
            if (controllerCollection != null)
                return controllerCollection as ICollection<T>;

            controllerCollection = controllerContext.Controller.TempData[collectionKey] = new Collection<T>();

            return controllerCollection as ICollection<T>;
        }
    }
}