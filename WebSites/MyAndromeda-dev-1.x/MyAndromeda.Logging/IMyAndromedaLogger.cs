using System;

namespace MyAndromeda.Logging
{
    public interface IMyAndromedaLogger
    {
        void Debug(string message);

        void Debug(string format, object arg0);
        void Debug(string format, object arg0, object arg1);
        void Debug(string format, object arg0, object arg1, object arg2);
        
        void Debug(string format, params object[] args);

        void Info(string message);

        void Info(string format, object arg0);
        void Info(string format, object arg0, object arg1);
        void Info(string format, object arg0, object arg1, object arg2);
        void Info(string format, params object[] args);

        void Trace(string message);

        void Trace(string format, object arg0);
        void Trace(string format, object arg0, object arg1);
        void Trace(string format, object arg0, object arg1, object arg2);
        void Trace(string format, params object[] args);

        void Warn(string message);
        void Warn(string format, object arg0);
        void Warn(string format, object arg0, object arg1);
        void Warn(string format, object arg0, object arg1, object arg2);
        void Warn(string format, params object[] args);

        void Error(string message);

        void Error(Exception exception);
        void Error(string format, object arg0);
        void Error(string format, object arg0, object arg1);
        void Error(string format, object arg0, object arg1, object arg2);
        void Error(string format, params object[] args);

        void Fatal(string message);

        void Fatal(Exception exception);
        void Fatal(string format, object arg0);
        void Fatal(string format, object arg0, object arg1);
        void Fatal(string format, object arg0, object arg1, object arg2);
        void Fatal(string format, params object[] args);
    }
}