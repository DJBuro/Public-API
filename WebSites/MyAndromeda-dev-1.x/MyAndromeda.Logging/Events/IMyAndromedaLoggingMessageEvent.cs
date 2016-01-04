using MyAndromeda.Core;
using System;
using System.Linq;

namespace MyAndromeda.Logging.Events
{
    public interface IMyAndromedaLoggingMessageEvent : IDependency 
    {
        void OnDebug(string message);
        void OnInfo(string message);
        void OnTrace(string message);
        void OnWarn(string message);
        void OnError(string message);
        void OnError(Exception exception);
        void OnFatal(string message);
        void OnFatal(Exception exception);
    }
}
