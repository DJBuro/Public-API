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

    internal static class LoggerFactory
    {
        private static readonly Dictionary<Type, IMyAndromedaLogger> typeToLoggerMap = new Dictionary<Type, IMyAndromedaLogger>();

        public static IMyAndromedaLogger GetLogger(Ninject.Activation.IContext context)
        {
            //var typeForLogger = context.Request.Target != null
            //            ? context.Request.Target.Member.DeclaringType
            //            : context.Request.Service;

            IMyAndromedaLogger logger = null;
            Type type = context.Request.Target == null ? typeof(IMyAndromedaLogger) : context.Request.Target.Member.DeclaringType;
            try
            {
                logger = GetLogger(type);
            }
            catch (Exception e) 
            {
                System.Diagnostics.Trace.WriteLine("logger cannot be created");
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            return logger;
        }

        private static IMyAndromedaLogger GetLogger(Type type)
        {
            lock (typeToLoggerMap)
            {
                if (typeToLoggerMap.ContainsKey(type))
                    return typeToLoggerMap[type];

                IMyAndromedaLogger logger = new MyAndromedaLogger(type);
                typeToLoggerMap.Add(type, logger);

                return logger;
            }
        }
    }
}
