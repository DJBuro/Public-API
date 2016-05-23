using System;
using System.Collections.Generic;

namespace MyAndromeda.Logging
{
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
                System.Diagnostics.Trace.WriteLine(message: "logger cannot be created");
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