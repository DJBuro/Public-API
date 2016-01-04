using System.Diagnostics;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Events;

namespace MyAndromeda.Menus.Ftp.Helpers
{
    public class FtpListener : TraceListener
    {
        private readonly IFtpEvents[] events;

        public FtpListener(IFtpEvents[] events)
        {
            this.events = events;
        }

        public int AndromedaId { get; set; }

        public override void Write(string message)
        {
            var logger = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IMyAndromedaLogger)) as IMyAndromedaLogger;
            logger.Info(message);

            foreach (var ev in this.events)
            {
                ev.TransactionLog(new DatabaseUpdatedEventContext(this.AndromedaId), message);
            }
        }

        public override void WriteLine(string message)
        {
            var logger = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IMyAndromedaLogger)) as IMyAndromedaLogger;
            logger.Info(message);

            foreach (var ev in this.events)
            {
                ev.TransactionLog(new DatabaseUpdatedEventContext(this.AndromedaId), message);
            }
        }
    }
}