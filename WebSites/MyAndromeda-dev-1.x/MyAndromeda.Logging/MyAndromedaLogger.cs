using System.Diagnostics;
using System.Web.Mvc;
using MyAndromeda.Logging.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAndromeda.Logging
{
    public class MyAndromedaLogger : Ninject.Extensions.Logging.Log4net.Infrastructure.Log4NetLogger, IMyAndromedaLogger
    {
        private readonly IMyAndromedaLoggingMessageEvent[] loggingEvents; 

        public MyAndromedaLogger(Type type) : base(type)
        {
            this.loggingEvents = DependencyResolver.Current.GetServices(typeof(IMyAndromedaLoggingMessageEvent))
                    .Select(e => e as IMyAndromedaLoggingMessageEvent)
                    .ToArray();
        }

        public override void Debug(string message)
        {
            foreach(var ev in this.loggingEvents)
            {
                ev.OnDebug(message);
            }

 	        base.Debug(message);
        }

        public override void Debug(string format, params object[] args)
        {
            var message = string.Format(format, args);
            this.Debug(message);
        }

        public override void Error(string message)
        {
            foreach(var ev in this.loggingEvents)
            {
                ev.OnError(message);
            }

 	        base.Error(message);
        }

        public override void Fatal(string message)
        {
            foreach(var ev in this.loggingEvents)
            {
                ev.OnFatal(message);
            }

 	        base.Fatal(message);
        }

        public override void Info(string message)
        {
            foreach(var ev in this.loggingEvents)
            {
                ev.OnInfo(message);
            }

 	        base.Info(message);
        }
        
        public void Error(Exception exception)
        {
            foreach(var ev in this.loggingEvents)
            {
                ev.OnError(exception);
            }

            base.Error(exception.Message);
            base.Error(exception.Source);
            base.Error(exception.StackTrace);

            if (exception.InnerException != null) { this.Error(exception.InnerException); }
        }

        public void Fatal(Exception exception)
        {
            foreach(var ev in this.loggingEvents)
            {
                ev.OnFatal(exception);
            }

            base.Fatal(exception.Message);
            base.Fatal(exception.Source);
            base.Fatal(exception.StackTrace);
        }
    }

}
