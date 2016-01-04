using System;
using System.Linq;
using NLog;
using Ninject.Extensions.Logging;
using MyAndromeda.Logging.Events;
using System.Web.Mvc;

namespace MyAndromeda.Logging.NlogImplementation
{
    public class MyAndromedaNLogger : IMyAndromedaLogger
    {
        private readonly IMyAndromedaLoggingMessageEvent[] loggingEvents;
        private readonly Logger logger;

        private Type context;

        public MyAndromedaNLogger(Type type)
        {
            this.context = type;
            this.loggingEvents = DependencyResolver.Current.GetServices(typeof(IMyAndromedaLoggingMessageEvent))
                .Select(e => e as IMyAndromedaLoggingMessageEvent)
                .ToArray();

            this.logger = NLog.LogManager.GetLogger(type.Name);//new NLogLogger(type);
        }

        public void Debug(string message)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnDebug(message);
            }

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Debug, context.Name, message);
            logger.Log(typeof(MyAndromedaNLogger), logEvent);
            //this.logger.Debug(message);
        }

        public void Debug(string format, params object[] args)
        {
            var message = string.Format(format, args);
            this.Debug(message);
        }

        public void Error(string message)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnError(message);
            }

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Error, context.Name, message);
            logger.Log(typeof(MyAndromedaNLogger), logEvent);

            //this.logger.Error(message);
        }

        public void Fatal(string message)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnFatal(message);
            }

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Fatal, context.Name, message);
            logger.Log(typeof(MyAndromedaNLogger), logEvent);
            //this.logger.Fatal(message);
        }

        public void Info(string message)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnInfo(message);
            }

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Info, context.Name, message);
            logger.Log(typeof(MyAndromedaNLogger), logEvent);

            //this.logger.Info(message);
        }

        public void Error(Exception exception)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnError(exception);
            }

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Error, context.Name, exception.Message);
            logEvent.Exception = exception;
            logger.Log(typeof(MyAndromedaNLogger), logEvent);


            //this.logger.Error(exception.Message);
            //this.logger.Error(exception.Source);
            //this.logger.Error(exception.StackTrace);

            if (exception.InnerException != null) { this.Error(exception.InnerException); }
        }

        public void Fatal(Exception exception)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnFatal(exception);
            }

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Fatal, context.Name, exception.Message);
            logEvent.Exception = exception;
            logger.Log(typeof(MyAndromedaNLogger), logEvent);

            //this.logger.Fatal(exception.Message);
            //this.logger.Fatal(exception.Source);
            //this.logger.Fatal(exception.StackTrace);
        }


        public void Info(string format, params object[] args)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnInfo(string.Format(format, args));
            }

            string message = string.Format(format, args);
            LogEventInfo logEvent = new LogEventInfo(LogLevel.Info, context.Name, message);

            logger.Log(typeof(MyAndromedaNLogger), logEvent);
        }

        public void Trace(string message)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnInfo(message);
            }

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Trace, context.Name, message);
            logger.Log(typeof(MyAndromedaNLogger), logEvent);

            this.logger.Trace(message);
        }

        public void Trace(string format, params object[] args)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnTrace(string.Format(format, args));
            }

            string message = string.Format(format, args);

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Trace, context.Name, message);
            logger.Log(typeof(MyAndromedaNLogger), logEvent);
            //this.logger.Trace(format, args);
        }

        public void Warn(string message)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnWarn(message);
            }

            this.logger.Warn(message);
        }

        public void Warn(string format, params object[] args)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnWarn(string.Format(format, args));
            }


            string message = string.Format(format, args);

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Warn, context.Name, message);
            logger.Log(typeof(MyAndromedaNLogger), logEvent);

            //this.logger.Trace(format, args);
        }

        public void Error(string format, params object[] args)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnError(string.Format(format, args));
            }

            string message = string.Format(format, args);

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Error, context.Name, message);
            logger.Log(typeof(MyAndromedaNLogger), logEvent);

            //this.logger.Error(format, args);
        }

        public void Fatal(string format, params object[] args)
        {
            foreach (var ev in this.loggingEvents)
            {
                ev.OnFatal(string.Format(format, args));
            }

            string message = string.Format(format, args);

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Fatal, context.Name, message);
            logger.Log(typeof(MyAndromedaNLogger), logEvent);

            //this.logger.Fatal(format, args);
        }
    }   

    /// <summary>
    /// A logger that integrates with nlog, passing all messages to a <see cref="Logger"/>.
    /// </summary>
    public class NLogLogger : LoggerBase
    {
        /// <summary>
        /// NLog logger.
        /// </summary>
        private readonly Logger nlogLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogLogger"/> class.
        /// </summary>
        /// <param name="type">The type to create a logger for.</param>
        public NLogLogger(Type type)
            : base(type)
        {
            this.nlogLogger = LogManager.GetLogger(type.FullName);

            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogLogger"/> class.
        /// </summary>
        /// <param name="name">A custom name to use for the logger.  If null, the type's FullName will be used.</param>
        public NLogLogger(string name)
            : base(name)
        {
            this.nlogLogger = LogManager.GetLogger(name);
        }

        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        /// <value>The name of the logger.</value>
        public override string Name
        {
            get
            {
                return this.nlogLogger.Name;
            }
        }

        /// <summary>
        /// Gets a value indicating whether messages with Debug severity should be logged.
        /// </summary>
        public override bool IsDebugEnabled
        {
            get { return this.nlogLogger.IsDebugEnabled; }
        }

        /// <summary>
        /// Gets a value indicating whether messages with Info severity should be logged.
        /// </summary>
        public override bool IsInfoEnabled
        {
            get { return this.nlogLogger.IsInfoEnabled; }
        }

        /// <summary>
        /// Gets a value indicating whether messages with Trace severity should be logged.
        /// </summary>
        public override bool IsTraceEnabled
        {
            get { return this.nlogLogger.IsTraceEnabled; }
        }

        /// <summary>
        /// Gets a value indicating whether messages with Warn severity should be logged.
        /// </summary>
        public override bool IsWarnEnabled
        {
            get { return this.nlogLogger.IsWarnEnabled; }
        }

        /// <summary>
        /// Gets a value indicating whether messages with Error severity should be logged.
        /// </summary>
        public override bool IsErrorEnabled
        {
            get { return this.nlogLogger.IsErrorEnabled; }
        }

        /// <summary>
        /// Gets a value indicating whether messages with Fatal severity should be logged.
        /// </summary>
        public override bool IsFatalEnabled
        {
            get { return this.nlogLogger.IsFatalEnabled; }
        }

        /// <summary>
        /// Logs the specified exception with Debug severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public override void Debug(Exception exception, string format, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LogLevel.Debug, exception, format, args);
            }
        }

        /// <summary>
        /// Logs the specified exception with Debug severity.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log.</param>
        public override void DebugException(string message, Exception exception)
        {
            if (this.IsDebugEnabled)
            {
                nlogLogger.Debug(exception, message);
            }
        }

        /// <summary>
        /// Logs the specified message with Info severity.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Info(string message)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LogLevel.Info, "{0}", message);
            }
        }

        /// <summary>
        /// Logs the specified message with Debug severity.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Debug(string message)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LogLevel.Debug, "{0}", message);
            }
        }

        /// <summary>
        /// Logs the specified message with Debug severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public override void Debug(string format, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LogLevel.Debug, format, args);
            }
        }

        /// <summary>
        /// Logs the specified exception with Error severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public override void Error(Exception exception, string format, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LogLevel.Error, exception, format, args);
            }
        }

        /// <summary>
        /// Logs the specified exception with Error severity.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log.</param>
        public override void ErrorException(string message, Exception exception)
        {
            if (this.IsErrorEnabled)
            {
                this.nlogLogger.Error(exception, message);
            }
        }

        public void Error(Exception exception)
        {
            this.Error("A exception occurred", exception);
        }

        /// <summary>
        /// Logs the specified message with Fatal severity.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Fatal(string message)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LogLevel.Fatal, "{0}", message);
            }
        }

        public void Fatal(Exception exception)
        {
            this.Fatal(exception.Message);
        }

        /// <summary>
        /// Logs the specified message with Warn severity.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log.</param>
        public override void WarnException(string message, Exception exception)
        {
            if (this.IsWarnEnabled)
            {
                this.nlogLogger.Warn(exception, message);
            }
        }

        /// <summary>
        /// Logs the specified message with Error severity.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Error(string message)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LogLevel.Error, "{0}", message);
            }
        }

        /// <summary>
        /// Logs the specified message with Error severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public override void Error(string format, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LogLevel.Error, format, args);
            }
        }

        /// <summary>
        /// Logs the specified message with Fatal severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public override void Fatal(string format, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LogLevel.Fatal, format, args);
            }
        }

        /// <summary>
        /// Logs the specified exception with Fatal severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public override void Fatal(Exception exception, string format, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LogLevel.Fatal, exception, format, args);
            }
        }

        /// <summary>
        /// Logs the specified exception with Fatal severity.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log.</param>
        public override void FatalException(string message, Exception exception)
        {
            if (this.IsFatalEnabled)
            {
                this.nlogLogger.Fatal(exception, message);
            }
        }

        /// <summary>
        /// Logs the specified message with Info severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public override void Info(string format, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LogLevel.Info, format, args);
            }
        }

        /// <summary>
        /// Logs the specified exception with Info severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public override void Info(Exception exception, string format, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LogLevel.Info, exception, format, args);
            }
        }

        /// <summary>
        /// Logs the specified exception with Info severity.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log.</param>
        public override void InfoException(string message, Exception exception)
        {
            if (this.IsInfoEnabled)
            {
                this.nlogLogger.Info(exception, message);
            }
        }

        /// <summary>
        /// Logs the specified message with Trace severity.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Trace(string message)
        {
            if (this.IsTraceEnabled)
            {
                this.Log(LogLevel.Trace, "{0}", message);
            }
        }

        /// <summary>
        /// Logs the specified message with Trace severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public override void Trace(string format, params object[] args)
        {
            if (this.IsTraceEnabled)
            {
                this.Log(LogLevel.Trace, format, args);
            }
        }

        /// <summary>
        /// Logs the specified exception with Trace severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public override void Trace(Exception exception, string format, params object[] args)
        {
            if (this.IsTraceEnabled)
            {
                this.Log(LogLevel.Trace, exception, format, args);
            }
        }

        /// <summary>
        /// Logs the specified exception with Trace severity.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log.</param>
        public override void TraceException(string message, Exception exception)
        {
            if (this.IsTraceEnabled)
            {
                this.nlogLogger.Trace(exception, message);
            }
        }

        /// <summary>
        /// Logs the specified message with Warn severity.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Warn(string message)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LogLevel.Warn, "{0}", message);
            }
        }

        /// <summary>
        /// Logs the specified message with Warn severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public override void Warn(string format, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LogLevel.Warn, format, args);
            }
        }

        

        /// <summary>
        /// Logs the specified exception with Warn severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public override void Warn(Exception exception, string format, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LogLevel.Warn, exception, format, args);
            }
        }

        /// <summary>
        /// Logs with the specified level while preserving the call site.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The argsuments.</param>
        private void Log(LogLevel level, string format, params object[] args)
        {
            var le = new LogEventInfo(level, this.nlogLogger.Name, null, format, args);
            this.nlogLogger.Log(typeof(NLogLogger), le);
        }

        /// <summary>
        /// Logs with the specified level while preserving the call site.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="format">The message format.</param>
        /// <param name="args">The argsuments.</param>
        private void Log(LogLevel level, Exception exception, string format, params object[] args)
        {
            var le = new LogEventInfo(level, this.nlogLogger.Name, null, format, args, exception);
            this.nlogLogger.Log(typeof(NLogLogger), le);
        }
    }
}
