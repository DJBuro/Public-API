using MyAndromeda.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.WebJobs.EventMarketing.Services
{
    public class MiniLogger : IMyAndromedaLogger
    {
        public void Debug(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            System.Diagnostics.Trace.WriteLine(message);
            Console.WriteLine(message);
        }

        public void Debug(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            System.Diagnostics.Trace.WriteLine(string.Format(format, args));
            Console.WriteLine(format, args);
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

        public void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            System.Diagnostics.Trace.WriteLine(message);
            Console.WriteLine(message);
        }

        public void Info(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            System.Diagnostics.Trace.WriteLine(string.Format(format, args));
            Console.WriteLine(format, args);
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

        public void Trace(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            System.Diagnostics.Trace.WriteLine(message);
        }

        public void Trace(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            System.Diagnostics.Trace.WriteLine(string.Format(format, args));
            Console.WriteLine(format, args);
        }

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
            string trace = string.Format(format, arg0, arg1, arg2);
            this.Trace(trace);
        }


        public void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message);
        }

        public void Warn(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Diagnostics.Trace.WriteLine(string.Format(format, args));
            Console.WriteLine(format, args);
        }

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

        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            System.Diagnostics.Trace.WriteLine(message);
            Console.WriteLine(message);
        }

        public void Error(Exception exception)
        {

            this.Error(exception.Message);
            //Console.WriteLine(exception.Message);
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

        public void Error(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(format, args);
        }

        public void Fatal(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);
            Console.WriteLine(message);
        }

        public void Fatal(Exception exception)
        {
            System.Diagnostics.Trace.WriteLine(exception.Message);
            //Console.WriteLine(message);
        }

        public void Fatal(string format, params object[] args)
        {
            System.Diagnostics.Trace.WriteLine(string.Format(format, args));
            Console.WriteLine(format, args);
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
