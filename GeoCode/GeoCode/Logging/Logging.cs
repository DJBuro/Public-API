using GeoCode.Domain;
using log4net;

namespace GeoCode
{
    public static class Logging
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Log));
        const string Message = "log";

        //public static void LogNotice(string methodName, string externalStoreId, string severity, string message, string result)
        //{
        //    log4net.GlobalContext.Properties["storeId"] = externalStoreId;
        //    log4net.GlobalContext.Properties["method"] = methodName;
        //    log4net.GlobalContext.Properties["severity"] = severity;


        //    if (externalStoreId == null && severity == null)
        //    {
        //        //note: this should catch random errors
        //        Logger.Error(message);
        //    }

        //    Logger.Error(result);
        //}

        public static string GetStoreId(string username)
        {
            if (username.StartsWith("MONITOR"))
            {
                return username.Substring(7);
            }

            return "?";
        }

        public static void LogNotice(string methodName, string externalStoreId, string severity, string message)
        {
            log4net.GlobalContext.Properties["storeId"] = externalStoreId;
            log4net.GlobalContext.Properties["method"] = methodName;
            log4net.GlobalContext.Properties["severity"] = severity;

            if (severity == "info")
            {
                Logger.Info(message);
            }
            else if (severity == "debug")
            {
                Logger.Debug(message);
            }
            else
            {
                Logger.Error(message);
            }
        }
    }
}
