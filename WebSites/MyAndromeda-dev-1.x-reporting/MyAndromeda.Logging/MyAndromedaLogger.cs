using System.Diagnostics;
using System.Web.Mvc;
using MyAndromeda.Logging.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAndromeda.Logging
{
    public class MyAndromedaLogger : 
        Ninject.Extensions.Logging.Log4net.Infrastructure.Log4NetLogger, 
        IMyAndromedaLogger
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

        public void Debug(string format, object arg0)
        {
            string message = string.Format(format, arg0);
            this.Debug(message);
        }

        public void Debug(string format, object arg0, object arg1)
        {
            string message = string.Format(format, arg0, arg1);
            this.Debug(message);
        }

        public void Debug(string format, object arg0, object arg1, object arg2)
        {
            string message = string.Format(format, arg0, arg1, arg2);
            this.Debug(message);
        }

        public override void Debug(string format, params object[] args)
        {
            string message = string.Format(format, args);
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

        public void Info(string format, object arg0)
        {
            string info = string.Format(format, arg0);
            this.Info(info);
        }

        public void Info(string format, object arg0, object arg1)
        {
            string info = string.Format(format, arg0, arg1);
            this.Info(info);
        }

        public void Info(string format, object arg0, object arg1, object arg2)
        {
            string info = string.Format(format, arg0, arg1, arg2);
            this.Info(info);
        }


        ///todo add trace(message) override 




        public void Trace(string format, object arg0)
        {
            string trace = string.Format(format, arg0);
            this.Trace(trace);
        }

        public void Trace(string format, object arg0, object arg1)
        {
            string trace = string.Format(format, arg0, arg1);
            this.Trace(trace);
        }

        public void Trace(string format, object arg0, object arg1, object arg2)
        {
            string trace = string.Format(format, arg0,arg1,arg2);
            this.Trace(trace);
        }

        ///todo add Warn(message) override 

        public void Warn(string format, object arg0)
        {
            string warn = string.Format(format, arg0);
            this.Warn(warn);
        }

        public void Warn(string format, object arg0, object arg1)
        {
            string warn = string.Format(format, arg0, arg1);
            this.Warn(warn);
        }

        public void Warn(string format, object arg0, object arg1, object arg2)
        {
            string warn = string.Format(format, arg0, arg1);
            this.Warn(warn);
        }

        public void Error(Exception exception)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnError(exception);
            }

            base.Error(exception.Message);
            base.Error(exception.Source);
            base.Error(exception.StackTrace);

            if (exception.InnerException != null) { this.Error(exception.InnerException); }
        }

        public void Error(string format, object arg0)
        {
            string error = string.Format(format, arg0);
            this.Error(error);
        }

        public void Error(string format, object arg0, object arg1)
        {
            string error = string.Format(format, arg0, arg1);
            this.Error(error);
        }

        public void Error(string format, object arg0, object arg1, object arg2)
        {
            string error = string.Format(format, arg0, arg1, arg2);
            this.Error(error);
        }
        public void Fatal(Exception exception)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnFatal(exception);
            }

            base.Fatal(exception.Message);
            base.Fatal(exception.Source);
            base.Fatal(exception.StackTrace);
        }
        public void Fatal(string format, object arg0)
        {
            string fatal = string.Format(format, arg0);
            this.Fatal(fatal);
        }

        public void Fatal(string format, object arg0, object arg1)
        {
            string fatal = string.Format(format, arg0, arg1);
            this.Fatal(fatal);
        }

        public void Fatal(string format, object arg0, object arg1, object arg2)
        {
            string fatal = string.Format(format, arg0, arg1, arg2);
            this.Fatal(fatal);
        }
    }

}
